using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VisionFilters.Filters;
using System.Drawing;
using RANSAC.Functions;

namespace RANSAC
{
    /// <summary>
    /// RANdom SAmple Consensus implementation.
    /// It is an iterative method to estimate parameters of a mathematical model from a set of observed data which contains outliers.
    /// http://en.wikipedia.org/wiki/RANSAC
    /// </summary>
    public class RANSAC
    {
        /// <summary>
        /// Tries to find the best quadratic model for data.
        /// It may find none even if there might exists solution.
        /// </summary>
        /// <param name="iterations">number of iterations. More means higher probability for finding optimal solution but costs more time.</param>
        /// <param name="init_samples">how many random samples are needed to prepare model</param>
        /// <param name="n">how many points must fit between error threshold</param>
        /// <param name="error_threshold">what is the biggest error that may pass</param>
        /// <param name="inputData">observed data</param>
        /// <returns>parabola object or null if no good enough model was found.</returns>
        public static Parabola fit(int iterations, int init_samples, int n, double error_threshold, List<Point> inputData)
        {
            Parabola best_fit = null;
            double best_error = double.MaxValue;
            double model_error;

            for (int i = 0; i < iterations; ++i)
            {
                List<Point> initial = GetRandomSample(inputData, init_samples);
                Parabola model = Parabola.fit(initial);
                if (model == null) continue;
                List<Point> consensus_set = new List<Point>();
                
                double err;
                model_error = 0;
                foreach (var p in inputData)
                {
                    err = model.value(p.Y) - p.X;
                    if (Math.Abs(err) < error_threshold ) {
                        consensus_set.Add(p);
                        model_error += err;
                    }
                }

                if (consensus_set.Count > n)
                {
                    if (model_error < best_error)
                    {
                        best_fit = model;
                        best_error = model_error;
                    }
                }

            }

            return best_fit;
        }

        /// <summary>
        /// Gets random sample of points, at most samplesCount.
        /// </summary>
        /// <param name="input">Input points</param>
        /// <param name="samplesCount">how many points take</param>
        /// <returns>random subset of input</returns>
        public static List<Point> GetRandomSample(List<Point> input, int samplesCount)
        {
            List<Point> sample = new List<Point>(samplesCount);

            List<int> indexToTake = new List<int>(input.Count);
            for (int i = 0; i < input.Count; ++i)
                indexToTake.Add(i);

            Random rnd = new Random();
            int choice;
            int limit = Math.Min(input.Count, samplesCount);
            for (int i = 0; i < limit; ++i)
            {
                choice = rnd.Next(indexToTake.Count);
                sample.Add(input[indexToTake[choice]]);
                indexToTake.RemoveAt(choice);
            }

            return sample;
        }
    }
}
