// Copyright (c) All contributors. All rights reserved. Licensed under the MIT license.

using Microsoft.UI.Xaml;
using Tinyhand;

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
        // Prevents multiple instances.
        var mutex = new System.Threading.Mutex(false, AppConst.MutexName);
        if (!mutex.WaitOne(0, false))
        {
            mutex.Close(); // Release mutex.

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

            return; // Exit.
        }

        this.InitializeComponent();
    }

    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
}
