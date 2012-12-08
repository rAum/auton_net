using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VisionFilters
{
    internal class InputProperties
    {
        public int width, height;

        public InputProperties(int width_ = 320, int height_ = 240)
        {
            width  = width_;
            height = height_;
        }
    }

    public class CamModel
    {
        static InputProperties prop = new InputProperties();
        static float metersToPixelsRatio = 20.0f; // 1m = 20 px

        public static int Width
        {
            get { return prop.width; }
        }

        public static int Height
        {
            get { return prop.height; }
        }

        /// <summary>
        /// after perspective transform
        /// </summary>
        /// <param name="meters"></param>
        /// <returns></returns>
        public static int ToPixels(float meters)
        {
            return prop.height - (int)Math.Round(meters * metersToPixelsRatio);
        }
    }
}
