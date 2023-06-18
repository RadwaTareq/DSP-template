using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class Normalizer : Algorithm
    {
        public Signal InputSignal { get; set; }
        public float InputMinRange { get; set; }
        public float InputMaxRange { get; set; }
        public Signal OutputNormalizedSignal { get; set; }

        public override void Run()
        {
            float result = 0;
            List<float> normalizedSignal = new List<float>();
            for (int sample = 0; sample < InputSignal.Samples.Count; sample++)
            {
                result = ((InputMaxRange - InputMinRange) * ((InputSignal.Samples[sample] - InputSignal.Samples.Min()) / (InputSignal.Samples.Max() - InputSignal.Samples.Min()))) + (InputMinRange);
                normalizedSignal.Add(result);
            }
            OutputNormalizedSignal = new Signal(normalizedSignal, false);
        }
    }
}
