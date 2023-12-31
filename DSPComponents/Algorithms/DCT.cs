﻿using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSPAlgorithms.Algorithms
{
    public class DCT: Algorithm
    {
        public Signal InputSignal { get; set; }
        public Signal OutputSignal { get; set; }

        public override void Run()
        {
            float sum;
            float result;
            List<float> allresult = new List<float>();
            for (int i = 0; i < InputSignal.Samples.Count; i++)
            {
                sum = 0;
                for (int j = 0; j < InputSignal.Samples.Count; j++)
                {
                    sum += (float)(InputSignal.Samples[j] * Math.Cos((Math.PI / (4 * InputSignal.Samples.Count)) * ((2 * j) - 1) * ((2 * i) - 1)));
                }
                result = (float)(Math.Sqrt((2.0 / InputSignal.Samples.Count )) * sum);
                allresult.Add(result);
               
              
            }
            OutputSignal = new Signal(allresult, false);
        }
    }
}

