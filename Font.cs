using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrueType.TTF.Tables.Cmap;
using TrueType.TTF.Tables.FontDirectory;

namespace TrueType
{
    public class Font
    {
        public Font(byte[] data, int index)
        {
            FontDirectory fontDirectory = new FontDirectory(data, index);

            uint cmapOffset = fontDirectory.Directory.Where(dir => dir.Tag == "cmap").FirstOrDefault()?.Offset ?? throw new Exception("cmap not found");
            Cmap cmap = new Cmap(data, (int)(index + cmapOffset));
        }
    }
}
