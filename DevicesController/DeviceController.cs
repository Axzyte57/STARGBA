namespace STARGBA.Device_Controller
{
    internal abstract class DeviceController
    {
        

        protected readonly Device device;

        protected DeviceController(Device device)
        {
            this.device = device;
        }

        public abstract void TurnScreenOn();

        public abstract void TurnScreenOff();

        public abstract bool IsScreenOn();
    }
}