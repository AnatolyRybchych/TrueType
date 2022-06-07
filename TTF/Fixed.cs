using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrueType.TTF
{
    public class Fixed
    {
        public short Hi { get; set; }
        public ushort Low { get; set; }
        public double Double => Hi + (double)Low / ushort.MaxValue;

        public Fixed(short hi, ushort low)
        {
            this.Hi = hi;
            this.Low = low;
        }
    }
}
