using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace CakeShop
{
    class DateTimeFormatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var dateTime = value as Nullable<DateTime>;
            string dateStr = "";
            if (dateTime.HasValue)
            {
                CultureInfo iv = CultureInfo.InvariantCulture;
                dateStr = dateTime.Value.ToString("dd/MM/yyyy", iv);
            }
            return dateStr;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
