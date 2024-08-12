using FileTransferServerCsharp.Services;
using System.Text;

Console.OutputEncoding = Encoding.UTF8;

string ROOT_DIRECTORY = Directory.GetParent(AppContext.BaseDirectory).Parent.Parent.Parent.FullName;

int port = 100;
string directoryPath = Path.Combine(ROOT_DIRECTORY, "FileReceived");

FileServer server = new FileServer(port, directoryPath);
server.Start();