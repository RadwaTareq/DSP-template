using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace DSPAlgorithms.Algorithms
{
    public class PracticalTask2 : Algorithm
    {
        public String SignalPath { get; set; }
        public float Fs { get; set; }
        public float miniF { get; set; }
        public float maxF { get; set; }
        public float newFs { get; set; }
        public int L { get; set; } //upsampling factor
        public int M { get; set; } //downsampling factor
        public Signal OutputFreqDomainSignal { get; set; }
        public override void Run()
        {
            Signal InputSignal = LoadSignal(SignalPath);
            FIR fir = new FIR();
            fir.InputFilterType = DSPAlgorithms.DataStructures.FILTER_TYPES.BAND_PASS;
            fir.InputFS = Fs;
            fir.InputStopBandAttenuation = 50;
            fir.InputTransitionBand = 500;
            fir.InputF1 = miniF;
            fir.InputF2 = maxF;
            fir.InputTimeDomainSignal = InputSignal;
            fir.Run();
            Signal outputfirSignal = fir.OutputYn;
            save_file(outputfirSignal, outputfirSignal.Samples.Count, "FirResults.ds");
            Signal ResultedSignal;
            if (newFs >= (Fs / 2))
            {
                Sampling sampling = new Sampling();
                sampling.InputSignal = outputfirSignal;
                sampling.L = L;
                sampling.M = M;
                sampling.Run();
                Signal outputSamplingSignal = sampling.OutputSignal;
                save_file(outputSamplingSignal, outputSamplingSignal.Samples.Count, "samplingResults.ds");
                ResultedSignal = outputSamplingSignal;
            }
            else
            {
                ResultedSignal = outputfirSignal;
            }
            DC_Component dC_Component = new DC_Component();
            dC_Component.InputSignal = ResultedSignal;
            dC_Component.Run();
            Signal OutputSignal = dC_Component.OutputSignal;
            save_file(OutputSignal, OutputSignal.Samples.Count, "RemovingDC_ComponentResults.ds");
            Normalizer normalizer = new Normalizer();
            normalizer.InputSignal = OutputSignal;
            normalizer.InputMinRange = -1.0f;
            normalizer.InputMaxRange = 1.0f;
            normalizer.Run();
            Signal outputNormalizedSignal = normalizer.OutputNormalizedSignal;
            save_file(outputNormalizedSignal, outputNormalizedSignal.Samples.Count, "NormalizedSignalResults.ds");
            DiscreteFourierTransform discreteFourierTransform = new DiscreteFourierTransform();
            discreteFourierTransform.InputTimeDomainSignal = outputNormalizedSignal;
            discreteFourierTransform.InputSamplingFrequency = newFs;
            discreteFourierTransform.Run();
            OutputFreqDomainSignal = discreteFourierTransform.OutputFreqDomainSignal;
            for (int i = 0; i < OutputFreqDomainSignal.Frequencies.Count; i++)
            {
                Math.Round(OutputFreqDomainSignal.Frequencies[i], 1);
            }
            save_file(OutputFreqDomainSignal, OutputFreqDomainSignal.Frequencies.Count, "DFT_Results.ds");
        }
        public static void save_file(Signal signal, int length, string file_name)
        {  // "fir.ds"
            //Text file is saved inside DSPComponentsUnitTes\bin\Debug folder if test is run from there
            var signal_type = 0;
            FileStream file = new FileStream(file_name, FileMode.OpenOrCreate);
            StreamWriter Stream_writer = new StreamWriter(file);
            Stream_writer.WriteLine(Convert.ToInt32(signal.Periodic).ToString());
            Stream_writer.WriteLine(signal_type.ToString());
            Stream_writer.WriteLine(length.ToString());
            if (signal.Periodic == false)
            {
                for (int i = 0; i < length; i++)
                    Stream_writer.WriteLine(signal.SamplesIndices[i].ToString() + " " + signal.Samples[i].ToString());
            }
            else
            {
                for (int i = 0; i < length; i++)
                    Stream_writer.WriteLine(signal.Frequencies[i].ToString() + " " + signal.FrequenciesAmplitudes[i].ToString() + " " + signal.FrequenciesPhaseShifts[i].ToString());
            }

            Stream_writer.Close();
            file.Close();
        }
        public Signal LoadSignal(string filePath)
        {
            Stream stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            var sr = new StreamReader(stream);
            var sigType = byte.Parse(sr.ReadLine());
            var isPeriodic = byte.Parse(sr.ReadLine());
            long N1 = long.Parse(sr.ReadLine());
            List<float> SigSamples = new List<float>(unchecked((int)N1));
            List<int> SigIndices = new List<int>(unchecked((int)N1));
            List<float> SigFreq = new List<float>(unchecked((int)N1));
            List<float> SigFreqAmp = new List<float>(unchecked((int)N1));
            List<float> SigPhaseShift = new List<float>(unchecked((int)N1));
            if (sigType == 1)
            {
                SigSamples = null;
                SigIndices = null;
            }
            for (int i = 0; i < N1; i++)
            {
                if (sigType == 0 || sigType == 2)
                {
                    var timeIndex_SampleAmplitude = sr.ReadLine().Split();
                    SigIndices.Add(int.Parse(timeIndex_SampleAmplitude[0]));
                    SigSamples.Add(float.Parse(timeIndex_SampleAmplitude[1]));
                }
                else
                {
                    var Freq_Amp_PhaseShift = sr.ReadLine().Split();
                    SigFreq.Add(float.Parse(Freq_Amp_PhaseShift[0]));
                    SigFreqAmp.Add(float.Parse(Freq_Amp_PhaseShift[1]));
                    SigPhaseShift.Add(float.Parse(Freq_Amp_PhaseShift[2]));
                }
            }
            if (!sr.EndOfStream)
            {
                long N2 = long.Parse(sr.ReadLine());
                for (int i = 0; i < N2; i++)
                {
                    var Freq_Amp_PhaseShift = sr.ReadLine().Split();
                    SigFreq.Add(float.Parse(Freq_Amp_PhaseShift[0]));
                    SigFreqAmp.Add(float.Parse(Freq_Amp_PhaseShift[1]));
                    SigPhaseShift.Add(float.Parse(Freq_Amp_PhaseShift[2]));
                }
            }
            stream.Close();
            return new Signal(SigSamples, SigIndices, isPeriodic == 1, SigFreq, SigFreqAmp, SigPhaseShift);
        }
    }
}

