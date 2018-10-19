using System.Runtime.InteropServices;

namespace Task1.Models
{
    [StructLayout(LayoutKind.Sequential)]
    public struct SystemPowerInformation
    {
        public uint MaxIdlenessAllowed;
        public uint Idleness;
        public uint TimeRemaining;
        public byte CoolingMode;
    }
}