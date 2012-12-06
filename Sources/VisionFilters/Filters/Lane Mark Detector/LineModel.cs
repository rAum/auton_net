using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.Structure;
using System.Runtime.InteropServices;

namespace VisionFilters.Filters.Lane_Mark_Detector
{
    class LineModel
    {
       public static List<List<Point>> Lines(List<Point> input, int lanes = 2)
        {
            float[,] samples = new float[input.Count, 2];
            int i = 0;
            foreach (var p in input)
            {
                samples[i, 0] = p.X;
                samples[i, 1] = p.Y;
                ++i;
            }

           MCvTermCriteria term = new MCvTermCriteria();

           Matrix<float> samplesMatrix = new Matrix<float>(samples);
           Matrix<Int32> labels = new Matrix<Int32>(input.Count, 1);
           CvInvoke.cvKMeans2(samplesMatrix, 2, labels, term, lanes, IntPtr.Zero, Emgu.CV.CvEnum.KMeansInitType.RandomCenters, IntPtr.Zero, IntPtr.Zero);

           List<Point> leftLane = new List<Point>(input.Count);
           List<Point> rightLane = new List<Point>(input.Count);
           for (i = 0; i < input.Count; ++i)
           {
               if (labels[i, 0] == 0)
                   leftLane.Add(input[i]);
               else
                   rightLane.Add(input[2]);
           }

           return new List<List<Point>> { leftLane, rightLane };
        }
    }
}
