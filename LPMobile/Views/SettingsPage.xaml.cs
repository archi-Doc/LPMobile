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

    private void ContentPage_Disappearing(object sender, EventArgs e)
    {
        this.viewModel.Exit(true);
    }

    private SettingsViewModel viewModel;
}
