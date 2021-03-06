using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Media;

namespace ModernWPF
{
    // modified from Rich Newman's HSLColor class for wpf
    // http://richnewman.wordpress.com/about/code-listings-and-diagrams/hslcolor-class/

    class HSLColor
    {
        // Private data members below are on scale 0-1
        // They are scaled for use externally based on scale
        private double hue = 1.0;
        private double saturation = 1.0;
        private double luminosity = 1.0;

        private const double scale = 1.0;//240.0;

        public double Hue
        {
            get { return hue * scale; }
            //set { hue = CheckRange(value / scale); }
        }
        public double Saturation
        {
            get { return saturation * scale; }
            //set { saturation = CheckRange(value / scale); }
        }
        public double Luminosity
        {
            get { return luminosity * scale; }
            set { luminosity = CheckRange(value / scale); }
        }

        static double CheckRange(double value)
        {
            if (value < 0.0)
                value = 0.0;
            else if (value > 1.0)
                value = 1.0;
            return value;
        }

        public override string ToString()
        {
            return string.Format(CultureInfo.CurrentCulture, "H: {0:#0.##}\tS: {1:#0.##}\tL: {2:#0.##}", Hue, Saturation, Luminosity);
        }

        //public string ToRGBString()
        //{
        //    Color color = (Color)this;
        //    return string.Format(CultureInfo.CurrentCulture, "R: {0:#0.##}\tG: {1:#0.##}\tB: {2:#0.##}", color.R, color.G, color.B);
        //}

        #region Casts to/from System.Drawing.Color
        public static implicit operator Color(HSLColor hslColor)
        {
            double r = 0, g = 0, b = 0;
            if (hslColor.luminosity != 0)
            {
                if (hslColor.saturation == 0)
                    r = g = b = hslColor.luminosity;
                else
                {
                    double temp2 = GetTemp2(hslColor);
                    double temp1 = 2.0 * hslColor.luminosity - temp2;

                    r = GetColorComponent(temp1, temp2, hslColor.hue + 1.0 / 3.0);
                    g = GetColorComponent(temp1, temp2, hslColor.hue);
                    b = GetColorComponent(temp1, temp2, hslColor.hue - 1.0 / 3.0);
                }
            }
            return Color.FromRgb((byte)(255 * r), (byte)(255 * g), (byte)(255 * b));
        }

        private static double GetColorComponent(double temp1, double temp2, double temp3)
        {
            temp3 = MoveIntoRange(temp3);
            if (temp3 < 1.0 / 6.0)
                return temp1 + (temp2 - temp1) * 6.0 * temp3;
            else if (temp3 < 0.5)
                return temp2;
            else if (temp3 < 2.0 / 3.0)
                return temp1 + ((temp2 - temp1) * ((2.0 / 3.0) - temp3) * 6.0);
            else
                return temp1;
        }
        private static double MoveIntoRange(double temp3)
        {
            if (temp3 < 0.0)
                temp3 += 1.0;
            else if (temp3 > 1.0)
                temp3 -= 1.0;
            return temp3;
        }
        private static double GetTemp2(HSLColor hslColor)
        {
            double temp2;
            if (hslColor.luminosity < 0.5)  //<=??
                temp2 = hslColor.luminosity * (1.0 + hslColor.saturation);
            else
                temp2 = hslColor.luminosity + hslColor.saturation - (hslColor.luminosity * hslColor.saturation);
            return temp2;
        }

        public static implicit operator HSLColor(Color color)
        {
            System.Drawing.Color dummy = System.Drawing.Color.FromArgb(color.R, color.G, color.B);

            HSLColor hslColor = new HSLColor();
            hslColor.hue = dummy.GetHue() / 360.0; // we store hue as 0-1 as opposed to 0-360 
            hslColor.luminosity = dummy.GetBrightness();
            hslColor.saturation = dummy.GetSaturation();
            return hslColor;
        }
        #endregion

        //public void SetRGB(byte red, byte green, byte blue)
        //{
        //    HSLColor hslColor = Color.FromRgb(red, green, blue);
        //    this.hue = hslColor.hue;
        //    this.saturation = hslColor.saturation;
        //    this.luminosity = hslColor.luminosity;
        //}

        public HSLColor() { }
        //public HSLColor(Color color)
        //{
        //    SetRGB(color.R, color.G, color.B);
        //}
        //public HSLColor(byte red, byte green, byte blue)
        //{
        //    SetRGB(red, green, blue);
        //}
        //public HSLColor(double hue, double saturation, double luminosity)
        //{
        //    this.Hue = hue;
        //    this.Saturation = saturation;
        //    this.Luminosity = luminosity;
        //}

    }

}
