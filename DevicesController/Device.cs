namespace STARGBA.Device_Controller
{
    internal class Device
    {
        public string Serial { get; }
        public string State { get; }
        public int Number { get; }

        public Device(string serial, string state, int number)
        {
            Serial = serial;
            State = state;
            Number = number;
        }
    }
}