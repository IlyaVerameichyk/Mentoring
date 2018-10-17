using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Task1
{
    public class BatteryInterop
    {
        const int LastSleepTime = 15;
        const uint STATUS_SUCCESS = 0;
        
        [DllImport("powrprof.dll", EntryPoint = "CallNtPowerInformation")]
        static extern int CallNtPowerInformation(
            int InformationLevel,
            [In]IntPtr lpInputBuffer,
            int nInputBufferSize,
            [In, Out] IntPtr lpOutputBuffer,
            int nOutputBufferSize
        );

        public TimeSpan GetLastSleepTime()
        {
            var lastSleep = IntPtr.Zero;
            try
            {
                lastSleep = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(long)));

                int ntStatus = CallNtPowerInformation(LastSleepTime, IntPtr.Zero, 0, lastSleep,
                    Marshal.SizeOf(typeof(long)));

                if (ntStatus != STATUS_SUCCESS)
                {
                    throw new Win32Exception(ntStatus);
                }

                long lastSleepTimeIn100Nanoseconds = Marshal.ReadInt64(lastSleep, 0);
                var result = TimeSpan.FromSeconds(lastSleepTimeIn100Nanoseconds / 10000000.0);
                return result;
            }
            finally
            {
                if (lastSleep != IntPtr.Zero)
                    Marshal.FreeCoTaskMem(lastSleep);
            }
        }

        public TimeSpan GetWakeSleepTime()
        {
            var lastWake = IntPtr.Zero;
            try
            {
                lastWake = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(long)));

                int ntStatus = CallNtPowerInformation(LastSleepTime, IntPtr.Zero, 0, lastWake,
                    Marshal.SizeOf(typeof(long)));

                if (ntStatus != STATUS_SUCCESS)
                {
                    throw new Win32Exception(ntStatus);
                }

                long lastWakeTimeIn100Nanoseconds = Marshal.ReadInt64(lastWake, 0);
                var result = TimeSpan.FromSeconds(lastWakeTimeIn100Nanoseconds / 10000000.0);
                return result;
            }
            finally
            {
                if (lastWake != IntPtr.Zero)
                    Marshal.FreeCoTaskMem(lastWake);
            }
        }
    }
}