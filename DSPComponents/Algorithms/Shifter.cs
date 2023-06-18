using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class Shifter : Algorithm
    {
        public Signal InputSignal { get; set; }
        public int ShiftingValue { get; set; }
        public Signal OutputShiftedSignal { get; set; }

        public override void Run() 

        {
             List<float> samples = new List<float>();
             List<int> samples_indecis = new List<int>();

            if (InputSignal.Periodic == false)
            {
                for (int i = 0; i < InputSignal.Samples.Count; i++)
                {
                    samples.Add(InputSignal.Samples[i]);
                    samples_indecis.Add(InputSignal.SamplesIndices[i] - ShiftingValue);
                }
            }
            else
            {
                for (int i = 0; i < InputSignal.Samples.Count; i++)
                {
                    samples.Add(InputSignal.Samples[i]);
                    samples_indecis.Add(InputSignal.SamplesIndices[i] + ShiftingValue);
                }
            }
            OutputShiftedSignal = new Signal(InputSignal.Samples, samples_indecis, InputSignal.Periodic);



          
            

        }
    }
}
