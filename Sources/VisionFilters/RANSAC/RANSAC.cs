using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VisionFilters.Filters;
using System.Drawing;
using RANSAC.Functions;
using VisionFilters.Filters.Lane_Mark_Detector;

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
            double err;
            int consensus_set;

            for (int i = 0; i < iterations; ++i)
            {
                ShuffleAtBegin(ref inputData, init_samples);
                Parabola model = Parabola.fit(inputData, init_samples);
                if (model == null) continue;

                consensus_set = 0;
                model_error = 0;
                foreach (var p in inputData)
                {
                    err = model.value(p.Y) - p.X;
                    if (Math.Abs(err) < error_threshold ) {
                        consensus_set += 1;
                        model_error   += err;
                    }
                }

                if (consensus_set >= n)
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

        public static List<Parabola> fit2(int iterations, int init_samples, int n, double error_threshold, List<Point> inputData)
        {
            Parabola best1 = null;
            Parabola best2 = null;
            double best_error1 = double.MaxValue;
            double best_error2 = double.MaxValue;
            double model_error1;
            double model_error2;
            double err1, err2;
            int consensus_set1;
            int consensus_set2;

            Parabola model1, model2;
            for (int i = 0; i < iterations; ++i)
            {
                ShuffleAtBegin(ref inputData, init_samples);
                model1 = Parabola.fit(inputData, init_samples);
                ShuffleAtBegin(ref inputData, init_samples);
                model2 = Parabola.fit(inputData, init_samples);

                consensus_set1 = consensus_set2 = 0;
                model_error1 = model_error2 = 0;

                if (model1 != null)
                {
                    foreach (var p in inputData)
                    {
                        err1 = Math.Abs(model1.value(p.Y) - p.X);
                        if (err1 <= error_threshold)
                        {
                            consensus_set1 += 1;
                            model_error1 += err1;
                        }
                    }

                    if (consensus_set1 >= n)
                    {
                        if (model_error1 < best_error1)
                        {
                            best1 = model1;
                            best_error1 = model_error1;
                        }
                    }
                }
                if (model2 != null)
                {
                    foreach (var p in inputData)
                    {
                        err2 = Math.Abs(model2.value(p.Y) - p.X);
                        if (err2 <= error_threshold)
                        {
                            consensus_set2 += 1;
                            model_error2 += err2;
                        }
                    }

                    if (consensus_set2 >= n)
                    {
                        if (model_error2 < best_error2)
                        {
                            best2 = model2;
                            best_error2 = model_error2;
                        }
                    }
                }

            }
            return new List<Parabola>() { best1, best2 };
            //return new SimpleRoadModel(best1, best2);
        }

        /// <summary>
        /// Shuffle at begin #count objects in-place
        /// </summary>
        /// <param name="input">list of objects</param>
        /// <param name="count">how many objects shuffle</param>
        public static void ShuffleAtBegin(ref List<Point> input, int count)
        {
            Random rnd = new Random();
            int max = input.Count - 1;
            Point p;
            for (int i = 0; i < count; ++i)
            {
                int j = rnd.Next(i, max);
                p = input[j];
                input[j] = input[i];
                input[i] = p;
            }
        }
    }
}
