using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace CakeShop
{
    class IsEmptyStringConverter : IValueConverter                              /* converter to check if text in a view is empty ỏ not */
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string textType = value as string;

            if (textType.Length > 0)
                return 1;
            else
                return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
