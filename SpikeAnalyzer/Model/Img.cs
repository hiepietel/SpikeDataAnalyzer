using System;
using System.Collections.Generic;
using System.Text;

namespace SpikeAnalyzer.Model
{
    public class Img
    {
        public string Name { get; set; }
        public bool IsLandscape { get; set; }
        public int ProperDetected { get; set; }
        public int Analyzed { get; set; }
        public double Ratio { get; set; }
    }
}
