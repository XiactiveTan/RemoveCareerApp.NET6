using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
namespace RemoveCareerNET6
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("-- RemoveCareer .NET6 --");
            string exePath = Environment.GetCommandLineArgs()[0];
            string exeFullPath = System.IO.Path.GetFullPath(exePath);
            string startupPath = System.IO.Path.GetDirectoryName(exeFullPath);




            var proc = new Process();
            proc.StartInfo.FileName = $@"{startupPath}\platform-tools\adb.exe";
            proc.StartInfo.Arguments = "start-server";
            proc.StartInfo.CreateNoWindow = true;
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.StartInfo.RedirectStandardError = true;
            proc.OutputDataReceived += Proc_OutputDataReceived;
            proc.ErrorDataReceived += Proc_ErrorDataReceived;
            proc.Start();
            proc.BeginOutputReadLine();
            proc.BeginErrorReadLine();
            await Task.Run(() => proc.WaitForExit());
            await Task.Delay(500);
            proc.CancelOutputRead(); // 使い終わったら止める
            proc.CancelErrorRead();
            Log = new List<string?>();


            proc = new Process();


            proc.StartInfo.FileName = $@"{startupPath}\platform-tools\adb.exe";
            proc.StartInfo.Arguments = "devices";
            proc.StartInfo.CreateNoWindow = true;
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.StartInfo.RedirectStandardError = true;
            proc.OutputDataReceived += Proc_OutputDataReceived;
            proc.ErrorDataReceived += Proc_ErrorDataReceived;
            proc.Start();
            proc.BeginOutputReadLine();
            proc.BeginErrorReadLine();
            await Task.Run(() => proc.WaitForExit());
            await Task.Delay(500);
            proc.CancelOutputRead(); // 使い終わったら止める
            proc.CancelErrorRead();

            bool device_found = false;
            foreach(var line in Log)
            {
                if (!string.IsNullOrEmpty(line) && line.Contains("device") && !line.Contains("List of devices attached"))
                {
                    device_found = true;
                    break;
                }
            }

            if (!device_found)
            {
                Console.WriteLine("デバイスが見つかりませんでした。");
                proc = new Process();


                proc.StartInfo.FileName = $@"{startupPath}\platform-tools\adb.exe";
                proc.StartInfo.Arguments = "kill-server";
                proc.StartInfo.CreateNoWindow = true;
                proc.StartInfo.UseShellExecute = false;
                proc.StartInfo.RedirectStandardOutput = true;
                proc.StartInfo.RedirectStandardError = true;
                proc.OutputDataReceived += Proc_OutputDataReceived;
                proc.ErrorDataReceived += Proc_ErrorDataReceived;
                proc.Start();
                proc.BeginOutputReadLine();
                proc.BeginErrorReadLine();
                await Task.Run(() => proc.WaitForExit());
                await Task.Delay(500);
                proc.CancelOutputRead(); // 使い終わったら止める
                proc.CancelErrorRead();
                Console.ReadKey();
                return;
            }
            else
            {
                Console.WriteLine("デバイスが見つかりました。");
            }

            Log = new List<string>();

            proc = new Process();


            proc.StartInfo.FileName = $@"{startupPath}\platform-tools\adb.exe";
            proc.StartInfo.Arguments = "shell pm list packages | sort";
            proc.StartInfo.CreateNoWindow = true;
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.StartInfo.RedirectStandardError = true;
            proc.OutputDataReceived += Proc_OutputDataReceived;
            proc.ErrorDataReceived += Proc_ErrorDataReceived;
            proc.Start();
            proc.BeginOutputReadLine();
            proc.BeginErrorReadLine();
            await Task.Run(() => proc.WaitForExit());
            await Task.Delay(500);
            proc.CancelOutputRead(); // 使い終わったら止める
            proc.CancelErrorRead();

            List<string> Packages = new();
            List<string> CareerPackages = new();
            foreach(var line in Log)
            {
                if (!string.IsNullOrEmpty(line))
                {
                    Packages.Add(line);
                    if(line.Contains("docomo") || line.Contains("ntt") || line.Contains("auone") || line.Contains("rakuten") || line.Contains("rakuten") || line.Contains("kddi") || line.Contains("softbank"))
                    {
                        CareerPackages.Add(line);
                    }
                }
            }

            Console.WriteLine("-- ↓見つかったキャリアアプリ一覧↓ --");
            foreach(var package in CareerPackages)
            {
                Console.WriteLine(package);
            }

            Console.WriteLine($"{Packages.Count}個/{CareerPackages.Count}個のパッケージが見つかりました。");
            while (true)
            {
                Console.Write("以上のアプリケーションが削除されます。よろしいでしょうか？[y/n]:");
                string? Input = Console.ReadLine();

                switch (Input)
                {
                    case "y":
                        {
                            Console.WriteLine("削除を開始します。");
                            foreach (var package in CareerPackages)
                            {
                                Console.WriteLine(package + "を削除中");
                                proc = new Process();
                                proc.StartInfo.FileName = $@"{startupPath}\platform-tools\adb.exe";
                                proc.StartInfo.Arguments = $"shell pm uninstall --user 0 {package.Replace("package:","")}";
                                proc.StartInfo.CreateNoWindow = true;
                                proc.StartInfo.UseShellExecute = false;
                                proc.StartInfo.RedirectStandardOutput = true;
                                proc.StartInfo.RedirectStandardError = true;
                                proc.OutputDataReceived += Proc_OutputDataReceived;
                                proc.ErrorDataReceived += Proc_ErrorDataReceived;
                                proc.Start();
                                proc.BeginOutputReadLine();
                                proc.BeginErrorReadLine();
                                await Task.Run(() => proc.WaitForExit());
                                await Task.Delay(500);
                                proc.CancelOutputRead(); // 使い終わったら止める
                                proc.CancelErrorRead();
                                Console.WriteLine("アプリの消去を実行しました。");
                            }
                            Console.WriteLine("処理は終了しました。");
                            Console.WriteLine("終了します。");
                            proc = new Process();


                            proc.StartInfo.FileName = $@"{startupPath}\platform-tools\adb.exe";
                            proc.StartInfo.Arguments = "kill-server";
                            proc.StartInfo.CreateNoWindow = true;
                            proc.StartInfo.UseShellExecute = false;
                            proc.StartInfo.RedirectStandardOutput = true;
                            proc.StartInfo.RedirectStandardError = true;
                            proc.OutputDataReceived += Proc_OutputDataReceived;
                            proc.ErrorDataReceived += Proc_ErrorDataReceived;
                            proc.Start();
                            proc.BeginOutputReadLine();
                            proc.BeginErrorReadLine();
                            await Task.Run(() => proc.WaitForExit());
                            await Task.Delay(500);
                            proc.CancelOutputRead(); // 使い終わったら止める
                            proc.CancelErrorRead();
                            Console.ReadKey();
                            return;
                        }
                        break;
                    case "n":
                        {
                            Console.WriteLine("終了します。");
                            proc = new Process();


                            proc.StartInfo.FileName = $@"{startupPath}\platform-tools\adb.exe";
                            proc.StartInfo.Arguments = "kill-server";
                            proc.StartInfo.CreateNoWindow = true;
                            proc.StartInfo.UseShellExecute = false;
                            proc.StartInfo.RedirectStandardOutput = true;
                            proc.StartInfo.RedirectStandardError = true;
                            proc.OutputDataReceived += Proc_OutputDataReceived;
                            proc.ErrorDataReceived += Proc_ErrorDataReceived;
                            proc.Start();
                            proc.BeginOutputReadLine();
                            proc.BeginErrorReadLine();
                            await Task.Run(() => proc.WaitForExit());
                            await Task.Delay(500);
                            proc.CancelOutputRead(); // 使い終わったら止める
                            proc.CancelErrorRead();
                            Console.ReadKey();
                            return;
                        }
                        break;
                    default:
                        {
                            Console.WriteLine("yかnを入力してください。");
                        }
                        break;
                }


            }
            

        }

        public static List<string?> Log = new List<string>();

        private static void Proc_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            Log.Add(e.Data);
            Console.WriteLine(e.Data);
        }

        private static void Proc_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            Log.Add(e.Data);
            Console.WriteLine(e.Data);
        }
    }
}