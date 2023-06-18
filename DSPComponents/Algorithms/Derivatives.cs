using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSPAlgorithms.Algorithms
{
    public class Derivatives: Algorithm
    {
        public Signal InputSignal { get; set; }
        public Signal FirstDerivative { get; set; }
        public Signal SecondDerivative { get; set; }

        public override void Run()
        {

            List<float> fd = new List<float>();
            List<float> sd = new List<float>();
            fd.Add(InputSignal.Samples[0]);
            sd.Add(InputSignal.Samples[1] - (2 * InputSignal.Samples[0]));
            float fder;
            float sder;
            for (int i = 1; i < InputSignal.Samples.Count - 1; i++)
            {
                fder = InputSignal.Samples[i] - InputSignal.Samples[i - 1];
                sder = InputSignal.Samples[i + 1] - (2 * InputSignal.Samples[i]) + InputSignal.Samples[i - 1];
                fd.Add(fder);
                sd.Add(sder);

            }

            FirstDerivative = new Signal(fd, false);
            SecondDerivative = new Signal(sd, false);

        }
    }
}
