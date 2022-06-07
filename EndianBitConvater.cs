using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrueType.Extensions;

namespace TrueType
{
    internal class EndianBitConvater
    {
        public bool ToBigEndian { get; private set; }
        private bool reverseEndian;

        public EndianBitConvater(bool toBigEndian)
        {
            ToBigEndian = toBigEndian;
            reverseEndian = BitConverter.IsLittleEndian == ToBigEndian;
        }

        public ulong ToUInt64(byte[] bytes) => BitConverter.ToUInt64(bytes.HandleIf(reverseEndian, (ref byte[] b) => b.ReverseEndian()));
        public long ToInt64(byte[] bytes) => BitConverter.ToInt64(bytes.HandleIf(reverseEndian, (ref byte[] b) => b.ReverseEndian()));
        public uint ToUInt32(byte[] bytes) => BitConverter.ToUInt32(bytes.HandleIf(reverseEndian, (ref byte[] b) => b.ReverseEndian()));
        public int ToInt32(byte[] bytes) => BitConverter.ToInt32(bytes.HandleIf(reverseEndian, (ref byte[] b) => b.ReverseEndian()));
        public ushort ToUInt16(byte[] bytes) => BitConverter.ToUInt16(bytes.HandleIf(reverseEndian, (ref byte[] b) => b.ReverseEndian()));
        public short ToInt16(byte[] bytes) => BitConverter.ToInt16(bytes.HandleIf(reverseEndian, (ref byte[] b) => b.ReverseEndian()));


        public byte[] GetBytes(ulong val) => BitConverter.GetBytes(val).HandleIf(reverseEndian, (ref byte[] b) => b.ReverseEndian());
        public byte[] GetBytes(long val) => BitConverter.GetBytes(val).HandleIf(reverseEndian, (ref byte[] b) => b.ReverseEndian());
        public byte[] GetBytes(uint val) => BitConverter.GetBytes(val).HandleIf(reverseEndian, (ref byte[] b) => b.ReverseEndian());
        public byte[] GetBytes(int val) => BitConverter.GetBytes(val).HandleIf(reverseEndian, (ref byte[] b) => b.ReverseEndian());
        public byte[] GetBytes(ushort val) => BitConverter.GetBytes(val).HandleIf(reverseEndian, (ref byte[] b) => b.ReverseEndian());
        public byte[] GetBytes(short val) => BitConverter.GetBytes(val).HandleIf(reverseEndian, (ref byte[] b) => b.ReverseEndian());

    }
}
