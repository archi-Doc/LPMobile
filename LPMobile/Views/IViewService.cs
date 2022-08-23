// Copyright (c) All contributors. All rights reserved. Licensed under the MIT license.

namespace LPMobile.Views;

public interface IViewService
{
    Task ExitAsync(bool confirmation = false);

    bool DisplayAlert();
}

public enum MessageId
{ // ViewService: MessageId
    SwitchCulture, // switch culture
    Information, // information dialog
    Settings, // settings dialog
    Help, // help
    DisplayScaling, // Update display scaling.
    ActivateWindow, // Brings the window into the foreground and activates the window.
    ActivateWindowForce, // Brings the window into the foreground forcibly, and activates the window.
    DataFolder, // Open data folder.
}
