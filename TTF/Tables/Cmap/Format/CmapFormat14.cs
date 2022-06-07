using TrueType.Extensions;
using static TrueType.TTF.Tables.Cmap.Format.CmapFormat14.VariationSelectorRecord;

namespace TrueType.TTF.Tables.Cmap.Format
{
    public class CmapFormat14 : CmapFormat
    {
        /// <summary>
        /// Byte length of this subtable (including this header)
        /// </summary>
        public uint Length { get; private set; }

        /// <summary>
        /// Number of variation Selector Records
        /// </summary>
        public uint NumVarSelectorRecords { get; private set; }
        public VariationSelectorRecord[] Records { get; private set; }

        public CmapFormat14(byte[] source, int index) : base(14)
        {
            EndianBitConvater c = new EndianBitConvater(true);

            if (index + 8 >= source.Length)
                SizeError();

            List<VariationSelectorRecord> records = new List<VariationSelectorRecord>();

            Length = c.ToUInt32(source.SubArr(index, 4));
            index += 4;

            NumVarSelectorRecords = c.ToUInt32(source.SubArr(index, 4));
            index += 4;

            for (int varRec = 0; varRec < NumVarSelectorRecords; varRec++)
            {
                if (index + 11 >= source.Length)
                    SizeError();

                uint24 varSelector;
                uint defaultUVSOffset;
                uint nonDefaultUVSOffset;
                DefaultUVS? defaultUVS = null;
                NonDefaultUVS? nonDefaultUVS = null;

                varSelector = c.ToUInt32(source.SubArr(index, 4));
                index += 3;

                defaultUVSOffset = c.ToUInt32(source.SubArr(index, 4));
                index += 4;

                nonDefaultUVSOffset = c.ToUInt32(source.SubArr(index, 4));
                index += 4;

                if (defaultUVSOffset != 0)
                {
                    if (index + 4 >= source.Length)
                        SizeError();

                    uint offs = defaultUVSOffset;
                    uint recordsCount;
                    List<DefaultUVS.Record> rs = new List<DefaultUVS.Record>();

                    recordsCount = c.ToUInt32(source.SubArr((int)offs, 4));
                    offs += 4;

                    if (offs + recordsCount * DefaultUVS.Record.RawSize >= source.Length)
                        SizeError();

                    for (int r = 0; r < recordsCount; r++)
                    {
                        rs.Add(
                            new DefaultUVS.Record(
                                c.ToUInt32(source.SubArr((int)offs, 4)),
                                source[offs + 4]
                            )
                        );
                        offs += DefaultUVS.Record.RawSize;
                    }

                    defaultUVS = new DefaultUVS(recordsCount, rs.ToArray());
                }

                if (nonDefaultUVSOffset != 0)
                {
                    if (index + 4 >= source.Length)
                        SizeError();

                    uint offs = defaultUVSOffset;
                    uint recordsCount;
                    List<NonDefaultUVS.Record> rs = new List<NonDefaultUVS.Record>();

                    recordsCount = c.ToUInt32(source.SubArr((int)offs, 4));
                    offs += 4;

                    if (offs + recordsCount * NonDefaultUVS.Record.RawSize >= source.Length)
                        SizeError();

                    for (int r = 0; r < recordsCount; r++)
                    {
                        rs.Add(
                            new NonDefaultUVS.Record(
                                c.ToUInt32(source.SubArr((int)offs, 4)),
                                c.ToUInt16(source.SubArr((int)offs + 4, 2))
                            )
                        );
                        offs += DefaultUVS.Record.RawSize;
                    }

                    nonDefaultUVS = new NonDefaultUVS(recordsCount, rs.ToArray());
                }

                records.Add(
                    new CmapFormat14.VariationSelectorRecord(
                        varSelector,
                        defaultUVSOffset,
                        nonDefaultUVSOffset,
                        defaultUVS,
                        nonDefaultUVS
                    )
                );
            }

            Records = records.ToArray();
        }


        public class VariationSelectorRecord
        {
            /// <summary>
            /// Variation selector
            /// </summary>
            public uint24 VarSelector { get; private set; }

            /// <summary>
            /// Offset to Default UVS Table. May be 0.
            /// </summary>
            public uint DefaultUVSOffset { get; private set; }

            /// <summary>
            /// Offset to Non-Default UVS Table. May be 0
            /// </summary>
            public uint NonDefaultUVSOffset { get; private set; }

            /// <summary>
            /// DafaultUvs or NonDefaultUvs soulde not null
            /// </summary>
            public DefaultUVS? DefaultUvs { get; private set; }

            /// <summary>
            /// DafaultUvs or NonDefaultUvs soulde not null
            /// </summary>
            public NonDefaultUVS? NonDefaultUvs { get; private set; }

            public VariationSelectorRecord(uint24 varSelector, uint defaultUVSOffset, uint nonDefaultUVSOffset, DefaultUVS? defaultUvs, NonDefaultUVS? nonDefaultUvs)
            {
                if (defaultUVSOffset != 0 && nonDefaultUVSOffset != 0) throw new Exception("defaultUVSOffset != 0 && nonDefaultUVSOffset != 0");
                if (defaultUVSOffset == 0 && defaultUvs != null) throw new Exception("defaultUVSOffset == 0 && defaultUvs != null");
                if (nonDefaultUVSOffset == 0 && defaultUvs != null) throw new Exception("nonDefaultUVSOffset == 0 && defaultUvs != null");

                VarSelector = varSelector;
                DefaultUVSOffset = defaultUVSOffset;
                NonDefaultUVSOffset = nonDefaultUVSOffset;
                DefaultUvs = defaultUvs;
                NonDefaultUvs = nonDefaultUvs;
            }

            public struct NonDefaultUVS
            {
                public uint RecordsCount { get; private set; }
                public Record[] Records { get; private set; }

                public NonDefaultUVS(uint recordsCount, Record[] records)
                {
                    if (records.Length != recordsCount) throw new Exception("NotDefaultUVS records.Length != recordsCount");

                    RecordsCount = recordsCount;
                    Records = records;
                }

                public class Record
                {
                    public const int RawSize = 5;
                    /// <summary>
                    /// Base Unicode value of the UVS
                    /// </summary>
                    public uint24 UnicodeValue { get; private set; }

                    /// <summary>
                    /// Glyph ID of the UVS
                    /// </summary>
                    public ushort GlyphId { get; private set; }

                    public Record(uint24 unicodeValue, ushort glyphId)
                    {
                        UnicodeValue = unicodeValue;
                        GlyphId = glyphId;
                    }
                }
            }

            public struct DefaultUVS
            {
                public uint RecordsCount { get; private set; }
                public Record[] Records { get; private set; }

                public DefaultUVS(uint recordsCount, Record[] records)
                {
                    if (records.Length != recordsCount) throw new Exception("DafaultUVS records.Length != recordsCount");

                    RecordsCount = recordsCount;
                    Records = records;
                }

                /// <summary>
                /// A Default UVS Table is simply a range-compressed list of Unicode scalar values, 
                /// representing the base characters of the default UVSes which use the varSelector of the associated Variation Selector Record.
                /// 
                /// (startUnicodeValue + additionalCount) must not exceed 0xFFFFFF.
                /// </summary>
                public class Record
                {
                    public const int RawSize = 4;
                    /// <summary>
                    /// First value in this range
                    /// </summary>
                    public uint24 StartUnicodeValue { get; private set; }

                    /// <summary>
                    /// Number of additional values in this range
                    /// </summary>
                    public byte AdditionalCount { get; private set; }

                    public Record(uint24 startUnicodeValue, byte additionalCount)
                    {
                        StartUnicodeValue = startUnicodeValue;
                        AdditionalCount = additionalCount;
                    }
                }
            }


            public struct uint24
            {
                public byte hiByte;
                public byte midByte;
                public byte lowByte;

                public uint24(byte hiByte, byte midByte, byte liwByte)
                {
                    this.hiByte = hiByte;
                    this.midByte = midByte;
                    this.lowByte = liwByte;
                }

                public uint24(uint val)
                {
                    val = val & 0x00ffffff;
                    hiByte = unchecked((byte)(val >> 16));
                    midByte = unchecked((byte)((val >> 8) & 0x000000ff));
                    lowByte = unchecked((byte)(val & 0x000000ff));
                }

                public static implicit operator uint(uint24 val) => ((uint)val.hiByte << 16) | ((uint)val.midByte << 8) | ((uint)val.lowByte);
                public static implicit operator uint24(uint val) => new uint24(val);
            }
        }
    }
}
