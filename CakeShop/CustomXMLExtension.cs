using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace CakeShop
{
    public static class CustomXMLExtension
    {
        public static void MoveTo(this Image target, double newX, double newY, double Width, double Height, double during)
        {
            //Vector offset = VisualTreeHelper.GetOffset(target);
            //var top = offset.Y;
            //var left = offset.X;
            //TranslateTransform trans = new TranslateTransform();
            //target.RenderTransform = trans;
            //DoubleAnimation anim1 = new DoubleAnimation(0, newY - top, TimeSpan.FromSeconds(10));
            //DoubleAnimation anim2 = new DoubleAnimation(0, newX - left, TimeSpan.FromSeconds(10));
            //trans.BeginAnimation(TranslateTransform.YProperty, anim1);
            //trans.BeginAnimation(TranslateTransform.XProperty, anim2);
        }
        public static void test(this Image target)
        {

        }
    }
}
