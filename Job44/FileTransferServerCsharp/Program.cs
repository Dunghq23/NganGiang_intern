using FileTransferServerCsharp.Services;
using System.Text;

Console.OutputEncoding = Encoding.UTF8;

string ROOT_DIRECTORY = Directory.GetParent(AppContext.BaseDirectory).Parent.Parent.Parent.FullName;
//Console.WriteLine("Thư mục hiện tại: " + Directory.GetCurrentDirectory());
//Console.WriteLine("Thư mục gốc: " + ROOT_DIRECTORY);

int port = 100;
string directoryPath = Path.Combine(ROOT_DIRECTORY, "FileReceived");

FileServer server = new FileServer(port, directoryPath);
server.Start();
