// Copyright (c) All contributors. All rights reserved. Licensed under the MIT license.

using System;
using System.Runtime.InteropServices;
using Tinyhand;

#pragma warning disable SA1600 // Elements should be documented

namespace Arc.WinAPI;

[TinyhandObject]
[Serializable]
[StructLayout(LayoutKind.Sequential)]
public partial struct POINT
{
    [Key(0)]
    public int X;
    [Key(1)]
    public int Y;

    public POINT(int x, int y)
    {
        this.X = x;
        this.Y = y;
    }
}

[TinyhandObject]
[Serializable]
[StructLayout(LayoutKind.Sequential)]
public partial struct RECT
{
    [Key(0)]
    public int Left;
    [Key(1)]
    public int Top;
    [Key(2)]
    public int Right;
    [Key(3)]
    public int Bottom;

    public RECT(int left, int top, int right, int bottom)
    {
        this.Left = left;
        this.Top = top;
        this.Right = right;
        this.Bottom = bottom;
    }
}
