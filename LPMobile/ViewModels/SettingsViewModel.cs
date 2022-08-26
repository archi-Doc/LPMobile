// Copyright (c) All contributors. All rights reserved. Licensed under the MIT license.

using System.Windows.Input;
using Arc.Views;
using LPMobile.Models;
using ValueLink;

#pragma warning disable SA1201 // Elements should appear in the correct order

namespace LPMobile.ViewModels;

[ValueLinkObject]
public partial class SettingsViewModel
{
    [Link(AutoNotify = true)]
    private string testString = string.Empty;

    private ICommand? exitCommand;

    public ICommand ExitCommand
    {
        get => this.exitCommand ??= new Command(async () =>
        {
            await Task.Delay(1000);
            await this.viewService.ExitAsync(true);
        });
    }

    public SettingsViewModel(IViewService viewService, AppData appData)
    {
        this.viewService = viewService;
        this.appData = appData;
    }

    public void Exit(bool save)
    {
    }

    private IViewService viewService;
    private AppData appData;
}
