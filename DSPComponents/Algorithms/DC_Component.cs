using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSPAlgorithms.Algorithms
{
    public class DC_Component: Algorithm
    {
        public Signal InputSignal { get; set; }
        public Signal OutputSignal { get; set; }

        public override void Run()
        {
            float sum = 0;
            float mean;
            List<float> sad = new List<float>();
            float result = 0;
            for (int sample = 0; sample < InputSignal.Samples.Count; sample++)
            {
                sum += InputSignal.Samples[sample];
            }
            mean = sum / InputSignal.Samples.Count;
            for (int sample = 0; sample < InputSignal.Samples.Count; sample++)
            {
                result = InputSignal.Samples[sample] - mean;
                sad.Add(result);
            }
            OutputSignal = new Signal(sad, false);
        }
    }
}
