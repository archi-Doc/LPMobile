// Copyright (c) All contributors. All rights reserved. Licensed under the MIT license.

using Tinyhand;

namespace LPMobile;

[TinyhandObject(ImplicitKeyAsName = true)]
public partial class AppSettings : ITinyhandSerializationCallback
{// Application Settings
    // public DipWindowPlacement WindowPlacement { get; set; } = default!;

    public string Culture { get; set; } = AppConst.DefaultCulture; // Default culture

    public AppWindow AppWindow { get; set; } = default!;

    public double FontScaling { get; set; } = 1.0d; // Font Scaling

    // [Key(4)]
    // public TestItem.GoshujinClass TestItems { get; set; } = default!;

    public void OnAfterDeserialize()
    {
    }

    public void OnBeforeSerialize()
    {
    }
}
