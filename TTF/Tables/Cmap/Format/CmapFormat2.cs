using TrueType.Extensions;

namespace TrueType.TTF.Tables.Cmap.Format
{
    /// <summary>
    /// The format 2 mapping subtable type is used for fonts containing Japanese, Chinese, or Korean characters. 
    /// The code standards used in this table are supported on Macintosh systems in Asia. 
    /// These fonts contain a mixed 8/16-bit encoding, 
    /// in which certain byte values are set aside to signal the first byte of a 2-byte character. 
    /// These special values are also legal as the second byte of a 2-byte character.
    /// The following table shows the format of a format 2 encoding subtable.
    /// The subHeaderKeys array maps each possible high byte into a particular member of the suborders array.
    /// This allows the determination of whether or not a second byte is used.
    /// In either case, the path leads into the glyphIndexArray from which the mapped glyph index is obtained.
    /// The sequence of operations is as follows:
    /// Consider a high byte, i, designating an integer between 0 and 255. 
    /// The value subHeaderKeys[i], divided by 8, is the index k into the subHeaders array.
    /// The value k equals 0 is special.It means that i is a one-byte code and no second byte will be referenced. 
    /// If k is positive, then i is the high-byte of a two-byte code and its second byte, j, will be consumed.
    /// </summary>
    public class CmapFormat2 : CmapFormat
    {
        /// <summary>
        /// Total table length in bytes
        /// </summary>
        public ushort Length { get; private set; }

        /// <summary>
        /// Language code
        /// </summary>
        public ushort Language { get; private set; }

        /// <summary>
        /// Array that maps high bytes to subHeaders: value is index * 8
        /// </summary>
        public ushort[] SubHeaderKeys { get; private set; }

        /// <summary>
        /// Variable length array of subHeader structures
        /// </summary>
        public SubHeader[] SubHeaders { get; private set; }

        /// <summary>
        /// Variable length array containing subarrays
        /// </summary>
        public ushort[] GlyphIndexArray { get; private set; }

        public CmapFormat2(byte[] source, int index) : base(2)
        {
            EndianBitConvater c = new EndianBitConvater(true);

            if (index + 4 >= source.Length)
                SizeError();

            List<ushort> subHeaderKeys = new List<ushort>();
            List<CmapFormat2.SubHeader> subHeaders = new List<CmapFormat2.SubHeader>();
            List<ushort> glyphIndexArray = new List<ushort>();

            Length = c.ToUInt16(source.SubArr(index, 2));
            index += 2;

            Language = c.ToUInt16(source.SubArr(index, 2));
            index += 2;

            if (index + 256 * 2 >= source.Length)
                SizeError();

            for (int i = 0; i < 256; i++)
            {
                subHeaderKeys.Add(c.ToUInt16(source.SubArr(index, 2)));
                index += 2;
            }

            int subHeadersCount = subHeaderKeys.Max();
            if (index + subHeadersCount * CmapFormat2.SubHeader.RawSize >= source.Length)
                SizeError();

            for (int subHeader = 0; subHeader < subHeadersCount; subHeader++)
            {
                subHeaders.Add(
                    new CmapFormat2.SubHeader(
                        c.ToUInt16(source.SubArr(index, 2)),
                        c.ToUInt16(source.SubArr(index + 2, 2)),
                        c.ToInt16(source.SubArr(index + 4, 2)),
                        c.ToUInt16(source.SubArr(index + 6, 2))
                    )
                );
                index += CmapFormat2.SubHeader.RawSize;
            }

            if (index + subHeadersCount * 2 >= source.Length)
                SizeError();

            for (int glyphIndex = 0; glyphIndex < subHeadersCount; glyphIndex++)
            {
                glyphIndexArray.Add(c.ToUInt16(source.SubArr(index, 2)));
                index += 2;
            }

            SubHeaderKeys = subHeaderKeys.ToArray();
            SubHeaders = subHeaders.ToArray();
            GlyphIndexArray = glyphIndexArray.ToArray();
        }

        /// <summary>
        /// If k is positive, then the four values belonging to subheaders[k] 
        /// are used as follows with firstCode and entryCount defining the allowable range for the second byte j:
        ///     firstCode <= j < (firstCode + entryCount)
        /// If j is outside this range, index 0 (the missing character glyph) is returned. 
        /// Otherwise, idRangeOffset is used to identify the associated range within the glyphIndexArray. 
        /// The glyphIndexArray immediately follows the subHeaders array and may be loosely viewed as an extension to it. 
        /// The value of the idRangeOffset is the number of bytes past the actual location of the idRangeOffset word 
        /// where the glyphIndexArray element corresponding to firstCode appears. If p is zero, it is returned directly. 
        /// If p is nonzero, p = p + idDelta is returned. The sum is reduced modulo 65536, if necessary.
        /// 
        /// For the one-byte case with k = 0, the structure subHeaders[0] will show firstCode = 0, 
        /// entryCount = 256, and idDelta = 0.The idRangeOffset will point, 
        /// as previously discussed, to the beginning of the glyphIndexArray.
        /// Indexing i words into this array gives the returned value p = glyphIndexArray[i].
        /// 
        /// The format 2 cmap is targeted at mixed 8/16-bit encodings such as Big Five and Shift-JIS.
        /// Such encodings are still widely used, and Apple platforms will correctly support them even if a format 2 cmap is not present.
        /// </summary>
        public class SubHeader
        {
            public const int RawSize = 8;

            public ushort FirstCode { get; private set; }
            public ushort EntryCount { get; private set; }
            public short IdDelta { get; private set; }
            public ushort IdRangeOffset { get; private set; }

            public SubHeader(ushort firstCode, ushort entryCount, short idDelta, ushort idRangeOffset)
            {
                FirstCode = firstCode;
                EntryCount = entryCount;
                IdDelta = idDelta;
                IdRangeOffset = idRangeOffset;
            }
        }
    }
}
