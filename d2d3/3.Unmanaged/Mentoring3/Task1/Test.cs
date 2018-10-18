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
            a.GetSystemBatteryState();
            a.GetSystemPowerInformation();
        }
    }
}