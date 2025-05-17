using System;
using WinRT.Interop;
using Redo.Libs;
using System.Reflection.Metadata;
using static Redo.Libs.WindowAPI;
using Microsoft.UI.Xaml;
using System.Collections.Generic;

namespace Redo.Helpers
{
    internal class WindowHelper
    {
        static private List<Window> _activeWindows = new List<Window>();
        public static void HideWindow(object Window)
        {
            // Get the native window handle (HWND) from the WinUI 3 window
            IntPtr hWnd = WindowNative.GetWindowHandle(Window);

            // Get current extended window styles
            uint exStyle = WindowAPI.GetWindowLong(hWnd, WindowAPI.GWL_EXSTYLE);

            // Remove WS_EX_APPWINDOW to hide from taskbar and Alt+Tab
            // Add WS_EX_TOOLWINDOW to make it a tool window (hidden from taskbar and Alt+Tab)
            exStyle &= ~WindowAPI.WS_EX_APPWINDOW;
            exStyle |= WindowAPI.WS_EX_TOOLWINDOW;
            exStyle |= WindowAPI.WS_EX_NOACTIVATE;

            // Apply the new extended window styles
            WindowAPI.SetWindowLong(hWnd, WindowAPI.GWL_EXSTYLE, exStyle);
        }

        private static WindowAPI.ITaskbarList3 taskbarInstance = (ITaskbarList3)new TaskbarInstance();
        private static bool taskbarSupported = Environment.OSVersion.Version >= new Version(6, 1);
        public static void SetState(IntPtr windowHandle, TaskbarStates taskbarState)
        {
            if (taskbarSupported) taskbarInstance.SetProgressState(windowHandle, taskbarState);
        }
        public static void SetValue(object Window, double progressValue, double progressMax)
        {
            // Get the native window handle (HWND) from the WinUI 3 window
            IntPtr hWnd = WindowNative.GetWindowHandle(Window);

            if (taskbarSupported) taskbarInstance.SetProgressValue(hWnd, (ulong)progressValue, (ulong)progressMax);
        }

    }
}
