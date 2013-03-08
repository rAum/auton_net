using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace VisionFilters.Filters.Lane_Mark_Detector
{
    /// <summary>
    /// Point Cloud of lane candidates
    /// </summary>
    public class LanePointCloud : IEnumerable<Point>
    {
        const int SECTOR_HEIGHT = 80;
        const int SECTOR_WIDTH_ALLOCATION = 5120;
        int sectors;
        List<Point>[] points;

        public LanePointCloud()
        {
            sectors = CamModel.Height / SECTOR_HEIGHT;
            points = new List<Point>[sectors];
            for (int i = 0; i < sectors; ++i)
            {
                points[i] = new List<Point>(SECTOR_WIDTH_ALLOCATION);
            }
        }

        public void Add(Point p)
        {
            points[p.Y / SECTOR_HEIGHT].Add(p); 
        }

        public List<Point> GetBin(int i)
        {
            return points[i % sectors];
        }

        Random rnd = new Random();

        public Point GetRandom(int bin)
        {
            int i = bin % sectors;
            
            while (points[i].Count == 0) i = (i + 1) % sectors;

            return points[i][rnd.Next(0, points[i].Count)];
        }

        public IEnumerator<Point> GetEnumerator()
        {
            for (int i = 0; i < sectors; ++i)
                foreach (Point p in points[i])
                    yield return p;
        }

        private int ComputeCount()
        {
            int count = 0;
            foreach (var list in points)
                count += list.Count;
            return count;
        }

        public int Count { get { return ComputeCount(); } }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
