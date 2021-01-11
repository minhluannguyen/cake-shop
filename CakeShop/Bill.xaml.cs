using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for Bill.xaml
    /// </summary>
    public partial class Bill : Window
    {

        BillInfo billInfo;
        public Bill()
        {
            InitializeComponent();
            billInfo = new BillInfo();
            this.DataContext = billInfo;

            dynamic query = QueryDB.Instance.getListProductInCart();
            listProductInCart.ItemsSource = query;

            int total = 0;
            foreach (var item in query)
            {
                total = total + item.Amount;
            }
            billInfo.TotalPrice = total;
        }
        public class BillInfo : INotifyPropertyChanged
        {

            private string _phoneNumber;
            public string PhoneNumber
            {
                get { return _phoneNumber; }
                set
                {
                    _phoneNumber = value;
                    PropertyChanged?.Invoke(this,
                        new PropertyChangedEventArgs("PhoneNumber"));
                }
            }

            private string _nameCustomer;
            public string NameCustomer
            {
                get { return _nameCustomer; }
                set
                {
                    _nameCustomer = value;
                    PropertyChanged?.Invoke(this,
                        new PropertyChangedEventArgs("NameCustomer"));
                }
            }

            public int _totalPrice = 0;
            public int TotalPrice
            {
                get { return _totalPrice; }
                set
                {
                    _totalPrice = value;
                    PropertyChanged?.Invoke(this,
                        new PropertyChangedEventArgs("TotalPrice"));
                }
            }

            public BillInfo()
            {
                                
            }

            public event PropertyChangedEventHandler PropertyChanged;

        }

        private void phoneNumberPlace_LostFocus(object sender, RoutedEventArgs e)
        {
            //this.NameCustomer = QueryDB.Instance.getNameCustomerIfExistByPhoneNumber(this.PhoneNumber);
            
        }

        private void phoneNumberPlace_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                //this.NameCustomer = QueryDB.Instance.getNameCustomerIfExistByPhoneNumber(this.PhoneNumber);
                Debug.WriteLine($"<{billInfo.PhoneNumber}>");
                this.billInfo.NameCustomer = "Yasuo";
            }
            else
            {
                // do nothing
            }
        }


        private void payBtn_Click(object sender, RoutedEventArgs e)
        {

        }
        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
