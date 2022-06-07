using TrueType.Extensions;

namespace TrueType.TTF.Tables.Cmap.Format
{
    /// <summary>
    /// Format 0 is suitable for fonts whose character codes and glyph indices are restricted to single bytes. 
    /// This was a very common situation when TrueType was introduced but is rarely encountered now.
    /// </summary>
    public class CmapFormat0 : CmapFormat
    {
        public const int RawSize = 260;
        /// <summary>
        /// Length in bytes of the subtable (set to 262 for format 0)
        /// </summary>
        public ushort Length { get; private set; }

        /// <summary>
        /// Language code
        /// </summary>
        public ushort Language { get; private set; }

        /// <summary>
        /// An array that maps character codes to glyph index values
        /// </summary>
        public byte[] GlyphIndexArray { get; private set; }

        public CmapFormat0(byte[] source, int index) : base(0)
        {
            if (index + RawSize >= source.Length)
                SizeError();

            EndianBitConvater c = new EndianBitConvater(true);

            Length = c.ToUInt16(source.SubArr((index += 2) - 2, 2));
            Language = c.ToUInt16(source.SubArr((index += 2) - 2, 2));
            GlyphIndexArray = source.SubArr(index, 256);
        }
    }
}
