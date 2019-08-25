using CefSharp;
using CefSharp.Wpf;
using System;
using System.Threading;
using System.Windows;
using System.Windows.Forms;

namespace WallPaperTest
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {

        private MouseHook hook = new MouseHook();
        public MainWindow()
        {
            InitializeComponent();
            browser.LifeSpanHandler = new LifeSpanHandler();
            hook.Start();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            WPTool.WallPaper(this);
            browser.Loaded += BrowserLoaded;
        }

        private void BrowserLoaded(object sender, EventArgs args)
        {
            IntPtr folder = WPTool.GetFolder();
            hook.OnMouseActivity += delegate (object hs, MouseEventArgs he)
            {
                WPTool.WinRef.GetCursorPos(out WPTool.WinRef.CPoint p);
                IntPtr cursor = WPTool.WinRef.WindowFromPoint(p);
                var host = browser.GetBrowser().GetHost();
                switch (he.Button)
                {
                    case MouseButtons.Left:
                        if (cursor == folder)
                        {
                            host.SendMouseClickEvent(new CefSharp.MouseEvent(he.X, he.Y, CefSharp.CefEventFlags.None), CefSharp.MouseButtonType.Left, false, he.Clicks);
                            Thread.Sleep(3);
                            host.SendMouseClickEvent(new CefSharp.MouseEvent(he.X, he.Y, CefSharp.CefEventFlags.None), CefSharp.MouseButtonType.Left, true, he.Clicks);
                        }
                        break;
                    case MouseButtons.Right:
                        host.SendMouseClickEvent(new CefSharp.MouseEvent(he.X, he.Y, CefSharp.CefEventFlags.None), CefSharp.MouseButtonType.Right, false, he.Clicks);
                        Thread.Sleep(3);
                        host.SendMouseClickEvent(new CefSharp.MouseEvent(he.X, he.Y, CefSharp.CefEventFlags.None), CefSharp.MouseButtonType.Right, true, he.Clicks);
                        break;
                    case MouseButtons.None:
                        if (cursor == folder)
                        {
                            host.SendMouseMoveEvent(new CefSharp.MouseEvent(he.X, he.Y, CefSharp.CefEventFlags.None), false);
                        }
                        break;

                }
            };
        }

        private class LifeSpanHandler : ILifeSpanHandler
        {
            public bool DoClose(IWebBrowser chromiumWebBrowser, IBrowser browser)
            {
                return false;
            }

            public void OnAfterCreated(IWebBrowser chromiumWebBrowser, IBrowser browser)
            {
            }

            public void OnBeforeClose(IWebBrowser chromiumWebBrowser, IBrowser browser)
            {
            }

            public bool OnBeforePopup(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, string targetUrl, string targetFrameName, WindowOpenDisposition targetDisposition, bool userGesture, IPopupFeatures popupFeatures, IWindowInfo windowInfo, IBrowserSettings browserSettings, ref bool noJavascriptAccess, out IWebBrowser newBrowser)
            {
                newBrowser = null;
                ((ChromiumWebBrowser)chromiumWebBrowser).Load(targetUrl);
                return true;
            }
        }
    }
}
