using static TrueType.TTF.Tables.Glyf.Glyph;

namespace TrueType.TTF.Tables.Glyf
{
    /// <summary>
    /// https://docs.microsoft.com/en-us/typography/opentype/spec/glyf
    /// </summary>
    public class Glyf
    {
        public Glyph[] Glyphs { get; private set; }

        public Glyf(byte[] source, int index, Loca.Loca loca)
        {
            List<Glyph> glyphs = new List<Glyph>();

            try
            {
                Glyphs = loca.Offsets.Select((offset) =>
                {
                    Glyph? glyph = null;
                    try { glyph = new SimpleGlyph(source, (int)(index + offset)); }
                    catch (GlyphFormatException) { }
                    return glyph ?? new CompoundGlyph(source, (int)(index + offset));
                }).ToArray();
            }
            catch (Exception)
            {
                throw new Exception("source corrupted");
            }
        }
    }
}
