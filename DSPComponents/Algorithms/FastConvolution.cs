using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;
using System.Numerics;
namespace DSPAlgorithms.Algorithms
{
    public class FastConvolution : Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public Signal OutputConvolvedSignal { get; set; }
        public float InputSamplingFrequency = 4;
        /// <summary>
        /// Convolved InputSignal1 (considered as X) with InputSignal2 (considered as H)
        /// </summary>
        public override void Run()
        {
            List<Complex> dft1 = new List<Complex>();
            List<Complex> dft2 = new List<Complex>();
            List<float> amplitude1 = new List<float>();
            List<float> phase1 = new List<float>();
            List<float> frequencies1 = new List<float>();
            List<float> amplitude2 = new List<float>();
            List<float> phase2 = new List<float>();
            List<float> frequencies2 = new List<float>();
            Complex var1;
            Complex sum;
            float cosPart;
            float sinPart;
            int i;
            Complex mul;
            List<Complex> mullists = new List<Complex>();
            InverseDiscreteFourierTransform IDFT = new InverseDiscreteFourierTransform();
            //length
            int length = InputSignal1.Samples.Count + InputSignal2.Samples.Count - 1;
            for (i = InputSignal1.Samples.Count; i < length; i++)
            {
                InputSignal1.Samples.Add(0);
                InputSignal1.SamplesIndices.Add(i);
            }
            for (i = InputSignal2.Samples.Count; i < length; i++)
            {
                InputSignal2.Samples.Add(0);
                InputSignal2.SamplesIndices.Add(i);
            }
            //dft for s1
            for (i = 0; i < length; i++)
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
            for (i = 0; i < length; i++)
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
            for (i = 0; i < length; i++)
            {
                mul = Complex.Multiply(dft1[i], dft2[i]);
                mullists.Add(mul);
            }
            List<float> amp = new List<float>();
            List<float> phash = new List<float>();
            List<float> freq = new List<float>();
            for (i = 0; i < length; i++)
            {
                float real_p2 = (float)Math.Pow(mullists[i].Real, 2);
                float img_p2 = (float)Math.Pow(mullists[i].Imaginary, 2);
                amp.Add((float)(Math.Sqrt(real_p2 + img_p2)));
                phash.Add((float)Math.Atan2(mullists[i].Imaginary, mullists[i].Real));
                freq.Add((float)(((Math.PI * 2 * InputSamplingFrequency) / length) * i));
            }
            Signal result=new Signal(false, freq, amp, phash);
            IDFT.InputFreqDomainSignal = result;
            IDFT.Run(); 
            OutputConvolvedSignal = new Signal(IDFT.OutputTimeDomainSignal.Samples, false);
        }
    }
}
