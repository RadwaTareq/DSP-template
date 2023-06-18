using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class InverseDiscreteFourierTransform : Algorithm
    {
        public Signal InputFreqDomainSignal { get; set; }
        public Signal OutputTimeDomainSignal { get; set; }

        public override void Run()
        {
            List<float> idft = new List<float>();
            float cosPart;
            float sinPart;
            List<Complex> comNumbers = new List<Complex>();
            float real;
            float imag;
            for (int i = 0; i < InputFreqDomainSignal.Frequencies.Count; i++)
            {
                real = (float)(InputFreqDomainSignal.FrequenciesAmplitudes[i] *
                    Math.Cos(InputFreqDomainSignal.FrequenciesPhaseShifts[i]));

                imag = (float)(InputFreqDomainSignal.FrequenciesAmplitudes[i] *
                    Math.Sin(InputFreqDomainSignal.FrequenciesPhaseShifts[i]));

                comNumbers.Add(new Complex(real, imag));
            }

            Complex compVar;
            float harmonic_result;

            for (int i = 0; i < InputFreqDomainSignal.Frequencies.Count; i++)
            { 
                harmonic_result = 0;
                for (int j = 0; j < InputFreqDomainSignal.Frequencies.Count; j++)
                {
                    cosPart = (float)Math.Cos((2 * Math.PI * i * j) / InputFreqDomainSignal.Frequencies.Count);
                    sinPart = (float)Math.Sin((2 * Math.PI * i * j) / InputFreqDomainSignal.Frequencies.Count);
                    compVar = new Complex(cosPart, sinPart);
                    Complex var = Complex.Multiply(comNumbers[j], compVar);
                    harmonic_result += (float)(var.Real + var.Imaginary);
                }
                idft.Add(harmonic_result/ InputFreqDomainSignal.Frequencies.Count);
            }
            OutputTimeDomainSignal = new Signal(idft,false);
        }
    }
    }

