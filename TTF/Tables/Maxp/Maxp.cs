using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrueType.Extensions;

namespace TrueType.TTF.Tables.Maxp
{
    public class Maxp
    {
        public const int RawSize = 34;

        public Fixed Version { get; private set; }
        public ushort NumGlyphs { get; private set; }
        public ushort MaxPoints { get; private set; }
        public ushort MaxContours { get; private set; }
        public ushort MaxComponentPoints { get; private set; }
        public ushort MaxComponentContours { get; private set; }
        public ushort MaxZones { get; private set; }
        public ushort MaxTwilightPoints { get; private set; }
        public ushort MaxStorage { get; private set; }
        public ushort MaxFunctionDefs { get; private set; }
        public ushort MaxInstructionDefs { get; private set; }
        public ushort MaxStackElements { get; private set; }
        public ushort MaxSizeOfInstructions { get; private set; }
        public ushort MaxComponentElements { get; private set; }
        public ushort MaxComponentDepth { get; private set; }

        public Maxp(byte[] source, int index)
        {
            if (index + RawSize > source.Length)
                throw new Exception("index + Maxp length > source length");

            EndianBitConvater c = new EndianBitConvater(true);

            Version = new Fixed(c.ToInt16(source.SubArr(index, 2)), c.ToUInt16(source.SubArr(index + 2, 2)));
            index += 4;

            NumGlyphs = c.ToUInt16(source.SubArr(index + 2, 2));
            index += 2;

            MaxPoints = c.ToUInt16(source.SubArr(index + 2, 2));
            index += 2;

            MaxContours = c.ToUInt16(source.SubArr(index + 2, 2));
            index += 2;

            MaxComponentPoints = c.ToUInt16(source.SubArr(index + 2, 2));
            index += 2;

            MaxComponentContours = c.ToUInt16(source.SubArr(index + 2, 2));
            index += 2;

            MaxZones = c.ToUInt16(source.SubArr(index + 2, 2));
            index += 2;

            MaxTwilightPoints = c.ToUInt16(source.SubArr(index + 2, 2));
            index += 2;

            MaxStorage = c.ToUInt16(source.SubArr(index + 2, 2));
            index += 2;

            MaxFunctionDefs = c.ToUInt16(source.SubArr(index + 2, 2));
            index += 2;

            MaxInstructionDefs = c.ToUInt16(source.SubArr(index + 2, 2));
            index += 2;

            MaxStackElements = c.ToUInt16(source.SubArr(index + 2, 2));
            index += 2;

            MaxSizeOfInstructions = c.ToUInt16(source.SubArr(index + 2, 2));
            index += 2;

            MaxComponentElements = c.ToUInt16(source.SubArr(index + 2, 2));
            index += 2;

            MaxComponentDepth = c.ToUInt16(source.SubArr(index + 2, 2));
            index += 2;
        }
    }
}
