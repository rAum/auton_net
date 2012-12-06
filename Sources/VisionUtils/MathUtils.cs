using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Emgu.CV.Structure;

namespace Auton.CarVision.Video.Utils
{
    class MathUtils
    {
        public static double[] calculatePartialSums(double[] input) 
        { 
            int n = input.Length;
            double[] p = new double[n];
            p[0] = input[0];
            for (int i = 1; i < n; i++)
                p[i] = input[i] + p[i - 1];
            return p;
        }
    }
}
