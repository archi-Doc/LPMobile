// Copyright (c) All contributors. All rights reserved. Licensed under the MIT license.

namespace LPMobile;

public interface IAppService
{
    void Message(MessageId id); // Message Id
}

public enum MessageId
{ // ViewService: MessageId
    Exit, // exit application with confirmation
    ExitWithoutConfirmation, // exit application without confirmation
    SwitchCulture, // switch culture
    Information, // information dialog
    Settings, // settings dialog
    Help, // help
    DisplayScaling, // Update display scaling.
    ActivateWindow, // Brings the window into the foreground and activates the window.
    ActivateWindowForce, // Brings the window into the foreground forcibly, and activates the window.
    DataFolder, // Open data folder.
}
