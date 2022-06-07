using TrueType.Extensions;

namespace TrueType.TTF.Tables.Head
{
    public class Head
    {
        public const int RawSize = 54;

        public Fixed Version { get; private set; }
        public Fixed FontRevision { get; private set; }
        public uint CheckSumAdjustment { get; private set; }
        public uint MagicNumber { get; private set; }
        public ushort Flags { get; private set; }
        public ushort UnitsPerEm { get; private set; }
        public DateTime Created { get; private set; }
        public DateTime Modified { get; private set; }
        public short XMin { get; private set; }
        public short YMin { get; private set; }
        public short XMax { get; private set; }
        public short YMax { get; private set; }
        public Style MacStyle { get; private set; }
        public ushort LowestRecPPEM { get; private set; }
        public short FontDirectionHint { get; private set; }
        public short IndexToLocFormat { get; private set; }
        public short GlyphDataFormat { get; private set; }

        public Head(byte[] source, int index)
        {
            if (index + RawSize >= source.Length)
                throw new Exception("index + head length > source langth");

            EndianBitConvater c = new EndianBitConvater(true);

            Version = new Fixed(c.ToInt16(source.SubArr(index, 2)), c.ToUInt16(source.SubArr(index + 2, 2)));
            index += 4;

            FontRevision = new Fixed(c.ToInt16(source.SubArr(index, 2)), c.ToUInt16(source.SubArr(index + 2, 2)));
            index += 4;

            CheckSumAdjustment = c.ToUInt32(source.SubArr(index, 4));
            index += 4;

            MagicNumber = c.ToUInt32(source.SubArr(index, 4));
            index += 4;

            Flags = c.ToUInt16(source.SubArr(index, 2));
            index += 2;

            UnitsPerEm = c.ToUInt16(source.SubArr(index, 2));
            index += 2;

            long created = c.ToInt64(source.SubArr(index, 8));
            Created = new DateTime(1904, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(created);
            index += 8;

            long modified = c.ToInt64(source.SubArr(index, 8));
            Modified = new DateTime(1904, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(created);
            index += 8;

            XMin = c.ToInt16(source.SubArr(index, 2));
            index += 2;

            YMin = c.ToInt16(source.SubArr(index, 2));
            index += 2;

            XMax = c.ToInt16(source.SubArr(index, 2));
            index += 2;

            YMax = c.ToInt16(source.SubArr(index, 2));
            index += 2;

            MacStyle = (Style)c.ToUInt16(source.SubArr(index, 2));
            index += 2;

            LowestRecPPEM = c.ToUInt16(source.SubArr(index, 2));
            index += 2;

            FontDirectionHint = c.ToInt16(source.SubArr(index, 2));
            index += 2;

            IndexToLocFormat = c.ToInt16(source.SubArr(index, 2));
            index += 2;

            GlyphDataFormat = c.ToInt16(source.SubArr(index, 2));
            index += 2;
        }


        public enum Style:ushort
        {
            Bold =      0b1000000000000000,
            Italic =    0b0100000000000000,
            Undeline =  0b0010000000000000,
            Outline =   0b0001000000000000,
            Shadow =    0b0000100000000000,
            Narrow =    0b0000010000000000,
            Extended =  0b0000001000000000
        }
    }
}
