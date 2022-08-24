// Copyright (c) All contributors. All rights reserved. Licensed under the MIT license.

using Tinyhand;

namespace LPMobile;

[TinyhandObject(ImplicitKeyAsName = true)]
public partial class AppData
{// Application Data
    [IgnoreMember]
    public bool LoadError { get; set; } // True if a loading error occured.

    public AppSettings Settings { get; set; } = default!;
}
