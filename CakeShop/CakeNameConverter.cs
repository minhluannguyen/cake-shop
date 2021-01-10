using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace CakeShop
{
    class CakeNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int ID_Product = (value as int?) ?? -1;
            string nameProduct = "";

            if (ID_Product < 0)
            {
                nameProduct = "Chọn Sản Phẩm";
            }
            else
            {
                nameProduct = QueryDB.Instance.getNameCakeByID(ID_Product);

                if (string.IsNullOrEmpty(nameProduct))
                {
                    nameProduct = "Không thể tìm thấy";
                }
                else
                {
                    // do nothing
                }
            }

            return nameProduct;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
