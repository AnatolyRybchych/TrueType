using System.Text;
using TrueType.Extensions;

namespace TrueType.TTF.Tables.FontDirectory
{
    /// <summary>
    /// The table directory follows the offset subtable.
    /// Entries in the table directory must be sorted in ascending order by tag. 
    /// Each table in the font file must have its own table directory entry.
    /// Table 5 documents the structure of the table directory.
    /// </summary>
    public class DirectorySubtableRecord
    {
        public const int RawSize = 16;
        
        /// <summary>
        /// 4-byte identifier
        /// </summary>
        public string Tag { get; private set; }
        /// <summary>
        /// checksum for this table
        /// </summary>
        public uint CheckSum { get; private set; }
        /// <summary>
        /// offset from beginning of sfnt
        /// </summary>
        public uint Offset { get; private set; }
        /// <summary>
        /// length of this table in byte (actual length not padded length)
        /// </summary>
        public uint Length { get; private set; }

        public DirectorySubtableRecord(byte[] source, int index)
        {
            if (OffsetSubtable.RawSize + RawSize >= source.Length)
                throw new Exception("data length < DirectorySubtableRecord length");

            EndianBitConvater c = new EndianBitConvater(true);

            Tag = Encoding.ASCII.GetString(source, index, 4);
            index += 4;

            CheckSum = c.ToUInt32(source.SubArr(index, 4));
            index += 4;

            Offset = c.ToUInt32(source.SubArr(index, 4));
            index += 4;

            Length = c.ToUInt32(source.SubArr(index, 4));
            index += 4;
        }
    }
}
