// Copyright (c) All contributors. All rights reserved. Licensed under the MIT license.

using System.Windows.Input;
using Arc.Views;
using LPMobile.Models;
using Tinyhand;
using ValueLink;

#pragma warning disable SA1201 // Elements should appear in the correct order

namespace LPMobile.ViewModels;

[ValueLinkObject]
public partial class SettingsViewModel
{
    public List<int> CultureList { get; private set; } = new() { 0, 1, };

    public string AppLicense { get; }

    [Link(AutoNotify = true)]
    private string testString = string.Empty;

    [Link(AutoNotify = true)]
    private int cultureIndex;

    [Link(AutoNotify = true)]
    private ulong currentCulture;

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

        this.AppLicense = HashedString.Get("App.License");
    }

    public void OnNavigatedTo()
    {
        this.CurrentCultureValue = Converters.CultureStringToId(this.appData.Settings.Culture);
        this.CultureIndexValue = Converters.CultureStringToIndex(this.appData.Settings.Culture);
    }

    public void Appearing()
    {
        // this.CurrentCultureValue = Converters.CultureStringToId(this.appData.Settings.Culture);
    }

    public void OnNavigatedFrom()
    {
        var culture = Converters.CultureIndexToString(this.CultureIndexValue);
        if (this.appData.Settings.Culture != culture)
        {
            this.appData.Settings.Culture = culture;
            this.viewService.SwitchCulture(culture);
        }
    }

    private IViewService viewService;
    private AppData appData;
}
