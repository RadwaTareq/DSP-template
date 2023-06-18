using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class Adder : Algorithm
    {
        public List<Signal> InputSignals { get; set; }
        public Signal OutputSignal { get; set; }

        public override void Run()
        {
            float result = 0;
            List<float> sum = new List<float>();
            foreach (int signal in InputSignals[0].Samples)
            {
                result = InputSignals[0].Samples[signal] + InputSignals[1].Samples[signal];
                sum.Add(result);
            }
            OutputSignal = new Signal(sum, false);
        }
    }
}