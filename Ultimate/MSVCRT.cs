using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Collections.Concurrent;

namespace Ultimate
{
    //public unsafe class MSVCRT
    //{

    //    #region memcpy
    //    public static void* memcpy(byte* dst, byte* src, int length)
    //    {
    //        for (var i = 0; i < length; i++)
    //            *(dst + i) = *(src + i);
    //        return dst;
    //    }
    //    public static void* memcpy(sbyte* dst, sbyte* src, int length)
    //    {
    //        for (var i = 0; i < length; i++)
    //            *(dst + i) = *(src + i);
    //        return dst;
    //    }

    //    public static void* memcpy(sbyte* dst, byte* src, int length)
    //    {
    //        for (var i = 0; i < length; i++)
    //            *(dst + i) = *((sbyte*)(src + i));
    //        return dst;
    //    }

    //    public static void* memcpy(byte* dst, sbyte* src, int length)
    //    {
    //        for (var i = 0; i < length; i++)
    //            *(dst + i) = *((byte*)(src + i));
    //        return dst;
    //    }
    //    private const string MSVCRT_DLL = @"C:\Windows\system32\msvcrt.dll";
    //    private const string MSVCRT_DLL_alt = @"D:\Windows\system32\msvcrt.dll";
    //    [DllImport(MSVCRT_DLL, EntryPoint = "memcpy", CallingConvention = CallingConvention.Cdecl)]
    //    private static extern void* _memcpy(void* dst, void* src, int length);

    //    [DllImport(MSVCRT_DLL_alt, EntryPoint = "memcpy", CallingConvention = CallingConvention.Cdecl)]
    //    private static extern void* _memcpy_alt(void* dst, void* src, int length);


    //    public static void* memcpy(uint* dst, byte* src, int length)
    //    {
    //        if (Environment.SystemDirectory.StartsWith("D"))
    //            return _memcpy_alt(dst, src, length);
    //        return _memcpy(dst, src, length);
    //    }

    //    #endregion


    //    #region memset
    //    public static void* memset(byte* dst, byte fill, int length)
    //    {
    //        for (var i = 0; i < length; i++)
    //            *(dst + i) = fill;
    //        return dst;
    //    }
    //    public static void* memset(sbyte* dst, sbyte fill, int length)
    //    {
    //        for (var i = 0; i < length; i++)
    //            *(dst + i) = fill;
    //        return dst;
    //    }
    //    #endregion


    //}

    public static/* unsafe */class NativeExtensions
    {
        //public static int IndexOf<T>(this IEnumerable<T> col, Predicate<T> match)
        //{
        //    int i = 0;
        //    foreach (var item in col)
        //    {
        //        if (match(item))
        //            return i;
        //        else
        //            i++;
        //    }
        //    return -1;
        //}

        public static bool Remove<T, T2>(this ConcurrentDictionary<T, T2> dictionary, T key)
        {
            T2 dummy;
            return dictionary.TryRemove(key, out dummy);
        }

        //public static IEnumerable<T> Flatten<T>(this T[,] map, int startX, int startY, int endX, int endY)
        //{
        //    var maxX = Math.Min(endX, map.GetLength(0));
        //    var maxY = Math.Min(endY, map.GetLength(1));

        //    var minX = Math.Max(Math.Min(endX, startX), 0);
        //    var minY = Math.Max(Math.Min(endY, startY), 0);

        //    for (var x = minX; x < maxX; x++)
        //    {
        //        for (var y = minY; y < maxY; y++)
        //        {
        //            yield return map[x, y];
        //        }
        //    }
        //}

        //public static IEnumerable<T> Flatten<T>(this T[,] map)
        //{
        //    for (int row = 0; row < map.GetLength(0); row++)
        //    {
        //        for (int col = 0; col < map.GetLength(1); col++)
        //        {
        //            yield return map[row, col];
        //        }
        //    }
        //}

        //public static void CopyTo(this string str, void* pDest)
        //{
        //    var dest = (byte*)pDest;
        //    for (var i = 0; i < str.Length; i++)
        //    {
        //        dest[i] = (byte)str[i];
        //    }
        //}

        //public static byte[] UnsafeClone(this byte[] buffer)
        //{
        //    var bufCopy = new byte[buffer.Length];
        //    Buffer.BlockCopy(buffer, 0, bufCopy, 0, buffer.Length);
        //    return bufCopy;
        //}
    }
}