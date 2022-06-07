
using TrueType.Extensions;
using TrueType.TTF.Tables.Cmap.Format;

namespace TrueType.TTF.Tables.Cmap
{

    /// <summary>
    /// https://developer.apple.com/fonts/TrueType-Reference-Manual/RM06/Chap6cmap.html
    /// </summary>
    public class Cmap
    {
        public const string Tag = "cmap";

        /// <summary>
        /// Version number (Set to zero)
        /// </summary>
        public ushort Version { get; private set; }

        /// <summary>
        /// Number of encoding subtables
        /// </summary>
        public ushort NumberOfSubtables { get; private set; }

        public CmapPlatformSubtableRecord[] Platforms { get; private set; }
        public CmapFormat Format { get; private set; }

        public Cmap(byte[] source, int index)
        {
            EndianBitConvater c = new EndianBitConvater(true);

            int currByte = index;

            if (currByte + 4 >= source.Length) throw new Exception("Data is corrupted");

            Version = c.ToUInt16(source.SubArr(currByte, 2));
            currByte += 2;

            NumberOfSubtables = c.ToUInt16(source.SubArr(currByte, 2));
            currByte += 2;

            if (currByte + NumberOfSubtables * CmapPlatformSubtableRecord.RawSize >= source.Length) throw new Exception("Data is corrupted");

            List<CmapPlatformSubtableRecord> platforms = new List<CmapPlatformSubtableRecord>();
            for (int platform = 0; platform < NumberOfSubtables; platform++)
            {
                platforms.Add(new CmapPlatformSubtableRecord(source, currByte));
                currByte += CmapPlatformSubtableRecord.RawSize;
            }
            Platforms = platforms.ToArray();

            if (currByte + 2 >= source.Length) throw new Exception("Data is corrupted");
            ushort format;

            format = c.ToUInt16(source.SubArr(currByte, 2));
            currByte += 2;

            switch (format)
            {
                case 0:
                    Format = new CmapFormat0(source, currByte);
                    break;
                case 2:
                    Format = new CmapFormat2(source, currByte);
                    break;
                case 4:
                    Format = new CmapFormat4(source, currByte);
                    break;
                case 6:
                    Format = new CmapFormat6(source, currByte);
                    break;
                case 8:
                    Format = new CmapFormat8(source, currByte);
                    break;
                case 10:
                    Format = new CmapFormat10(source, currByte);
                    break;
                case 12:
                    Format = new CmapFormat12(source, currByte);
                    break;
                case 13:
                    Format = new CmapFormat13(source, currByte);
                    break;
                case 14:
                    Format = new CmapFormat14(source, currByte);
                    break;
                default:
                    throw new Exception("incompatible cmap format");
            }
        }
    }
}
