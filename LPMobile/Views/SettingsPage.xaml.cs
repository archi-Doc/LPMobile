// Copyright (c) All contributors. All rights reserved. Licensed under the MIT license.

using LPMobile.ViewModels;

namespace LPMobile.Views;

public partial class SettingsPage : ContentPage
{
    public SettingsPage(SettingsViewModel vm)
    {
        this.InitializeComponent();
        this.BindingContext = vm;
        this.viewModel = vm;
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        this.viewModel.OnNavigatedTo();
    }

    protected override void OnNavigatedFrom(NavigatedFromEventArgs args)
    {
        base.OnNavigatedFrom(args);
        this.viewModel.OnNavigatedFrom();
    }

    private SettingsViewModel viewModel;
}
