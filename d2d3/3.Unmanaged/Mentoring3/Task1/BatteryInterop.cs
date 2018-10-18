using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Task1
{
    public class BatteryInterop
    {
        private const int LastSleepTime = 15;
        private const int LastWakeTime = 14;
        const uint STATUS_SUCCESS = 0;

        [StructLayout(LayoutKind.Sequential)]
        public struct SYSTEM_BATTERY_STATE
        {
            public byte AcOnLine;
            public byte BatteryPresent;
            public byte Charging;
            public byte Discharging;
            public byte spare1;
            public byte spare2;
            public byte spare3;
            public byte spare4;
            public uint MaxCapacity;
            public uint RemainingCapacity;
            public int Rate;
            public uint EstimatedTime;
            public uint DefaultAlert1;
            public uint DefaultAlert2;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SYSTEM_POWER_INFORMATION
        {
            public ulong MaxIdlenessAllowed;
            public ulong Idleness;
            public ulong TimeRemaining;
            public byte CoolingMode;
        }


        [DllImport("powrprof.dll", EntryPoint = "CallNtPowerInformation")]
        static extern int CallNtPowerInformation(
            int InformationLevel,
            [In]IntPtr lpInputBuffer,
            int nInputBufferSize,
            [In, Out] IntPtr lpOutputBuffer,
            int nOutputBufferSize
        );

        [DllImport("powrprof.dll", EntryPoint = "CallNtPowerInformation")]
        static extern int CallNtPowerInformation(
            int InformationLevel,
            [In]IntPtr lpInputBuffer,
            int nInputBufferSize,
            [Out] out SYSTEM_BATTERY_STATE lpOutputBuffer,
            int nOutputBufferSize
        );

        [DllImport("powrprof.dll", EntryPoint = "CallNtPowerInformation")]
        static extern int CallNtPowerInformation(
            int InformationLevel,
            [In]IntPtr lpInputBuffer,
            int nInputBufferSize,
            [Out] out SYSTEM_POWER_INFORMATION lpOutputBuffer,
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

        public TimeSpan GetLastWakeTime()
        {
            var lastWake = IntPtr.Zero;
            try
            {
                lastWake = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(long)));

                int ntStatus = CallNtPowerInformation(LastWakeTime, IntPtr.Zero, 0, lastWake,
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

        public SYSTEM_BATTERY_STATE GetSystemBatteryState()
        {
            SYSTEM_BATTERY_STATE state;
            var result = CallNtPowerInformation(
                5,
                IntPtr.Zero,
                0,
                out state,
                Marshal.SizeOf(typeof(SYSTEM_BATTERY_STATE))
            );
            if (result != STATUS_SUCCESS)
            {
                throw new Win32Exception(result);
            }
            return state;
        }
        public SYSTEM_POWER_INFORMATION GetSystemPowerInformation()
        {
            SYSTEM_POWER_INFORMATION state;
            var result = CallNtPowerInformation(
                5,
                IntPtr.Zero,
                0,
                out state,
                Marshal.SizeOf(typeof(SYSTEM_POWER_INFORMATION))
            );
            if (result != STATUS_SUCCESS)
            {
                throw new Win32Exception(result);
            }
            return state;
        }
    }
}