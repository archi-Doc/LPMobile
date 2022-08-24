// Copyright (c) All contributors. All rights reserved. Licensed under the MIT license.

using Tinyhand;

namespace LPMobile;

[TinyhandObject(ImplicitKeyAsName = true)]
public partial class AppSettings : ITinyhandSerializationCallback
{// Application Settings
    // public DipWindowPlacement WindowPlacement { get; set; } = default!;

    public string Culture { get; set; } = AppConst.DefaultCulture; // Default culture

    public AppWindow AppWindow { get; set; } = default!;

    public double DisplayScaling { get; set; } = 1.0d; // Display Scaling

    // [Key(4)]
    // public TestItem.GoshujinClass TestItems { get; set; } = default!;

    public void OnAfterDeserialize()
    {
        // Transformer.Instance.ScaleX = this.DisplayScaling;
        // Transformer.Instance.ScaleY = this.DisplayScaling;
    }

    public void OnBeforeSerialize()
    {
    }
}
