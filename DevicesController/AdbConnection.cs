
using System;
using System.Diagnostics;
using System.IO;

namespace STARGBA.Device_Controller
{
    internal class AdbConnection
    {
        private readonly string adbPath;

        public AdbConnection()
        {
            adbPath = Path.Combine(
                AppContext.BaseDirectory,
                "adb",
                "adb.exe");
        }

        public void SendCommand(params string[] arguments)
        {
            var process = new Process();

            process.StartInfo.FileName = adbPath;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;

            foreach (var argument in arguments)
            {
                process.StartInfo.ArgumentList.Add(argument);
            }

            process.Start();
            process.WaitForExit();
        }

        public void SendCommand(Device device, params string[] arguments)
        {
            var process = new Process();

            process.StartInfo.FileName = adbPath;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;

            process.StartInfo.ArgumentList.Add("-s");
            process.StartInfo.ArgumentList.Add(device.Serial);

            foreach (var argument in arguments)
            {
                process.StartInfo.ArgumentList.Add(argument);
            }

            process.Start();
            process.WaitForExit();
        }

        public string RunCommand(params string[] arguments)
        {
            var process = new Process();

            process.StartInfo.FileName = adbPath;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;

            process.StartInfo.RedirectStandardOutput = true;

            foreach (var argument in arguments)
            {
                process.StartInfo.ArgumentList.Add(argument);
            }

            process.Start();

            string output = process.StandardOutput.ReadToEnd();

            process.WaitForExit();

            return output;
        }

        public string RunCommand(Device device, params string[] arguments)
        {
            var process = new Process();

            process.StartInfo.FileName = adbPath;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.RedirectStandardOutput = true;

            process.StartInfo.ArgumentList.Add("-s");
            process.StartInfo.ArgumentList.Add(device.Serial);

            foreach (var argument in arguments)
            {
                process.StartInfo.ArgumentList.Add(argument);
            }

            process.Start();

            string output = process.StandardOutput.ReadToEnd();

            process.WaitForExit();

            return output;
        }

        public bool IsConnected()
        {
            string output = RunCommand("devices");

            return output.Contains("\tdevice");
        }
    }
}
