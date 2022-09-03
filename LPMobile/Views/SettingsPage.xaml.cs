// Copyright (c) All contributors. All rights reserved. Licensed under the MIT license.

using LPMobile.ViewModels;

namespace LPMobile.Views;

public partial class SettingsPage : ContentPage
{
    public SettingsPage(SettingsViewModel viewModel)
    {
        this.InitializeComponent();

        this.BindingContext = viewModel;
        this.viewModel = viewModel;
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        this.viewModel.OnNavigatedTo();
        base.OnNavigatedTo(args);
    }

    protected override void OnNavigatedFrom(NavigatedFromEventArgs args)
    {
        this.viewModel.OnNavigatedFrom();
        base.OnNavigatedFrom(args);
    }

    protected override bool OnBackButtonPressed()
    {
        this.viewModel.BackCommand.Execute(null);
        return true;
    }

    private SettingsViewModel viewModel;

    private void ScalingPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.viewModel?.OnScaleChanged();
    }
}
