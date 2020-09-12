using SpikeAnalyzer.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SpikeAnalyzer
{
    class Program
    {
        static void Analyze(List<Stat> stats)
        {
            var imgs = Parser.ReturnImgList(stats);

            Analyzer.ReturnBest(stats);
            Analyzer.ReturnBestHist(stats);
            Analyzer.ReturnBestSky(stats);
            Analyzer.ReturnImgBest(imgs);
        }

        static void Main(string[] args)
        {
            var statList = new List<Stat>();
            string[] statFiles = FileReader.GetStatFiles();
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == "-one")
                {
                    string statFile = Array.Find(statFiles,
                        element => element.Contains(args[i + 1], StringComparison.Ordinal));
                    statFiles = new string[1];
                    statFiles[0] = statFile;
                    //statFiles = new string[statFile];
                }
            }

            foreach(string stat in statFiles)
            {
                string file = FileReader.ReadFile(stat);
                string filename = Path.GetFileNameWithoutExtension(stat);
                var statt = Parser.SearchStat(file, filename);
                statList.Add(statt);
            }
            var statListTwo = statList.Where(x => x.Version == "0.3").ToList();

            Analyze(statListTwo);
        }
    }

}
