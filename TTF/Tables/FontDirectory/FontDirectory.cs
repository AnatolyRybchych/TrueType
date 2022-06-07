using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrueType.TTF.Tables.FontDirectory
{
    /// <summary>
    /// The font directory, the first of the tables, 
    /// is a guide to the contents of the font file. 
    /// It provides the information required to access the data in the other tables.
    /// The directory consists of two parts:
    /// the offset subtable and the table directory.
    /// The offset subtable records the number of tables in the font and provides offset information enabling quick access to the directory tables.
    /// The table directory consists of a sequence of entries, one for each table in the font.
    /// </summary>
    public class FontDirectory
    {
        /// <summary>
        /// The number of tagged tables in the 'sfnt' follows.The table directory itself and any subtables are not included in this count. 
        /// The entries for searchRange, 
        /// entrySelector and rangeShift are used to facilitate quick binary searches of the table directory that follows. 
        /// Unless a font has a large number of tables, a sequential search will be fast enough.
        /// </summary>
        public OffsetSubtable Offset { get; private set; }

        /// <summary>
        /// The table directory follows the offset subtable.
        /// Entries in the table directory must be sorted in ascending order by tag. 
        /// Each table in the font file must have its own table directory entry.
        /// Table 5 documents the structure of the table directory.
        /// </summary>
        public ReadOnlyCollection<DirectorySubtableRecord> Directory { get; private set; }

        public FontDirectory(byte[] data, int index)
        {
            Offset = new OffsetSubtable(data, (index += OffsetSubtable.RawSize) - OffsetSubtable.RawSize);

            List<DirectorySubtableRecord> directory = new List<DirectorySubtableRecord>();
            for(int i = 0; i < Offset.NumTables; i++)
                directory.Add(new DirectorySubtableRecord(data, (index += DirectorySubtableRecord.RawSize) - DirectorySubtableRecord.RawSize));

            Directory = new ReadOnlyCollection<DirectorySubtableRecord>(directory);
        }
    }
}
