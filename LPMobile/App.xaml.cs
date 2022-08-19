// Copyright (c) All contributors. All rights reserved. Licensed under the MIT license.

global using System;
global using System.Threading;
global using System.Threading.Tasks;
global using Arc.Threading;
global using CrossChannel;
global using LP;
global using Netsphere;
using Arc.Unit;
using Microsoft.Extensions.DependencyInjection;
using SimpleCommandLine;

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously

namespace LPMobile;

public partial class App : Application
{
    public App()
    {
        this.InitializeComponent();
        this.InitializeLP();

        this.MainPage = new AppShell();
    }

    private void InitializeLP()
    {
        var builder = new NetControl.Builder();
        var options = new LP.Data.NetsphereOptions();
        options.EnableAlternative = true;
        options.EnableTestFeatures = true;

        var unit = builder.Build();
        var param = new NetControl.Unit.Param(true, () => new ServerContext(), () => new CallContext(), "test", options, true);
        unit.RunStandalone(param).Wait();
    }
}
