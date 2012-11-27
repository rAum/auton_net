using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



namespace BarcodeDetector
{
    class POI
    {
        public int X;
        public int Y;
        public double GX;
        public double GY;
        public double Angle;

        public POI(int x, int y, double gx, double gy, double Angle) 
        {
            this.X = x;
            this.Y = y;
            this.Angle = Angle;
            this.GY = gy;
            this.GX = gx;
        }
    }
}
