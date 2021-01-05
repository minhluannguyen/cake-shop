using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace CakeShop
{
    class RawPriceToPriceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var price = value as Nullable<int>;
            string result = "Không rõ";

            if (price != null)
            {
                result = String.Format("{0:n0}", price) + " VNĐ";
            }

            return result; 
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
