// Copyright (c) All contributors. All rights reserved. Licensed under the MIT license.

#pragma warning disable SA1210 // Using directives should be ordered alphabetically by namespace

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
        // UnitBuilder
        var builder = new NetControl.Builder().Configure(context =>
        {
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

        // Build Maui & Unit.
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

/// <summary>
/// Creates an <see cref="IServiceProviderFactory{UnitBuilder}"/> instance from <see cref="UnitBuilder"/> instance.
/// </summary>
public class UnitBuilderToServiceProviderFactory : IServiceProviderFactory<UnitBuilder>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UnitBuilderToServiceProviderFactory"/> class.
    /// </summary>
    /// <param name="builder">The underlying <see cref="UnitBuilder"/> instance that creates <see cref="UnitBuilderToServiceProviderFactory"/>.</param>
    public UnitBuilderToServiceProviderFactory(UnitBuilder builder)
    {
        this.builder = builder;
    }

    /// <inheritdoc/>
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

    /// <inheritdoc/>
    public IServiceProvider CreateServiceProvider(UnitBuilder builder)
    {
        var unit = builder.Build();
        return unit.Context.ServiceProvider;
    }

    private UnitBuilder? builder;
}
