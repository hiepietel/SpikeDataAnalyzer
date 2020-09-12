using SpikeAnalyzer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SpikeAnalyzer
{
    public static class Parser
    {
        //private static readonly string SingleImgRgx = @"\WHIST\Wu_step:\W\[(?<h_u>[0-9\-]*).\]\WSKY\Wu_step:\W\[(?<s_u>[0-9-]*).\]\s(?<model>True|False):\W..\\db\\img_spike\\spike_all_s\\(true|false)\\(?<imgname>[0-9A-z()-_ ]*).jpg\WisLandscape:\W(?<result>True|False)\Wtime:\W(?<time>[0-9.]*)";
        private static readonly string SingleImgRgx = @"\WHIST\Wu_step:\W\[(?<h_u>[0-9\-]*).\]\WSKY\Wu_step:\W\[(?<s_u>[0-9-]*).\]\r\n(?<model>True|False):\W..\\db\\img_spike\\[0-9A-z_]*\\(true|false)\\(?<imgname>[0-9A-z()-_ ]*).jpg\WisLandscape:\W(?<result>True|False)\Wtime:\W(?<time>[0-9.]*)";
        private static readonly string NeuronsAmountRgx = @"neurons\Wamount:\W(?<amount>[0-9.]*)";
        private static readonly string ModelImgAmountRgx = @"model\Wimg\Wamount:\W(?<amount>[0-9.]*)";
        private static readonly string TresholdHistRgx = @"treshold hist:\W(?<amount>[0-9.-]*)";
        private static readonly string TresholdSkyRgx = @"treshold sky:\W(?<amount>[0-9.-]*)";
        private static readonly string OutTresholdRgx = @"out treshold:\W(?<amount>[0-9.]*)";
        //out sky treshold: 980.4
        private static readonly string SkyOutTresholdRgx = @"out sky treshold:\W(?<amount>[0-9.]*)";
        private static readonly string HistCountOffsetRgx = @"hist_count_offset:\W(?<amount>[0-9.]*)";
        private static readonly string SkyCountOffsetRgx = @"sky_count_offset:\W(?<amount>[0-9.]*)";
        private static readonly string TotalTimeRgx = @"Total Time:\W(?<amount>[0-9.]*)";
        private static readonly string ConfusionMatrixRgx = @"\[\[(?<tp>[0-9 ]*).\W(?<tn>[0-9 ]*).\]\r\n\W\[(?<fn>[0-9 ]*).\W(?<fp>[0-9 ]*).\]\]";
        private static readonly string VersionRgx = @"log version:\W(?<amount>[0-9.]*)";
        public static List<SingleImg> SearchSingleImg(string text)
        {
            Regex rgx = new Regex(SingleImgRgx, RegexOptions.Multiline | RegexOptions.Compiled);

            MatchCollection matches = rgx.Matches(text);
            var singleImgList = new List<SingleImg>();
            foreach (Match match in matches)
            {
                GroupCollection groups = match.Groups;                
                var singleImg = new SingleImg()
                {
                    HistUStep = Convert.ToInt32(groups["h_u"].Value),
                    SkyUStep = Convert.ToInt32(groups["s_u"].Value),
                    IsLandscape = Convert.ToBoolean(groups["model"].Value),
                    Prediction = Convert.ToBoolean(groups["result"].Value),
                    ImgName = groups["imgname"].Value.ToString(),
                    Time = Convert.ToDouble(groups["time"].Value)
                };
                singleImgList.Add(singleImg);
            }
            return singleImgList;
        }
        public static T GetSingleMatch<T>(string regex, string text)
        {
            Regex rgx = new Regex(regex, RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
            Match match = rgx.Match(text);
            var result = match.Groups["amount"].Value;
            return (T)Convert.ChangeType(result, typeof(T));
        }
        public static int[,] GetConfusionMatrix(string text)
        {
            Regex rgx = new Regex(ConfusionMatrixRgx, RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.CultureInvariant);
            Match match = rgx.Match(text);
            int[,] matrix = new int[2, 2];
            matrix[0,0] = Convert.ToInt32(match.Groups["tp"].Value);
            matrix[0,1] = Convert.ToInt32(match.Groups["tn"].Value);
            matrix[1,0] = Convert.ToInt32(match.Groups["fn"].Value);
            matrix[1,1] = Convert.ToInt32(match.Groups["fp"].Value);
            return matrix;

        }
        public static Stat SearchStat(string text, string filename)
        {
            int[,] matrix = GetConfusionMatrix(text);
            var stat = new Stat()
            {
                NeuronsAmount = GetSingleMatch<int>(NeuronsAmountRgx, text),
                ImgAmount = GetSingleMatch<int>(ModelImgAmountRgx, text),
                TresholdHist = GetSingleMatch<double>(TresholdHistRgx, text),
                TresholdSky = GetSingleMatch<double>(TresholdSkyRgx, text),
                OutTresholdHist = GetSingleMatch<double>(OutTresholdRgx, text),
                HistCountOffset = GetSingleMatch<int>(HistCountOffsetRgx, text),
                SkyCountOffset = GetSingleMatch<int>(SkyCountOffsetRgx, text),
                TotalTime = GetSingleMatch<double>(TotalTimeRgx, text),
                SingleImgList = SearchSingleImg(text),
                Matrix = matrix,
                ProperDetected = matrix[0,0] + matrix[1,1],
                FileName = filename,
                Version = GetSingleMatch<string>(VersionRgx, text)
            };
            try
            {
                stat.OutTresholdSky = GetSingleMatch<double>(SkyOutTresholdRgx, text);
            }
            catch (Exception)
            {

                stat.OutTresholdSky = GetSingleMatch<double>(OutTresholdRgx, text);
            } 

            if (stat.SingleImgList.Count == 0)
                return null;
            stat.Ratio = (double)stat.ProperDetected / (double)stat.SingleImgList.Count;
            stat.CountProperValues();
            return stat;


        }
        public static List<Img> ReturnImgList(List<Stat> stats)
        {
            var imglist = new List<Img>();

            foreach (var stat in stats)
            {
                foreach (var singleImg in stat.SingleImgList)
                {
                    var imgNameList = imglist.Where(x => x.Name == singleImg.ImgName && x.IsLandscape == singleImg.IsLandscape).ToList();
                    if (imgNameList.Any())
                    {
                        var img = imgNameList.FirstOrDefault();
                        var newImg = img;
                        //if (singleImg.IsLandscape)

                        //    newImg.IsLandscape = true;
                        //else
                        //    newImg.IsLandscape = false;
                        if (singleImg.Prediction == singleImg.IsLandscape)
                            newImg.ProperDetected++;
                        newImg.Analyzed++;
                        newImg.Ratio = (double)newImg.ProperDetected / (double)newImg.Analyzed;
                        imglist.Remove(img);
                        imglist.Add(newImg);
                    }
                    else
                    {
                        var img = new Img()
                        {
                            Name = singleImg.ImgName,
                            IsLandscape = singleImg.IsLandscape,
                            ProperDetected = singleImg.Prediction == singleImg.IsLandscape ? 1 : 0,
                            Analyzed = 1

                        };
                        img.Ratio = (double)img.ProperDetected / (double)img.Analyzed;
                        imglist.Add(img);
                    }
                }
            }
            return imglist;

        }
    }
}
