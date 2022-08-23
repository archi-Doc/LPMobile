// Copyright (c) All contributors. All rights reserved. Licensed under the MIT license.

namespace LPMobile.Views;

internal class ViewServiceImpl : IViewService
{
    public Page CurrentPage
    {
        get => this.currentPage;
        set => this.currentPage = value;
    }

    public bool DisplayAlert()
    {
        throw new NotImplementedException();
    }

    public async Task ExitAsync(bool confirmation)
    {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        MainThread.InvokeOnMainThreadAsync(async () =>
        {
            if (confirmation)
            {
                if (await this.currentPage.DisplayAlert("Question?", "Would you like to exit", "Yes", "No") == false)
                {
                    return;
                }
            }

            Application.Current?.CloseWindow(this.currentPage.Window); // Microsoft.Maui.MauiWinUIApplication.Current.Exit();
        });
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
    }

    private Page currentPage = default!;
}
