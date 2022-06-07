using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrueType.Extensions;

namespace TrueType.TTF.Tables.Loca
{
    public class Loca
    {
        public uint[] Offsets { get; private set; }

        public Loca(byte[] source, int index, Maxp.Maxp maxp, Head.Head head):this(source, index, maxp.NumGlyphs, head.IndexToLocFormat){}

        /// <param name="numGlyphs">Maxp.NumGlyphs</param>
        /// <param name="indexToLocFormat">Head.IndexToLocFormat</param>
        public Loca(byte[] source, int index, ushort numGlyphs, short indexToLocFormat)
        {
            EndianBitConvater c = new EndianBitConvater(true);
            List<uint> offsets = new List<uint>();
            if(indexToLocFormat == 0)
            {
                if (index + numGlyphs * 2 > source.Length)
                    throw new Exception("index + Loca length > source length");
                for(int i = 0; i < numGlyphs; i++)
                {
                    offsets.Add(c.ToUInt16(source.SubArr(index, 2)));
                    index += 2;
                }
            }
            else
            {
                if (index + numGlyphs * 4 > source.Length)
                    throw new Exception("index + Loca length > source length");
                for (int i = 0; i < numGlyphs; i++)
                {
                    offsets.Add(c.ToUInt32(source.SubArr(index, 4)));
                    index += 4;
                }
            }

            Offsets = offsets.ToArray();
        }
    }
}
