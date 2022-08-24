// Copyright (c) All contributors. All rights reserved. Licensed under the MIT license.

using Tinyhand;

namespace LPMobile;

[TinyhandObject(ImplicitKeyAsName = true)]
public partial class AppSettings : ITinyhandSerializationCallback
{// Application Settings
    public bool LoadError { get; set; } // True if a load error occured.

    // public DipWindowPlacement WindowPlacement { get; set; } = default!;

    public string Culture { get; set; } = AppConst.DefaultCulture; // Default culture

    public int WindowX { get; set; }

    public int WindowY { get; set; }

    public int WindowWidth { get; set; }

    public int WindowHeight { get; set; }

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
