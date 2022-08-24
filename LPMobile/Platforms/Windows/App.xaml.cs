﻿// Copyright (c) All contributors. All rights reserved. Licensed under the MIT license.

using LPMobile.Views;
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Tinyhand;
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

        int windowWidth = 900;
        int windowHeight = 600;
        Microsoft.Maui.Handlers.WindowHandler.Mapper.AppendToMapping(nameof(IWindow), (handler, view) =>
        {
            // var mauiWindow = handler.VirtualView;
            var nativeWindow = handler.PlatformView;
            nativeWindow.Activate();
            var windowHandle = WinRT.Interop.WindowNative.GetWindowHandle(nativeWindow);
            var windowId = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(windowHandle);
            var appWindow = Microsoft.UI.Windowing.AppWindow.GetFromWindowId(windowId);
            appWindow.Resize(new SizeInt32(windowWidth, windowHeight));

            appWindow.Closing += async (s, e) =>
            {
                if (MauiProgram.ServiceProvider.GetService<IViewService>() is { } viewService)
                {
                    e.Cancel = true;
                    await viewService.ExitAsync(true);
                }
            };

            appWindow.Title = HashedString.GetOrAlternative(Hashed.App.Name, AppInfo.Current.Name);
            /*if (MauiProgram.ServiceProvider.GetService<ViewServiceImpl>() is { } viewService)
            {
                viewService.SetTitleDelegate(() => appWindow.Title, x => appWindow.Title = x);
            }*/
        });
    }

    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

#if WINDOWS
    private static Mutex? mutex;
#endif
}
