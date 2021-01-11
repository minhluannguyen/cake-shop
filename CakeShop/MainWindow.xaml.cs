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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CakeShop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Fluent.RibbonWindow, INotifyPropertyChanged
    {
        CakeStoreDBEntities db = new CakeStoreDBEntities();

        TypeCake type_filter = null;

        public int _totalPrice = 0;
        private string NameItemCart = string.Empty;         //      <--- this will resolve allllllllllllllllllllllllllllllllllllllllllllllll

        public int TotalPrice {
            get { return _totalPrice; }
            set
            {
                _totalPrice = value;
                PropertyChanged?.Invoke(this,
                    new PropertyChangedEventArgs("TotalPrice"));
            }
        }


        public int RibbonItem { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            this.updateListviewSource();
            this.DataContext = this;
        }
        public MainWindow(bool a)
        {
            InitializeComponent();
            this.updateListviewSource();
            this.DataContext = this;
        }

        private void FluentButtonQuit_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void txtboxSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var keyword = (sender as TextBox).Text as string;
                var listItem = QueryDB.Instance.getBindingCakeList(ConstantVariable.FILTER_BY_KEYWORD,keyword);
                dataListView.ItemsSource = listItem;

            }
            else
            {
                // do nothing
            }
            /*
            //BindingList<FoodRecipe> listBySearch = new BindingList<FoodRecipe>();
            keyword = ConstantAction.Convertor_UNICODE_ASCII(keyword, true);

            //foreach (var food in this.recipes)
            //{
            //    listBySearch.Add(food);
            //}

            // Define the search terms. This list could also be dynamically populated at runtime.  
            string[] keys = keyword.ToLower().Split(new char[] { '.', '?', '!', ' ', ';', ':', ',' },
                                                    StringSplitOptions.RemoveEmptyEntries);
            // search version find correct a word
            // Find sentences that contain all the terms in the wordsToMatch array.  
            // Note that the number of terms to match is not specified at compile time.  
            //var sentenceQuery = from recipe in listBySearch
            //                    let wordName = (ConstantAction.Convertor_UNICODE_ASCII(recipe.Name, true)).Split(new char[] { '.', '?', '!', ' ', ';', ':', ',' },
            //                                            StringSplitOptions.RemoveEmptyEntries)
            //                    where (
            //                        wordName.Distinct().Intersect(keys).Count() == keys.Count()
            //                    )
            //                    select recipe;

            // search version find nearest same word
            var sentenceQuery = (
                from recipe in this.recipes
                where keys.All(s => (ConstantAction.Convertor_UNICODE_ASCII(recipe.Name, true)).IndexOf(s) > -1)
                select recipe
                );


            //Debug.WriteLine("-------------------------------------------------------");
            //foreach (var re in results)
            //{
            //    Debug.WriteLine(re.Name);
            //}
            //Debug.WriteLine("=======================================================");


            BindingList<FoodRecipe> resultSearch = new BindingList<FoodRecipe>();
            foreach (var nameFood in sentenceQuery)
            {
                resultSearch.Add(nameFood);
            }

            return resultSearch;
            */
            
        }

        private void BackstageTabItem_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var senderItem = sender as Fluent.BackstageTabItem;

            switch (senderItem.Name)
            {
                case "TypeCakeItem":
                    this.RibbonItem = ConstantVariable.RIBBON_TYPECAKE;
                    {
                        dynamic query = QueryDB.Instance.getBindingTypeCake();
                        typeCakeListViewRibbon.ItemsSource = query;
                    }
                    break;
                case "cakeItem":
                    this.RibbonItem = ConstantVariable.RIBBON_CAKE;
                    {
                        dynamic query = QueryDB.Instance.getBindingCakeList();
                        cakeListViewRibbon.ItemsSource = query;
                    }
                    break;
                case "cakeImportItem":
                    this.RibbonItem = ConstantVariable.RIBBON_CAKEIMPORT;
                    {
                        dynamic query = QueryDB.Instance.getBindingCakeImport();
                        cakeImportOrderListViewRibbon.ItemsSource = query;
                    }
                    break;
                case "CartItem":
                    this.RibbonItem = ConstantVariable.RIBBON_PAYMENT;
                    {
                        dynamic query = QueryDB.Instance.getListProductInCart();
                        detailCartListViewRibbon.ItemsSource = query;

                        if(query.Count == 0)
                        {
                            payCartBtn.IsEnabled = false;
                        }

                        int total = 0;
                        foreach(var item in query)
                        {
                            total = total + item.Amount;
                        }
                        this.TotalPrice = total;
                    }
                    break;
                case "SettingItem":
                    this.RibbonItem = ConstantVariable.RIBBON_SETTING;
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

                case ConstantVariable.ADD_CAKEIMPORT:
                case ConstantVariable.UPDATE_CAKEIMPORT:
                case ConstantVariable.DEL_CAKEIMPORT:
                    //Debug.WriteLine($"{(sender as Product).Name} - {action}");
                    this.BackstageTabItem_MouseLeftButtonDown(this.cakeImportItem, null);
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
                case "addNewCakeImportBtn":
                    {
                        var screen = new DialogCakeImport(null, ConstantVariable.ADD_CAKEIMPORT);
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

                    case ConstantVariable.RIBBON_CAKEIMPORT:
                        {
                            dynamic selectedItem = cakeImportOrderListViewRibbon.SelectedItem;
                            //MessageBox.Show($"{selectedItem.NameTypeCake}");

                            CakeImportOrder cakeImportOrder = new CakeImportOrder();
                            cakeImportOrder.ID = selectedItem.ID;
                            cakeImportOrder.ImportOrderName = selectedItem.ImportOrderName;
                            cakeImportOrder.ProductID = selectedItem.ProductID;
                            cakeImportOrder.ImportDate = selectedItem.ImportDate;
                            cakeImportOrder.ExpirationDate = selectedItem.ExpirationDate;
                            cakeImportOrder.Quantity = selectedItem.Quantity;
                            cakeImportOrder.ImportPrice = selectedItem.ImportPrice;
                            cakeImportOrder.Total = selectedItem.Total;

                            var screen = new DialogCakeImport(cakeImportOrder, ConstantVariable.UPDATE_CAKEIMPORT);
                            screen.handler += this.ObjectWindowHandler;
                            screen.Owner = this;
                            screen.ShowDialog();

                        }

                        break;
                    case ConstantVariable.RIBBON_PAYMENT:
                        {
                            dynamic selectedItem = detailCartListViewRibbon.SelectedItem;

                            Product product = QueryDB.Instance.findProductByID(selectedItem.ID_Product);

                            DetailCakeScreen screen = new DetailCakeScreen(product);
                            screen.Owner = this;
                            screen.ShowDialog();

                            BackstageTabItem_MouseLeftButtonDown(CartItem, null);
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
            this.updateListviewSource();
        }

        private void ItemProductPreview_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var selectedProduct = (Grid)sender;
            //Get current item.
            //var senderStackPanel = (StackPanel)((Grid)((Border)((Canvas)((StackPanel)(selectedProductImg).Parent).Parent).Parent).Parent).Parent;
            var senderStackPanel = (StackPanel)(selectedProduct.Parent);

            //Get TextBlock contain item's id.
            var ID_Product = ((TextBlock)VisualTreeHelper.GetChild(senderStackPanel, 2)).Text as string;
            //var nameProduct = ((TextBlock)VisualTreeHelper.GetChild(senderStackPanel, 1)).Text as string;
            //MessageBox.Show($"> Name: {nameProduct} - ID: {ID_Product}");

            // get Product by that ID
            Product product = QueryDB.Instance.findProductByID(Int32.Parse(ID_Product));

            DetailCakeScreen screen = new DetailCakeScreen(product);
            screen.Owner = this;
            screen.ShowDialog();
            BackstageTabItem_MouseLeftButtonDown(CartItem, null);
        }
        private void Sort_Filter_Options_Click(object sender, RoutedEventArgs e)
        {
            var options = sender as Button;

            switch (options.Name)
            {
                case "fullFilter":
                    dataListView.ItemsSource = QueryDB.Instance.getBindingCakeList(ConstantVariable.FILTER_ALL);
                    break;
                case "sortByAZ":
                    dataListView.ItemsSource = QueryDB.Instance.getBindingCakeList(ConstantVariable.SORT_BY_AZ);
                    break;
                case "sortByZA":
                    dataListView.ItemsSource = QueryDB.Instance.getBindingCakeList(ConstantVariable.SORT_BY_ZA);
                    break;
                case "sortByIncPrice":
                    dataListView.ItemsSource = QueryDB.Instance.getBindingCakeList(ConstantVariable.SORT_BY_INC_PRICE);
                    break;
                case "sortByDecPrice":
                    dataListView.ItemsSource = QueryDB.Instance.getBindingCakeList(ConstantVariable.SORT_BY_DEC_PRICE);
                    break;
                case "selectTypeFilter":
                    {
                        ListTypeCake screen = new ListTypeCake();
                        screen.Owner = this;
                        screen.handler += Screen_handler;
                        screen.ShowDialog();
                        if(type_filter != null)
                        {
                            dataListView.ItemsSource = QueryDB.Instance.getBindingCakeList(ConstantVariable.FILTER_BY_TYPE, null, this.type_filter.ID);
                        }
                        else
                        {
                            // do nothing
                        }
                    }
                    break;
                default:
                    //Get current item.
                    //var senderStackPanel = (StackPanel)((Grid)((Border)((Canvas)((StackPanel)(selectedProductImg).Parent).Parent).Parent).Parent).Parent;
                    var senderParent = (WrapPanel)(options.Parent);
                    //Get TextBlock contain item's id.
                    var ID_Product = Int32.Parse(((TextBlock)VisualTreeHelper.GetChild(senderParent, 1)).Text as string);
                    Debug.WriteLine($">{ID_Product}");
                    dataListView.ItemsSource = QueryDB.Instance.getBindingCakeList(ConstantVariable.FILTER_BY_TYPE, null, ID_Product);
                    break;
            }
        }

        private void Screen_handler(object sender)
        {
            if(sender != null)
            {
                var type = sender as TypeCake;
                this.type_filter = new TypeCake();
                this.type_filter.ID = type.ID;
                this.type_filter.NameTypeCake = type.NameTypeCake;
            }
            else
            {
                this.type_filter = null;
            }
        }

        private void updateListviewSource()
        {
            dataListView.ItemsSource = QueryDB.Instance.getBindingCakeList();
            filterOptionList.ItemsSource = QueryDB.Instance.getTopTypeCake(ConstantVariable.TOP_TYPE_CAKE);
        }

        private void ButtonFilterTpe_MouseDown(object sender, MouseButtonEventArgs e)
        {


        }

        private void DeleteDetail_MouseEnter(object sender, MouseEventArgs e)
        {
            var deleteSymbol = sender as TextBlock;

            var converter = new System.Windows.Media.BrushConverter();
            var brush = (Brush)converter.ConvertFromString("#F73148");

            deleteSymbol.Foreground = brush;
        }

        private void DeleteDetail_MouseLeave(object sender, MouseEventArgs e)
        {
            var deleteSymbol = sender as TextBlock;

            var converter = new System.Windows.Media.BrushConverter();
            var brush = (Brush)converter.ConvertFromString("#403432");

            deleteSymbol.Foreground = brush;
        }

        private void DeleteDetail_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MessageBoxResult userChoose = MessageBox.Show("Bạn có chắc chắn xóa sản phẩm này khỏi giỏ hàng?", "Thông báo", MessageBoxButton.YesNo);
            if(userChoose == MessageBoxResult.Yes)
            {
                dynamic cartProduct = detailCartListViewRibbon.SelectedItem;
                Debug.WriteLine($"ID -- {cartProduct.Name}");
                BackstageTabItem_MouseLeftButtonDown(CartItem, null);
            }
            else
            {
                // do nothing
            }
        }

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void ImageProductInCart_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void PaymentCommand_Click(object sender, RoutedEventArgs e)
        {
            Bill screeen = new Bill();
            screeen.Owner = this;
            screeen.ShowDialog();

            BackstageTabItem_MouseLeftButtonDown(CartItem, null);
        }



        public event PropertyChangedEventHandler PropertyChanged;
    }
}
