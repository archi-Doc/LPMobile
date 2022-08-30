// Copyright (c) All contributors. All rights reserved. Licensed under the MIT license.

using System.Windows.Input;
using Arc.Views;
using LPMobile.Models;
using Tinyhand;
using Tinyhand.IO;
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

    private ICommand? backCommand;

    public ICommand BackCommand
    {
        get => this.backCommand ??= new Command(async () =>
        {
            await Shell.Current.GoToAsync("//main");
        });
    }

    private ICommand? defaultCommand;

    public ICommand DefaultCommand
    {
        get => this.defaultCommand ??= new Command(async () =>
        {
            if (await this.viewService.DisplayYesOrNo(0, Hashed.Dialog.Default) == true)
            {
                RestoreDefault();
            }

            void RestoreDefault()
            {
                var b = TinyhandSerializer.Serialize(TinyhandSerializer.Reconstruct<AppSettings>());
                var r = new TinyhandReader(b);
                this.appData.Settings.Deserialize(ref r, TinyhandSerializerOptions.Standard);

                this.OnNavigatedTo();
            }
        });
    }

    public SettingsViewModel(IViewService viewService, AppData appData)
    {
        this.viewService = viewService;
        this.appData = appData;

        this.AppLicense = HashedString.Get(Hashed.App.Name) + " " + HashedString.Get("License.App");
    }

    public void OnNavigatedTo()
    {
        this.viewService.SetFontScale(this.appData.Settings.FontScale);
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
            this.viewService.ChangeCulture(culture);
        }
    }

    private IViewService viewService;
    private AppData appData;
}
