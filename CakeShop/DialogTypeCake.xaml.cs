using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CakeShop
{
    /// <summary>
    /// Interaction logic for DialogTypeCake.xaml
    /// </summary>
    public partial class DialogTypeCake : Window
    {
        public delegate void WindowHandler(object sender, int action);
        public event WindowHandler handler;

        
        public int Action { set; get; }
        public string ActionName { set; get; }
        public string TitleAction { set; get; }

        public TypeCake Type { set; get; }

        public DialogTypeCake(TypeCake typeCake, int action)
        {
            InitializeComponent();
            this.DataContext = this;

            this.Type = new TypeCake();
            this.Action = action;

            if (typeCake != null)
            {
                // clone
                this.Type.ID = typeCake.ID;
                this.Type.NameTypeCake = typeCake.NameTypeCake;
            }
            else
            {
                // create new
                this.Type.ID = QueryDB.Instance.getLastIDTypeCake() + 1;
            }
            if (this.Action == ConstantVariable.ADD_TYPECAKE)
            {
                this.TitleAction = "Thêm Loại Bánh";
                this.ActionName = "Thêm";
            }
            else if (this.Action == ConstantVariable.UPDATE_TYPECAKE)
            {
                this.TitleAction = "Cập Nhật Tên Loại Bánh";
                this.ActionName = "Cập nhật";
                this.keywordPlaceholderTextBlock.Visibility = Visibility.Hidden;
            }
            else
            {
                // do nothing
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            handler?.Invoke(this.Type, this.Action);
        }

        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Action = ConstantVariable.CANCEL_ACTION;
            this.Close();
        }

        private void actionBtn_Click(object sender, RoutedEventArgs e)
        {
            //Debug.WriteLine($"--{this.Type.NameTypeCake}");
            if(this.Action == ConstantVariable.ADD_TYPECAKE)
            {
                QueryDB.Instance.addATypeCake(this.Type);
            }
            else if (this.Action == ConstantVariable.UPDATE_TYPECAKE)
            {
                QueryDB.Instance.updateATypeCake(this.Type);
            }
            this.Close();
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            keywordPlaceholderTextBlock.Visibility = Visibility.Hidden;
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (keywordTextBox.Text.Length == 0)
            {
                keywordPlaceholderTextBlock.Visibility = Visibility.Visible;
            }
        }
    }
}
