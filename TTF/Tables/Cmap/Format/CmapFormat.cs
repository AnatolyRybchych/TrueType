using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrueType.TTF.Tables.Cmap.Format
{
    public class CmapFormat
    {
        public ushort Format { get; private set; }

        public CmapFormat(ushort format)
        {
            Format = format;
        }

        public CmapFormat0? Format0 => this as CmapFormat0;
        public CmapFormat2? Format2 => this as CmapFormat2;
        public CmapFormat4? Format4 => this as CmapFormat4;
        public CmapFormat6? Format6 => this as CmapFormat6;
        public CmapFormat8? Format8 => this as CmapFormat8;
        public CmapFormat10? Format10 => this as CmapFormat10;
        public CmapFormat12? Format12 => this as CmapFormat12;
        public CmapFormat13? Format13 => this as CmapFormat13;
        public CmapFormat14? Format14 => this as CmapFormat14;

        protected void SizeError() => throw new Exception($"index + CmapFormat{Format} length > source length");
    }
}
