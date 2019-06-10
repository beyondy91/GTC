using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Amazon.DynamoDBv2.DocumentModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static DynamoIO.Dynamo;
using Preferences;
using Plink;
using static Plink.PlinkS3;
using Progress;

namespace CalculationResult
{
    public partial class Calculate
    {
        public static CalcResult calcGenotype(string code,
            string sn,
            string tableRegion = null,
            string tableName = null,
            string bucketRegion = null,
            string bucketName = null,
            string bucketRegionBIM = null,
            string bucketNameBIM = null,
            Plink.Plink plink = null)
        {
            var preference = new Preference().preference;
            JArray metas = new JArray();
            string stage = String.Format("{0}-{1}-{2}", code, "genotype", "calculation");
            ProgressWriter.writer(sn: sn, stage: stage, status: "start").Wait();

            foreach (var meta in readDynamoTable(tableRegion: tableRegion?? preference["Dynamo"]["MetaGenotype"]["Region"].ToString(),
                tableName: tableName?? preference["Dynamo"]["MetaGenotype"]["Name"].ToString(),
                hashKey: code,
                queryFilter: new QueryFilter()).Select(x => JObject.Parse(x.ToJson())))
            {
                metas.Add(meta);
            }

            List<string> snpnames = metas.Select(meta => meta["var"].ToString()).ToList();

            plink = plink?? readPlinkS3(bucketRegion: bucketRegion?? preference["S3"]["Plink"]["Region"].ToString(),
                bucketName: bucketName?? preference["S3"]["Plink"]["Name"].ToString(),
                keyFAM: sn + ".fam",
                keyBED: sn + ".bed",
                keyBIMFile: sn + "_bim.txt",
                bucketRegionBIM: bucketRegionBIM?? preference["S3"]["BPM"]["Region"].ToString(),
                bucketNameBIM: bucketNameBIM?? preference["S3"]["BPM"]["Name"].ToString());

            var genotype = plink.checkRepeat(snpnames: snpnames).AsParallel().Select(x => x[0]).ToArray();
            var alleles = plink.getAlleles(snpnames: snpnames);

            float result = 0;

            for(int metaIdx = 0; metaIdx < metas.Count; ++metaIdx)
            {
                var meta = metas[metaIdx];
                if(genotype[metaIdx] != 0)
                {
                    if(meta["meta"]["Eff"].ToString() == alleles[metaIdx][1])
                    {
                        result += (genotype[metaIdx] - 1) * Convert.ToSingle(meta["meta"]["beta"].ToString());
                    }
                    else if (meta["meta"]["Eff"].ToString() == alleles[metaIdx][0])
                    {
                        result += (3 - genotype[metaIdx]) * Convert.ToSingle(meta["meta"]["beta"].ToString());
                    }
                }
            }

            metas = new JArray(metas.Where((meta, metaIdx) => genotype[metaIdx] != 0));

            metas = new JArray(metas.Concat(metas));

            ProgressWriter.writer(sn: sn, stage: stage, status: "end").Wait();

            return new CalcResult(result, metas);
        }
    }
}
