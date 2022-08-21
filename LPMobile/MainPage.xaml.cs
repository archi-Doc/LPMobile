// Copyright (c) All contributors. All rights reserved. Licensed under the MIT license.

using System.Diagnostics;
using Arc.Unit;
using LP.Subcommands;

namespace LPMobile;

public partial class MainPage : ContentPage
{
    public MainPage(ILogger<MainPage> logger, NetControl netControl)
    {
        this.InitializeComponent();

        this.logger = logger;
        this.netControl = netControl;
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

        SemanticScreenReader.Announce(this.CounterBtn.Text);
    }

    private ILogger<MainPage> logger;
    private NetControl netControl;
    private int count;
}
