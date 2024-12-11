using System;
using System.Diagnostics;

namespace VehicleDetection_8._0_.src.CSharp.Services
{
    public static class PortKiller
    {
        public static void KillProcessOnPort(int port)
        {
            try
            {
                // Sử dụng netstat để lấy thông tin chi tiết về kết nối
                Process netstatProcess = Process.Start(new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = $"/c netstat -ano | findstr :{port} | findstr LISTENING",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                });

                string output = netstatProcess.StandardOutput.ReadToEnd();
                netstatProcess.WaitForExit();

                if (!string.IsNullOrWhiteSpace(output))
                {
                    // Tách và lấy PID từ dòng cuối cùng
                    string[] lines = output.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var line in lines)
                    {
                        string[] parts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                        if (parts.Length > 0)
                        {
                            int pid = int.Parse(parts[parts.Length - 1]);

                            // Kill process bằng taskkill để đảm bảo force kill
                            Process killProcess = Process.Start(new ProcessStartInfo
                            {
                                FileName = "taskkill",
                                Arguments = $"/PID {pid} /F",
                                UseShellExecute = false,
                                CreateNoWindow = true
                            });

                            killProcess.WaitForExit();

                            Console.WriteLine($"Killed process with PID {pid} on port {port}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error killing process on port {port}: {ex.Message}");
            }
        }

        // Phương thức bổ sung để kill tất cả các kết nối TIME_WAIT, ESTABLISHED, và các kết nối khác trên cổng
        public static void KillAllConnectionsOnPort(int port)
        {
            try
            {
                // Lấy tất cả các kết nối đang sử dụng cổng này và kill các kết nối trong các trạng thái khác nhau
                Process killAllProcess = Process.Start(new ProcessStartInfo
                {
                    FileName = "powershell",
                    Arguments = $"-Command \"Get-NetTCPConnection -LocalPort {port} | ForEach-Object {{ Remove-NetTCPConnection -ConnectionId $_.ConnectionId -Force }}\"",
                    UseShellExecute = false,
                    CreateNoWindow = true
                });

                killAllProcess.WaitForExit();
                Console.WriteLine($"Attempted to kill all connections on port {port}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error killing connections on port {port}: {ex.Message}");
            }
        }

        // Phương thức để Kill cả tiến trình và tất cả các kết nối (bao gồm TIME_WAIT, ESTABLISHED, v.v.) trên cổng
        public static void KillAllProcessesAndConnectionsOnPort(int port)
        {
            try
            {
                // Kill tất cả các tiến trình đang lắng nghe trên cổng đó
                Process netstatProcess = Process.Start(new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = $"/c netstat -ano | findstr :{port} | findstr LISTENING",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                });

                string output = netstatProcess.StandardOutput.ReadToEnd();
                netstatProcess.WaitForExit();

                if (!string.IsNullOrWhiteSpace(output))
                {
                    string[] lines = output.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var line in lines)
                    {
                        string[] parts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                        if (parts.Length > 0)
                        {
                            int pid = int.Parse(parts[parts.Length - 1]);

                            // Kill tiến trình bằng taskkill
                            Process killProcess = Process.Start(new ProcessStartInfo
                            {
                                FileName = "taskkill",
                                Arguments = $"/PID {pid} /F",
                                UseShellExecute = false,
                                CreateNoWindow = true
                            });

                            killProcess.WaitForExit();

                            Console.WriteLine($"Killed process with PID {pid} on port {port}");
                        }
                    }
                }

                // Sau đó kill tất cả các kết nối còn lại trên cổng này (bao gồm TIME_WAIT, ESTABLISHED, v.v.)
                Process killAllConnectionsProcess = Process.Start(new ProcessStartInfo
                {
                    FileName = "powershell",
                    Arguments = $"-Command \"Get-NetTCPConnection -LocalPort {port} | ForEach-Object {{ Remove-NetTCPConnection -ConnectionId $_.ConnectionId -Force }}\"",
                    UseShellExecute = false,
                    CreateNoWindow = true
                });

                killAllConnectionsProcess.WaitForExit();
                Console.WriteLine($"Attempted to kill all connections on port {port}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error killing processes and connections on port {port}: {ex.Message}");
            }
        }
    }
}
