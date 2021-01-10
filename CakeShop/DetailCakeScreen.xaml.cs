using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
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
    /// Interaction logic for DetailCakeScreen.xaml
    /// </summary>
    public partial class DetailCakeScreen : Window
    {
        DetailCakeScreenVM _vm;
        Product currentProduct;
        private DetailCakeScreen()
        {
            InitializeComponent();
        }

        public DetailCakeScreen(Product product)
        {
            InitializeComponent();

            _vm = new DetailCakeScreenVM(product);
            currentProduct = product;
            this.DataContext = _vm;
            addToCartButton.IsEnabled = false;
        }

        public class DetailCakeScreenVM : INotifyPropertyChanged
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public int Amount { get; set; }
            public string CakeTypeName { get; set; }
            public int Price { get; set; }
            public List<ProductImage> ImageList { get; set; }
            public string _thumbnail;
            public string Thumbnail
            {
                get { return _thumbnail; }
                set
                {
                    _thumbnail = value;
                    PropertyChanged?.Invoke(this,
                        new PropertyChangedEventArgs("Thumbnail"));
                }
            }


            public int _quantityInCart;
            public int QuantityInCart
            {
                get { return _quantityInCart; }
                set
                {
                    _quantityInCart = value;
                    PropertyChanged?.Invoke(this,
                        new PropertyChangedEventArgs("QuantityInCart"));
                }
            }

            public DetailCakeScreenVM(Product product)
            {
                this.Name = product.Name;
                this.Description = product.Description;
                this.Amount = product.Amount.GetValueOrDefault();
                this.Thumbnail = "/Images/no_image_available.jpeg";
                this.QuantityInCart = QueryDB.Instance.countQuantityOfProductInCart(product.ID);

                CakeStoreDBEntities db = new CakeStoreDBEntities();

                //Get type cake name
                var queryCakeType = db.TypeCakes
                    .Where(
                        x => x.ID == product.IDTypeCake
                    )
                    .Select(
                        x => x.NameTypeCake
                    );
                this.CakeTypeName = queryCakeType.FirstOrDefault();

                //Get price string
                this.Price = product.Price.Value;

                //Get image names
                var queryImageNames = db.ProductImages
                    .Where(
                        x => x.ID_Product == product.ID
                    );
                // Generate images path
                var ImageNameList = queryImageNames.ToList();
                string path = Directory.GetCurrentDirectory();
                for (int i = 0; i < ImageNameList.Count(); ++i)
                {
                    //string realImagePath = path + "\\Images\\Products\\" + Ulties.Convertor_UNICODE_ASCII(product.Name, false) + "\\" + ImageNameList[i].ImageName;
                    string realImagePath = path + $"\\Images\\Products\\{ImageNameList[i].ImageName}";
                    ImageNameList[i].ImageName = realImagePath;
                }

                if (ImageNameList.Count() > 0)
                {
                    this.Thumbnail = ImageNameList[0].ImageName;
                }

                this.ImageList = new List<ProductImage>(ImageNameList);
            }

            public event PropertyChangedEventHandler PropertyChanged;

        }

        private void ListViewItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ListViewItem selectedItem = sender as ListViewItem;
            var selectedProductImage = selectedItem.Content as ProductImage;

            _vm.Thumbnail = selectedProductImage.ImageName;
        }

        private void AddAmountButton_Click(object sender, RoutedEventArgs e)
        {
            string amountStr = AmountTextBox.Text;
            int amount = 0;

            if (!int.TryParse(amountStr, out amount))
            {
                MessageBox.Show("Số lượng phải là số!", "Lưu ý", MessageBoxButton.OK);
                return;
            }

            if (amount == currentProduct.Amount.Value)
            {
                return;
            }

            amount++;
            AmountTextBox.Text = amount.ToString();
        }

        private void SubtractAmountButton_Click(object sender, RoutedEventArgs e)
        {
            string amountStr = AmountTextBox.Text;
            int amount = 0;
            if (!int.TryParse(amountStr, out amount))
            {
                MessageBox.Show("Số lượng phải là số!", "Lưu ý", MessageBoxButton.OK);
                return;
            }


            if (amount == -_vm.QuantityInCart)
            {
                return;
            }

            amount--;
            AmountTextBox.Text = amount.ToString();
        }

        private void AmountTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (currentProduct == null)
                return;
            

            string amountStr = AmountTextBox.Text;
            int amount = 0;

            if (amountStr.Length == 0) return;
            
            if (amountStr[0] == '-'){
                if(amountStr.Length == 1)
                {
                    return;
                }
                else
                {
                    if (!int.TryParse(amountStr, out amount))
                    {
                        MessageBox.Show("Số lượng phải là số!", "Lưu ý", MessageBoxButton.OK);
                        AmountTextBox.Text = "0";
                        return;
                    }
                    //if(amount != 0)
                    //{
                    //    amount = -amount;
                    //}
                }
            }
            else
            {
                if (!int.TryParse(amountStr, out amount))
                {
                    MessageBox.Show("Số lượng phải là số!", "Lưu ý", MessageBoxButton.OK);
                    AmountTextBox.Text = "0";
                    return;
                }
            }
            Debug.WriteLine($"-->> {amountStr[0] == '-'} - {amountStr} - {amount}");
            if (amountStr[0] == '0' && amountStr.Length > 1)
            {
                AmountTextBox.Text = amountStr.Substring(1, amountStr.Length-1);
            }
            

            //if (amountStr[0] == '-')
            //{
            //    amount = -amount;
            //}

            if (amount < -_vm.QuantityInCart)
            {
                AmountTextBox.Text = $"{-_vm.QuantityInCart}";
                return;
            }

            if (amount > currentProduct.Amount.Value)
            {
                AmountTextBox.Text = currentProduct.Amount.Value.ToString();
                return;
            }

            if(amount != 0)
            {
                addToCartButton.IsEnabled = true;
            }
            else
            {
                addToCartButton.IsEnabled = false;
            }

            AmountTextBox.Text = amount.ToString();
        }
        private void AddToCartButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int value = Int32.Parse(AmountTextBox.Text);
                QueryDB.Instance.addToCart(currentProduct, value);
                if (value > 0)
                {
                    MessageBox.Show("Thêm vào giỏ hàng thành công!");
                }
                else
                {
                    MessageBox.Show("Loại sản phẩm khỏi giỏ hàng thành công!");
                }
            }
            catch(Exception )
            {
                MessageBox.Show("Thêm vào giỏ hàng thất bại!");
            }

        }

        private void AmountTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if(AmountTextBox.Text.Length == 0)
            {
                AmountTextBox.Text = "0";
            }
            else
            {
                // do nothing
            }
        }
    }
}
