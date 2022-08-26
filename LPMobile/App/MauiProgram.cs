// Copyright (c) All contributors. All rights reserved. Licensed under the MIT license.

#pragma warning disable SA1210 // Using directives should be ordered alphabetically by namespace

global using System;
global using System.Threading;
global using System.Threading.Tasks;
global using Arc.Threading;
global using LP;
global using Netsphere;
using Arc.Unit;
using Arc.Views;
using LP.Data;
using LPMobile.Views;
using Microsoft.Extensions.DependencyInjection;
using Tinyhand;

namespace LPMobile;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        // UnitBuilder
        var builder = new NetControl.Builder()
            .Preload(context =>
            {
                context.RootDirectory = FileSystem.Current.AppDataDirectory;
                context.DataDirectory = FileSystem.Current.AppDataDirectory;

                LoadStrings(context);
                LoadData(context);
            })
            .Configure(context =>
            {
                // App
                context.AddSingleton<App>();

                // Views
                context.Services.AddSingleton<IViewService, ViewServiceImpl>();
                context.AddSingleton<Views.AppShell>();
                context.AddSingleton<Views.MainPage>();
                context.AddSingleton<Views.SettingsPage>();

                // ViewModels
                context.AddSingleton<ViewModels.MainViewModel>();
                context.AddSingleton<ViewModels.SettingsViewModel>();
            })
            .SetupOptions<FileLoggerOptions>((context, options) =>
            {// FileLoggerOptions
                var logfile = "Logs/Log.txt";
                options.Path = Path.Combine(context.DataDirectory, logfile);
                options.MaxLogCapacity = 20;
            })
            .SetupOptions<ConsoleLoggerOptions>((context, options) =>
            {// ConsoleLoggerOptions
                options.Formatter.EnableColor = true;
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
        ServiceProvider = mauiApp.Services;

        // Run
        var options = new LP.Data.NetsphereOptions();
        var param = new NetControl.Unit.Param(false, () => new ServerContext(), () => new CallContext(), "test", options, true);
        var unit = builder.GetBuiltUnit();
        unit.RunStandalone(param).Wait();

        return mauiApp;
    }

    public static IServiceProvider ServiceProvider { get; private set; } = default!;

    private static void LoadStrings(IUnitPreloadContext context)
    {
        // HashedString
        try
        {
            HashedString.SetDefaultCulture(AppConst.DefaultCulture); // default culture

            var asm = System.Reflection.Assembly.GetExecutingAssembly();
            HashedString.LoadAssembly("en", asm, "Resources.Tinyhand.License.tinyhand");
            HashedString.LoadAssembly("en", asm, "Resources.Tinyhand.Strings-en.tinyhand");
            HashedString.LoadAssembly("ja", asm, "Resources.Tinyhand.Strings-ja.tinyhand");
        }
        catch
        {
        }
    }

    private static void LoadData(IUnitPreloadContext context)
    {
        AppData? appData = null;
        bool loadError = false;

        try
        {
            var data = File.ReadAllBytes(Path.Combine(context.DataDirectory, AppConst.AppDataFile));
            appData = TinyhandSerializer.DeserializeFromUtf8<AppData>(data);
        }
        catch
        {
            loadError = true;
        }

        if (appData == null)
        {
            appData = TinyhandSerializer.Reconstruct<AppData>();
        }

        appData.LoadError = loadError;

        context.SetOptions(appData);
        context.SetOptions(appData.Settings);

        try
        {
            HashedString.ChangeCulture(appData.Settings.Culture);
        }
        catch
        {
        }
    }
}
