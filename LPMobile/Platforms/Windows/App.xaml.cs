// Copyright (c) All contributors. All rights reserved. Licensed under the MIT license.

using Microsoft.UI;
using Microsoft.UI.Windowing;
using Windows.Graphics;

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
        App.mutex = new Mutex(true, AppConst.MutexName, out var createdNew);
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

        int WindowWidth = 900;
        int WindowHeight = 600;
        Microsoft.Maui.Handlers.WindowHandler.Mapper.AppendToMapping(nameof(IWindow), (handler, view) =>
        {
            var mauiWindow = handler.VirtualView;
            var nativeWindow = handler.PlatformView;
            nativeWindow.Activate();
            IntPtr windowHandle = WinRT.Interop.WindowNative.GetWindowHandle(nativeWindow);
            WindowId windowId = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(windowHandle);
            AppWindow appWindow = Microsoft.UI.Windowing.AppWindow.GetFromWindowId(windowId);
            appWindow.Resize(new SizeInt32(WindowWidth, WindowHeight));
            // appWindow.Title = "test";
        });
    }

    protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
    {
        base.OnLaunched(args);
    }

    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

#if WINDOWS
    private static Mutex? mutex;
#endif
}
