Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports Task1
Imports Task1.Models

<TestClass()> Public Class UnitTest1
    Private ReadOnly _batteryManager As BatteryManager

    Sub New ()
        _batteryManager = New BatteryManager()
    End Sub

    <TestMethod()> Public Sub TestLastSleepTime()
        Dim lastSleepTime As TimeSpan = _batteryManager.GetLastSleepTime()
        Assert.AreNotEqual(TimeSpan.Zero, lastSleepTime)
    End Sub

    <TestMethod()> Public Sub TestLastWakeTime()
        Dim lastWakeTime As TimeSpan = _batteryManager.GetLastWakeTime()
        Assert.AreNotEqual(TimeSpan.Zero, lastWakeTime)
    End Sub

    <TestMethod()> Public Sub TestGetSystemBatteryState()
        Dim state As SystemBatteryState = _batteryManager.GetSystemBatteryState()

        Assert.AreEqual(CByte(1), state.AcOnLine)
        Assert.AreEqual(CByte(0), state.BatteryPresent)
    End Sub

    <TestMethod()> Public Sub TestGetSystemPowerInformation()
        Dim info As SystemPowerInformation = _batteryManager.GetSystemPowerInformation()

        Assert.AreNotEqual(CUInt(0), info.TimeRemaining)
    End Sub
End Class