using TrueType.Extensions;

namespace TrueType.TTF.Tables.Cmap.Format
{
    public class CmapFormat6 : CmapFormat
    {
        /// <summary>
        /// Length in bytes
        /// </summary>
        public ushort Length { get; private set; }

        /// <summary>
        /// Language code
        /// </summary>
        public ushort Language { get; private set; }

        /// <summary>
        /// First character code of subrange
        /// </summary>
        public ushort FirstCode { get; private set; }

        /// <summary>
        /// Number of character codes in subrange
        /// </summary>
        public ushort EntryCount { get; private set; }

        /// <summary>
        /// Array of glyph index values for character codes in the range
        /// </summary>
        public ushort[] GlyphIndexArray { get; private set; }
        
        public CmapFormat6(byte[] source, int index) : base(6)
        {
            EndianBitConvater c = new EndianBitConvater(true);

            if (index + 8 >= source.Length)
                SizeError();

            List<ushort> glyphIndexArray = new List<ushort>();

            Length = c.ToUInt16(source.SubArr(index, 2));
            index += 2;

            Language = c.ToUInt16(source.SubArr(index, 2));
            index += 2;

            FirstCode = c.ToUInt16(source.SubArr(index, 2));
            index += 2;

            EntryCount = c.ToUInt16(source.SubArr(index, 2));
            index += 2;

            if (index + EntryCount * 2 >= source.Length)
                SizeError();

            for (int entry = 0; entry < EntryCount; entry++)
            {
                glyphIndexArray.Add(c.ToUInt16(source.SubArr(index, 2)));
                index += 2;
            }

            GlyphIndexArray = glyphIndexArray.ToArray();
        }
    }
}
