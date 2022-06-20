using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Media;

namespace Graphics
{
    static class ColorSet
    {
        private static Random rnd = new Random();
        private static List<Color> lColor;
        public static bool IsGenNewListColorOnIteration = true;

        private static int countRepeat = 0;
        private static readonly int countRepeatBeforeDecres = 200;
        private static int delta = 50;

        public static List<Color> GetSetColor(int countColor)
        {
            if (!IsGenNewListColorOnIteration && !(lColor == null || countColor != lColor.Count))
                return lColor;
            return GenerateSetColor(countColor);
        }

        private static bool IsContainsSameColor(Color color)
        {
            foreach(var item in lColor)
            {
                if (Math.Abs(item.R - color.R) < delta) return true;
                if (Math.Abs(item.G - color.G) < delta) return true;
                if (Math.Abs(item.B - color.B) < delta) return true;
            }
            return false;
        }

        public static List<Color> GenerateSetColor(int countColor)
        {
            delta = 50;
            countRepeat = 0;

            lColor = new List<Color>();

            while(lColor.Count != countColor)
            {
                countRepeat += 1;

                var r = rnd.Next(0, 255);
                var g = rnd.Next(0, 255);
                var b = rnd.Next(0, 255);
                var color = Color.FromRgb((byte)r, (byte)g, (byte)b);

                if (!IsContainsSameColor(color))
                    lColor.Add(color);

                if (countRepeat % countRepeatBeforeDecres == 0)
                    delta -= 5;
            }
            
            return lColor;
        }

    }
}

#region Variant2
/*Debug.WriteLine($"range = {rangeColor}");
var initialColorValue = rnd.Next(0, 255);
var distanceBetweenColors = rangeColor / countColor;
Debug.WriteLine("distanceBetweenColors = {0}", distanceBetweenColors);
for (int index = 0; index < countColor; index++)
{
    var buffColorValue = (initialColorValue + distanceBetweenColors) % rangeColor;
    Debug.WriteLine("");
    Debug.WriteLine("buffColorValue = {0}", buffColorValue);
    var r = buffColorValue / Math.Pow(255, 2); buffColorValue /= 255;
    var g = buffColorValue / Math.Pow(255, 1); buffColorValue /= 255;
    var b = buffColorValue;
    Debug.WriteLine("r = {0}, g = {1}, b = {2}", r, g, b);
    lColor.Add(Color.FromRgb((byte)r, (byte)g, (byte)b));
    initialColorValue += distanceBetweenColors;
}*/
#endregion
