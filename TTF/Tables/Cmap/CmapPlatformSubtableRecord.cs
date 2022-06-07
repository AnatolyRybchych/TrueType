using TrueType.Extensions;

namespace TrueType.TTF.Tables.Cmap
{
    public class CmapPlatformSubtableRecord
    {
        public const int RawSize = 8;
        /// <summary>
        /// Platform identifier
        /// </summary>
        public ushort PlatformId { get; private set; }

        /// <summary>
        /// Platform-specific encoding identifier
        /// </summary>
        public ushort PlatformSpecificID { get; private set; }

        /// <summary>
        /// Offset of the mapping table
        /// </summary>
        public uint Offset { get; private set; }

        public CmapPlatformSubtableRecord(byte[] source, int index)
        {
            if (index + 18 >= source.Length)
                throw new Exception("index + CmapPlatformSubtableRecord length > source length");

            EndianBitConvater c = new EndianBitConvater(true);

            PlatformId = c.ToUInt16(source.SubArr(index, 2));
            PlatformSpecificID = c.ToUInt16(source.SubArr(index + 2, 2));
            Offset = c.ToUInt32(source.SubArr(index + 4, 4));
        }

        /// <summary>
        /// PlatformId converted to enum type
        /// </summary>
        public CmapPlatform Platform => (CmapPlatform)PlatformId;

        /// <summary>
        /// PlatformSpecificID converted to enum type
        /// !!!USE only if Platform == CmapPlatform.Unicode
        /// </summary>
        public CmapUnicodePlatform PlatformSpecificUnicode => (CmapUnicodePlatform)PlatformSpecificID;

        /// <summary>
        /// PlatformSpecificID converted to enum type
        /// !!!USE only if Platform == CmapPlatform.Windows
        /// </summary>
        public CmapWindowsPlatform PlatformSpecificWindows => (CmapWindowsPlatform)PlatformSpecificID;
    }

    public enum CmapWindowsPlatform : ushort
    {
        Symbol = 0,
        UnicodeBMPOnly = 1,
        Shift_JIS = 2,
        PRC = 3,
        BifFive = 4,
        johab = 5,
        UnicodeUCS4 = 10,
    }

    public enum CmapUnicodePlatform : ushort
    {
        /// <summary>
        /// Version 1.0 semantics
        /// </summary>
        Version1_0 = 0,

        /// <summary>
        /// Version 1.1 semantics
        /// </summary>
        Version1_1 = 1,

        /// <summary>
        /// ISO 10646 1993 semantics (deprecated)
        /// </summary>
        ISO10646 = 2,

        /// <summary>
        /// Unicode 2.0 or later semantics (BMP only)
        /// </summary>
        Unicode2_0_BMPOnly = 3,

        /// <summary>
        /// Unicode 2.0 or later semantics (non-BMP characters allowed)
        /// </summary>
        Unicode2_0 = 4,

        /// <summary>
        /// Unicode Variation Sequences
        /// </summary>
        UnicodeVariation = 5,

        /// <summary>
        /// Last Resort
        /// </summary>
        LastResort = 6,
    }

    //Platform IDs
    public enum CmapPlatform : ushort
    {
        Unicode = 0,
        Macintosh = 1,
        Reserved = 2,
        Microsoft = 3,
    }
}
