using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using CommunityToolkit.HighPerformance;

namespace Dark.Utils
{

    public static class Span2DExtensions
    {
        public static Span2D<T> GetTop<T>(this Span2D<T> buffer) => buffer[..1, 0..]; 
        public static Span2D<T> GetBottom<T>(this Span2D<T> buffer) => buffer[^1.., 0..];
        public static Span2D<T> GetLeft<T>(this Span2D<T> buffer) => buffer[0.., ..1];
        public static Span2D<T> GetRight<T>(this Span2D<T> buffer) => buffer[0.., ^1..];
    }
}