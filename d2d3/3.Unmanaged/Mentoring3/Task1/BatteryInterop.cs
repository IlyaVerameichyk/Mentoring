using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using Task1.Models;

namespace Task1
{
    public class BatteryInterop
    {
        public TimeSpan GetLastSleepTime()
        {
            ulong time;
            var result = CallNtPowerInformationInternal(InformationLevel.LastSleepTime, out time);
            if (result != NtStatus.StatusSuccess)
            {
                throw new Win32Exception();
            }
            var resultTime = TimeSpan.FromSeconds(time / 10000000.0);
            return resultTime;
        }

        public TimeSpan GetLastWakeTime()
        {
            ulong time;
            var result = CallNtPowerInformationInternal(InformationLevel.LastWakeTime, out time);
            if (result != NtStatus.StatusSuccess)
            {
                throw new Win32Exception();
            }
            var resultTime = TimeSpan.FromSeconds(time / 10000000.0);
            return resultTime;
        }

        public SystemBatteryState GetSystemBatteryState()
        {
            SystemBatteryState state;
            var result = CallNtPowerInformationInternal(InformationLevel.SystemBatteryState, out state);
            if (result != NtStatus.StatusSuccess)
            {
                throw new Win32Exception();
            }
            return state;
        }

        public SystemPowerInformation GetSystemPowerInformation()
        {
            SystemPowerInformation information;
            var result = CallNtPowerInformationInternal(InformationLevel.SystemPowerInformation, out information);
            if (result != NtStatus.StatusSuccess)
            {
                throw new Win32Exception();
            }
            return information;
        }

        public void WriteHiberFile(bool write)
        {
            var inValue = write ? (byte)1 : (byte)0;
            var outputPtr = IntPtr.Zero;
            try
            {
                var inV = Marshal.AllocCoTaskMem(inValue);
                Marshal.WriteByte(inV, 0);
                var ntStatus = (NtStatus)CallNtPowerInformation(
                    (int)10,
                    inV,
                    Marshal.SizeOf(typeof(byte)),
                    IntPtr.Zero,
                    0);
                if (ntStatus != NtStatus.StatusSuccess)
                {
                    throw new Win32Exception();
                }
            }
            finally
            {
                if (outputPtr != IntPtr.Zero)
                    Marshal.FreeCoTaskMem(outputPtr);
            }
        }

        [DllImport("powrprof.dll", EntryPoint = "CallNtPowerInformation")]
        static extern uint CallNtPowerInformation(
            int InformationLevel,
            [In]IntPtr lpInputBuffer,
            int nInputBufferSize,
            [In, Out] IntPtr lpOutputBuffer,
            int nOutputBufferSize
        );

        private NtStatus CallNtPowerInformationInternal<TOut>(InformationLevel informationLevel, out TOut outputValue) where TOut : struct
        {
            var outputPtr = IntPtr.Zero;
            try
            {
                outputPtr = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(TOut)));

                var ntStatus = CallNtPowerInformation(
                    (int)informationLevel,
                    IntPtr.Zero,
                    0,
                    outputPtr,
                    Marshal.SizeOf(typeof(TOut)));

                outputValue = (TOut)Marshal.PtrToStructure(outputPtr, typeof(TOut));
                return (NtStatus)ntStatus;
            }
            finally
            {
                if (outputPtr != IntPtr.Zero)
                    Marshal.FreeCoTaskMem(outputPtr);
            }
        }

        [DllImport("Powrprof.dll", SetLastError = true)]
        static extern bool SetSuspendState(byte hibernate, byte forceCritical, byte disableWakeEvent);

        public void SetSleep()
        {
            SetSuspendState(0, 0, 0);
        }

        public void SetHibernation()
        {
            SetSuspendState(1, 0, 0);
        }
    }
}