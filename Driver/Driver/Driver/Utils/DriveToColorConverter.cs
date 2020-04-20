using Driver.ViewModels;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace Driver.Utils
{
    public class DriveToColorConverter : IValueConverter
    {
        private const string PositiveDrivesColor = "#66CC00";
        private const string NegativeDrivesColor = "#FF3333";

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var friendUsename = ((parameter as Label).BindingContext as FriendPopupViewModel).Friend.Username;
            string driverUsername = value as string;

            if (value == null)
            {
                return Color.Transparent;
            }

            if (friendUsename == driverUsername)
            {
                return Color.FromHex(NegativeDrivesColor);
            }
            else
            {
                return Color.FromHex(PositiveDrivesColor);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}