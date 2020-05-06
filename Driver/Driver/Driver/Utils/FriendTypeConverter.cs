using Driver.Models;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace Driver.Utils
{
    public class FriendTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((FriendRequestStatus)value == (FriendRequestStatus)parameter)
            {
                return true;
            }

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}