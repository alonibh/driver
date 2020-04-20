using System;
using System.Globalization;
using Xamarin.Forms;

namespace Driver.Utils
{
    public class DriveCounterToColorConverter : IValueConverter
    {
        private const string PositiveDrivesColor = "#66CC00";
        private const string NegativeDrivesColor = "#FF3333";

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return Color.Transparent;
            }

            int counter = (int)value;
            if (counter == 0)
            {
                return Color.DarkGray;
            }

            if (counter > 0)
            {
                return Color.FromHex(PositiveDrivesColor);
            }
            else
            {
                return Color.FromHex(NegativeDrivesColor);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}