using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;
using System.Collections.Specialized;

namespace DSPAlgorithms.Algorithms
{
    public class SinCos : Algorithm
    {
        public string type { get; set; }
        public float A { get; set; }
        public float PhaseShift { get; set; }
        public float AnalogFrequency { get; set; }
        public float SamplingFrequency { get; set; }
        public List<float> samples { get; set; }
        // x(n) = A cos(2pifn + theta)  
        public override void Run()
        {
            samples = new List<float>();
            float result = 0;
            if (SamplingFrequency >= (2 * AnalogFrequency))
            {
                for (int sample = 0; sample < SamplingFrequency; sample++)
                {
                    if (type == "sin")
                    {
                        result = (float)(A * Math.Sin(2 * Math.PI * (AnalogFrequency / SamplingFrequency) * sample + PhaseShift));
                        samples.Add(result);
                    }
                    else
                    {
                        result = (float)(A * Math.Cos(2 * Math.PI * (AnalogFrequency / SamplingFrequency) * sample + PhaseShift));
                        samples.Add(result);
                    }
                }
            }
        }
    }
} 
