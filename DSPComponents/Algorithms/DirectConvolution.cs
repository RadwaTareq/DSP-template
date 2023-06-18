using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class DirectConvolution : Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public Signal OutputConvolvedSignal { get; set; }

        /// <summary>
        /// Convolved InputSignal1 (considered as X) with InputSignal2 (considered as H)
        /// </summary>
        public override void Run()
        {
            List<float> con = new List<float>();
            List<int> indices = new List<int>();
            float res = 0;
            for (int i = 0; i < InputSignal1.Samples.Count + InputSignal2.Samples.Count - 1; i++)
            {
                for (int j = 0; j < InputSignal1.Samples.Count; j++)
                {
                    if (i - j >= 0 && i - j < InputSignal2.Samples.Count)
                    {
                        res += InputSignal1.Samples[j] * InputSignal2.Samples[i - j];
                    }
                }
                //empty signal
                if (res == 0 && (i + 1) == InputSignal1.Samples.Count + InputSignal2.Samples.Count - 1)
                {
                    break;
                }
                //add index
                if (i == 0)
                {
                    indices.Add(InputSignal1.SamplesIndices[i] + InputSignal2.SamplesIndices[i]);
                }
                else
                {
                    indices.Add(indices.Last<int>() + 1);
                }
                con.Add(res);
                res = 0;

            }
            OutputConvolvedSignal = new Signal(con, indices, false);

        }
    }
}