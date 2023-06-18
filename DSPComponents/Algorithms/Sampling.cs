using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace DSPAlgorithms.Algorithms
{
    public class Sampling : Algorithm
    {
        public int L { get; set; } //upsampling factor
        public int M { get; set; } //downsampling factor
        public Signal InputSignal { get; set; }
        public Signal OutputSignal { get; set; }
        public override void Run()
        {
            FIR fir = new FIR();
            fir.InputFilterType = DSPAlgorithms.DataStructures.FILTER_TYPES.LOW;
            fir.InputFS = 8000;
            fir.InputStopBandAttenuation = 50;
            fir.InputCutOffFrequency = 1500;
            fir.InputTransitionBand = 500;
            Signal outputfirSamples;
            List<float> finalsamples = new List<float>();
            List<float> upsample = new List<float>();
            List<int> indecis = new List<int>();
            int index = 0;
            int min_index = 0;
            if (L == 0 && M != 0)
            {
                fir.InputTimeDomainSignal = InputSignal;
                fir.Run();
                outputfirSamples = fir.OutputYn;
                min_index = outputfirSamples.SamplesIndices[0];
                for (int i = 0; i < outputfirSamples.Samples.Count; i += M)
                {
                    indecis.Add(min_index);
                    finalsamples.Add(outputfirSamples.Samples[i]);
                    min_index++;
                }
                OutputSignal = new Signal(finalsamples, indecis, false);
            }
            else if (L != 0 && M == 0)
            {
                min_index = InputSignal.SamplesIndices[0];
                for (int i = 0; i < InputSignal.Samples.Count; i++)
                {
                    finalsamples.Add(InputSignal.Samples[i]);
                    indecis.Add(min_index);
                    min_index++;
                    if (i != InputSignal.Samples.Count - 1)
                    {
                        for (int j = 0; j < L - 1; j++)
                        {
                            finalsamples.Add(0.0f);
                            indecis.Add(min_index);
                            min_index++;
                        }
                    }
                }
                fir.InputTimeDomainSignal = new Signal(finalsamples, indecis, false);
                fir.InputTimeDomainSignal.SamplesIndices = indecis;
                fir.Run();
                OutputSignal = fir.OutputYn;
            }
            else if (L != 0 && M != 0)
            {
                for (int i = 0; i < InputSignal.Samples.Count; i++)
                {
                    upsample.Add(InputSignal.Samples[i]);
                    indecis.Add(InputSignal.SamplesIndices[i]);
                    if (i != InputSignal.Samples.Count - 1)
                    {
                        for (int j = 0; j < L - 1; j++)
                        {
                            upsample.Add(0.0f);
                        }
                    }
                }
                fir.InputTimeDomainSignal = new Signal(upsample, false);
                fir.InputTimeDomainSignal.SamplesIndices = indecis;
                fir.Run();
                outputfirSamples = fir.OutputYn;
                min_index = outputfirSamples.SamplesIndices[0];
                for (int i = 0; i < outputfirSamples.Samples.Count; i += M)
                {
                    indecis.Add(min_index);
                    min_index++;
                    finalsamples.Add(outputfirSamples.Samples[i]);
                }
                OutputSignal = new Signal(finalsamples, indecis, false);
            }
        }
    }
}