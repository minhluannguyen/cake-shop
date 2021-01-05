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
    class PageInfo
    {
        public int Page { get; set; }
        public int TotalPages { get; set; }
    }

    class Paging
    {
        private int _totalPages;

        public int CurrentPage { get; set; }
        public int ItemsPerPage { get; set; } = 9;
        public int TotalPages
        {
            get => _totalPages; set
            {
                _totalPages = value;
                Pages = new List<PageInfo>();
                for (int i = 1; i <= _totalPages; i++)
                {
                    Pages.Add(new PageInfo()
                    {
                        Page = i,
                        TotalPages = _totalPages
                    });
                }
            }
        }
        public List<PageInfo> Pages { get; set; }
    }
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Fluent.RibbonWindow
    {
        Paging _paging;
        MainScreenListViewVM _vm;
        public MainWindow()
        {
            InitializeComponent();
        }
        public MainWindow(bool a)
        {
            InitializeComponent();
            calculatePagingInfo();
            displayProducts();
        }

        private void RibbonWindow_Loaded(object sender, RoutedEventArgs e)
        {
            loadDatas();

        }

        private void loadDatas()
        {
            CakeStoreDBEntities db = new CakeStoreDBEntities();
            var queryProduct = db.Products
                .Select(
                    x => x
                );
            _vm = new MainScreenListViewVM(queryProduct.ToList());
            productListView.ItemsSource = _vm.listVM;

            var queryCakeType = db.TypeCakes
                .Select(
                    x => x
                );
            var listCakeType = queryCakeType.ToList();


            ComboBoxItem comboBoxItem = new ComboBoxItem();
            TypeCake tmpProduct = new TypeCake();
            tmpProduct.NameTypeCake = "Tất cả";
            listCakeType.Insert(0, tmpProduct);

            CakeTypeComboBox.ItemsSource = listCakeType;
            CakeTypeComboBox.SelectedIndex = 0;
        }

        private Button createNewPageButton(int numPage)
        {
            Button tmpBtn = new Button();
            tmpBtn.Content = $"{numPage}";

            //Margin
            Thickness margin = tmpBtn.Margin;
            margin.Left = 5;
            tmpBtn.Margin = margin;

            //Font size
            tmpBtn.FontSize = 15;

            //Button sizes
            tmpBtn.Height = 30;
            tmpBtn.Width = 30;

            //Button color
            tmpBtn.BorderBrush = Brushes.Transparent;
            tmpBtn.Foreground = Brushes.White;

            //Add event
            tmpBtn.Click += numberPageButton_Click;

            var bc = new BrushConverter();
            tmpBtn.Background = (Brush)bc.ConvertFrom("#FD5533");

            return tmpBtn;
        }

        void calculatePagingInfo()
        {
            CakeStoreDBEntities db = new CakeStoreDBEntities();
            TypeCake cakeType = CakeTypeComboBox.SelectedItem as TypeCake;
            int index = CakeTypeComboBox.SelectedIndex;
            var keyword = txtboxSearch.Text;
            string[] keys = keyword.ToLower().Split(new char[] { '.', '?', '!', ' ', ';', ':', ',' },
                                                    StringSplitOptions.RemoveEmptyEntries);
            //Find searched cakes name's id
            IQueryable<Product> sentenceQuery = db.Products;
            List<int> IDProductsList = new List<int>();
            if (!string.IsNullOrEmpty(keyword))
            {
                foreach (var product in db.Products.ToList())
                {
                    string convertedName = Ulties.Convertor_UNICODE_ASCII(product.Name, true);

                    foreach (var key in keys)
                    {
                        string tmpkey = Ulties.Convertor_UNICODE_ASCII(key, true);
                        if (convertedName.IndexOf(tmpkey) > -1)
                        {
                            IDProductsList.Add(product.ID);
                        }
                    }
                }
                sentenceQuery = db.Products
                    .Join(
                        IDProductsList,
                        p => p.ID,
                        i => i,
                        (p, i) => new
                        {
                            products = p
                        }
                    ).Select(
                        x => x.products
                    );
            }


            int count = 0;
            if (index <= 0)
            {
                var queryProducts = sentenceQuery;
                count = queryProducts.Count();
            }
            else
            {
                var query = sentenceQuery
                    .Select(
                        x => x.IDTypeCake == cakeType.ID
                    );
                count = query.Count();
            }

            int ItemsPerPage = 9;

            _paging = new Paging()
            {
                CurrentPage = 1,
                ItemsPerPage = 9,
                TotalPages = count / ItemsPerPage +
                    (((count % ItemsPerPage) == 0) ? 0 : 1)
            };

            foreach (var pageNum in _paging.Pages)
            {
                var tmpButton = createNewPageButton(pageNum.Page);
                if (_paging.CurrentPage == pageNum.Page)
                {
                    var bc = new BrushConverter();
                    tmpButton.Background = (Brush)bc.ConvertFrom("#356829");
                }
                pageNumberStackPanel.Children.Insert(pageNumberStackPanel.Children.Count - 2, tmpButton);
            }
        }

        void displayProducts()
        {
            if (_paging == null)
                return;
            CakeStoreDBEntities db = new CakeStoreDBEntities();
            var page = _paging.CurrentPage;
            var cakeType = CakeTypeComboBox.SelectedItem as TypeCake;
            int index = CakeTypeComboBox.SelectedIndex;

            var ItemsPerPage = 9;
            var skip = (page - 1) * ItemsPerPage;
            var take = ItemsPerPage;

            var keyword = txtboxSearch.Text;
            string[] keys = keyword.ToLower().Split(new char[] { '.', '?', '!', ' ', ';', ':', ',' },
                                                    StringSplitOptions.RemoveEmptyEntries);
            //Find searched cakes name's id
            IQueryable<Product> sentenceQuery = db.Products;
            List<int> IDProductsList = new List<int>();
            if (!string.IsNullOrEmpty(keyword))
            {
                foreach (var product in db.Products.ToList())
                {
                    string convertedName = Ulties.Convertor_UNICODE_ASCII(product.Name, true);

                    foreach (var key in keys)
                    {
                        string tmpkey = Ulties.Convertor_UNICODE_ASCII(key, true);
                        if (convertedName.IndexOf(tmpkey) > -1)
                        {
                            IDProductsList.Add(product.ID);
                        }
                    }
                }
                sentenceQuery = db.Products
                    .Join(
                        IDProductsList,
                        p => p.ID,
                        i => i,
                        (p, i) => new
                        {
                            products = p
                        }
                    ).Select(
                        x => x.products
                    );
            }


            IQueryable<Product> queryProducts;
            if (index <= 0)
            {
                queryProducts = sentenceQuery;
            }
            else
            {
                queryProducts = sentenceQuery
                    .Where(
                        x => x.IDTypeCake == cakeType.ID
                    );

            }

            int sortOption = SortOptionComboBox.SelectedIndex;
            List<Product> products = null;
            switch (sortOption)
            {
                case 0:
                    products = queryProducts
                        .OrderBy(x => x.Name)
                        .Skip(skip).Take(take)
                        .ToList();
                    break;
                case 1:
                    products = queryProducts
                        .OrderByDescending(x => x.Name)
                        .Skip(skip).Take(take)
                        .ToList();
                    break;
                case 2:
                    products = queryProducts
                        .OrderBy(x => x.Price)
                        .Skip(skip).Take(take)
                        .ToList();
                    break;
                case 3:
                    products = queryProducts
                        .OrderByDescending(x => x.Price)
                        .Skip(skip).Take(take)
                        .ToList();
                    break;
            }
            _vm = new MainScreenListViewVM(products);
            productListView.ItemsSource = _vm.listVM;
        }

        private void numberPageButton_Click(object sender, RoutedEventArgs e)
        {
            Button currentButton = (Button)sender;
            _paging.CurrentPage = int.Parse(currentButton.Content.ToString()) - 1;
            displayProducts();
        }

        private void firstPageNumButton_Click(object sender, RoutedEventArgs e)
        {
            _paging.CurrentPage = 1;
            displayProducts();
        }

        private void lastPageNumButton_Click(object sender, RoutedEventArgs e)
        {
            _paging.CurrentPage = _paging.TotalPages;
            displayProducts();
        }

        private void prevPageButton_Click(object sender, RoutedEventArgs e)
        {
            if (_paging.CurrentPage > 1)
            {
                _paging.CurrentPage--;
                displayProducts();
            }
            else
            {
                //do nothing
            }
        }

        private void nextPageButton_Click(object sender, RoutedEventArgs e)
        {
            if (_paging.CurrentPage < _paging.CurrentPage)
            {
                _paging.CurrentPage++;
                displayProducts();
            }
            else
            {
                //do nothing
            }
        }

        private void FluentButtonQuit_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void TypeCake_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void addNewTypeCake_Click(object sender, RoutedEventArgs e)
        {

        }

        private void txtboxSearch_KeyDown(object sender, KeyEventArgs e)
        {
            displayProducts();
        }

        private void CakeTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            displayProducts();
        }

        private void SortOptionComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            displayProducts();
        }

        private void ListViewItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ListViewItem selectedItem = sender as ListViewItem;
            var selectedItemVM= selectedItem.Content as MainScreenListViewItemVM;
            DetailCakeScreen detailCakeScreen = new DetailCakeScreen(selectedItemVM.ProductInfo);

            detailCakeScreen.ShowDialog();
        }

        public class MainScreenListViewItemVM
        {
            public Product ProductInfo { get; set; }
            public string Thumbnail { get; set; }
        }

        public class MainScreenListViewVM
        {
            public List<MainScreenListViewItemVM> listVM;

            public MainScreenListViewVM(List<Product> listProduct)
            {
                CakeStoreDBEntities db = new CakeStoreDBEntities();

                listVM = new List<MainScreenListViewItemVM>();
                foreach(var product in listProduct)
                {
                    MainScreenListViewItemVM mainScreenListViewItemVM = new MainScreenListViewItemVM();
                    mainScreenListViewItemVM.ProductInfo = product;
                    var queryImage = db.ProductImages
                        .Where(
                            x => x.ID_Product == product.ID
                        )
                        .Select(
                            x => x.ImageName
                        );
                    int count = queryImage.ToList().Count();
                    if (count == 0)
                    { 
                        mainScreenListViewItemVM.Thumbnail = "/Images/no_image_available.jpeg"; 
                    }
                    else
                    {
                        mainScreenListViewItemVM.Thumbnail = $"{Directory.GetCurrentDirectory()}\\Images\\Products\\{Ulties.Convertor_UNICODE_ASCII(product.Name, false)}\\{queryImage.ToList()[0]}";
                    }
                    listVM.Add(mainScreenListViewItemVM);
                }
            }
        }
    }
    
}
