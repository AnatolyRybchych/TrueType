using TrueType.Extensions;

namespace TrueType.TTF.Tables.Cmap.Format
{
    /// <summary>
    /// Format 12 is a bit like format 4, in that it defines segments for sparse representation in 4-byte character space. 
    /// </summary>
    public class CmapFormat12:CmapFormat
    {
        public ushort Reserved { get; private set; }

        /// <summary>
        /// Set to 0.
        /// </summary>
        public uint Length { get; private set; }

        /// <summary>
        /// Byte length of this subtable (including the header)
        /// </summary>
        public uint Language { get; private set; }

        /// <summary>
        /// Language code
        /// </summary>
        public uint NGroups { get; private set; }

        /// <summary>
        /// Number of groupings which follow
        /// </summary>
        public Group[] Groups { get; private set; }


        public CmapFormat12(byte[] source, int index) : base(12)
        {
            EndianBitConvater c = new EndianBitConvater(true);

            if (index + 14 >= source.Length)
                SizeError();

            List<CmapFormat12.Group> groups = new List<CmapFormat12.Group>();

            Reserved = c.ToUInt16(source.SubArr(index, 2));
            index += 2;

            Length = c.ToUInt32(source.SubArr(index, 4));
            index += 4;

            Language = c.ToUInt32(source.SubArr(index, 4));
            index += 4;

            NGroups = c.ToUInt32(source.SubArr(index, 4));
            index += 4;

            if (index + NGroups * CmapFormat12.Group.RawSize >= source.Length)
                SizeError();

            for (int group = 0; group < NGroups; group++)
            {
                groups.Add(
                    new CmapFormat12.Group(
                        c.ToUInt32(source.SubArr(index, 4)),
                        c.ToUInt32(source.SubArr(index + 4, 4)),
                        c.ToUInt32(source.SubArr(index + 8, 4))
                    )
                );
                index += CmapFormat12.Group.RawSize;
            }

            Groups = groups.ToArray();
        }


        public class Group
        {
            public const int RawSize = 12;
            /// <summary>
            /// First character code in this group
            /// </summary>
            public uint StartCharCode { get; private set; }

            /// <summary>
            /// Last character code in this group
            /// </summary>
            public uint EndCharCode { get; private set; }

            /// <summary>
            /// Glyph index corresponding to the starting character code; subsequent charcters are mapped to sequential glyphs
            /// </summary>
            public uint StartGlyphCode { get; private set; }

            public Group(uint startCharCode, uint endCharCode, uint startGlyphCode)
            {
                StartCharCode = startCharCode;
                EndCharCode = endCharCode;
                StartGlyphCode = startGlyphCode;
            }
        }
    }
}
