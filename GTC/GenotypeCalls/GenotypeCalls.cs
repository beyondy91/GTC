using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using static GTCParse.BeadArrayUtility;

namespace GTCParse
{
    public partial class GenotypeCalls
    {

        public static string[] code2genotype = {
            "NC",
            "AA",
            "AB",
            "BB",
            "NULL",
            "A",
            "B",
            "AAA",
            "AAB",
            "ABB",
            "BBB",
            "AAAA",
            "AAAB",
            "AABB",
            "ABBB",
            "BBBB",
            "AAAAA",
            "AAAAB",
            "AAABB",
            "AABBB",
            "ABBBB",
            "BBBBB",
            "AAAAAA",
            "AAAAAB",
            "AAAABB",
            "AAABBB",
            "AABBBB",
            "ABBBBB",
            "BBBBBB",
            "AAAAAAA",
            "AAAAAAB",
            "AAAAABB",
            "AAAABBB",
            "AAABBBB",
            "AABBBBB",
            "ABBBBBB",
            "BBBBBBB",
            "AAAAAAAA",
            "AAAAAAAB",
            "AAAAAABB",
            "AAAAABBB",
            "AAAABBBB",
            "AAABBBBB",
            "AABBBBBB",
            "ABBBBBBB",
            "BBBBBBBB"
        };

        const char NC = (char)0;
        const char AA = (char)1;
        const char AB = (char)2;
        const char BB = (char)3;


        public static short __ID_NUM_SNPS = 1;
        public static short __ID_PLOIDY = 2;
        public static short __ID_PLOIDY_TYPE = 3;
        public static short __ID_SAMPLE_NAME = 10;
        public static short __ID_SAMPLE_PLATE = 11;
        public static short __ID_SAMPLE_WELL = 12;
        public static short __ID_CLUSTER_FILE = 100;
        public static short __ID_SNP_MANIFEST = 101;
        public static short __ID_IMAGING_DATE = 200;
        public static short __ID_AUTOCALL_DATE = 201;
        public static short __ID_AUTOCALL_VERSION = 300;
        public static short __ID_NORMALIZATION_TRANSFORMS = 400;
        public static short __ID_CONTROLS_X = 500;
        public static short __ID_CONTROLS_Y = 501;
        public static short __ID_RAW_X = 1000;
        public static short __ID_RAW_Y = 1001;
        public static short __ID_GENOTYPES = 1002;
        public static short __ID_BASE_CALLS = 1003;
        public static short __ID_GENOTYPE_SCORES = 1004;
        public static short __ID_SCANNER_DATA = 1005;
        public static short __ID_CALL_RATE = 1006;
        public static short __ID_GENDER = 1007;
        public static short __ID_LOGR_DEV = 1008;
        public static short __ID_GC10 = 1009;
        public static short __ID_GC50 = 1011;
        public static short __ID_B_ALLELE_FREQS = 1012;
        public static short __ID_LOGR_RATIOS = 1013;
        public static short __ID_PERCENTILES_X = 1014;
        public static short __ID_PERCENTILES_Y = 1015;
        public static short __ID_SLIDE_IDENTIFIER = 1016;

        public static int[] supported_version = { 3, 4, 5 };



        /*
            Class to    Class to parse gtc files as produced by Illumina AutoConvert
                and AutoCall software.

            Attributes:
                supported_versions(int[]) : Supported file versions as a list of integers
        */
        public int version;
        public int number_toc_entries;
        public Dictionary<short, int> toc_table;
        public MemoryStreamNotDispose gtc_stream;

        public GenotypeCalls(BinaryReader gtc_br, bool ignore_version = false, bool check_write_complete = true)
        {

            /*
            Constructor

            Args:
                filename(string): GTC filename
                ignore_version(bool): boolean to ignore automated checks on
                                file version, not recommended(default: False)

            Returns:
                GenotypeCalls
            */
            this.gtc_stream = new MemoryStreamNotDispose();
            gtc_br.BaseStream.CopyTo(this.gtc_stream);
            BinaryReader gtc_handle = new BinaryReader(this.gtc_stream);
            gtc_handle.BaseStream.Seek(0, SeekOrigin.Begin);

            string identifier = new string(gtc_handle.ReadChars(3));
            if (identifier != "gtc")
            {
                throw new Exception("GTC format error: bad format identifier");
            }
            this.version = (int)(byte)read_byte(gtc_handle);
            if (!new List<int>(GenotypeCalls.supported_version).Contains(version))
            {
                throw new Exception("Unsupported GTC File version (" + this.version.ToString() + ")");
            }
            this.number_toc_entries = (int)read_int(gtc_handle);

            /*
                # Parse the table of contents and map the toc entry
                # to the lookup
            */
            this.toc_table = new Dictionary<short, int>();
            short id;
            int offset;
            for (int toc_idx = 0; toc_idx < this.number_toc_entries; ++toc_idx)
            {
                id = gtc_handle.ReadInt16();
                offset = (int)gtc_handle.ReadUInt32();
                this.toc_table[id] = offset;
            }

            if (check_write_complete && !this.is_write_complete())
            {
                throw new Exception("GTC file is incomplete");
            }
        }

    }
}
