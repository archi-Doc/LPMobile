// Copyright (c) All contributors. All rights reserved. Licensed under the MIT license.

using System.Collections.Concurrent;
using Tinyhand;

namespace LPMobile;

internal class ViewServiceImpl : Arc.Views.IViewService
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
                if (await page.DisplayAlert(
                    HashedString.Get(Hashed.App.Name),
                    HashedString.Get(Hashed.Dialog.Exit),
                    HashedString.Get(Hashed.Dialog.Yes),
                    HashedString.Get(Hashed.Dialog.No)) == false)
                {
                    return;
                }
            }

            Application.Current?.Quit();
            /*if (page != null)
            {
                Application.Current?.CloseWindow(page.Window); // Microsoft.Maui.MauiWinUIApplication.Current.Exit();
            }*/
        });
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
    }

    public void SetFontScale(double scale)
    {
        if (Application.Current?.MainPage is { } mainPage)
        {
            if (mainPage is Shell shell)
            {
                mainPage = shell.CurrentPage;
            }

            if (this.pageToFontScale.TryGetValue(mainPage, out var current))
            {
                if (current == scale)
                {
                    return;
                }
            }
            else
            {
                current = 1.0d;
            }

            MainThread.BeginInvokeOnMainThread(() =>
            {
                var ratio = scale / current;
                ProcessElements(mainPage.GetVisualTreeDescendants(), ratio);
                this.pageToFontScale[mainPage] = scale;
            });
        }

        static void ProcessElements(IReadOnlyList<IVisualTreeElement> list, double ratio)
        {
            foreach (var x in list)
            {// IFontElement
                if (x is Button button)
                {
                    button.FontSize *= ratio;
                }
                else if (x is Editor editor)
                {
                    editor.FontSize *= ratio;
                }
                else if (x is Entry entry)
                {
                    entry.FontSize *= ratio;
                }
                else if (x is Label label)
                {
                    label.FontSize *= ratio;
                }
                else if (x is Picker picker)
                {
                    picker.FontSize *= ratio;
                }
            }
        }
    }

    public void ChangeCulture(string culture)
    {
        HashedString.ChangeCulture(culture);
        Arc.Views.C4Updater.C4Update();
    }

    public Task<bool> DisplayAlert(ulong title, ulong message, ulong accept, ulong cancel)
    {
        var page = Application.Current?.MainPage;
        if (page == null)
        {
            return Task.FromResult(false);
        }

        return page.DisplayAlert(
            title == 0 ? string.Empty : HashedString.Get(title),
            message == 0 ? string.Empty : HashedString.Get(message),
            accept == 0 ? string.Empty : HashedString.Get(accept),
            cancel == 0 ? string.Empty : HashedString.Get(cancel));
    }

    public Task<bool> DisplayYesOrNo(ulong title, ulong message)
        => this.DisplayAlert(title, message, Hashed.Dialog.Yes, Hashed.Dialog.No);

    private ConcurrentDictionary<Page, double> pageToFontScale = new();

    // private Page currentPage = default!;

    /*internal void SetTitleDelegate(Func<string>? titleGetter, Action<string>? titleSetter)
    {
        this.titleGetter = titleGetter;
        this.titleSetter = titleSetter;
    }

    private Func<string>? titleGetter;
    private Action<string>? titleSetter;*/
}
