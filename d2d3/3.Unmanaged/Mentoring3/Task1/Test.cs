using Xunit;

namespace Task1
{
    public class Test
    {
        [Fact]
        public void Tests()
        {
            var a = new BatteryInterop();
            var s = a.GetLastSleepTime();
            var w = a.GetLastWakeTime();
            var state = a.GetSystemBatteryState();
            var info = a.GetSystemPowerInformation();
            //a.GetSystemPowerInformation();
            a.WriteHiberFile(true);
            //a.SetSleep();
            a.SetHibernation();
        }
    }
}