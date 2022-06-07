using TrueType.Extensions;

namespace TrueType.TTF.Tables.Cmap.Format
{
    public class CmapFormat8 : CmapFormat
    {
        /// <summary>
        /// Set to 0
        /// </summary>
        public ushort Reserved { get; private set; }

        /// <summary>
        /// Byte length of this subtable (including the header)
        /// </summary>
        public uint Length { get; private set; }

        /// <summary>
        /// Language code
        /// </summary>
        public uint Language { get; private set; }

        /// <summary>
        /// Tightly packed array of bits (8K bytes total) indicating whether the particular 16-bit (index) value is the start of a 32-bit character code
        /// </summary>
        public byte[] Is32 { get; private set; }

        /// <summary>
        /// Number of groupings which follow
        /// </summary>
        public uint NGroups { get; private set; }
        public NGroup[] Groups { get; private set; }

        public CmapFormat8(byte[] source, int index) : base(9)
        {
            EndianBitConvater c = new EndianBitConvater(true);

            if (index + 14 + 65536 >= source.Length)
                SizeError();

            List<CmapFormat8.NGroup> groups = new List<CmapFormat8.NGroup>();


            Reserved = c.ToUInt16(source.SubArr(index, 2));
            index += 2;

            Length = c.ToUInt16(source.SubArr(index, 4));
            index += 4;

            Language = c.ToUInt16(source.SubArr(index, 4));
            index += 4;

            Is32 = source.SubArr(index, 65536);
            index += 65536;

            NGroups = c.ToUInt16(source.SubArr(index, 4));
            index += 4;

            if (index + NGroups * CmapFormat8.NGroup.RawSize >= source.Length)
                SizeError();

            for (int group = 0; group < NGroups; group++)
            {
                groups.Add(
                    new CmapFormat8.NGroup(
                        c.ToUInt32(source.SubArr(index, 4)),
                        c.ToUInt32(source.SubArr(index + 4, 4)),
                        c.ToUInt32(source.SubArr(index + 8, 4))
                    )
                );
                index += CmapFormat8.NGroup.RawSize;
            }
            Groups = groups.ToArray();
        }

        public class NGroup
        {
            public const int RawSize = 12;
            /// <summary>
            /// First character code in this group; 
            /// note that if this group is for one or more 16-bit character codes (which is determined from the is32 array), 
            /// this 32-bit value will have the high 16-bits set to zero
            /// </summary>
            public uint StartCharCode { get; private set; }

            /// <summary>
            /// Last character code in this group; same condition as listed above for the startCharCode
            /// </summary>
            public uint EndCharCode { get; private set; }

            /// <summary>
            /// Glyph index corresponding to the starting character code
            /// </summary>
            public uint StartGlyphCode { get; private set; }

            public NGroup(uint startCharCode, uint endCharCode, uint startGlyphCode)
            {
                StartCharCode = startCharCode;
                EndCharCode = endCharCode;
                StartGlyphCode = startGlyphCode;
            }
        }
    }
}
