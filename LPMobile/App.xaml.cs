// Copyright (c) All contributors. All rights reserved. Licensed under the MIT license.

using Arc.Unit;

namespace LPMobile;

public partial class App : Application
{
    public App()
    {
        this.InitializeComponent();

        this.MainPage = new AppShell();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        var window = base.CreateWindow(activationState);
        window.Destroying += (s, e) =>
        {
            ThreadCore.Root.Terminate();
            ThreadCore.Root.WaitForTermination(-1); // Wait for the termination infinitely.
            // unit.Context.ServiceProvider.GetService<UnitLogger>()?.FlushAndTerminate();
            ThreadCore.Root.TerminationEvent.Set(); // The termination process is complete (#1).
        };

        return window;
    }
}
