// Copyright (c) All contributors. All rights reserved. Licensed under the MIT license.

using Arc.WinAPI;
using LPMobile.Views;
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Tinyhand;
using Windows.Graphics;
using Windows.Storage;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace LPMobile.WinUI;

/// <summary>
/// Provides application-specific behavior to supplement the default Application class.
/// </summary>
public partial class App : MauiWinUIApplication
{
    public App()
    {
#if WINDOWS
        // Prevents multiple instances.
        App.mutex = new Mutex(true, AppInfo.Current.PackageName, out var createdNew);
        if (!createdNew)
        {
            var prevProcess = Arc.WinAPI.Methods.GetPreviousProcess();
            if (prevProcess != null)
            {
                var handle = prevProcess.MainWindowHandle; // The window handle that associated with the previous process.
                if (handle == IntPtr.Zero)
                {
                    handle = Arc.WinAPI.Methods.GetWindowHandle(prevProcess.Id, string.Empty); // Get handle.
                }

                if (handle != IntPtr.Zero)
                {
                    Arc.WinAPI.Methods.ActivateWindow(handle);
                }
            }

            throw new Exception();
        }
#endif

        this.InitializeComponent();

        Microsoft.Maui.Handlers.WindowHandler.Mapper.AppendToMapping(nameof(IWindow), (handler, view) =>
        {
            var mauiWindow = handler.VirtualView;
            var nativeWindow = handler.PlatformView;
            nativeWindow.Activate();
            var windowHandle = WinRT.Interop.WindowNative.GetWindowHandle(nativeWindow);
            var windowId = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(windowHandle);
            var appWindow = Microsoft.UI.Windowing.AppWindow.GetFromWindowId(windowId);

            var appData = handler.MauiContext?.Services.GetService<AppData>();
            if (appData?.LoadError == false)
            {
                var hmonitor = Arc.WinAPI.Methods.MonitorFromWindow(windowHandle, MonitorDefaultTo.MONITOR_DEFAULTTONEAREST);
                /*uint dpiX = 96;
                uint dpiY = 96;
                Methods.GetDpiForMonitor(hmonitor, Arc.WinAPI.MonitorDpiType.Default, ref dpiX, ref dpiY);*/
                var monitorInfo = new Arc.WinAPI.MONITORINFOEX();
                Methods.GetMonitorInfo(hmonitor, monitorInfo);

                // Limit window position and size.
                var window = appData.Settings.AppWindow;
                if (window.X < monitorInfo.rcWork.Left)
                {
                    window.X = monitorInfo.rcWork.Left;
                }

                if (window.Y < monitorInfo.rcWork.Top)
                {
                    window.Y = monitorInfo.rcWork.Top;
                }

                var maxWidth = monitorInfo.rcWork.Right - monitorInfo.rcWork.Left;
                if (window.Width > maxWidth)
                {
                    window.Width = maxWidth;
                }

                var maxHeight = monitorInfo.rcWork.Bottom - monitorInfo.rcWork.Top;
                if (window.Height > maxHeight)
                {
                    window.Height = maxHeight;
                }

                var maxX = monitorInfo.rcWork.Right - window.Width;
                if (window.X > maxX)
                {
                    window.X = maxX;
                }

                var maxY = monitorInfo.rcWork.Bottom - window.Height;
                if (window.Y > maxY)
                {
                    window.Y = maxY;
                }

                var r = default(RectInt32);
                r.X = window.X;
                r.Y = window.Y;
                r.Width = window.Width;
                r.Height = window.Height;
                appWindow.MoveAndResize(r);
                if (window.IsMaximized && appWindow.Presenter is OverlappedPresenter p)
                {
                    p.Maximize();
                }
            }

            appWindow.Closing += async (s, e) =>
            {
                if (MauiProgram.ServiceProvider.GetService<IViewService>() is { } viewService)
                {
                    e.Cancel = true;
                    await viewService.ExitAsync(true);
                }
            };

            appWindow.Changed += (sender, args) =>
            {
                if (args.DidPositionChange || args.DidSizeChange)
                {
                    if (handler.MauiContext?.Services.GetService<AppSettings>() is { } appSettings &&
                    sender.Presenter is OverlappedPresenter presenter)
                    {
                        appSettings.AppWindow.IsMaximized = presenter.State == OverlappedPresenterState.Maximized;
                        if (!appSettings.AppWindow.IsMaximized)
                        {
                            appSettings.AppWindow.X = sender.Position.X;
                            appSettings.AppWindow.Y = sender.Position.Y;
                            appSettings.AppWindow.Width = sender.Size.Width;
                            appSettings.AppWindow.Height = sender.Size.Height;
                        }
                    }
                }
            };

            appWindow.Title = HashedString.GetOrAlternative(Hashed.App.Name, AppInfo.Current.Name);
        });
    }

    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

#if WINDOWS
    private static Mutex? mutex;
#endif
}
