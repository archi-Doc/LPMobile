// Copyright (c) All contributors. All rights reserved. Licensed under the MIT license.

using System.Diagnostics;
using Arc.Unit;

namespace LPMobile.Views;

public partial class MainPage : ContentPage, IViewService
{
    public MainPage(ILogger<MainPage> logger, NetControl netControl)
    {
        this.InitializeComponent();
        // this.BindingContext = vm;

        this.logger = logger;
        this.netControl = netControl;
    }

    public bool DisplayAlert()
    {
        // MainThread.BeginInvokeOnMainThread(() => this.DisplayAlert("Question?", "Would you like to exit", "Yes", "No"));
        MainThread.InvokeOnMainThreadAsync(() => this.DisplayAlert("Question?", "Would you like to exit", "Yes", "No"));
        // this.DisplayAlert("Question?", "Would you like to exit", "Yes", "No").Wait();
        return true;
    }

    public async Task ExitAsync(bool confirmation)
    {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        MainThread.InvokeOnMainThreadAsync(async () =>
        {
            if (confirmation)
            {
                if (await this.DisplayAlert("Question?", "Would you like to exit", "Yes", "No") == false)
                {
                    return;
                }

                /*if (await this.DisplayAlert("Question?", "Would you like to exit", "Yes", "No") == false)
                {// Cancel
                    return;
                }*/
            }

            Application.Current?.CloseWindow(this.Window); // Microsoft.Maui.MauiWinUIApplication.Current.Exit();
        });
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
    }

    private async void OnExitButtonClicked(object sender, EventArgs e)
    {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        Task.Run(async () =>
        {
            await Task.Delay(1000);
            await this.ExitAsync(true);
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

        this.Title = "test2";

        SemanticScreenReader.Announce(this.CounterBtn.Text);
    }

    private async void OnSettingsButtonClicked(object sender, EventArgs e)
    {
        // this.Scale *= 1.2;
        await Shell.Current.GoToAsync("//settings");
    }

    private ILogger<MainPage> logger;
    private NetControl netControl;
    private int count;

    private void ContentPage_Appearing(object sender, EventArgs e)
    {
#if WINDOWS
        /*try
        {
            if (this.GetParentWindow().Handler?.PlatformView is { } nativeWindow)
            {
                IntPtr windowHandle = WinRT.Interop.WindowNative.GetWindowHandle(nativeWindow);
                Microsoft.UI.WindowId windowId = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(windowHandle);
                Microsoft.UI.Windowing.AppWindow appWindow = Microsoft.UI.Windowing.AppWindow.GetFromWindowId(windowId);
            }
        }
        catch
        {
        }*/
#endif
    }
}
