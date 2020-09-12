using SpikeAnalyzer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpikeAnalyzer
{
    public static class Analyzer
    {
        public readonly static int MaxFileNameSpaces = 20;
        public static string GenerateFileNameWithSpaces(string filename, int extra = 0)
        {
            string spaces = "";
            for (int i = 0; i < (MaxFileNameSpaces + extra - filename.Length); i++)
                spaces += " ";

            return filename + spaces;
        }
        
        public static List<Img> ReturnImgBest(List<Img> imglist)
        {
            List<Img> imgs = imglist.OrderByDescending(x => x.Ratio).ToList();
            foreach (var item in imgs)
            {
                double ratio = (double)item.ProperDetected / (double)item.Analyzed;
                Console.WriteLine($"Name:{ GenerateFileNameWithSpaces(item.Name, 50)}"+
                    $"IsLandscape: {item.IsLandscape}. " +
                    $"ProperDetected: {item.ProperDetected}. " +
                    $"Analyzed: {item.Analyzed}. " +
                    $"Ratio: {item.Ratio}. ");
            }
            return imgs;
        }
        public static List<Stat> ReturnBest(List<Stat> list)
        {
            List<Stat> bestList = list.OrderByDescending(x => x.Ratio).ToList();
            
            Console.WriteLine("Best:");
            foreach (Stat item in bestList)
            {
                Console.WriteLine($"Proper detected: {item.ProperDetected}. " +
                    $"FileName: { GenerateFileNameWithSpaces(item.FileName)} " +
                    $"R: {item.Ratio}. " +
                    $"Imgs: {item.SingleImgList.Count}. " +
                    $"Proper hist detected: {item.ProperHistDetected}. " +
                    $"Proper sky detected: {item.ProperSkyDetected}. " +
                    $"NeuronAmounts: {item.NeuronsAmount}. ");
            }
            return bestList;
        }
        public static List<Stat> ReturnBestHist(List<Stat> list)
        {
            List<Stat> bestList = list.OrderByDescending(x => x.ProperHistDetected).ToList();
            Console.WriteLine("BestHist:");
            foreach (Stat item in bestList)
            {
                Console.WriteLine($"Proper hist detected: {item.ProperHistDetected}. " +
                    $"FileName: { GenerateFileNameWithSpaces(item.FileName)} " +
                    $"SingleImages: {item.SingleImgList.Count}. " +
                    $"HistCountOffset: {item.HistCountOffset}. " +
                    $"ProperHistCountOffset: {item.ProperHistCountOffset}. " +
                    $"ProperHistOffset {item.TotalHistProperOffset} ");


            }
            return bestList;
        }
        public static List<Stat> ReturnBestSky(List<Stat> list)
        {
            List<Stat> bestList = list.OrderByDescending(x => x.ProperSkyDetected).ToList();
            Console.WriteLine("BestSky:");
            foreach (Stat item in bestList)
            {
                Console.WriteLine($"Proper sky detected: {item.ProperSkyDetected}. " +
                    $"FileName: { GenerateFileNameWithSpaces(item.FileName)} " +
                    $"SingleImages: {item.SingleImgList.Count}. " +
                    $"SkyCountOffset: {item.SkyCountOffset}. " +
                    $"ProperSkyCountOffset:  {item.ProperSkyCountOffset}. " +
                   $"ProperSkyOffset {item.TotalSkyProperOffset}. ");
            }
            return bestList;
        }

        // public bestList;

    }
}
