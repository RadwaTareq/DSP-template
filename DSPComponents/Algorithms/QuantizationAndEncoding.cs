using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class QuantizationAndEncoding : Algorithm
    {
        // You will have only one of (InputLevel or InputNumBits), the other property will take a negative value
        // If InputNumBits is given, you need to calculate and set InputLevel value and vice versa
        public int InputLevel { get; set; }
        public int InputNumBits { get; set; }
        public Signal InputSignal { get; set; }
        public Signal OutputQuantizedSignal { get; set; }
        public List<int> OutputIntervalIndices { get; set; }
        public List<string> OutputEncodedSignal { get; set; }
        public List<float> OutputSamplesError { get; set; }

        public override void Run()
        {


            OutputIntervalIndices = new List<int>();
            OutputEncodedSignal = new List<string>();
            OutputSamplesError = new List<float>();
           
            if (InputNumBits == 0)
            {
                InputNumBits = (int)Math.Log(InputLevel, 2);

            }
            if(InputLevel == 0)
            {
                InputLevel = (int)Math.Pow(2, InputNumBits);
            }
            float delta = (InputSignal.Samples.Max() - InputSignal.Samples.Min()) / InputLevel;
            List<float> ranges = new List<float>();

            //Mesh m7tageen nlf l7d el rakam bta3 el input level

            for (int i = 0; i <= InputLevel; i++)
            {
                if (i == 0)
                {
                    ranges.Add(InputSignal.Samples.Min());
                }
                else
                {
                    ranges.Add((float)Math.Round((ranges[i - 1] + delta), 4));
                }

            }
            List<float> midpoints = new List<float>();
            for (int i = 0; i < InputLevel; i++)
            {
                midpoints.Add((ranges[i] + ranges[i + 1]) / 2);
            }

            //el list de httb3t ll signal
            List<float> quantValues = new List<float>();
            for (int i = 0; i < InputSignal.Samples.Count; i++)
            {
                for (int j = 0; j < InputLevel; j++)
                {
                    if (InputSignal.Samples[i] >= ranges[j] && InputSignal.Samples[i] <= ranges[j + 1])
                    {
                        // el 7agat de dependent 3ala el interval fa msh lazem yt7to bl tarteb [mohem!!]
                        OutputEncodedSignal.Add(Convert.ToString(j, 2).PadLeft(InputNumBits, '0'));
                        quantValues.Add(midpoints[j]);
                        OutputIntervalIndices.Add(j + 1);
                        break;

                    }

                }

            }
            OutputQuantizedSignal = new Signal(quantValues, false);
            /*for (int i = 0; i < quantValues.Count; i++) { 
                Console.WriteLine(quantValues[i]);
                Console.WriteLine(OutputEncodedSignal[i]);
            }   */
            List<float> outputErrorSamples = new List<float>();
            for( int i=0; i < InputSignal.Samples.Count; i++)
            {
                outputErrorSamples.Add(OutputQuantizedSignal.Samples[i] - InputSignal.Samples[i]);
            }

            OutputSamplesError = outputErrorSamples;
           
        }
    }
}
