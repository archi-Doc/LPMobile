using System.Diagnostics;
using Arc.Unit;
using LP.Subcommands;

namespace LPMobile;

public partial class MainPage : ContentPage
{
    int count = 0;

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
            this.CounterBtn.Text = $"Clicked {this.count} time";
        else
            this.CounterBtn.Text = $"Clicked {this.count} times";

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

        /*if (!SubcommandService.TryParseNodeAddress(this.logger, options.Node, out var node))
        {
            return;
        }*/

        SemanticScreenReader.Announce(this.CounterBtn.Text);
    }

    private ILogger<MainPage> logger;
    private NetControl netControl;
}
