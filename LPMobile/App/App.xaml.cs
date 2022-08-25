// Copyright (c) All contributors. All rights reserved. Licensed under the MIT license.

using Arc.Unit;
using Arc.Views;
using LPMobile.Views;
using Tinyhand;

namespace LPMobile;

public partial class App : Application
{
    public App(IServiceProvider serviceProvider, AppData appData)
    {
        this.serviceProvider = serviceProvider;
        this.appData = appData;
        this.InitializeComponent();

        this.MainPage = this.serviceProvider.GetRequiredService<AppShell>();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        var window = base.CreateWindow(activationState);

        window.Activated += this.Window_Activated;
        window.Stopped += this.Window_Stopped;
        window.Resumed += this.Window_Resumed;
        window.Destroying += this.Window_Destroying;

        return window;
    }

    private void Window_Activated(object? sender, EventArgs e)
    {
        if (this.serviceProvider.GetService<IViewService>() is { } viewService)
        {
            viewService.SetFontScale(this.appData.Settings.FontScale);
        }
    }

    private void Window_Stopped(object? sender, EventArgs e)
    {// Exit1 (Window is still visible)
        try
        {
            if (this.serviceProvider.GetService<IViewService>() is { } viewService)
            {
                this.appData.Settings.FontScale = viewService.GetFontScale();
            }

            var bin = TinyhandSerializer.SerializeToUtf8(this.appData);
            Directory.CreateDirectory(FileSystem.Current.AppDataDirectory);
            File.WriteAllBytes(Path.Combine(FileSystem.Current.AppDataDirectory, AppConst.AppDataFile), bin);
        }
        catch
        {
        }
    }

    private void Window_Resumed(object? sender, EventArgs e)
    {
    }

    private void Window_Destroying(object? sender, EventArgs e)
    {
        ThreadCore.Root.Terminate();
        ThreadCore.Root.WaitForTermination(-1); // Wait for the termination infinitely.
        this.serviceProvider.GetService<UnitLogger>()?.FlushAndTerminate();
        ThreadCore.Root.TerminationEvent.Set(); // The termination process is complete (#1).
    }

    private IServiceProvider serviceProvider;
    private AppData appData;
}
