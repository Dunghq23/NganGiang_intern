using System.Net;
using System.Net.Sockets;
using System.Text;

namespace FileTransferServerCsharp.Services
{
    public class FileServer
    {
        private readonly int _port;
        private readonly string _directoryPath;

        public FileServer(int port, string directoryPath)
        {
            _port = port;
            _directoryPath = directoryPath;

            // Tạo thư mục nếu nó không tồn tại
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
                Console.WriteLine($"Thư mục {directoryPath} đã được tạo.");
            }
        }

        public void Start()
        {
            try
            {
                // Chỉ lắng nghe trên IPv4
                TcpListener listener = new TcpListener(IPAddress.Any, _port);
                listener.Start();


                // In ra địa chỉ IP và cổng của server
                IPEndPoint localEndPoint = listener.LocalEndpoint as IPEndPoint;
                if (localEndPoint != null)
                {
                    IPAddress ipv4Address = GetLocalIPv4Address();
                    Console.WriteLine($"Server đang lắng nghe trên IP: {ipv4Address} và cổng: {localEndPoint.Port}");
                }

                while (true)
                {
                    // Chấp nhận kết nối từ client
                    using (TcpClient client = listener.AcceptTcpClient())
                    {
                        IPEndPoint remoteEndpoint = client.Client.RemoteEndPoint as IPEndPoint;
                        if (remoteEndpoint != null)
                        {
                            Console.WriteLine($"Đã kết nối với client: {remoteEndpoint.Address}:{remoteEndpoint.Port}");
                        }

                        using (NetworkStream stream = client.GetStream())
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            // Nhận tên file từ Client
                            string fileName = reader.ReadLine();
                            if (string.IsNullOrEmpty(fileName))
                            {
                                Console.WriteLine("Tên file không hợp lệ.");
                                continue;
                            }
                            string filePath = Path.Combine(_directoryPath, fileName);

                            // Nhận dữ liệu file
                            using (FileStream fileStream = File.Create(filePath))
                            {
                                byte[] buffer = new byte[4096];
                                int bytesRead;
                                while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
                                {
                                    fileStream.Write(buffer, 0, bytesRead);
                                }
                            }

                            Console.WriteLine($"File {fileName} đã được nhận và lưu vào thư mục {_directoryPath}");

                            // Gửi phản hồi lại cho client
                            byte[] response = Encoding.UTF8.GetBytes("Hello, File received successfully!\n");
                            stream.Write(response, 0, response.Length);
                            stream.Flush(); // Đảm bảo tất cả dữ liệu đã được gửi đi
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi khởi động server: " + ex.ToString());
            }
        }

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
