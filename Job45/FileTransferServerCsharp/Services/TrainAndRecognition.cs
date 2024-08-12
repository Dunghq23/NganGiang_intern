using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;

namespace FileTransferServerCsharp.Services
{
    
    public class TrainAndRecognition
    {
        private readonly string _rootDirectory;
        private readonly string _pythonScriptPath;
        private readonly string _encodingPath;
        private readonly string _resultPath;

        public TrainAndRecognition()
        {
            _rootDirectory = Directory.GetParent(AppContext.BaseDirectory)?.Parent?.Parent?.Parent?.FullName ?? string.Empty;
            _pythonScriptPath = Path.Combine(_rootDirectory, "Services", "Main.py").Replace('\\', '/');
            _encodingPath = Path.Combine(_rootDirectory, "Models", "encodings.txt").Replace('\\', '/');
            _resultPath = Path.Combine(_rootDirectory, "Services", "results.txt").Replace('\\', '/');
        }

        private string RunPythonScript(string cmd, string args)
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = cmd,
                Arguments = args,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            };

            using (var process = Process.Start(startInfo))
            {
                if (process == null) return string.Empty;
                using (var reader = process.StandardOutput)
                {
                    string result = reader.ReadToEnd();
                    Console.WriteLine(result);
                    return result;
                }
            }
        }

        public string RunRecognition(string imagePath)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            RunPythonScript("python", $"\"{_pythonScriptPath}\" Recognize \"{imagePath}\" \"{_encodingPath}\" \"{_resultPath}\"");
            stopwatch.Stop();
            TimeSpan elapsedTime = stopwatch.Elapsed;
            Console.WriteLine($"Thời gian thực thi hàm Run ({elapsedTime.TotalMilliseconds} ms)");
            // Đọc nội dung từ file kết quả
            string output = string.Empty;
            if (File.Exists(_resultPath))
            {
                try
                {
                    output = File.ReadAllText(_resultPath);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Lỗi khi đọc file kết quả: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("File kết quả không tồn tại.");
            }

            return output;
            //return $"python \"{_pythonScriptPath}\" Recognize \"{imagePath}\" \"{_encodingPath}\" \"{_resultPath}\"";
        }

    }
}
