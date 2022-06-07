using TrueType.Extensions;

namespace TrueType.TTF.Tables.Hhea
{
    public class Hhea
    {
        public const int RawSize = 38;

        public Fixed Version { get; private set; }
        public short Ascent { get; private set; }
        public short Descent { get; private set; }
        public short LineGap { get; private set; }
        public ushort AdvanceWidthMax { get; private set; }
        public short MinLeftSideBearing { get; private set; }
        public short MinRigthSizeBearing { get; private set; }
        public short XMaxExtent { get; private set; }
        public short CareteSlopeRise { get; private set; }
        public short CaretSlopeRun { get; private set; }
        public short CaretOffset { get; private set; }
        public short MetricDataFormat { get; private set; }
        public ushort NumOfLongHorMetrics { get; private set; }

        public Hhea(byte[] source, int index)
        {
            if (index + RawSize >= source.Length)
                throw new Exception("index + Hhea length > source length");

            EndianBitConvater c = new EndianBitConvater(true);

            Version = new Fixed(c.ToInt16(source.SubArr(index, 2)), c.ToUInt16(source.SubArr(index + 2, 2)));
            index += 4;

            Ascent = c.ToInt16(source.SubArr(index, 2));
            index += 2;

            Descent = c.ToInt16(source.SubArr(index, 2));
            index += 2;

            LineGap = c.ToInt16(source.SubArr(index, 2));
            index += 2;

            AdvanceWidthMax = c.ToUInt16(source.SubArr(index, 2));
            index += 2;

            MinLeftSideBearing = c.ToInt16(source.SubArr(index, 2));
            index += 2;

            MinRigthSizeBearing = c.ToInt16(source.SubArr(index, 2));
            index += 2;

            CareteSlopeRise = c.ToInt16(source.SubArr(index, 2));
            index += 2;

            CaretSlopeRun = c.ToInt16(source.SubArr(index, 2));
            index += 2;

            CaretOffset = c.ToInt16(source.SubArr(index, 2));
            index += 2;

            //reserved
            index += 8;

            MetricDataFormat = c.ToInt16(source.SubArr(index, 2));
            index += 2;

            NumOfLongHorMetrics = c.ToUInt16(source.SubArr(index, 2));
            index += 2;
        }
    }
}
