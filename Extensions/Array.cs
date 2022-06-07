using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrueType.Extensions
{
    public static class Array
    {
        public static T[] SubArr<T>(this T[] arr, int start, int count = int.MaxValue)
        {
            if(start < 0) throw new ArgumentOutOfRangeException("start < 0");
            if((long)start + count > int.MaxValue) count = count - start;
            count = Math.Min(arr.Length, count);
            T[] result = new T[count];

            for (int i = 0; i < count; i++)
                result[i] = arr[i + start];

            return result;
        }

        public static byte[] ReverseEndian(this byte[] arr)
        {
            int cycles = arr.Length / 2;
            for(int cycle = 0; cycle < cycles; cycle++)
            {
                byte tmp = arr[cycle];
                arr[cycle] = arr[arr.Length - cycle - 1];
                arr[arr.Length - cycle - 1] = tmp;
            }
            return arr;
        }
    }
}
