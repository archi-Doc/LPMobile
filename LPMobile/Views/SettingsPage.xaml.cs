// Copyright (c) All contributors. All rights reserved. Licensed under the MIT license.

using System.Diagnostics;
using Arc.Unit;

namespace LPMobile.Views;

public partial class SettingsPage : ContentPage
{
    public SettingsPage(IViewService viewService)
    {
        this.InitializeComponent();

        this.viewService = viewService;
    }

    public void OnTapGestureRecognizerTapped(object sender, EventArgs args)
    {
        // Handle the tap
    }

    private async void OnExitButtonClicked(object sender, EventArgs e)
    {
        await this.viewService.ExitAsync(true);
    }

    private IViewService viewService;
}
