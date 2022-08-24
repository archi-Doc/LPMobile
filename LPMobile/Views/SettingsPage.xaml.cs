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

    private async void OnExitButtonClicked(object sender, EventArgs e)
    {
        var scale = this.viewService.GetFontScale();
        scale *= 1.2d;
        this.viewService.SetFontScale(scale);
        // await this.viewService.ExitAsync(true);
    }

    private IViewService viewService;
}
