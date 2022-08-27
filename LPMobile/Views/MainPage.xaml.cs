// Copyright (c) All contributors. All rights reserved. Licensed under the MIT license.

using Arc.Views;
using LPMobile.ViewModels;

namespace LPMobile.Views;

public partial class MainPage : ContentPage
{
    public MainPage(IServiceProvider serviceProvider, MainViewModel vm, IViewService viewService)
    {
        this.InitializeComponent();
        this.BindingContext = vm;

        this.serviceProvider = serviceProvider;
        this.viewService = viewService;
    }

    private IServiceProvider serviceProvider;
    private IViewService viewService;

    /*private async void Button_Clicked(object sender, EventArgs e)
    {
        await this.Navigation.PushAsync(this.serviceProvider.GetRequiredService<SettingsPage>());
    }*/
}
