// Copyright (c) All contributors. All rights reserved. Licensed under the MIT license.

using Arc.Views;

namespace LPMobile.Views;
using LPMobile.ViewModels;

public partial class AppShell : Shell
{
    public AppShell(IViewService viewService, MainViewModel viewModel)
    {
        this.InitializeComponent();
        this.BindingContext = viewModel;
        this.viewService = viewService;
        // viewService.CurrentPage = this;
    }

    private IViewService viewService;
}
