using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using DSPAlgorithms.DataStructures;
namespace DSPAlgorithms.Algorithms
{
    public class DiscreteFourierTransform : Algorithm
    {
        public Signal InputTimeDomainSignal { get; set; }
        public float InputSamplingFrequency { get; set; }
        public Signal OutputFreqDomainSignal { get; set; }
        public override void Run()
        {
            List<Complex> dft = new List<Complex>();
            List<float> amplitude = new List<float>();
            List<float> phase = new List<float>();
            List<float> frequencies = new List<float>();
            Complex var1;
            Complex sum;
            float cosPart;
            float sinPart;
            for (int i = 0; i < InputTimeDomainSignal.Samples.Count; i++)
            {
                sum = new Complex(0, 0);
                for (int j = 0; j < InputTimeDomainSignal.Samples.Count; j++)
                {
                    cosPart = (float)Math.Cos((2 * Math.PI * i * j) / InputTimeDomainSignal.Samples.Count);
                    sinPart = (float)Math.Sin((2 * Math.PI * i * j) / InputTimeDomainSignal.Samples.Count);  
                    var1 = Complex.Multiply(new Complex(cosPart, -sinPart), InputTimeDomainSignal.Samples[j]);
                    sum = Complex.Add(var1, sum);
                }
                dft.Add(sum);
                
                frequencies.Add((float)(((Math.PI * 2 * InputSamplingFrequency) / InputTimeDomainSignal.Samples.Count) * i));
                amplitude.Add((float)dft[i].Magnitude);
                phase.Add((float)dft[i].Phase);
            }
            OutputFreqDomainSignal = new Signal(true, frequencies, amplitude, phase);
        }
    }
}
