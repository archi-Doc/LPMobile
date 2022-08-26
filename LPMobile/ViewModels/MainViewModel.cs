// Copyright (c) All contributors. All rights reserved. Licensed under the MIT license.

using System.Windows.Input;
using Arc.Views;
using LPMobile.Models;
using Microsoft.Maui.Controls;
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
        get
        {
            return this.exitCommand ??= new Command(async () =>
            {
                await Task.Delay(1000);
                await this.viewService.ExitAsync(true);
            });
        }
    }

    private ICommand? commandAddItem;

    public ICommand CommandAddItem
    {
        get
        {
            return this.commandAddItem ??= new Command(() =>
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
    }

    private ICommand? commandClearItem;

    public ICommand CommandClearItem
    {
        get
        {
            return this.commandClearItem ??= new Command(() =>
            {
                this.TestGoshujin.Clear();
            });
        }
    }

    private ICommand? commandListViewIncrement;

    public ICommand CommandListViewIncrement
    {
        get
        {
            return this.commandListViewIncrement ??= new Command(() =>
            {
                foreach (var x in this.TestGoshujin.ObservableChain.Where(x => x.Selection == 2))
                {
                    x.IdValue++;
                }
            });
        }
    }

    private ICommand? commandListViewDecrement;

    public ICommand CommandListViewDecrement
    {
        get
        {
            return this.commandListViewDecrement ??= new Command(() =>
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
    }

    [Link(AutoNotify = true)]
    private bool commandFlag = true;

    private ICommand? testCommand4;

    public ICommand TestCommand4
    {
        get
        {
            return this.testCommand4 ??= new Command(async () =>
            { // execute
                this.HideDialogButtonValue = !this.HideDialogButtonValue;
                await Task.Delay(1000);
                this.CommandFlagValue = this.CommandFlagValue ? false : true;
                this.Number4Value++;

                // this.TestCommand.RaiseCanExecuteChanged(); // ObservesProperty(() => this.CommandFlag)
            });
        }
    }

    public DateTime Time1 { get; private set; } = DateTime.Now;

    public MainViewModel(IViewService viewService, AppData appData)
    {
        this.viewService = viewService;
        this.appData = appData;
        this.TestGoshujin = this.appData.TestItems;
    }

    private IViewService viewService;
    private AppData appData;
}
