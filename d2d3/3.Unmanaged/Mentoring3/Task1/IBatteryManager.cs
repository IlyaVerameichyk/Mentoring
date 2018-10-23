using System;
using System.Runtime.InteropServices;
using Task1.Models;

namespace Task1
{
    [ComVisible(true)]
    [Guid("78AFF0C7-73F5-4D51-8DC4-BB73D58151A7")]
    [InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IBatteryManager
    {
        TimeSpan GetLastSleepTime();
        TimeSpan GetLastWakeTime();
        SystemBatteryState GetSystemBatteryState();
        SystemPowerInformation GetSystemPowerInformation();
        void WriteHiberFile(bool write);
        void SetSleep();
        void SetHibernation();
    }
}