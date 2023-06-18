using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class Folder : Algorithm
    {
        public Signal InputSignal { get; set; }
        public Signal OutputFoldedSignal { get; set; }
        

        public override void Run()
        {
            List<float> samples = new List<float>();
            List<int> indices = new List<int>();
            for (int i=0;i< InputSignal.Samples.Count;i++)
            {
                samples.Add(InputSignal.Samples[InputSignal.Samples.Count-i-1]);
                indices.Add(InputSignal.SamplesIndices[InputSignal.Samples.Count - i - 1] *(-1)); 

            }
           
            OutputFoldedSignal = new Signal(samples, indices, !InputSignal.Periodic);
        }
    }
}
