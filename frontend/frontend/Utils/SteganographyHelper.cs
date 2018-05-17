using System;
using System.IO;

namespace frontend.Utils
{
    static class SteganographyHelper
    {
        public static void ImplantFile(string basePath, string implantPath, string outputPath)
        {
            using (BinaryReader baseFile = new BinaryReader(new FileStream(basePath, FileMode.Open)))
            using (BinaryReader implantFile = new BinaryReader(new FileStream(implantPath, FileMode.Open)))
            {
                // Check if bmp file is long enough to store implant file size and content
                if (baseFile.BaseStream.Length < implantFile.BaseStream.Length * 8 + 118)
                {
                    Console.WriteLine("BMP file is too small.");
                    return;
                }

                BinaryWriter output = new BinaryWriter(new FileStream(outputPath, FileMode.Create));

                // Copy 54 byte bmp header
                output.Write(baseFile.ReadBytes(54));

                // Write implant file size
                for (int i = 0; i < 64; i++)
                {
                    if ((implantFile.BaseStream.Length & (1 << (63 - i))) > 0)
                    {
                        output.Write((byte)(baseFile.ReadByte() | 0x0001));
                    }
                    else
                    {
                        output.Write((byte)(baseFile.ReadByte() & 0xfffe));
                    }
                }

                // Write implant file content
                for (long i = 0; i < implantFile.BaseStream.Length; i++)
                {
                    byte value = implantFile.ReadByte();
                    for (int j = 0; j < 8; j++)
                    {
                        if ((value & (1 << (7 - j))) > 0)
                        {
                            output.Write((byte)(baseFile.ReadByte() | 0x0001));
                        }
                        else
                        {
                            output.Write((byte)(baseFile.ReadByte() & 0xfffe));
                        }
                    }
                }

                // Copy remaining bytes
                output.Write(baseFile.ReadBytes((int)(baseFile.BaseStream.Length - baseFile.BaseStream.Position)));
                output.Close();
            }
        }

        public static void ExtractFile(string inputPath, string outputpath)
        {
            using (BinaryReader input = new BinaryReader(new FileStream(inputPath, FileMode.Open)))
            using (BinaryWriter output = new BinaryWriter(new FileStream(outputpath, FileMode.Create)))
            {
                int dataLength = 0;

                // Read implanted file size
                input.BaseStream.Position = 54;
                for (int i = 0; i < 64; i++)
                {
                    if ((input.ReadByte() & 0x0001) > 0)
                        dataLength |= (int)(1 << 63 - i);
                }

                // Read implanted file content
                for (long i = 0; i < dataLength; i++)
                {
                    byte value = 0;
                    for (int j = 0; j < 8; j++)
                    {
                        if ((input.ReadByte() & 0x0001) > 0)
                            value |= (byte)(1 << 7 - j);
                    }
                    output.Write(value);
                }
            }
        }
    }
}
