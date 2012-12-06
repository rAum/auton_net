using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace VisionFilters.Filters
{
    /// <summary>
    /// This class represents a set of pixels that may lie on line mark.
    /// </summary>
    class LaneMarkPixelCandidates
    {
        private List<Point> pixels;
        private int[] bbox;

        public LaneMarkPixelCandidates(int Capacity = 0)
        {
            pixels = new List<Point>(Capacity);
            bbox = new int[] { int.MaxValue, int.MaxValue, 0, 0 };
        }

        public void Add(int x, int y)
        {
            pixels.Add(new Point(x, y));
            if (x < bbox[0]) bbox[0] = x;
            else if (x > bbox[2]) bbox[2] = x;

            if (y < bbox[1]) bbox[1] = y;
            else if (y > bbox[3]) bbox[3] = y;
        }

        public Point CenterMass
        {
            get { return CalculateCenterOfMass();  }
        }

        public Rectangle BoundingBox
        {
            get { return new Rectangle(bbox[0], bbox[1], bbox[2] - bbox[0], bbox[3] - bbox[1]); }
        }

        /// <summary>
        /// center of mass
        /// </summary>
        /// <returns></returns>
        private Point CalculateCenterOfMass()
        {
            uint accX = 0;
            uint accY = 0;

            foreach (var p in pixels)
            {
                accX += (uint)p.X;
                accY += (uint)p.Y;
            }

            return new Point((int)accX / pixels.Count, (int)accY / pixels.Count);
        }

        public int Count { get { return pixels.Count; } }

        public List<Point> Pixels { get { return pixels; } }
    }
}
