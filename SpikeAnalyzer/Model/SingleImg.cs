using System;
using System.Collections.Generic;
using System.Text;

namespace SpikeAnalyzer.Model
{
    public class SingleImg
    {
        public int HistUStep { get; set; }
        public int SkyUStep { get; set; }
        public bool IsLandscape { get; set; }
        public string ImgName { get; set; }
        public bool Prediction { get; set; }
        public double Time { get; set; }
    }
}
