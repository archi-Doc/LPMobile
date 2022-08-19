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

namespace LPMobile;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = new NetControl.Builder().Configure(context =>
        {
            context.AddSingleton<MainPage>();
        });

        var options = new LP.Data.NetsphereOptions();
        options.EnableAlternative = true;
        options.EnableTestFeatures = true;

        var mauiBuilder = MauiApp.CreateBuilder();
        mauiBuilder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            })
            .ConfigureContainer(new UnitBuilderToServiceProviderFactory(builder));

        var mauiApp = mauiBuilder.Build();

        var param = new NetControl.Unit.Param(true, () => new ServerContext(), () => new CallContext(), "test", options, true);
        var unit = mauiApp.Services.GetRequiredService<NetControl.Unit>();
        unit.RunStandalone(param).Wait();

        return mauiApp;
    }
}

public class UnitBuilderToServiceProviderFactory : IServiceProviderFactory<UnitBuilder>
{
    public UnitBuilderToServiceProviderFactory()
    {
    }

    public UnitBuilderToServiceProviderFactory(UnitBuilder builder)
    {
        this.builder = builder;
    }

    public UnitBuilder CreateBuilder(IServiceCollection services)
    {
        this.builder ??= new UnitBuilder();
        this.builder.Configure(context =>
        {
            foreach (var x in services)
            {
                context.Services.Add(x);
            }
        });

        return this.builder;
    }

    public IServiceProvider CreateServiceProvider(UnitBuilder builder)
    {
        var unit = builder.Build();
        return unit.Context.ServiceProvider;
    }

    private UnitBuilder? builder;
}
