// Copyright (c) All contributors. All rights reserved. Licensed under the MIT license.

namespace Arc.Views;

public interface IViewService
{
    Task ExitAsync(bool confirmation = false);

    void ChangeCulture(string culture);

    void SetFontScale(double scale);

    Task<bool> DisplayAlert(ulong title, ulong message, ulong accept, ulong cancel);

    Task<bool> DisplayYesOrNo(ulong title, ulong message);
}
