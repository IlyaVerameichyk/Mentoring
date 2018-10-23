using System;
using System.Runtime.InteropServices;
using Task1.Models;

namespace Task1
{
    [ComVisible(true)]
    [Guid("5A7BDCF6-5C01-4A0A-83E1-10037326DDA8")]
    [ClassInterface(ClassInterfaceType.None)]
    public class BatteryManager : IBatteryManager
    {
        public TimeSpan GetLastSleepTime()
        {
            return BatteryInterop.GetLastSleepTime();
        }

        public TimeSpan GetLastWakeTime()
        {
            return BatteryInterop.GetLastWakeTime();
        }

        public SystemBatteryState GetSystemBatteryState()
        {
            return BatteryInterop.GetSystemBatteryState();
        }

        public SystemPowerInformation GetSystemPowerInformation()
        {
            return BatteryInterop.GetSystemPowerInformation();
        }

        public void WriteHiberFile(bool write)
        {
            BatteryInterop.WriteHiberFile(write);
        }

        public void SetSleep()
        {
            BatteryInterop.SetSleep();
        }

        public void SetHibernation()
        {
            BatteryInterop.SetHibernation();
        }
    }
}