using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CakeShop
{
    class Ulties
    {
        public static string Convertor_UNICODE_ASCII(string unicodestring, bool includeSpace = false)
        {                                                                               /* hàm convert từ chuỗi unicode sang chuỗi ascii                    */

            unicodestring = unicodestring.Normalize(NormalizationForm.FormD);


            StringBuilder stringBuilder = new StringBuilder(); /* tạo một string builder để xây dựng chuỗi*/

            for (int i = 0; i < unicodestring.Length; i++)
            {                                                    /* quét từng ký tự và chuyển ký tự đó thành ký tự ascii*/
                System.Globalization.UnicodeCategory unicodeCategory = System.Globalization.CharUnicodeInfo.GetUnicodeCategory(unicodestring[i]);
                if (unicodeCategory != System.Globalization.UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(unicodestring[i]);   /* lưu ký tự đó vào trong string builder dựng sẵn */
                }
            }
            stringBuilder.Replace("Đ", "D");             /* quá trình chuyển đổi không thể chuyển đổi 2 ký tự đ,Đ */
            stringBuilder.Replace("đ", "d");
            if (includeSpace)                            /* nếu người hàm kiểm tra có yêu cầu giữ lại ký tự space thì giữ */
            {
                // do nothing
            }
            else
            {
                stringBuilder.Replace(" ", "");
            }
            /* trả về kết quả chuyển đổi và không thay đổi chuỗi ban đầu */
            string result = ((stringBuilder.ToString()).Normalize(NormalizationForm.FormD)).ToLower();

            return result;
        }
    }
}
