// Copyright (c) All contributors. All rights reserved. Licensed under the MIT license.

namespace LPMobile.Views;

internal class ViewServiceImpl : IViewService
{
    /*public Page CurrentPage
    {
        get => this.currentPage;
        set => this.currentPage = value;
    }*/

    /*public string WindowTitle
    {
        get => this.titleGetter != null ? this.titleGetter() : string.Empty;
        set
        {
            if (this.titleSetter != null)
            {
                this.titleSetter(value);
            }
        }
    }*/

    public async Task ExitAsync(bool confirmation)
    {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        MainThread.InvokeOnMainThreadAsync(async () =>
        {
            var page = Application.Current?.MainPage;
            if (confirmation && page != null)
            {
                if (await page.DisplayAlert("Question?", "Would you like to exit", "Yes", "No") == false)
                {
                    return;
                }
            }

            if (page != null)
            {
                Application.Current?.CloseWindow(page.Window); // Microsoft.Maui.MauiWinUIApplication.Current.Exit();
            }
        });
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
    }

    // private Page currentPage = default!;

    /*internal void SetTitleDelegate(Func<string>? titleGetter, Action<string>? titleSetter)
    {
        this.titleGetter = titleGetter;
        this.titleSetter = titleSetter;
    }

    private Func<string>? titleGetter;
    private Action<string>? titleSetter;*/
}
