using System;

namespace STARGBA.Device_Controller
{
    internal class TabletController : DeviceController
    {
        private readonly AdbConnection adb;
        

        public TabletController(Device device)
     : base(device)
        {
            adb = new AdbConnection();
        }

        public override void TurnScreenOn()
        {
            if (!IsScreenOn())
            {
                adb.SendCommand(
                    device,
                    "shell",
                    "input",
                    "keyevent",
                    "26");
            }
        }

        public override void TurnScreenOff()
        {
            if (IsScreenOn())
            {
                adb.SendCommand(
                    device,
                    "shell",
                    "input",
                    "keyevent",
                    "26");
            }
        }

        public bool IsConnected()
        {
            return adb.IsConnected();
        }

        public override bool IsScreenOn()
        {
            string output = adb.RunCommand(
                device,
                "shell",
                "dumpsys",
                "power");

            return output.Contains("Display Power: state=ON");
        }
    }
}