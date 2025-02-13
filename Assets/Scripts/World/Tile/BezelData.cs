using System;
using System.Collections.Generic;
using CommunityToolkit.HighPerformance;
using UnityEngine;


[Flags]
public enum BezelState
{
    None = 0,
    CornerLT = 1 << 0,
    CornerRT = 1 << 1,
    CornerLB = 1 << 2,
    CornerRB = 1 << 3,
    Top = 1 << 4,
    Bottom = 1 << 5,
    Left = 1 << 6,
    Right = 1 << 7,
    Center = 1 << 8
}

public ref struct BezelData<T>
{
    //indexing order is left to right, top to bottom
    /* size = 4, innerSize = 2
        0123
        4567       56
        89AB       9A
        CDEF.
    */
    private Span2D<T> Buffer;
    public ref T CornerLT => ref Buffer[0, 0];
    public ref T CornerRT => ref Buffer[0, ^1];
    public ref T CornerLB => ref Buffer[^1, 0];
    public ref T CornerRB => ref Buffer[^1, ^1];

    public readonly Span2D<T> Top => Buffer[..1, 1..^1];
    public readonly Span2D<T> Bottom => Buffer[^1.., 1..^1];
    public readonly Span2D<T> Left => Buffer[1..^1, ..1];
    public readonly Span2D<T> Right => Buffer[1..^1, ^1..];
    public readonly Span2D<T> Center => Buffer[1..^1, 1..^1];

    public BezelData(Span2D<T> buffer)
    {
        Buffer = buffer;
    }
    public BezelData(T[] buffer, Vector2Int innerSize)
    {
        Buffer = new Span2D<T>(buffer, innerSize.y + 2, innerSize.x + 2);
    }

    public void Randomize(List<T> values)
    {
        var random = new System.Random();
        for (int x = 0; x < Buffer.Width; x++)
            for (int y = 0; y < Buffer.Height; y++)
                Buffer[y, x] = values[random.Next(values.Count)];
    }
}
