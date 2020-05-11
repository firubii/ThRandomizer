using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace THRandomizer.FileFormats
{
    public static class ECLRandomSettings
    {
        public static int[] AnimationTypes =
        {
            0,
            1,
            4,
            9
        };

        public static int[] EntityFlags =
        {
            0x0001,
            0x0002,
            0x0004,
            0x0008,
            0x0010,
            0x0020,
            //0x0040,
            0x0080,
            //0x0100,
            0x0200,
            //0x0400,
            0x0800,
            //0x1000,
            0x2000,
            //0x4000,
            //0x8000
        };

        //Specifically only the ones that won't break anything
        public static uint[] ComplexTransTypes =
        {
            0x00000001,
            0x00000002,
            0x00000004,
            0x00000008,
            0x00000010,
            0x00000020,
            0x00000040,
            0x00000100,
            0x00000200,
            0x00000400,
            0x00000800,
            0x00001000,
            0x00004000,
            0x20000000,
        };
    }

    public class ECL
    {
        /*
        public struct Header
        {
            public byte[] Magic;
            public ushort Unknown;
            public ushort IncludeLength;
            public uint IncludeOffset;
            //Padding - 4 bytes
            public uint SubCount;
            //Padding - 16 bytes
        }

        public struct IncludeList
        {
            public byte[] Magic;
            public uint Count;
            public string[] Include;
        }

        public struct Sub
        {
            public byte[] Magic;
            public uint Offset;
            //Padding - 8 bytes
            public byte[] Data;
        }

        public struct Instruction
        {
            public uint Time;
            public ushort Id;
            public ushort Size;
            public ushort ParamMask;
            public byte RankMask;
            public byte ParamCount;
            //Padding - 4 bytes
            public byte[] Data;
        }

        public Header header;
        public IncludeList anim;
        public IncludeList ecli;
        */

        //List of instructions that can be randomized
        public static ushort[] EntityInstructions =
        {
            //Spawning
            300,
            301,
            304,
            305,
            306,
            309,
            310,
            311,
            312,

            //Movement
            400,
            401,
            402,
            403,
            404,
            405,
            406,
            407,
            408,
            409,
            410,
            411,
            412,
            413,
            420,
            421,
            422,
            423,
            425,
            426,
            428,
            429,
            430,
            431,
            436,
            437,
            504,

            //Hitboxes, flags, and technical things
            502,
            503,
            511,
            512,
            515,
            565
        };

        public static ushort[] BulletInstructions =
        {
            //Basic bullet things
            602,
            603,
            604,
            605,
            606,
            607,
            608,

            //Complex bullet movement
            609,
            610,
            611,
            612,

            //Speed stuff
            624,
            625,
            626,
            627,
            628,

            //SUPER LASER PISS
            700,
            701,
            704,
            705,
            707,
            708,
            709
        };

        public static ushort[] WaitInstructions =
        {
            23,
            548
        };

        public List<long> DifficultyFlagOffsets;
        public List<long> EntityOffsets;
        public List<long> BulletOffsets;
        public List<long> WaitOffsets;

        public ECL(string path)
        {
            DifficultyFlagOffsets = new List<long>();
            EntityOffsets = new List<long>();
            BulletOffsets = new List<long>();
            WaitOffsets = new List<long>();

            using (BinaryReader reader = new BinaryReader(new FileStream(path, FileMode.Open)))
            {
                Read(reader);
            }
        }

        public void Read(BinaryReader reader)
        {
            byte[] magic = reader.ReadBytes(4);
            if (Encoding.UTF8.GetString(magic) != "SCPT")
            {
                throw new Exception("Not an ECL file");
            }

            reader.BaseStream.Seek(0x6, SeekOrigin.Begin);
            ushort inclLen = reader.ReadUInt16();
            uint inclOffs = reader.ReadUInt32();
            reader.BaseStream.Seek(0x10, SeekOrigin.Begin);
            uint subCount = reader.ReadUInt32();

            reader.BaseStream.Seek(inclOffs + inclLen, SeekOrigin.Begin);
            List<uint> subOffs = new List<uint>();
            for (int i = 0; i < subCount; i++)
            {
                subOffs.Add(reader.ReadUInt32());
            }
            
            for (int i = 0; i < subCount; i++)
            {
                reader.BaseStream.Seek(subOffs[i], SeekOrigin.Begin);
                byte[] eclhMagic = reader.ReadBytes(4);
                if (Encoding.UTF8.GetString(eclhMagic) != "ECLH")
                {
                    Console.WriteLine($"0x{reader.BaseStream.Position.ToString("X8")}: ECLH magic missing, skipping");
                    continue;
                }
                uint subDataOffs = reader.ReadUInt32();
                reader.BaseStream.Seek(subOffs[i] + subDataOffs, SeekOrigin.Begin);

                long readTo;
                if (i == subCount - 1)
                    readTo = reader.BaseStream.Length;
                else
                    readTo = subOffs[i + 1];

                while (reader.BaseStream.Position < readTo)
                {
                    long pos = reader.BaseStream.Position;

                    reader.BaseStream.Seek(0x4, SeekOrigin.Current);
                    ushort inst = reader.ReadUInt16();
                    ushort instSize = reader.ReadUInt16();

                    reader.BaseStream.Seek(0x2, SeekOrigin.Current);

                    //Ignore important instructions that would guarantee a crash
                    if (inst != 1 && inst != 10 && inst != 11 && inst != 15 && inst != 16 && inst != 40 && inst != 43 && inst != 45)
                        DifficultyFlagOffsets.Add(reader.BaseStream.Position);

                    if (EntityInstructions.Contains(inst))
                        EntityOffsets.Add(pos);
                    else if (BulletInstructions.Contains(inst))
                        BulletOffsets.Add(pos);
                    else if (WaitInstructions.Contains(inst))
                        WaitOffsets.Add(pos);

                    reader.BaseStream.Seek(pos + instSize, SeekOrigin.Begin);
                }
            }

        }

        /* 
         * I was originally going to read the entire ECL file and then rebuild it, but then I realized looking for the
         * instructions whose data I want to randomize and just storing the offsets for those is faster and more reliable,
         * so this entire function is just scrapped now but I'm keeping it here just because it's useful
        */

        /*void ReadFull(BinaryReader reader)
        {

            //Header
            header.Magic = reader.ReadBytes(4);
            if (Encoding.UTF8.GetString(header.Magic) != "SCPT")
            {
                throw new Exception("Not an ECL file");
            }
            header.Unknown = reader.ReadUInt16();
            header.IncludeLength = reader.ReadUInt16();
            header.IncludeOffset = reader.ReadUInt32();
            reader.BaseStream.Seek(0x4, SeekOrigin.Current);
            header.SubCount = reader.ReadUInt32();

            //ANIM Includes
            reader.BaseStream.Seek(header.IncludeOffset, SeekOrigin.Begin);

            Console.WriteLine($"Reading ANIM at: 0x{reader.BaseStream.Position.ToString("X8")}");
            anim = new IncludeList();
            anim.Magic = reader.ReadBytes(4);
            anim.Count = reader.ReadUInt32();

            List<string> strings = new List<string>();
            for (int i = 0; i < anim.Count; i++)
            {
                List<byte> str = new List<byte>();
                for (int t = 0; t < header.IncludeLength; t++)
                {
                    byte b = reader.ReadByte();
                    if (b == 0)
                    {
                        break;
                    }
                    else
                    {
                        str.Add(b);
                    }
                }
                strings.Add(Encoding.UTF8.GetString(str.ToArray()));
            }
            anim.Include = strings.ToArray();
            while (reader.BaseStream.Position.ToString("X").Last() != '0'
                && reader.BaseStream.Position.ToString("X").Last() != '4'
                && reader.BaseStream.Position.ToString("X").Last() != '8'
                && reader.BaseStream.Position.ToString("X").Last() != 'C')
                reader.BaseStream.Seek(1, SeekOrigin.Current);

            Console.WriteLine($"Reading ECLI at: 0x{reader.BaseStream.Position.ToString("X8")}");
            //ECLI Includes
            ecli = new IncludeList();
            ecli.Magic = reader.ReadBytes(4);
            ecli.Count = reader.ReadUInt32();

            strings = new List<string>();
            for (int i = 0; i < ecli.Count; i++)
            {
                List<byte> str = new List<byte>();
                for (int t = 0; t < header.IncludeLength; t++)
                {
                    byte b = reader.ReadByte();
                    if (b == 0)
                    {
                        break;
                    }
                    else
                    {
                        str.Add(b);
                    }
                }
                strings.Add(Encoding.UTF8.GetString(str.ToArray()));
            }
            ecli.Include = strings.ToArray();
            while (reader.BaseStream.Position.ToString("X").Last() != '0'
                && reader.BaseStream.Position.ToString("X").Last() != '4'
                && reader.BaseStream.Position.ToString("X").Last() != '8'
                && reader.BaseStream.Position.ToString("X").Last() != 'C')
                reader.BaseStream.Seek(1, SeekOrigin.Current);

            Console.WriteLine($"Reading Sub Offsets at: 0x{reader.BaseStream.Position.ToString("X8")}");
            List<uint> subOffs = new List<uint>();
            for (int i = 0; i < header.SubCount; i++)
            {
                subOffs.Add(reader.ReadUInt32());
            }

            Console.WriteLine($"Reading Sub Names at: 0x{reader.BaseStream.Position.ToString("X8")}");
            List<string> subNames = new List<string>();
            for (int i = 0; i < header.SubCount; i++)
            {
                List<byte> str = new List<byte>();
                for (int t = 0; t < reader.BaseStream.Length; t++)
                {
                    byte b = reader.ReadByte();
                    if (b == 0)
                    {
                        break;
                    }
                    else
                    {
                        str.Add(b);
                    }
                }
                subNames.Add(Encoding.UTF8.GetString(str.ToArray()));
            }

            while (reader.BaseStream.Position.ToString("X").Last() != '0'
                && reader.BaseStream.Position.ToString("X").Last() != '4'
                && reader.BaseStream.Position.ToString("X").Last() != '8'
                && reader.BaseStream.Position.ToString("X").Last() != 'C')
                reader.BaseStream.Seek(1, SeekOrigin.Current);


        }*/
    }
}
