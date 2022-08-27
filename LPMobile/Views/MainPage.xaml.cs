// Copyright (c) All contributors. All rights reserved. Licensed under the MIT license.

using Arc.Views;
using LPMobile.ViewModels;

namespace LPMobile.Views;

public partial class MainPage : ContentPage
{
    public MainPage(MainViewModel vm, IViewService viewService)
    {
        this.InitializeComponent();
        this.BindingContext = vm;

        this.viewService = viewService;
    }

    private IViewService viewService;
}
