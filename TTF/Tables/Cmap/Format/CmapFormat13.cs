using TrueType.Extensions;

namespace TrueType.TTF.Tables.Cmap.Format
{
    /// <summary>
    /// Format 13 is like a 12, but for apple
    /// </summary>
    public class CmapFormat13 : CmapFormat
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


        public CmapFormat13(byte[] source, int index) : base(13)
        {
            EndianBitConvater c = new EndianBitConvater(true);

            if (index + 14 >= source.Length)
                SizeError();

            List<Group> groups = new List<Group>();

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
                    new Group(
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
