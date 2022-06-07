using TrueType.Extensions;

namespace TrueType.TTF.Tables.FontDirectory
{
    /// <summary>
    /// The number of tagged tables in the 'sfnt' follows.The table directory itself and any subtables are not included in this count. 
    /// The entries for searchRange, 
    /// entrySelector and rangeShift are used to facilitate quick binary searches of the table directory that follows. 
    /// Unless a font has a large number of tables, a sequential search will be fast enough.
    /// </summary>
    public class OffsetSubtable
    {
        public const int RawSize = 12;

        /// <summary>
        /// A tag to indicate the OFA scaler to be used to rasterize this font; see the note on the scaler type below for more information.
        /// </summary>
        public uint ScalerType { get; private set; }

        /// <summary>
        /// number of tables
        /// </summary>
        public ushort NumTables { get; private set; }

        /// <summary>
        /// (maximum power of 2 <= numTables)*16
        /// </summary>
        public ushort SearchRange { get; private set; }

        /// <summary>
        /// log2(maximum power of 2 <= numTables)
        /// </summary>
        public ushort EntrySelector { get; private set; }

        /// <summary>
        /// numTables*16-searchRange
        /// </summary>
        public ushort RangeShift { get; private set; }

        public OffsetSubtable(byte[] source, int index)
        {
            if (index + RawSize > source.Length)
                throw new ArgumentException("source length < index + OffsetTable.length");
            
            EndianBitConvater c = new EndianBitConvater(true);

            ScalerType = (uint)c.ToUInt32(source.SubArr(index, 4));
            index += 4;

            NumTables = (ushort)c.ToUInt16(source.SubArr(index, 2));
            index += 2;

            SearchRange = (ushort)c.ToUInt16(source.SubArr(index, 2));
            index += 2;

            EntrySelector = (ushort)c.ToUInt16(source.SubArr(index, 2));
            index += 2;

            RangeShift = (ushort)c.ToUInt16(source.SubArr(index, 2));
            index += 2;
        }
    }
}
