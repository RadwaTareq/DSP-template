using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;
namespace DSPAlgorithms.Algorithms
{
    public class FastCorrelation : Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }   
        public List<float> OutputNonNormalizedCorrelation { get; set; }
        public List<float> OutputNormalizedCorrelation { get; set; }
        public float InputSamplingFrequency = 4;
        public override void Run()
        {    
            List<Complex> dft1 = new List<Complex>();
            List<Complex> dft2 = new List<Complex>();
            List<Complex> conj = new List<Complex>();
            Complex var1;
            Complex sum;
            float cosPart;
            float sinPart;
            int length;
            Complex mul;
            List<Complex> mullists = new List<Complex>();
            length = InputSignal1.Samples.Count;
            InverseDiscreteFourierTransform IDFT = new InverseDiscreteFourierTransform();
            if (InputSignal2 == null)
            {
                InputSignal2 = InputSignal1;  
            }
            //nor or nonnor
            double sumSquare1 = 0;
            double sumSquare2 = 0;
            for (int i = 0; i < InputSignal1.Samples.Count; i++)
            {
                sumSquare1 += Math.Pow(InputSignal1.Samples[i], 2);
                sumSquare2 += Math.Pow(InputSignal2.Samples[i], 2);
            }
            for (int i = 0; i < length; i++)
            {
                sum = new Complex(0, 0);
                for (int j = 0; j < length; j++)
                {
                    cosPart = (float)Math.Cos((2 * Math.PI * i * j) / length);
                    sinPart = (float)Math.Sin((2 * Math.PI * i * j) / length);
                    var1 = Complex.Multiply(new Complex(cosPart, -sinPart), InputSignal1.Samples[j]);
                    sum = Complex.Add(var1, sum);
                }
                dft1.Add(sum);
            }
            //dft for s2
            for (int i = 0; i < length; i++)
            {
                sum = new Complex(0, 0);
                for (int j = 0; j < length; j++)
                {
                    cosPart = (float)Math.Cos((2 * Math.PI * i * j) / length);
                    sinPart = (float)Math.Sin((2 * Math.PI * i * j) / length);
                    var1 = Complex.Multiply(new Complex(cosPart, -sinPart), InputSignal2.Samples[j]);
                    sum = Complex.Add(var1, sum);
                }
                dft2.Add(sum);
            }
            // compute x* 
            for (int i = 0; i < dft1.Count; i++)
            {
                conj.Add(new Complex(dft1[i].Real, -1.0 * dft1[i].Imaginary));
            }
            // compute multiplication of signal1 by signal2                
            for (int i  = 0; i < conj.Count; i++)
            {
                mul = Complex.Multiply(conj[i], dft2[i]);
                mullists.Add(mul);
            }    
            List<float> amp = new List<float>();
            List<float> phash = new List<float>();
            List<float> freq = new List<float>();
            for (int i  = 0; i < mullists.Count; i++)
            {
                float real_p2 = (float)Math.Pow(mullists[i].Real, 2);
                float img_p2 = (float)Math.Pow(mullists[i].Imaginary, 2);
                amp.Add((float)(Math.Sqrt(real_p2 + img_p2)));
                phash.Add((float)Math.Atan2(mullists[i].Imaginary, mullists[i].Real));
                freq.Add((float)(((Math.PI * 2.0 * InputSamplingFrequency) / mullists.Count) * i));
            }
            Signal result = new Signal(false, freq, amp, phash);
            IDFT.InputFreqDomainSignal = result;
            IDFT.Run();
            OutputNonNormalizedCorrelation = new List<float>();
            OutputNormalizedCorrelation = new List<float>();
            //nonnormalized
            for (int i = 0; i < IDFT.OutputTimeDomainSignal.Samples.Count; i++)
            {
                OutputNonNormalizedCorrelation.Add((IDFT.OutputTimeDomainSignal.Samples[i] / InputSignal2.Samples.Count));
                OutputNormalizedCorrelation.Add(OutputNonNormalizedCorrelation[i] / (float)(Math.Sqrt(sumSquare1 * sumSquare2) / length));
            }        
        }
    }
}