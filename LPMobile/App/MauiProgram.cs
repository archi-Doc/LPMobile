// Copyright (c) All contributors. All rights reserved. Licensed under the MIT license.

#pragma warning disable SA1210 // Using directives should be ordered alphabetically by namespace

global using System;
global using System.Threading;
global using System.Threading.Tasks;
global using Arc.Threading;
global using LP;
global using Netsphere;
using Arc.Unit;

namespace LPMobile;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        // UnitBuilder
        var builder = new NetControl.Builder().Configure(context =>
        {
            context.AddSingleton<App>();
            context.AddSingleton<MainPage>();
        });

        // Maui Builder
        var mauiBuilder = MauiApp.CreateBuilder();
        mauiBuilder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            })
            .ConfigureContainer(new UnitBuilderToServiceProviderFactory(builder));

        // Build MauiApp & Unit.
        var mauiApp = mauiBuilder.Build();

        var options = new LP.Data.NetsphereOptions();
        options.EnableAlternative = true;
        options.EnableTestFeatures = true;
        var param = new NetControl.Unit.Param(true, () => new ServerContext(), () => new CallContext(), "test", options, true);
        var unit = builder.GetBuiltUnit();
        unit.RunStandalone(param).Wait();

        return mauiApp;
    }
}
