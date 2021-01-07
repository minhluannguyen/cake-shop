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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CakeShop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Fluent.RibbonWindow
    {
        CakeStoreDBEntities db = new CakeStoreDBEntities();

        public int RibbonItem { get; set; }
        public MainWindow()
        {
            InitializeComponent();
        }
        public MainWindow(bool a)
        {
            InitializeComponent();
        }


        private void FluentButtonQuit_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void txtboxSearch_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void BackstageTabItem_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var senderItem = sender as Fluent.BackstageTabItem;
            dynamic query;

            switch (senderItem.Name)
            {
                case "TypeCakeItem":
                    this.RibbonItem = ConstantVariable.RIBBON_TYPECAKE;
                    query = QueryDB.Instance.getBindingTypeCake();
                    typeCakeListViewRibbon.ItemsSource = query;
                    break;
                case "cakeItem":
                    this.RibbonItem = ConstantVariable.RIBBON_CAKE;
                    query = QueryDB.Instance.getBindingCakeList();
                    cakeListViewRibbon.ItemsSource = query;
                    break;
            }
        }

        private void ObjectWindowHandler(object sender, int action)
        {
            
            switch (action)
            {
                case ConstantVariable.ADD_TYPECAKE:
                case ConstantVariable.UPDATE_TYPECAKE:
                case ConstantVariable.DEL_TYPECAKE:
                    Debug.WriteLine($"{(sender as TypeCake).NameTypeCake} - {action}");
                    this.BackstageTabItem_MouseLeftButtonDown(this.TypeCakeItem, null);
                    break;
            }
        }
        private void addItem_Click(object sender, RoutedEventArgs e)
        {
            var senderButton = sender as Button;

            switch (senderButton.Name)
            {
                case "addNewTypeCakeBtn":
                    var screen = new DialogTypeCake(null, ConstantVariable.ADD_TYPECAKE);
                    screen.handler += this.ObjectWindowHandler;
                    screen.Owner = this;
                    screen.ShowDialog();

                    break;
            }
        }

        private void TypeCake_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void ListViewItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var item = sender as ListViewItem;
            
            if (item != null && item.IsSelected)
            {          
                switch (this.RibbonItem)
                {
                    case ConstantVariable.RIBBON_TYPECAKE:
                        dynamic selectedItem = typeCakeListViewRibbon.SelectedItem;
                        //MessageBox.Show($"{selectedItem.NameTypeCake}");

                        TypeCake typeCake = new TypeCake();
                        typeCake.ID = selectedItem.ID;
                        typeCake.NameTypeCake = selectedItem.NameTypeCake;

                        var screen = new DialogTypeCake(typeCake, ConstantVariable.UPDATE_TYPECAKE);
                        screen.handler += this.ObjectWindowHandler;
                        screen.Owner = this;
                        screen.ShowDialog();

                        break;
                }

            }

            
        }
    }
}
