using TrueType.TTF.Tables.Cmap;
using TrueType.TTF.Tables.FontDirectory;
using TrueType.TTF.Tables.Glyf;
using TrueType.TTF.Tables.Head;
using TrueType.TTF.Tables.Hhea;
using TrueType.TTF.Tables.Hmtx;
using TrueType.TTF.Tables.Loca;
using TrueType.TTF.Tables.Maxp;

namespace TrueType
{
    public class Font
    {
        public Font(byte[] data, int index)
        {
            FontDirectory fontDirectory = new FontDirectory(data, index);

            uint maxpOffset = fontDirectory.Directory.Where(dir => dir.Tag == "maxp").FirstOrDefault()?.Offset ?? throw new Exception("required table \"maxp\" not found");
            Maxp maxp = new Maxp(data, (int)(index + maxpOffset));

            uint headOffset = fontDirectory.Directory.Where(dir => dir.Tag == "head").FirstOrDefault()?.Offset ?? throw new Exception("required table \"head\" not found");
            Head head = new Head(data, (int)(index + headOffset));

            uint hheaOffset = fontDirectory.Directory.Where(dir => dir.Tag == "hhea").FirstOrDefault()?.Offset ?? throw new Exception("required table \"hhea\" not found");
            Hhea hhea = new Hhea(data, (int)(index + hheaOffset));

            uint hmtxOffset = fontDirectory.Directory.Where(dir => dir.Tag == "hmtx").FirstOrDefault()?.Offset ?? throw new Exception("required table \"hmtx\" not found");
            Hmtx hmtx = new Hmtx(data, (int)(index + hmtxOffset), hhea);

            uint locaOffset = fontDirectory.Directory.Where(dir => dir.Tag == "loca").FirstOrDefault()?.Offset ?? throw new Exception("required table \"loca\" not found");
            Loca loca = new Loca(data, (int)(index + locaOffset), maxp, head);

            uint cmapOffset = fontDirectory.Directory.Where(dir => dir.Tag == "cmap").FirstOrDefault()?.Offset ?? throw new Exception("required table \"cmap\" not found");
            Cmap cmap = new Cmap(data, (int)(index + cmapOffset));

            uint glyfOffset = fontDirectory.Directory.Where(dir => dir.Tag == "glyf").FirstOrDefault()?.Offset ?? throw new Exception("required table \"glyf\" not found");
            Glyf glyf = new Glyf(data, (int)(index + glyfOffset), loca);
        }
    }
}
