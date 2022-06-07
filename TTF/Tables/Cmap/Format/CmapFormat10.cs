using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrueType.Extensions;

namespace TrueType.TTF.Tables.Cmap.Format
{
    public class CmapFormat10 : CmapFormat
    {
        /// <summary>
        /// Set to 10
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
        /// First character code covered
        /// </summary>
        public uint StartCharCode { get; private set; }

        /// <summary>
        /// Number of character codes covered
        /// </summary>
        public uint NumChars { get; private set; }

        /// <summary>
        /// Array of glyph indices for the character codes covered
        /// </summary>
        public uint[] Glyphs { get; private set; }

        public CmapFormat10(byte[] source, int index) : base(10)
        {
            EndianBitConvater c = new EndianBitConvater(true);

            if (index + 18 >= source.Length)
                SizeError();

            List<uint> glyphs = new List<uint>();


            Reserved = c.ToUInt16(source.SubArr(index, 2));
            index += 2;

            Length = c.ToUInt32(source.SubArr(index, 4));
            index += 4;

            Language = c.ToUInt32(source.SubArr(index, 4));
            index += 4;

            StartCharCode = c.ToUInt32(source.SubArr(index, 4));
            index += 4;

            NumChars = c.ToUInt32(source.SubArr(index, 4));
            index += 4;

            if (index + NumChars * 2 >= source.Length)
                SizeError();

            for (int ch = 0; ch < NumChars; ch++)
            {
                glyphs.Add(c.ToUInt16(source.SubArr(index, 2)));
                index += 2;
            }

            Glyphs = glyphs.ToArray();
        }
    }
}
