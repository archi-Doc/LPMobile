using Arc.Unit;
using LP.Subcommands;

namespace LPMobile;

public partial class MainPage : ContentPage
{
    int count = 0;

    public MainPage(ILogger<MainPage> logger)
    {
        this.InitializeComponent();
    }

    private void OnCounterClicked(object sender, EventArgs e)
    {
        this.count++;

        if (this.count == 1)
            this.CounterBtn.Text = $"Clicked {this.count} time";
        else
            this.CounterBtn.Text = $"Clicked {this.count} times";

        this.TextLabel.Text = "test";

        /*if (!SubcommandService.TryParseNodeAddress(this.logger, options.Node, out var node))
        {
            return;
        }*/

        SemanticScreenReader.Announce(this.CounterBtn.Text);
    }
}
