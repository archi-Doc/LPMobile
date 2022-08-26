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
    public List<string> RawCultureList { get; private set; } = new List<string>() { "en", "ja" };

    [Link(AutoNotify = true)]
    private List<string> cultureList = new List<string>();

    [Link(AutoNotify = true)]
    private string testString = string.Empty;

    [Link(AutoNotify = true)]
    private string currentCulture = string.Empty;

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

    public void Appearing()
    {
        this.CurrentCultureValue = this.appData.Settings.Culture;
        this.CultureListValue = this.RawCultureList.Select(x => (string)CultureToStringConverter.Instance.Convert(x, null!, null!, null!)).ToList();
    }

    public void Disappearing(bool save)
    {
    }

    private IViewService viewService;
    private AppData appData;
}
