// Copyright (c) All contributors. All rights reserved. Licensed under the MIT license.

namespace LPMobile.Views;

public interface IViewService
{
    // Page CurrentPage { get; set; }

    // string WindowTitle { get; set; }

    Task ExitAsync(bool confirmation = false);
}

public enum MessageId
{
    SwitchCulture, // switch culture
    Information, // information dialog
    Help, // help
    DisplayScaling, // Update display scaling.
}
