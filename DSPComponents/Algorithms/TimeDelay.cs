using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class TimeDelay : Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public float InputSamplingPeriod { get; set; }
        public float OutputTimeDelay { get; set; }
        public List<float> OutputNonNormalizedCorrelation { get; set; }
        public List<float> OutputNormalizedCorrelation { get; set; }

            public override void Run()
            {
                List<float> nonNormalized = new List<float>();
                List<float> normalized = new List<float>();
                if (InputSignal2 == null)
                {
                    double sumSquare = 0;
                    for (int i = 0; i < InputSignal1.Samples.Count; i++)
                    {
                        sumSquare += Math.Pow(InputSignal1.Samples[i], 2);
                    }
                    if (InputSignal1.Periodic)
                    {
                        float res;
                        double sum = 0;
                        int n;
                        for (int i = 0; i < InputSignal1.Samples.Count; i++)
                        {
                            for (int j = 0; j < InputSignal1.Samples.Count; j++)
                            {
                                n = (i + j) % InputSignal1.Samples.Count;
                                sum += InputSignal1.Samples[j] * (InputSignal1.Samples[n]);
                            }
                            res = (float)(sum / InputSignal1.Samples.Count);
                            nonNormalized.Add(res);
                            normalized.Add(res / (float)((Math.Sqrt(sumSquare * sumSquare) / InputSignal1.Samples.Count)));
                            sum = 0;
                        }
                    }
                    else
                    {
                        float correlation = 0;
                        int n = 0;
                        for (int i = 0; i < InputSignal1.Samples.Count; i++)
                        {
                            for (int j = i; j < InputSignal1.Samples.Count; j++)
                            {
                                correlation += InputSignal1.Samples[n] * InputSignal1.Samples[j];
                                n++;
                            }
                            correlation /= (float)InputSignal1.Samples.Count;
                            nonNormalized.Add(correlation);
                            normalized.Add(correlation / (float)((Math.Sqrt(sumSquare * sumSquare) / InputSignal1.Samples.Count)));
                            correlation = 0;
                            n = 0;
                        }
                    }
                }
                else
                {
                    double sumSquare1 = 0;
                    double sumSquare2 = 0;
                    for (int i = 0; i < InputSignal1.Samples.Count; i++)
                    {
                        sumSquare1 += Math.Pow(InputSignal1.Samples[i], 2);
                    }
                    for (int i = 0; i < InputSignal2.Samples.Count; i++)
                    {
                        sumSquare2 += Math.Pow(InputSignal2.Samples[i], 2);
                    }
                    if (InputSignal1.Periodic == false || InputSignal2.Periodic == false)
                    {
                        int max = InputSignal1.Samples.Count;
                        float corr = 0;
                        List<float> samples = new List<float>();
                        samples = InputSignal2.Samples;
                        for (int i = 0; i < max; i++)
                        {
                            for (int j = 0; j < InputSignal2.Samples.Count; j++)
                            {
                                corr += InputSignal1.Samples[j] * samples[j];
                            }
                            samples.Add(0);
                            samples.RemoveAt(0);
                            float res = 0;
                            res = corr / (float)max;
                            nonNormalized.Add(res);

                            corr = 0;
                        }
                        for (int i = 0; i < nonNormalized.Count; i++)
                        {
                            normalized.Add(nonNormalized[i] / (float)((Math.Sqrt(sumSquare1 * sumSquare2) / max)));
                            Console.WriteLine(normalized[i]);
                        }
                    }
                    else
                    {
                        int max = InputSignal1.Samples.Count;
                        float corr = 0;
                        List<float> samples = new List<float>();
                        samples = InputSignal2.Samples;                    
                        for (int i = 0; i < max; i++)
                        {
                            for (int j = 0; j < InputSignal2.Samples.Count; j++)
                            {
                                corr += InputSignal1.Samples[j] * samples[j];
                            }
                            samples.Add(samples[0]);
                            samples.RemoveAt(0);
                            float res = 0;
                            res = corr / (float)max;
                            nonNormalized.Add(res);
                            corr = 0;
                        }
                        for (int i = 0; i < nonNormalized.Count; i++)
                        {
                            normalized.Add(nonNormalized[i] / (float)((Math.Sqrt(sumSquare1 * sumSquare2) / max)));
                            Console.WriteLine(normalized[i]);
                        }
                    }
                }
                OutputNormalizedCorrelation = normalized;
                OutputNonNormalizedCorrelation = nonNormalized;  
               float max_corr = 0;
               int index = 0;
               for(int i=0; i< OutputNormalizedCorrelation.Count; i++)
               {
                if(OutputNormalizedCorrelation[i] > max_corr)
                {
                    max_corr = OutputNormalizedCorrelation[i];
                    index= i;
                }
               }
             OutputTimeDelay = index * InputSamplingPeriod;

            }
        
    }
 }


        
    

    

