// Copyright (c) All contributors. All rights reserved. Licensed under the MIT license.

using Arc.Views;
using LPMobile.ViewModels;

namespace LPMobile.Views;

public partial class MainPage : ContentPage
{
    public MainPage(IServiceProvider serviceProvider, MainViewModel viewModel, IViewService viewService, AppData appData)
    {
        this.InitializeComponent();
        this.BindingContext = viewModel;

        this.serviceProvider = serviceProvider;
        this.viewService = viewService;
        this.appData = appData;
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        this.viewService.SetFontScale(this.appData.Settings.FontScale);
        base.OnNavigatedTo(args);
    }

    private IServiceProvider serviceProvider;
    private IViewService viewService;
    private AppData appData;

    /*private async void Button_Clicked(object sender, EventArgs e)
    {
        await this.Navigation.PushAsync(this.serviceProvider.GetRequiredService<SettingsPage>());
    }*/
}
