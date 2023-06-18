using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class MultiplySignalByConstant : Algorithm
    {
        public Signal InputSignal { get; set; }
        public float InputConstant { get; set; }
        public Signal OutputMultipliedSignal { get; set; }

        public override void Run()
        {
            float result = 0;
            List<float> normalizedSignal = new List<float>();
            for (int x = 0; x < InputSignal.Samples.Count; x++)
            {
                result = InputSignal.Samples[x] * InputConstant;
                normalizedSignal.Add(result);
            }
            OutputMultipliedSignal = new Signal(normalizedSignal, false);
        }
    }
}
