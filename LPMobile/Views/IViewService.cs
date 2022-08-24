// Copyright (c) All contributors. All rights reserved. Licensed under the MIT license.

namespace LPMobile.Views;

public interface IViewService
{
    Task ExitAsync(bool confirmation = false);

    void SwitchCulture(string culture);

    double GetFontScale();

    void SetFontScale(double scale);
}
