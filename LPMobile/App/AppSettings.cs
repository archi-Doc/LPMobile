﻿// Copyright (c) All contributors. All rights reserved. Licensed under the MIT license.

using LPMobile.Models;
using Tinyhand;

namespace LPMobile;

[TinyhandObject(ImplicitKeyAsName = true)]
public partial class AppSettings : ITinyhandSerializationCallback
{
    public string Culture { get; set; } = AppConst.DefaultCulture; // Default culture

    public AppWindow AppWindow { get; set; } = default!;

    public double FontScale { get; set; } = 1.0d; // Font scale

    public void OnAfterDeserialize()
    {
    }

    public void OnBeforeSerialize()
    {
    }
}
