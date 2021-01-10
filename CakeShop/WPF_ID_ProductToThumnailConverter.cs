using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace CakeShop
{
    class WPF_ID_ProductToThumnailConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int id = (value as int?) ?? 0;
            string Path = string.Empty;
            string nameImage = QueryDB.Instance.getThumbnailOf1Product(id);

            if (!string.IsNullOrEmpty(nameImage))
            {
                Path = $"{Directory.GetCurrentDirectory()}";
                Directory.CreateDirectory($"{Path}\\Images\\Products\\");  // if not exist

                Path = $"{Path}\\Images\\Products\\{nameImage}";
            }
            else
            {
                // do nothing
            }


            Debug.WriteLine($"/> {Path}");

            return Path;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
