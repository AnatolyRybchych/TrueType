using TrueType.Extensions;

namespace TrueType.TTF.Tables.Hmtx
{
    public class Hmtx
    {
        public LongHorMetric[] Metrics { get; private set; }

        public Hmtx(byte[] source, int index, Hhea.Hhea hhea):this(source, index, hhea.NumOfLongHorMetrics) {}

        /// <param name="numOfLongHorMetrics">numOfLongHorMetrics from hhea table</param>
        public Hmtx(byte[] source, int index, ushort numOfLongHorMetrics)
        {
            if (index + numOfLongHorMetrics * 4 > source.Length)
                throw new Exception("index + Hmtx length > source length");

            List<LongHorMetric> metrics = new List<LongHorMetric>();

            EndianBitConvater c = new EndianBitConvater(true);

            for(int i = 0; i < numOfLongHorMetrics; i++)
            {
                metrics.Add(new LongHorMetric(c.ToUInt16(source.SubArr(index, 2)), c.ToInt16(source.SubArr(index + 2, 2))));
                index += 2;
            }
            Metrics = metrics.ToArray();
        }

        public class LongHorMetric
        {
            public ushort AdvanceWidth { get; private set; }
            public short LeftSideBearing { get; private set; }

            public LongHorMetric(ushort advanceWidth, short leftSizeBearing)
            {
                AdvanceWidth = advanceWidth;
                LeftSideBearing = leftSizeBearing;
            }
        }
    }
}
