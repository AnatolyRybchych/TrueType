using TrueType.Extensions;

namespace TrueType.TTF.Tables.Glyf
{
    public abstract class Glyph
    {
        public const int GlyphDescriptionSize = 10;

        public short NumberOfCountours { get; private set; }
        public short XMin { get; private set; }
        public short YMin { get; private set; }
        public short XMax { get; private set; }
        public short YMax { get; private set; }

        public Glyph(byte[] source, int index)
        {
            if (index + GlyphDescriptionSize >= source.Length)
                SizeError();

            EndianBitConvater c = new EndianBitConvater(true);

            NumberOfCountours = c.ToInt16(source.SubArr(index + 0, 2));
            XMin = c.ToInt16(source.SubArr(index + 2, 2));
            YMin = c.ToInt16(source.SubArr(index + 4, 2));
            XMax = c.ToInt16(source.SubArr(index + 6, 2));
            YMax = c.ToInt16(source.SubArr(index + 8, 2));
        }

        protected void SizeError()
        {
            throw new Exception($"index + {GetType().Name} length > source length");
        }

        public class GlyphFormatException : Exception { }
    }
}
