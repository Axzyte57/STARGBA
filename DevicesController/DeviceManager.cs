using System;
using System.Collections.Generic;

namespace STARGBA.Device_Controller
{
    internal class DeviceManager
    {
        private readonly AdbConnection adb;

        public DeviceManager()
        {
            adb = new AdbConnection();
        }

        public List<Device> GetDevices()
        {
            string output = adb.RunCommand("devices");

            var devices = new List<Device>();

            string[] lines = output.Split(
                new[] { '\r', '\n' },
                StringSplitOptions.RemoveEmptyEntries);

            int counter = 1;

            foreach (string line in lines)
            {
                if (line.StartsWith("List of devices"))
                    continue;

                string[] parts = line.Split(
                    '\t',
                    StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length >= 2)
                {
                    string serial = parts[0];
                    string state = parts[1];

                    devices.Add(
                        new Device(serial, state, counter));

                    counter++;
                }
            }

            return devices;
        }
    }
}