// Copyright (c) All contributors. All rights reserved. Licensed under the MIT license.

namespace LPMobile.Views;

public partial class AppShell : Shell
{
    public AppShell(IViewService viewService)
    {
        this.InitializeComponent();

        this.viewService = viewService;
        // viewService.CurrentPage = this;
    }

    private IViewService viewService;
}
