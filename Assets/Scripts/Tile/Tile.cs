using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dark.Utils;

public ref struct BezelData<T>
{
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine($"[{ CornerLT }]{ Top.ToString() }[{ CornerRT }]");
        var l = Left.ToList();
        var r = Right.ToList();
        for (int y = 1; y <= innerSize; y++)
            sb.AppendLine($"[{ l[y] }]{ new string('.', innerSize) }[{ r[y] }]");
        sb.AppendLine($"[{ CornerLB }]{ Bottom.ToString() }[{ CornerRB }]");
        return sb.ToString();
    }
    //indexing order is left to right, top to bottom
    /* size = 4, innerSize = 2
        0123
        4567       56
        89AB       9A
        CDEF.
    */
    private readonly int Size => innerSize + 2;
    private readonly int innerSize;
    private Span<T> Buffer;
    public ref T CornerLT => ref Buffer[0];
    public ref T CornerRT => ref Buffer[Size-1];
    public ref T CornerLB => ref Buffer[Size*(Size-1)];
    public ref T CornerRB => ref Buffer[Size*Size-1];
    
    public readonly Span<T> Top => Buffer.Slice(1,innerSize);
    public readonly Span<T> Bottom => Buffer.Slice(Size*(Size-1)+1,innerSize);
    public readonly SpanExtensions.StrideEnumerator<T> Left => Buffer.Slice(Size).Stride(Size,1,innerSize);
    public readonly SpanExtensions.StrideEnumerator<T> Right => Buffer.Slice(Size+innerSize+1).Stride(Size,1,innerSize);
    public readonly SpanExtensions.StrideEnumerator<T> Center => Buffer.Slice(Size+1).Stride(3,innerSize,innerSize*innerSize);

    public BezelData(Span<T> buffer, int innerSize)
    {
        this.Buffer = buffer;
        this.innerSize = innerSize;
    }
    public void BindBuffer(Span<T> buffer)
    {
        Buffer = buffer;
    }

    public void Neighbors(BezelData<T> nTop, BezelData<T> nRight, BezelData<T> nBottom, BezelData<T> nLeft)
    {
        nTop.Bottom.CopyTo(Top);
        nBottom.Top.CopyTo(Bottom);

        var srcLeft = nLeft.Right.GetEnumerator();
        var dstLeft = Left.GetEnumerator();
        while(srcLeft.MoveNext() && dstLeft.MoveNext())
            dstLeft.Current = srcLeft.Current;

        var srcRight = nRight.Right.GetEnumerator();
        var dstRight = Right.GetEnumerator();
        while(srcRight.MoveNext() && dstRight.MoveNext())
            dstRight.Current = srcRight.Current;
    }

    public void Randomize(List<T> values)
    {
        Random random = new Random();
        for(int i = 0; i < Buffer.Length; i++)
            Buffer[i] = values[random.Next(values.Count)];
    }
}
