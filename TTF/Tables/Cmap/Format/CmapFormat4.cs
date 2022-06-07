using TrueType.Extensions;

namespace TrueType.TTF.Tables.Cmap.Format
{
    /// <summary>
    /// Format 4 is a two-byte encoding format. 
    /// It should be used when the character codes for a font fall into several contiguous ranges, 
    /// possibly with holes in some or all of the ranges. 
    /// That is, some of the codes in a range may not be associated with glyphs in the font. 
    /// Two-byte fonts that are densely mapped should use Format 6.
    /// The table begins with the format number, the length and language.
    /// The format-dependent data follows.It is divided into three parts:
    /// </summary>
    public class CmapFormat4 : CmapFormat
    {
        /// <summary>
        /// Length of subtable in bytes
        /// </summary>
        public ushort Length { get; private set; }

        /// <summary>
        /// Language code
        /// </summary>
        public ushort Language { get; private set; }

        /// <summary>
        /// 2 * segCount
        /// </summary>
        public ushort SegCountX2 { get; private set; }

        /// <summary>
        /// 2 * (2**FLOOR(log2(segCount)))
        /// </summary>
        public ushort SearchRange { get; private set; }

        /// <summary>
        /// log2(searchRange/2)
        /// </summary>
        public ushort EntrySelector { get; private set; }

        /// <summary>
        /// (2 * segCount) - searchRange
        /// </summary>
        public ushort RangeShift { get; private set; }

        /// <summary>
        /// Ending character code for each segment, last = 0xFFFF.
        /// </summary>
        public ushort[] EndCode { get; private set; }

        /// <summary>
        /// This value should be zero
        /// </summary>
        public ushort ReservedPad { get; private set; }

        /// <summary>
        /// Starting character code for each segment
        /// </summary>
        public ushort[] StartCode { get; private set; }

        /// <summary>
        /// Delta for all character codes in segment
        /// </summary>
        public ushort[] IdDelta { get; private set; }

        /// <summary>
        /// Offset in bytes to glyph indexArray, or 0
        /// </summary>
        public ushort[] IdRangeOffset { get; private set; }

        /// <summary>
        /// Glyph index array
        /// </summary>
        public ushort[] GlyphIdexOffset { get; private set; }

        public CmapFormat4(byte[] source, int index) : base(4)
        {
            EndianBitConvater c = new EndianBitConvater(true);

            if (index + 12 >= source.Length)
                SizeError();

            List<ushort> endCode = new List<ushort>();
            List<ushort> startCode = new List<ushort>();
            List<ushort> idDelta = new List<ushort>();
            List<ushort> idRangeOffset = new List<ushort>();
            List<ushort> glyphIndexArray = new List<ushort>();

            Length = c.ToUInt16(source.SubArr(index, 2));
            index += 2;

            Language = c.ToUInt16(source.SubArr(index, 2));
            index += 2;

            SegCountX2 = c.ToUInt16(source.SubArr(index, 2));
            index += 2;

            SearchRange = c.ToUInt16(source.SubArr(index, 2));
            index += 2;

            EntrySelector = c.ToUInt16(source.SubArr(index, 2));
            index += 2;

            RangeShift = c.ToUInt16(source.SubArr(index, 2));
            index += 2;

            int segCount = SegCountX2 / 2;

            if (index + 8 * segCount + 2 >= source.Length)
                SizeError();

            for (int i = 0; i < segCount; i++)
            {
                endCode.Add(c.ToUInt16(source.SubArr(index, 2)));
                index += 2;
            }

            ReservedPad = c.ToUInt16(source.SubArr(index, 2));
            index += 2;

            for (int i = 0; i < segCount; i++)
            {
                startCode.Add(c.ToUInt16(source.SubArr(index, 2)));
                index += 2;
            }

            for (int i = 0; i < segCount; i++)
            {
                idDelta.Add(c.ToUInt16(source.SubArr(index, 2)));
                index += 2;
            }

            for (int i = 0; i < segCount; i++)
            {
                idRangeOffset.Add(c.ToUInt16(source.SubArr(index, 2)));
                index += 2;
            }

            int glyphIndicesCount = 0;
            for (int i = 0; i < segCount; i++)
                glyphIndicesCount += endCode[i] - startCode[i] + 1;


            for (int glyphIndex = 0; glyphIndex < glyphIndicesCount; glyphIndex++)
            {
                glyphIndexArray.Add(c.ToUInt16(source.SubArr(index, 2)));
                index += 2;
            }


            EndCode = endCode .ToArray();
            StartCode = startCode .ToArray();
            IdDelta = idDelta .ToArray();
            IdRangeOffset= idRangeOffset .ToArray();
            GlyphIdexOffset = glyphIndexArray.ToArray();
        }
    }
}
