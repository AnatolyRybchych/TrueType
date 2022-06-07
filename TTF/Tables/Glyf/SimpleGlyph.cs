using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrueType.Extensions;

namespace TrueType.TTF.Tables.Glyf
{
    public class SimpleGlyph : Glyph
    {
        public Point[] Points { get; private set; }
        public int[] EndPointsOfCountours { get; private set; }
        public byte[] Instructions { get; private set; }

        public const byte ON_CURVE_POINT                        = 0b10000000;
        public const byte X_SHORT_VECTOR                        = 0b01000000;
        public const byte Y_SHORT_VECTOR                        = 0b00100000;
        public const byte REPEAT_FLAG                           = 0b00010000;
        public const byte X_IS_SAME_OR_POSITIVE_X_SHORT_VECTOR  = 0b00001000;
        public const byte Y_IS_SAME_OR_POSITIVE_Y_SHORT_VECTOR  = 0b00000100;

        public SimpleGlyph(byte[] source, int index) : base(source, index)
        {
            index += GlyphDescriptionSize;

            if (NumberOfCountours < 0) throw new GlyphFormatException();

            if (index + NumberOfCountours * 2 + 2 > source.Length)
                SizeError();

            EndianBitConvater c = new EndianBitConvater(true);

            List<uint> endpCountours = new List<uint>();
            for(int i =0; i < NumberOfCountours; i++)
            {
                endpCountours.Add(c.ToUInt16(source.SubArr(index, 2)));
                index += 2;
            }
            EndPointsOfCountours = endpCountours.Select(i=>(int)i).ToArray();

            ushort instrLen = c.ToUInt16(source.SubArr(index, 2));
            index += 2;

            if (index + instrLen > source.Length)
                SizeError();

            Instructions = source.SubArr(index, instrLen);
            index += instrLen;

            List<byte> flags = new List<byte>();

            int pointsCount = EndPointsOfCountours.LastOrDefault();
            for(int i = 0; i < pointsCount; i++)
            {
                byte flag = source[index++];
                if((flag & REPEAT_FLAG) != 0)
                {
                    int repetitions = source[index++];
                    for (int j = 0; j < repetitions; j++)
                        flags.Add(flag);
                    continue;
                }
                flags.Add(flag);
            }

            List<int> pxs = new List<int>();
            List<int> pys = new List<int>();
            List<bool> isOnCurve = new List<bool>();

            for (int i = 0; i < pointsCount; i++)
            {
                byte flag = flags[i];
                if((flag & X_SHORT_VECTOR) != 0)
                {
                    if((flag & X_IS_SAME_OR_POSITIVE_X_SHORT_VECTOR) != 0)
                        pxs.Add(source[index++]);
                    else
                        pxs.Add(-source[index++]);
                }
                else
                {
                    if ((flag & X_IS_SAME_OR_POSITIVE_X_SHORT_VECTOR) != 0)
                        pxs.Add(pxs.LastOrDefault());
                    else
                        pxs.Add(c.ToInt16(source.SubArr((index += 2) - 2, 2)));
                }
            }

            for (int i = 0; i < pointsCount; i++)
            {
                byte flag = flags[i];
                if ((flag & Y_SHORT_VECTOR) != 0)
                {
                    if ((flag & Y_IS_SAME_OR_POSITIVE_Y_SHORT_VECTOR) != 0)
                        pys.Add(source[index++]);
                    else
                        pys.Add(-source[index++]);
                }
                else
                {
                    if ((flag & Y_IS_SAME_OR_POSITIVE_Y_SHORT_VECTOR) != 0)
                        pys.Add(pys.LastOrDefault());
                    else
                        pys.Add(c.ToInt16(source.SubArr((index += 2) - 2, 2)));
                }
            }

            foreach (var flag in flags)
                isOnCurve.Add((flag & ON_CURVE_POINT) != 0);

            Points = pxs.Select((x, i) => new Point(x, pys[i], isOnCurve[i])).ToArray();
        }

        public class Point
        {
            public int X { get; private set; }
            public int Y { get; private set; }
            public bool OnCurve { get; private set; }
            

            public Point(int x, int y, bool onCurve)
            {
                OnCurve = onCurve;
                X = x;
                Y = y;
            }
        }
    }
}
