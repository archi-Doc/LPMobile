// Copyright (c) All contributors. All rights reserved. Licensed under the MIT license.

using System.Diagnostics;
using System.Windows.Input;
using Arc.Views;
using LPMobile.Models;
using Microsoft.Maui.Controls;
using Tinyhand;
using ValueLink;

#pragma warning disable SA1201 // Elements should appear in the correct order

namespace LPMobile.ViewModels;

[ValueLinkObject]
public partial class MainViewModel
{
    public TestItem.GoshujinClass TestGoshujin { get; }

    public DateTime CurrentTime { get; private set; } = DateTime.Now;

    [Link(AutoNotify = true)]
    private bool hideDialogButton;

    private int number1;

    public int Number1
    {
        get => this.number1;
        set
        {
            this.SetProperty(ref this.number1, value);
            this.Number3Value = this.Number1 + this.Number2;
        }
    }

    private int number2;

    public int Number2
    {
        get => this.number2;
        set
        {
            this.SetProperty(ref this.number2, value);
            this.Number3Value = this.Number1 + this.Number2;
        }
    }

    [Link(AutoNotify = true)]
    private int number3;

    [Link(AutoNotify = true)]
    private int number4;

    private ICommand? exitCommand;

    public ICommand ExitCommand
    {
        get => this.exitCommand ??= new Command(async () =>
        {
            await Task.Delay(1000);
            await this.viewService.ExitAsync(true);
        });
    }

    private ICommand? addItemCommand;

    public ICommand AddItemCommand
    {
        get => this.addItemCommand ??= new Command(() =>
        {
            if (this.TestGoshujin.QueueChain.Count >= 5)
            {// Limits the number of objects.
                this.TestGoshujin.QueueChain.Peek().Goshujin = null;
            }

            var last = this.TestGoshujin.IdChain.Last;
            var id = last == null ? 0 : last.IdValue + 1;
            var item = new TestItem(id, DateTime.UtcNow);
            item.Goshujin = this.TestGoshujin;
        });
    }

    private ICommand? clearItemCommand;

    public ICommand ClearItemCommand
    {
        get => this.clearItemCommand ??= new Command(() =>
        {
            this.TestGoshujin.Clear();
        });
    }

    private ICommand? incrementListViewCommand;

    public ICommand IncrementListViewCommand
    {
        get => this.incrementListViewCommand ??= new Command(() =>
        {
            foreach (var x in this.TestGoshujin.ObservableChain.Where(x => x.Selection == 2))
            {
                x.IdValue++;
            }
        });
    }

    private ICommand? decrementListViewCommand;

    public ICommand DecrementListViewCommand
    {
        get => this.decrementListViewCommand ??= new Command(() =>
        {
            foreach (var x in this.TestGoshujin.ObservableChain.Where(x => x.Selection == 2))
            {
                if (x.IdValue > 0)
                {
                    x.IdValue--;
                }
            }
        });
    }

    [Link(AutoNotify = true)]
    private bool commandFlag = true;

    private ICommand? testCommand4;

    public ICommand TestCommand4
    {
        get => this.testCommand4 ??= new Command(async () =>
        { // execute
            this.HideDialogButtonValue = !this.HideDialogButtonValue;
            await Task.Delay(1000);
            this.CommandFlagValue = this.CommandFlagValue ? false : true;
            this.Number4Value++;

            // this.TestCommand.RaiseCanExecuteChanged(); // ObservesProperty(() => this.CommandFlag)
        });
    }

    private ICommand? switchCultureCommand;

    public ICommand SwitchCultureCommand
    {
        get => this.switchCultureCommand ??= new Command(() =>
        {
            if (this.appData.Settings.Culture == "ja")
            {
                this.appData.Settings.Culture = "en";
            }
            else
            {
                this.appData.Settings.Culture = "ja";
            }

            this.viewService.SwitchCulture(this.appData.Settings.Culture);
        });
    }

    private ICommand? settingsCommand;

    public ICommand SettingsCommand
    {
        get => this.settingsCommand ??= new Command(async () =>
        {
            await Shell.Current.GoToAsync("//settings"); // "settings" is currently not supported.
        });
    }

    private ICommand? openDataDirectoryCommand;

    public ICommand OpenDataDirectoryCommand
    {
        get => this.openDataDirectoryCommand ??= new Command(async () =>
        {
#if WINDOWS
        System.Diagnostics.Process.Start("Explorer.exe", FileSystem.Current.AppDataDirectory);
#endif
        });
    }

    private ICommand? pingCommand;

    public ICommand PingCommand
    {
        get => this.pingCommand ??= new Command(async () =>
        {
            /*
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
                        // this.TextLabel.Text = $"{response.ToString()}, {sw.ElapsedMilliseconds} ms";
                    }
                }
            }*/

            var scale = this.viewService.GetFontScale();
            scale *= 1.2d;
            this.viewService.SetFontScale(scale);
        });
    }

    public MainViewModel(IViewService viewService, AppData appData)
    {
        this.viewService = viewService;
        this.appData = appData;
        this.TestGoshujin = this.appData.TestItems;
    }

    private IViewService viewService;
    private AppData appData;
}
