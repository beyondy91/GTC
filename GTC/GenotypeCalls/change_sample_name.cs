using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using static GTCParse.BeadArrayUtility;

namespace GTCParse
{
    public partial class GenotypeCalls
    {
        public void change_sample_name(string sample_name)
        {
            /*
            Returns:
                string: The name of the sample
            */

            MemoryStreamNotDispose new_stream = new MemoryStreamNotDispose();
            using (BinaryReader br_origin = new BinaryReader(this.gtc_stream))
            using (BinaryWriter bw_new = new BinaryWriter(new_stream))
            using (MemoryStream temp = new MemoryStream())
            using (BinaryWriter bw_temp = new BinaryWriter(temp))
            {
                Console.WriteLine("Length of original stream: " + br_origin.BaseStream.Length);
                br_origin.BaseStream.Seek(0, SeekOrigin.Begin);
                int sample_name_position = this.toc_table[GenotypeCalls.__ID_SAMPLE_NAME];
                Console.WriteLine("Sample name position: " + sample_name_position);

                // identifier
                bw_new.Write(br_origin.ReadChars(3));
                // version
                bw_new.Write(br_origin.ReadByte());
                // number of toc entries
                bw_new.Write(br_origin.ReadInt32());
                for (int toc_idx = 0; toc_idx < this.number_toc_entries; ++toc_idx)
                {
                    br_origin.ReadInt16();
                    br_origin.ReadUInt32();
                }

                long position_before_sample_name = br_origin.BaseStream.Position;
                Console.WriteLine("Position before sample name: " + position_before_sample_name);
                br_origin.ReadString();
                long position_after_sample_name = br_origin.BaseStream.Position;
                Console.WriteLine("Position after sample name: " + position_after_sample_name);
                long length_after_sample_name = this.gtc_stream.Length - br_origin.BaseStream.Position;
                Console.WriteLine("Length after sample name: " + length_after_sample_name);

                temp.Seek(0, SeekOrigin.Begin);
                bw_temp.Write(sample_name);
                bw_temp.Flush();
                long length_new_sample_name = temp.Position;
                Console.WriteLine("Length new sample name: " + length_new_sample_name);

                Dictionary<short, int> toc = new Dictionary<short, int>();

                foreach (var keyValue in this.toc_table)
                {
                    toc[keyValue.Key] = keyValue.Value;
                }

                foreach (var keyValue in toc)
                {
                    if (keyValue.Value > sample_name_position)
                    {
                        this.toc_table[keyValue.Key] += (int)(length_new_sample_name - (position_after_sample_name - position_before_sample_name));
                    }
                }

                foreach (var keyValue in this.toc_table)
                {
                    bw_new.Write(keyValue.Key);
                    bw_new.Write(keyValue.Value);
                }

                bw_new.Write(sample_name);

                bw_new.Write(br_origin.ReadBytes((int)length_after_sample_name));

                bw_new.Flush();

                Console.WriteLine("Length of new stream: " + new_stream.Length);
            }

            this.gtc_stream = new_stream;
        }
    }
}