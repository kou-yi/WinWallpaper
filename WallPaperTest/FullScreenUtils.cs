using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;

namespace WallPaperTest
{
    public static class FullScreenUtils
    {
        private static Window _fullWindow;
        private static WindowState _windowState;
        private static WindowStyle _windowStyle;
        private static bool _windowTopMost;
        private static ResizeMode _windowResizeMode;
        private static Rect _windowRect;
        public static void GoFullscreen(this Window window)
        {
            if (window.IsFullscreen()) return;
            _windowState = window.WindowState;
            _windowStyle = window.WindowStyle;
            _windowTopMost = window.Topmost;
            _windowResizeMode = window.ResizeMode;
            _windowRect.X = window.Left;
            _windowRect.Y = window.Top;
            _windowRect.Width = window.Width;
            _windowRect.Height = window.Height;
            window.WindowState = WindowState.Normal;
            window.WindowStyle = WindowStyle.None;
            window.ResizeMode = ResizeMode.NoResize;
            window.Topmost = true;
            IntPtr handle = new WindowInteropHelper(window).Handle;
            Screen screen = Screen.FromHandle(handle);
            window.Left = 0;
            window.Top = 0;
            window.Width = screen.Bounds.Width;
            window.Height = screen.Bounds.Height;
            window.WindowState = WindowState.Maximized;
            window.Activated += new EventHandler(window_Activated);
            window.Deactivated += new EventHandler(window_Deactivated);
            _fullWindow = window;
        }
        static void window_Deactivated(object sender, EventArgs e)
        {
            var window = sender as Window;
            window.Topmost = false;
        }
        static void window_Activated(object sender, EventArgs e)
        {
            Window window = sender as Window;
            window.Topmost = true;
        }
        public static void ExitFullscreen(this Window window)
        {
            if (!window.IsFullscreen()) return;
            window.Topmost = _windowTopMost;
            window.WindowStyle = _windowStyle;
            window.ResizeMode = ResizeMode.CanResize;
            window.Left = _windowRect.Left;
            window.Width = _windowRect.Width;
            window.Top = _windowRect.Top;
            window.Height = _windowRect.Height;
            window.WindowState = _windowState;
            window.ResizeMode = _windowResizeMode;
            window.Activated -= window_Activated;
            window.Deactivated -= window_Deactivated;
            _fullWindow = null;
        }
        public static bool IsFullscreen(this Window window)
        {
            if (window == null)
            {
                throw new ArgumentNullException("window");
            }
            return _fullWindow == window;
        }
    }
}
