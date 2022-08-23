// Copyright (c) All contributors. All rights reserved. Licensed under the MIT license.

using Arc.Unit;
using LPMobile.Views;

namespace LPMobile;

public partial class App : Application
{
    public App(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
        this.InitializeComponent();

        this.MainPage = this.serviceProvider.GetRequiredService<AppShell>();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        var window = base.CreateWindow(activationState);

        window.Destroying += (s, e) =>
        {
            ThreadCore.Root.Terminate();
            ThreadCore.Root.WaitForTermination(-1); // Wait for the termination infinitely.
            this.serviceProvider.GetService<UnitLogger>()?.FlushAndTerminate();
            ThreadCore.Root.TerminationEvent.Set(); // The termination process is complete (#1).
        };

        window.Stopped += async (s, e) =>
        {// Exit1 (Window is still visible)
            /*if (this.serviceProvider.GetService<IViewService>() is { } viewService)
            {
                await viewService.ExitAsync(true);
                return;
            }*/
        };

        // window.Resumed

        return window;
    }

    private IServiceProvider serviceProvider;
}
