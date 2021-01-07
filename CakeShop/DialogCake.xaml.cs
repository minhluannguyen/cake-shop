using System;
using System.Collections.Generic;
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
    /// Interaction logic for DialogCake.xaml
    /// </summary>
    public partial class DialogCake : Window
    {
        public delegate void WindowHandler(object sender, int action);
        public event WindowHandler handler;


        public int Action { set; get; }
        public string ActionName { set; get; }
        public string TitleAction { set; get; }

        public Product Cake { set; get; }
        private DialogCake()
        {
            InitializeComponent();
        }
        public DialogCake(Product product, int action)
        {
            InitializeComponent();
            this.DataContext = this;

            this.Action = action;
            this.Cake = new Product();

            if (product != null)
            {
                // clone
                this.Cake.ID = product.ID;
                this.Cake.Name = product.Name;
                this.Cake.Description = product.Description;
                this.Cake.Price = product.Price;
            }
            else
            {
                // create new
                this.Cake.ID = QueryDB.Instance.getLastIDCake() + 1;
            }
            if (this.Action == ConstantVariable.ADD_CAKE)
            {
                this.TitleAction = "Thêm Bánh";
                this.ActionName = "Thêm";
                this.deleteBtn.Visibility = Visibility.Collapsed;
            }
            else if (this.Action == ConstantVariable.UPDATE_CAKE)
            {
                this.TitleAction = "Cập Nhật Bánh";
                this.ActionName = "Cập nhật";
                //this.keywordPlaceholderTextBlock.Visibility = Visibility.Hidden;
            }
            else
            {
                // do nothing
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            handler?.Invoke(this.Cake, this.Action);
        }

        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Action = ConstantVariable.CANCEL_ACTION;
            this.Close();
        }


        private void deleteBtn_Click(object sender, RoutedEventArgs e)
        {
            //this.Action = ConstantVariable.DEL_TYPECAKE;
            //QueryDB.Instance.deleteATypeCake(this.Cake);
            //this.Close();
        }

        private void actionBtn_Click(object sender, RoutedEventArgs e)
        {
            //Debug.WriteLine($"--{this.Cake.NameTypeCake}");
            //if (!QueryDB.Instance.hasSameNameTypeCake(this.Cake))
            //{
            //    if (this.Action == ConstantVariable.ADD_TYPECAKE)
            //    {
            //        QueryDB.Instance.addATypeCake(this.Cake);
            //    }
            //    else if (this.Action == ConstantVariable.UPDATE_TYPECAKE)
            //    {
            //        QueryDB.Instance.updateATypeCake(this.Cake);
            //    }
            //    this.Close();
            //}
            //else
            //{
            //    MessageBox.Show("Đã tồn tại tên loại bánh này!");
            //}
        }

        private void TextBoxInput_KeyDown(object sender, KeyEventArgs e)
        {

        }

        //private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        //{
        //    keywordPlaceholderTextBlock.Visibility = Visibility.Hidden;
        //}

        //private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        //{
        //    if (keywordTextBox.Text.Length == 0)
        //    {
        //        keywordPlaceholderTextBlock.Visibility = Visibility.Visible;
        //    }
        //}
    }
}
