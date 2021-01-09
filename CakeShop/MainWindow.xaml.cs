using System;
using System.Collections.Generic;
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
            dataListView.ItemsSource = QueryDB.Instance.getBindingCakeList();
        }
        public MainWindow(bool a)
        {
            InitializeComponent();
            dataListView.ItemsSource = QueryDB.Instance.getBindingCakeList();
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

                case ConstantVariable.ADD_CAKE:
                case ConstantVariable.UPDATE_CAKE:
                case ConstantVariable.DEL_CAKE:
                    //Debug.WriteLine($"{(sender as Product).Name} - {action}");
                    this.BackstageTabItem_MouseLeftButtonDown(this.cakeItem, null);
                    break;
            }
        }
        private void addItem_Click(object sender, RoutedEventArgs e)
        {
            var senderButton = sender as Button;

            switch (senderButton.Name)
            {
                case "addNewTypeCakeBtn":
                    {
                        var screen = new DialogTypeCake(null, ConstantVariable.ADD_TYPECAKE);
                        screen.handler += this.ObjectWindowHandler;
                        screen.Owner = this;
                        screen.ShowDialog();
                    }

                    break;
                case "addNewCakeBtn":
                    {
                        var screen = new DialogCake(null, ConstantVariable.ADD_CAKE);
                        screen.handler += this.ObjectWindowHandler;
                        screen.Owner = this;
                        screen.ShowDialog();

                    }
                    break;
            }
        }


        private void ListViewItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var item = sender as ListViewItem;
            
            if (item != null && item.IsSelected)
            {          
                switch (this.RibbonItem)
                {
                    case ConstantVariable.RIBBON_TYPECAKE:
                        {
                            dynamic selectedItem = typeCakeListViewRibbon.SelectedItem;
                            //MessageBox.Show($"{selectedItem.NameTypeCake}");

                            TypeCake typeCake = new TypeCake();
                            typeCake.ID = selectedItem.ID;
                            typeCake.NameTypeCake = selectedItem.NameTypeCake;

                            var screen = new DialogTypeCake(typeCake, ConstantVariable.UPDATE_TYPECAKE);
                            screen.handler += this.ObjectWindowHandler;
                            screen.Owner = this;
                            screen.ShowDialog();
                        }

                        break;

                    case ConstantVariable.RIBBON_CAKE:
                        {
                            dynamic selectedItem = cakeListViewRibbon.SelectedItem;
                            //MessageBox.Show($"{selectedItem.NameTypeCake}");

                            Product product = new Product();
                            product.ID = selectedItem.ID;
                            product.Name = selectedItem.NameCake;
                            product.Price = selectedItem.Price;
                            product.IDTypeCake = selectedItem.Type;
                            product.Description = selectedItem.Description;

                            var screen = new DialogCake(product, ConstantVariable.UPDATE_CAKE);
                            screen.handler += this.ObjectWindowHandler;
                            screen.Owner = this;
                            screen.ShowDialog();

                        }

                        break;
                }

            }

            
        }

        private void RibbonWindow_Closed(object sender, EventArgs e)
        {
            
        }

        private void RibbonWindow_Loaded(object sender, RoutedEventArgs e)
        {
            StaticTask.Instance.cleanFileInDirectory($"{Directory.GetCurrentDirectory()}\\Images\\ImagesTemp");
        }

        private void backstage_IsOpenChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            dataListView.ItemsSource = QueryDB.Instance.getBindingCakeList();
        }

        private void ItemProductPreview_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var selectedProduct = (Grid)sender;
            //Get current item.
            //var senderStackPanel = (StackPanel)((Grid)((Border)((Canvas)((StackPanel)(selectedProductImg).Parent).Parent).Parent).Parent).Parent;
            var senderStackPanel = (StackPanel)(selectedProduct.Parent);

            //Get TextBlock contain item's id.
            var ID_Product = ((TextBlock)VisualTreeHelper.GetChild(senderStackPanel, 2)).Text as string;
            var nameProduct = ((TextBlock)VisualTreeHelper.GetChild(senderStackPanel, 1)).Text as string;
            MessageBox.Show($"> Name: {nameProduct} - ID: {ID_Product}");

            // get Product by that ID
            Product product = QueryDB.Instance.findProductByID(Int32.Parse(ID_Product));

        }
    }
}
