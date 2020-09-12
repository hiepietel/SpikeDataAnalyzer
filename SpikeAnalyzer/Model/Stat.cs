using System;
using System.Collections.Generic;
using System.Text;

namespace SpikeAnalyzer.Model
{
    public class Stat
    {
        public int NeuronsAmount { get; set; }
        public int ImgAmount { get; set; }
        public double TresholdHist { get; set; }
        public double TresholdSky { get; set; }
        public double OutTresholdHist { get; set; }
        public double OutTresholdSky { get; set; }
        public int HistCountOffset { get; set; }
        public int SkyCountOffset { get; set; }
        public List<SingleImg> SingleImgList { get; set; }
        public double TotalTime { get; set; }
        public int[,] Matrix { get; set; }
        public int ProperDetected { get; set; }
        public int ProperHistDetected { get; set; }
        public int ProperSkyDetected { get; set; }
        public double ProperHistCountOffset { get; set; }
        public double ProperSkyCountOffset { get; set; }
        public double TotalHistProperOffset { get; set; }
        public double TotalSkyProperOffset { get; set; }
        public string FileName { get; set; }
        public double Ratio { get; set; }
        public string Version { get; set; }
        public Stat()
        {
            //SingleImgList = new List<SingleImg>();
            Matrix = new int[2,2] {{ 0, 0 }, { 0, 0}};
        }
        public void CountProperValues()
        {
            ProperHistDetected = 0;
            ProperSkyDetected = 0;
            foreach(SingleImg singleImg in SingleImgList)
            {
                if (singleImg.HistUStep > this.OutTresholdHist && singleImg.IsLandscape == true)
                    ProperHistDetected++;
                if (singleImg.HistUStep < this.OutTresholdHist && singleImg.IsLandscape == false)
                    ProperHistDetected++;
                if (singleImg.SkyUStep > this.OutTresholdSky && singleImg.IsLandscape == true)
                    ProperSkyDetected++;                
                if (singleImg.SkyUStep < this.OutTresholdSky && singleImg.IsLandscape == false)
                    ProperSkyDetected++;
            }
            int d = 8;
            this.ProperHistCountOffset = this.TresholdHist / (-14 + this.NeuronsAmount * d);
            this.ProperSkyCountOffset = this.TresholdSky / (-14 + this.NeuronsAmount * d);
            //out_treshold = (-14 + image_list_count * d) * 0.4
            this.TotalHistProperOffset = this.OutTresholdHist / (-14 + this.ImgAmount * d);
            this.TotalSkyProperOffset = this.OutTresholdSky / (-14 + this.ImgAmount * d);

        }
    }
}
