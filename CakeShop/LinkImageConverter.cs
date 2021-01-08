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
    class LinkImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string nameImage = value as string;
            string Path = $"{Directory.GetCurrentDirectory()}";
            Directory.CreateDirectory($"{Path}\\Images\\Products\\");  // if not exist

            Path = $"{Path}\\Images\\Products\\{nameImage}";
            Debug.WriteLine($">{Path}<");

            return Path;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
