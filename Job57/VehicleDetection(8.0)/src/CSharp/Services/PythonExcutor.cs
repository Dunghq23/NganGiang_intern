﻿using System.Diagnostics;
namespace VehicleDetection.src.CSharp.Services
{
    public class PythonExecutor
    {
        private string pythonPath; // Đường dẫn đến interpreter Python
        private string scriptPath; // Đường dẫn đến script Python

        public PythonExecutor(string pythonPath, string scriptPath)
        {
            this.pythonPath = pythonPath;
            this.scriptPath = scriptPath;
        }

        public string Execute(string args)
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = pythonPath,
                Arguments = $"{scriptPath} {args}", // Thêm các đối số cần thiết
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (var process = Process.Start(startInfo))
            {
                using (var reader = process.StandardOutput)
                {
                    string result = reader.ReadToEnd();
                    process.WaitForExit();
                    return result;
                }
            }
        }
    }
}
