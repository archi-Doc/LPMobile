// Copyright (c) All contributors. All rights reserved. Licensed under the MIT license.

using System.Diagnostics;
using Arc.Unit;
using LP.Subcommands;

namespace LPMobile.Views;

public partial class MainPage : ContentPage, IViewService
{
    public MainPage(ILogger<MainPage> logger, NetControl netControl)
    {
        this.InitializeComponent();
        // this.BindingContext = vm;

        this.logger = logger;
        this.netControl = netControl;

        // this.Window.Deactivated += this.Window_Deactivated;
    }

    private void Window_Deactivated(object? sender, EventArgs e)
    {
    }

    public async Task ExitAsync(bool confirmation)
    {
        if (confirmation)
        {
            if (await this.DispatchIfRequired<bool>(() => this.DisplayAlert("Question?", "Would you like to exit", "Yes", "No")) == false)
            {
                return;
            }

            /*if (await this.DisplayAlert("Question?", "Would you like to exit", "Yes", "No") == false)
            {// Cancel
                return;
            }*/
        }

        Application.Current?.CloseWindow(this.Window);

#if WINDOWS
        // Microsoft.Maui.MauiWinUIApplication.Current.Exit();
#endif
    }

    private Task DispatchIfRequired(Func<Task> @delegate)
    {
        if (this.Dispatcher.IsDispatchRequired)
        {
            return this.Dispatcher.DispatchAsync(@delegate);
        }
        else
        {
            return @delegate();
        }
    }

    private Task<T> DispatchIfRequired<T>(Func<Task<T>> @delegate)
    {
        if (this.Dispatcher.IsDispatchRequired)
        {
            return this.Dispatcher.DispatchAsync(@delegate);
        }
        else
        {
            return @delegate();
        }
    }

    private async void OnExitButtonClicked(object sender, EventArgs e)
    {
        await this.ExitAsync(true);
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
        if (SubcommandService.TryParseNodeAddress(this.logger, node, out var nodeAddress))
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

        this.Title = "test2";

        SemanticScreenReader.Announce(this.CounterBtn.Text);
    }

    private ILogger<MainPage> logger;
    private NetControl netControl;
    private int count;
}
