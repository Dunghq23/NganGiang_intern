using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace FileTransferServerCsharp.Services
{
    /// <summary>
    /// Lớp để quản lý việc nhận file từ client qua TCP và thực hiện nhận diện khuôn mặt.
    /// </summary>
    public class FileServer
    {
        private readonly int _port;
        private readonly string _directoryPath;
        private TrainAndRecognition _trainAndRecognition;

        /// <summary>
        /// Khởi tạo đối tượng FileServer với cổng và thư mục lưu trữ.
        /// </summary>
        /// <param name="port">Cổng để server lắng nghe kết nối.</param>
        /// <param name="directoryPath">Đường dẫn đến thư mục lưu trữ file nhận được.</param>
        public FileServer(int port, string directoryPath)
        {
            _port = port;
            _directoryPath = directoryPath;
            _trainAndRecognition = new TrainAndRecognition();

            // Tạo thư mục nếu nó không tồn tại
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
                Console.WriteLine($"Thư mục {directoryPath} đã được tạo.");
            }
        }

        /// <summary>
        /// Bắt đầu lắng nghe các kết nối từ client và xử lý file nhận được.
        /// </summary>
        public void Start()
        {
            try
            {
                TcpListener listener = new TcpListener(IPAddress.Any, _port);
                listener.Start();

                IPEndPoint localEndPoint = listener.LocalEndpoint as IPEndPoint;
                if (localEndPoint != null)
                {
                    IPAddress ipv4Address = GetLocalIPv4Address();
                    Console.WriteLine($"Server đang lắng nghe trên IP: {ipv4Address} và cổng: {localEndPoint.Port}");
                }

                while (true)
                {
                    using (TcpClient client = listener.AcceptTcpClient())
                    {
                        IPEndPoint remoteEndpoint = client.Client.RemoteEndPoint as IPEndPoint;
                        if (remoteEndpoint != null)
                        {
                            Console.WriteLine($"\nĐã kết nối với client: {remoteEndpoint.Address}:{remoteEndpoint.Port}");
                        }

                        using (NetworkStream stream = client.GetStream())
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            Stopwatch stopwatch = new Stopwatch();
                            stopwatch.Start();
                            string fileName = reader.ReadLine();
                            if (string.IsNullOrEmpty(fileName))
                            {
                                Console.WriteLine("Tên file không hợp lệ.");
                                continue;
                            }
                            string filePath = Path.Combine(_directoryPath, fileName);

                            using (FileStream fileStream = File.Create(filePath))
                            {
                                byte[] buffer = new byte[4096];
                                int bytesRead;
                                while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
                                {
                                    fileStream.Write(buffer, 0, bytesRead);
                                }
                                Console.WriteLine($"File {fileName} đã được nhận và lưu vào thư mục.");
                            }
                            stopwatch.Stop();

                            // In ra thời gian đã trôi qua
                            TimeSpan elapsedTime = stopwatch.Elapsed;
                            Console.WriteLine($"Thời gian lưu file: {elapsedTime.TotalMilliseconds} ms");

                            stopwatch.Restart();
                            stopwatch.Start();
                            string output = _trainAndRecognition.RunRecognition(filePath);
                            stopwatch.Stop(); 
                            elapsedTime = stopwatch.Elapsed;
                            Console.WriteLine($"{output} ({elapsedTime.TotalMilliseconds} ms)");

                            byte[] response = Encoding.UTF8.GetBytes($"{output}");
                            stream.Write(response, 0, response.Length);
                            stream.Flush();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi khởi động server: " + ex.ToString());
            }
        }


        /// <summary>
        /// Lấy địa chỉ IPv4 của máy chủ.
        /// </summary>
        /// <returns>Địa chỉ IPv4 của máy chủ.</returns>
        private IPAddress GetLocalIPv4Address()
        {
            foreach (var ip in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip;
                }
            }
            return IPAddress.None;
        }
    }
}
