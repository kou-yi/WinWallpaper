using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace WallPaperTest
{
    class WPTool
    {

        private WPTool() { }

        public static void WallPaper(Window window)
        {
            FullScreenUtils.GoFullscreen(window);

            IntPtr i = new WindowInteropHelper(window).Handle;
            WinRef.SetParent(i, GetProgman());
        }

        public static IntPtr GetFolder()
        {
            IntPtr sdll = IntPtr.Zero;
            WinRef.EnumWindows(delegate (IntPtr tophandle, IntPtr topparamhandle)
            {
                IntPtr shelldll = WinRef.FindWindowEx(tophandle, IntPtr.Zero, "SHELLDLL_DefView", null);
                if (shelldll != IntPtr.Zero)
                {
                    sdll = shelldll;
                }
                return true;
            }, IntPtr.Zero);
            return WinRef.FindWindowEx(sdll, IntPtr.Zero, "SysListView32", null);
        }

        private static IntPtr GetProgman()
        {
            IntPtr windowHandle = WinRef.FindWindow("Progman", null);
            WinRef.SendMessageTimeout(windowHandle, 0x52c, new IntPtr(0), IntPtr.Zero, WinRef.SendMessageTimeoutFlags.SMTO_NORMAL, 0x3e8, out IntPtr zero);
            IntPtr workerw = IntPtr.Zero;
            WinRef.EnumWindows(delegate (IntPtr tophandle, IntPtr topparamhandle)
            {
                if (WinRef.FindWindowEx(tophandle, IntPtr.Zero, "SHELLDLL_DefView", null) != IntPtr.Zero)
                {
                    workerw = WinRef.FindWindowEx(IntPtr.Zero, tophandle, "WorkerW", null);
                }
                return true;
            }, IntPtr.Zero);
            WinRef.ShowWindow(workerw, WinRef.SW_HIDE);
            return windowHandle;
        }

        public static class WinRef
        {
            [DllImport("user32.dll")]
            public static extern IntPtr FindWindow(string className, string titleName);
            [DllImport("user32.dll")]
            public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string className, string title);
            [DllImport("user32.dll")]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);
            public delegate bool EnumWindowsProc(IntPtr hwnd, IntPtr lParam);
            [DllImport("user32.dll")]
            public static extern IntPtr SetParent(IntPtr hwndChild, IntPtr newParent);
            [DllImport("user32.dll")]
            public static extern bool ShowWindowAsync(IntPtr hWnd, int cmdShow);
            [DllImport("user32.dll")]
            public static extern bool ShowWindow(IntPtr hWnd, int cmdShow);
            public const int SW_SHOW = 5;
            public const int SW_HIDE = 0;
            public const int WS_SHOWNORMAL = 1;

            [DllImport("user32.dll")]
            public static extern void SetWindowPos(IntPtr hWnd, IntPtr hWndlnsertAfter, int x, int y, int cx, int cy, uint flag);
            public const int HWND_TOP = 0; // 在前面
            public const int HWND_BOTTOM = 1; // 在后面
            public const int HWND_TOPMOST = -1; // 在前面, 位于任何顶部窗口的前面
            public const int HWND_NOTOPMOST = -2; // 在前面, 位于其他顶部窗口的后面}

            [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            public static extern IntPtr SendMessageTimeout(IntPtr windowHandle, uint Msg, IntPtr wParam, IntPtr lParam, SendMessageTimeoutFlags flags, uint timeout, out IntPtr result);

            [Flags]
            public enum SendMessageTimeoutFlags : uint
            {
                SMTO_ABORTIFHUNG = 2,
                SMTO_BLOCK = 1,
                SMTO_ERRORONEXIT = 0x20,
                SMTO_NORMAL = 0,
                SMTO_NOTIMEOUTIFNOTHUNG = 8
            }

            [DllImport("user32.dll")]
            public static extern IntPtr WindowFromPoint(CPoint point);

            [DllImport("user32.dll")]
            public static extern bool GetCursorPos(out CPoint point);

            public struct CPoint
            {
                public int x;
                public int y;
            }
        }
    }
}