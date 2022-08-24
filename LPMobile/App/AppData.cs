// Copyright (c) All contributors. All rights reserved. Licensed under the MIT license.

using Tinyhand;

namespace LPMobile;

[TinyhandObject]
public partial class AppData
{// Application Data
    [Key(0)]
    public AppSettings Settings { get; set; } = default!;
}
