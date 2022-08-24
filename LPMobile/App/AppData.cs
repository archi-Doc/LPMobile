// Copyright (c) All contributors. All rights reserved. Licensed under the MIT license.

using Tinyhand;

namespace LPMobile;

[TinyhandObject(ImplicitKeyAsName = true)]
public partial class AppData
{// Application Data
    public AppSettings Settings { get; set; } = default!;
}
