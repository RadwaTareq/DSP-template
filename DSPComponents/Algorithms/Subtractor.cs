using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class Subtractor : Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public Signal OutputSignal { get; set; }

        /// <summary>
        /// To do: Subtract Signal2 from Signal1 
        /// i.e OutSig = Sig1 - Sig2 
        /// </summary>
        public override void Run()
        {
            float result = 0;
            List<float> subtractedSignal = new List<float>();
            for (int sample = 0; sample < InputSignal1.Samples.Count; sample++)
            {
                result = InputSignal1.Samples[sample] - InputSignal2.Samples[sample];
                subtractedSignal.Add(result);
            }
            OutputSignal = new Signal(subtractedSignal, false);
        }
    }
}