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
    /// Interaction logic for ListTypeCake.xaml
    /// </summary>
    public partial class ListTypeCake : Window
    {

        public delegate void WindowHandler(object sender);
        public event WindowHandler handler;

        public TypeCake Type { set; get; }
        public ListTypeCake()
        {
            InitializeComponent();

            typeCakeListView.ItemsSource = QueryDB.Instance.getOriginTypeCakeList();
        }

        private void ListViewItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var item = sender as ListViewItem;

            if (item != null && item.IsSelected)
            {
                dynamic selectedItem = typeCakeListView.SelectedItem;

                this.Type = new TypeCake();
                this.Type.ID = selectedItem.ID;
                this.Type.NameTypeCake = selectedItem.NameTypeCake;

                this.Close();
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            handler?.Invoke(this.Type);
        }
    }
}
