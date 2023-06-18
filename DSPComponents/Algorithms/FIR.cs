using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;
namespace DSPAlgorithms.Algorithms
{
    public class FIR : Algorithm
    {
        public Signal InputTimeDomainSignal { get; set; }
        public FILTER_TYPES InputFilterType { get; set; }
        public float InputFS { get; set; }
        public float? InputCutOffFrequency { get; set; }
        public float? InputF1 { get; set; }
        public float? InputF2 { get; set; }
        public float InputStopBandAttenuation { get; set; }
        public float InputTransitionBand { get; set; }
        public Signal OutputHn { get; set; }
        public Signal OutputYn { get; set; }
        public override void Run()
        {
            OutputHn = new Signal(new List<float>(), new List<int>(), false);
            float size = ((getN() - 1) / 2);
            for (int i = -(int)size; i <= size; i++)
            {
                OutputHn.Samples.Add(getfilter(InputFilterType, i) * window(i,getN()));
                OutputHn.SamplesIndices.Add(i);
            }
            DirectConvolution convolution = new DirectConvolution();
            convolution.InputSignal1 = InputTimeDomainSignal;
            convolution.InputSignal2 = OutputHn;
            convolution.Run();
            OutputYn = convolution.OutputConvolvedSignal;
        }
        public float getfilter(FILTER_TYPES type, int n)
        {
            float Fc;
            float Wc;
            float Fc1;
            float Fc2;
            float Wc1;
            float Wc2;
            if (type == FILTER_TYPES.LOW)
            {
                Fc = ((float)((InputCutOffFrequency + (InputTransitionBand / 2)) / InputFS));
                Wc = (float)(2.0f * Math.PI * Fc);
                if (n == 0)
                {
                    return 2.0f * Fc;
                }
                return (float)(2.0f * Fc * (Math.Sin(n * Wc) /( n * Wc)));
            }

            if (type == FILTER_TYPES.HIGH)
            {
                Fc = ((float)((InputCutOffFrequency - (InputTransitionBand / 2)) / InputFS));
                Wc = (float)(2.0f * Math.PI * Fc);
                if (n == 0)
                {
                    return 1 - (2.0f * Fc);
                }
                return (float)(-2.0f * Fc * (Math.Sin(n * Wc) / (n * Wc)));
            }

            if (type == FILTER_TYPES.BAND_PASS)
            {
                Fc1 = (float)((InputF1 - (InputTransitionBand / 2)) / InputFS);
                Fc2 = (float)((InputF2 + (InputTransitionBand / 2)) / InputFS);
                Wc1 = (float)(2.0f * Math.PI * Fc1);
                Wc2 = (float)(2.0f * Math.PI * Fc2);
                if (n == 0)
                {
                    return 2.0f * (Fc2 - Fc1);
                }
                return (float)((2.0f * Fc2 * (Math.Sin(n * Wc2) / (n * Wc2))) - (2* Fc1 * (Math.Sin(n * Wc1) / (n * Wc1))));
            }
            if (type == FILTER_TYPES.BAND_STOP)
            {
                Fc1 = (float)((InputF1 + (InputTransitionBand / 2)) / InputFS);
                Fc2 = (float)((InputF2 -(InputTransitionBand / 2)) / InputFS);
                Wc1 = (float)(2.0f * Math.PI * Fc1);
                Wc2 = (float)(2.0f * Math.PI * Fc2);
                if (n == 0)
                {
                    return 1 - (2.0f * (Fc2 - Fc1));
                }
                return (float)((2.0f * Fc1 * (Math.Sin(n * Wc1) /( n * Wc1))) - (2.0f * Fc2 * (Math.Sin(n * Wc2) / (n * Wc2))));
            }
            return 0.0f;
        }
        public float window(int n, float N)
        {
            if (InputStopBandAttenuation <= 21)
            {
                return 1.0f;
            }
            else if (InputStopBandAttenuation <= 44)
            {
                return (float)(0.5 + (0.5 * Math.Cos((2.0f * Math.PI * n) / N)));
            }
            else if (InputStopBandAttenuation <= 53)
            {
                return (float)(0.54 + (0.46 * Math.Cos((2.0f * Math.PI * n) / N)));
            }
            else if (InputStopBandAttenuation <= 74)
            {
                return (float)(0.42 + (0.5 * Math.Cos((2.0f * Math.PI * n) / (N - 1))) + (0.08 * Math.Cos((4.0f * Math.PI * n) / (N - 1))));
            }
            return 0.0f;
        }
        public float getoddN(float num)
        {
            if (num % 2 == 0)
                num++;
            else if (num % 2 != 0)
            {
                num = (float)Math.Ceiling(num);
                if (num % 2 == 0)
                    num++;
            }
            return num;
        }
        public float getN()
        {
            float normtrans = InputTransitionBand / InputFS;
            if (InputStopBandAttenuation <= 21)
            {
                return getoddN((float)(0.9f / normtrans));
            }
            else if (InputStopBandAttenuation <= 44)
            {
                return getoddN((float)(3.1f / normtrans));
            }
            else if (InputStopBandAttenuation <= 53)
            {
                return getoddN((float)(3.3f / normtrans));
            }
            else if (InputStopBandAttenuation <= 74)
            {
                return getoddN((float)(5.5f / normtrans));
            }
            return 0.0f;
        }      
    }
}