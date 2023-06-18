using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class MovingAverage : Algorithm
    {
        public Signal InputSignal { get; set; }
        public int InputWindowSize { get; set; }
        public Signal OutputAverageSignal { get; set; }
 
        public override void Run()
        { float Sum;
            float avgSum; 
            List<float> samples=new List<float>();
           for(int i = InputWindowSize-1; i < InputSignal.Samples.Count ; i++)
            { Sum = 0;
                avgSum = 0;
                int x = i;
                for(int j=0; j<InputWindowSize; j++)
                {
                    Sum += InputSignal.Samples[x];
                    x--;
                }
                avgSum = Sum / InputWindowSize; 
                samples.Add(avgSum);
                
            }
            OutputAverageSignal = new Signal(samples, false); 
        }
    }
}
