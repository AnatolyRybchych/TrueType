using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrueType.TTF.Tables.Glyf
{
    public class CompoundGlyph : Glyph
    {
        public CompoundGlyph(byte[] source, int index) : base(source, index)
        {
            index += GlyphDescriptionSize;

            if (NumberOfCountours >= 0) throw new GlyphFormatException();

            if (index + NumberOfCountours * 2 + 2 > source.Length)
                SizeError();


        }
    }
}
