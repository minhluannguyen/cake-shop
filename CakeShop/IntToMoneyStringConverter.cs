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
    class IntToMoneyStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int moneyVal = (value as int?) ?? 0;
            string moneyString = $"{moneyVal}";

            StringBuilder stringBuilder = new StringBuilder(moneyString);

            int countComma = moneyString.Length / 3;

            if((moneyString.Length * 1.0 / 3) - countComma < ConstantVariable.EPSILON)
            {
                countComma -= 1;
            }
            else
            {
                // do nothing
            }

            for (int i = 0; i < countComma; i++)
            {
                int positionComma = moneyString.Length % 3 + 3 * i;
                stringBuilder.Insert(positionComma, ',');
            }            

            stringBuilder.Append(" VNĐ");
            return stringBuilder.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
