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
            productListView.ItemsSource = queryProduct.ToList();

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

        string Convertor_UNICODE_ASCII(string unicodestring, bool includeSpace = false)
        {                                                                               /* hàm convert từ chuỗi unicode sang chuỗi ascii                    */

            unicodestring = unicodestring.Normalize(NormalizationForm.FormD);


            StringBuilder stringBuilder = new StringBuilder();                          /* tạo một string builder để xây dựng chuỗi                         */

            for (int i = 0; i < unicodestring.Length; i++)
            {                                                                           /* quét từng ký tự và chuyển ký tự đó thành ký tự ascii             */
                System.Globalization.UnicodeCategory unicodeCategory = System.Globalization.CharUnicodeInfo.GetUnicodeCategory(unicodestring[i]);
                if (unicodeCategory != System.Globalization.UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(unicodestring[i]);                             /* lưu ký tự đó vào trong string builder dựng sẵn                   */
                }
            }
            stringBuilder.Replace("Đ", "D");                                            /* quá trình chuyển đổi không thể chuyển đổi 2 ký tự đ,Đ            */
            stringBuilder.Replace("đ", "d");
            if (includeSpace)                                                           /* nếu người hàm kiểm tra có yêu cầu giữ lại ký tự space thì giữ    */
            {
                // do nothing
            }
            else
            {
                stringBuilder.Replace(" ", "");
            }
            /* trả về kết quả chuyển đổi và không thay đổi chuỗi ban đầu        */
            string result = ((stringBuilder.ToString()).Normalize(NormalizationForm.FormD)).ToLower();

            return result;
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
                    string convertedName = Convertor_UNICODE_ASCII(product.Name, true);

                    foreach (var key in keys)
                    {
                        if (convertedName.IndexOf(key) > -1)
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

            foreach(var pageNum in _paging.Pages)
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
                    string convertedName = Convertor_UNICODE_ASCII(product.Name, true);

                    foreach (var key in keys)
                    {
                        if (convertedName.IndexOf(key) > -1)
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
                        .OrderBy( x => x.Name)
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
            
            productListView.ItemsSource = products;
        }

        private void numberPageButton_Click(object sender, RoutedEventArgs e)
        {
            Button currentButton = (Button)sender;
            _paging.CurrentPage = int.Parse(currentButton.Content.ToString()) - 1;
            displayProducts();
        }

        private void firstPageNumButton_Click(object sender, RoutedEventArgs e)
        {
            _paging.CurrentPage = 0;
            displayProducts();
        }

        private void lastPageNumButton_Click(object sender, RoutedEventArgs e)
        {
            _paging.CurrentPage = _paging.TotalPages;
            displayProducts();
        }

        private void prevPageButton_Click(object sender, RoutedEventArgs e)
        {
            if (_paging.CurrentPage > 0)
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

        }

        private void CakeTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            displayProducts();
        }

        private void SortOptionComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            displayProducts();
        }
    }
}
