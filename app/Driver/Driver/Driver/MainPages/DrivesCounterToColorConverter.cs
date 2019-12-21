using System;
using Xamarin.Forms;
using System.Globalization;

namespace Driver.MainPages
{
    public class DrivesCounterToColorConverter : IValueConverter
    {
        public static string[] PositiveDrivesColors = new string[] { "#CEF6CE", "#A9F5A9", "#81F781", "#58FA58", "#2EFE2E", "#00FF00", "#01DF01" };
        public static string[] NegativeDrivesColors = new string[] { "#F5A9A9", "#F78181", "#FA5858", "#FE2E2E", "#FF0000", "#DF0101", "8A0808" };

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return Color.Transparent;

            int counter = (int)value;
            if (counter == 0)
                return Color.Transparent;

            if (counter > 0)
                return Color.FromHex((PositiveDrivesColors.Length > counter) ? PositiveDrivesColors[counter] : PositiveDrivesColors[PositiveDrivesColors.Length - 1]);
            else
            {
                counter *= -1;
                return Color.FromHex((NegativeDrivesColors.Length > counter) ? NegativeDrivesColors[counter] : NegativeDrivesColors[NegativeDrivesColors.Length - 1]);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
