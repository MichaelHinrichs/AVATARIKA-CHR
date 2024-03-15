//Written for AVATARIKA https://store.steampowered.com/app/583740/
using System.Collections.Generic;
using System.IO;
using System.Numerics;

namespace AVATARIKA_CHR
{
    public class CHR2
    {
        public static BinaryReader br;
        private List<Block2> block2;
        private List<Vector3> faces;
        private List<string> skel;

        private static CHR2 Read(string CHRFile)
        {
            BinaryReader br = new(File.OpenRead(CHRFile));

            if (new string(br.ReadChars(4)) != "chr2")
                throw new System.Exception("Wrong file. Input a chr file from AVATARIKA.");

            Block1 block1 = Block1.ReadBlock1();
            return new CHR2
            {
                block2 = Block2.ReadBlock2(block1.pointstart, block1.pointsize),
                faces = ReadBlock3(block1.facestart, block1.facesize),
                skel = ReadBlock4(block1.skelstart, block1.skelsize)
                //Todo: blocks 5 & 6
            };
        }

        public class Block1
        {
            public int pointstart;
            public int pointsize;
            public int facestart;
            public int facesize;
            public int skelstart;
            public int skelsize;
            public int endstart;
            public int endsize;
            public int veryendstart;
            public int veryendsize;
            public static Block1 ReadBlock1()
            {
                br.BaseStream.Position = 0x1c;

                Block1 block1 = new()
                {
                    pointstart = br.ReadInt32(),
                    pointsize = br.ReadInt32()
                };
                br.ReadInt32();
                block1.facestart = br.ReadInt32();
                block1.facesize = br.ReadInt32();
                br.ReadInt32();
                block1.skelstart = br.ReadInt32();
                block1.skelsize = br.ReadInt32();
                br.ReadInt32();
                block1.endstart = br.ReadInt32();
                block1.endsize = br.ReadInt32();
                br.ReadInt32();
                block1.veryendstart = br.ReadInt32();
                block1.veryendsize = br.ReadInt32();
                return block1;
            }
        }

        public class Block2
        {
            public Vector3 points;
            public Vector3 normals;
            public Vector2 uvs;

            public static List<Block2> ReadBlock2(int pointstart, int pointsize)
            {
                br.BaseStream.Position = pointstart;
                List<Block2> block2 = new();
                while (br.BaseStream.Position < pointsize - pointstart)
                {
                    block2.Add(new()
                    {
                        points = new Vector3(br.ReadInt32(), br.ReadInt32(), br.ReadInt32()),
                        normals = new Vector3(br.ReadInt32(), br.ReadInt32(), br.ReadInt32()),
                        uvs = new Vector2(br.ReadInt32(), br.ReadInt32())
                    });
                    br.ReadChars(8);//Unknown. Maybe rigging data.
                }
                return block2;
            }
        }

        public static List<Vector3> ReadBlock3(int facestart, int facesize)
        {
            br.BaseStream.Position = facestart;
            List<Vector3> block3 = new();
            while (br.BaseStream.Position < facesize - facestart)
            {
                block3.Add(new Vector3(br.ReadInt32(), br.ReadInt32(), br.ReadInt32()));
            }
            return block3;
        }

        public static List<string> ReadBlock4(int skelstart, int skelsize)
        {
            br.BaseStream.Position = skelstart;
            int boneCount = br.ReadInt32();
            List<string> block4 = new();
            for (int i = 0; i < boneCount; i++)
            {
                block4.Add(br.ReadString());
            }
            return block4;
        }
    }
}
