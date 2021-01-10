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
    /// Interaction logic for ListCake.xaml
    /// </summary>
    public partial class ListCake : Window
    {
        public delegate void WindowHandler(object sender);
        public event WindowHandler handler;

        public Product Cake { set; get; }
        public ListCake()
        {
            InitializeComponent();

            cakeListView.ItemsSource = QueryDB.Instance.getOriginProductList();
        }

        private void ListViewItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var item = sender as ListViewItem;

            if (item != null && item.IsSelected)
            {
                dynamic selectedItem = cakeListView.SelectedItem;

                this.Cake = new Product();
                this.Cake.ID = selectedItem.ID;
                this.Cake.Name = selectedItem.Name;

                this.Close();
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            handler?.Invoke(this.Cake);
        }
    }
}
