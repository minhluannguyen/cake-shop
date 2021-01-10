using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
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
    /// Interaction logic for DialogCakeImport.xaml
    /// </summary>
    public partial class DialogCakeImport : Window, INotifyPropertyChanged
    {
        public delegate void WindowHandler(object sender, int action);
        public event WindowHandler handler;

        public int Action { set; get; }
        public string ActionName { set; get; }
        public string TitleAction { set; get; }

        public CakeImportOrder CakeImport { set; get; }
        public int previousAmount { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        private DialogCakeImport()
        {
            InitializeComponent();
        }
        public DialogCakeImport(CakeImportOrder cakeImportOrder, int action)
        {
            InitializeComponent();
            this.DataContext = this;

            this.Action = action;
            this.CakeImport = new CakeImportOrder();

            if (cakeImportOrder != null)
            {
                // clone
                this.CakeImport.ID = cakeImportOrder.ID;
                this.CakeImport.ImportOrderName = cakeImportOrder.ImportOrderName;
                this.CakeImport.ProductID = cakeImportOrder.ProductID;
                this.CakeImport.ImportDate = cakeImportOrder.ImportDate;
                this.CakeImport.ExpirationDate = cakeImportOrder.ExpirationDate;
                this.CakeImport.Quantity = cakeImportOrder.Quantity;
                this.CakeImport.ImportPrice = cakeImportOrder.ImportPrice;
                this.CakeImport.Total = cakeImportOrder.Total;

                this.previousAmount = cakeImportOrder.Quantity.GetValueOrDefault();
            }
            else
            {
                //create new
                this.CakeImport.ImportOrderName = string.Empty;
                this.CakeImport.ProductID = null;
                this.CakeImport.ImportDate = null;
                this.CakeImport.ExpirationDate = null;
                this.CakeImport.Quantity = 0;
                this.CakeImport.ImportPrice = 0;
                this.CakeImport.Total = 0;

                this.previousAmount = 0;
            }
            if (this.Action == ConstantVariable.ADD_CAKEIMPORT)
            {
                this.TitleAction = "Nhập hàng";
                this.ActionName = "Nhập";
                this.deleteBtn.Visibility = Visibility.Collapsed;
            }
            else if (this.Action == ConstantVariable.UPDATE_CAKEIMPORT)
            {
                this.TitleAction = "Cập Nhật Đơn Nhập Hàng";
                this.ActionName = "Cập nhật";
            }
            else
            {
                // do nothing
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            handler?.Invoke(this.CakeImport, this.Action);
        }

        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Action = ConstantVariable.CANCEL_ACTION;
            this.Close();
        }


        private void deleteBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Action = ConstantVariable.DEL_CAKE;
            QueryDB.Instance.deleteCakeImport(this.CakeImport);
            this.Close();
        }

        private void actionBtn_Click(object sender, RoutedEventArgs e)
        {
            //Debug.WriteLine($"--{this.Cake.NameTypeCake}");
            string CurrentPath = $"{Directory.GetCurrentDirectory()}";

            if (!QueryDB.Instance.hasSameCakeImport(this.CakeImport))
            {
                if (this.Action == ConstantVariable.ADD_CAKEIMPORT)
                {
                    CakeImport.Total = CakeImport.Quantity * CakeImport.ImportPrice;
                    QueryDB.Instance.addCakeImport(this.CakeImport);
                }
                else if (this.Action == ConstantVariable.UPDATE_CAKEIMPORT)
                {
                    CakeImport.Total = CakeImport.Quantity * CakeImport.ImportPrice;
                    QueryDB.Instance.updateCakeImport(this.CakeImport, this.previousAmount);
                }
                this.Close();
            }
            else
            {
                MessageBox.Show("Đã tồn tại tên đơn nhập hàng này!");
            }
        }

        private void TextBoxInput_KeyDown(object sender, KeyEventArgs e)
        {

        }
        private void Window_Closed(object sender, EventArgs e)
        {

        }

        private void NameCake_Click(object sender, RoutedEventArgs e)
        {
            ListCake screen = new ListCake();
            screen.Owner = this;
            screen.handler += Screen_handler;
            screen.ShowDialog();
        }

        private void Screen_handler(object sender)
        {
            this.CakeImport.ProductID = (sender as Product).ID;
            NameCake.Content = QueryDB.Instance.getNameCakeByID(this.CakeImport.ProductID ?? -1);
        }
    }
}
