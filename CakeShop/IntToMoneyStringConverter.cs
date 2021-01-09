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
            int startInsert = (moneyString.Length % 3 == 0 ? 3 : moneyString.Length % 3);
            for (int i = 0; i < countComma; i++)
            {
                int positionComma = startInsert + 3 * i + i;
                stringBuilder.Insert(positionComma, ',');
            }            

            stringBuilder.Append(" VNĐ");


            Debug.WriteLine($"{moneyVal} - {stringBuilder.ToString()}");

            return stringBuilder.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var priceStr = value as string;
            StringBuilder builder = new StringBuilder();
            foreach(var character in priceStr)
            {
                if (ConstantVariable.numeric.Contains(character)){
                    builder.Append(character);
                }
            }
            var moneyVal = Int32.Parse(builder.ToString());

            return moneyVal;
        }
    }
}
