// Copyright (c) All contributors. All rights reserved. Licensed under the MIT license.

using System.Diagnostics;
using Arc.Unit;
using Arc.Views;
using LPMobile.ViewModels;
using Tinyhand;

namespace LPMobile.Views;

public partial class MainPage : ContentPage
{
    public MainPage(MainViewModel vm, IViewService viewService, ILogger<MainPage> logger, NetControl netControl)
    {
        this.InitializeComponent();
        this.BindingContext = vm;

        this.viewService = viewService;
        this.logger = logger;
        this.netControl = netControl;
    }

    private async void OnExitButtonClicked(object sender, EventArgs e)
    {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        Task.Run(async () =>
        {
            await Task.Delay(1000);
            await this.viewService.ExitAsync(true);
        });
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
    }

    private async void OnOpenDataDirectoryButtonClicked(object sender, EventArgs e)
    {
#if WINDOWS
        System.Diagnostics.Process.Start("Explorer.exe", FileSystem.Current.AppDataDirectory);
#endif
    }

    private void OnCounterClicked(object sender, EventArgs e)
    {
        this.count++;
        if (this.count == 1)
        {
            this.CounterBtn.Text = $"Clicked {this.count} time";
        }
        else
        {
            this.CounterBtn.Text = $"Clicked {this.count} times";
        }

        var utf8 = Tinyhand.TinyhandSerializer.SerializeToUtf8(this.count);

        var node = this.TextEntry.Text ?? string.Empty;
        if (LP.Subcommands.SubcommandService.TryParseNodeAddress(this.logger, node, out var nodeAddress))
        {
            using (var terminal = this.netControl.Terminal.Create(nodeAddress))
            {
                var p = new PacketPing("mobile");
                var sw = Stopwatch.StartNew();
                var t = terminal.SendAndReceiveAsync<PacketPing, PacketPingResponse>(p);
                if (t.Result.Result == NetResult.Success && t.Result.Value is { } response)
                {
                    this.TextLabel.Text = $"{response.ToString()}, {sw.ElapsedMilliseconds} ms";
                }
            }
        }

        SemanticScreenReader.Announce(this.CounterBtn.Text);
    }

    private async void OnSettingsButtonClicked(object sender, EventArgs e)
    {
        /*var scale = this.viewService.GetFontScale();
        scale *= 1.2d;
        this.viewService.SetFontScale(scale);*/

        if (HashedString.CurrentCultureName == "en-US")
        {
            this.viewService.SwitchCulture("ja");
        }
        else
        {
            this.viewService.SwitchCulture("en");
        }

        // await Shell.Current.GoToAsync("//settings"); // "settings" is currently not supported.
    }

    private IViewService viewService;
    private ILogger<MainPage> logger;
    private NetControl netControl;
    private int count;

    private void ContentPage_Appearing(object sender, EventArgs e)
    {
        // this.
    }
}
