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
    /// Interaction logic for DialogCake.xaml
    /// </summary>
    public partial class DialogCake : Window, INotifyPropertyChanged
    {
        public delegate void WindowHandler(object sender, int action);
        public event WindowHandler handler;

        public BindingList<ProductImage> listCurrentImage = null;
        public BindingList<ProductImage> listRemovedImage = null;

        public int Action { set; get; }
        public string ActionName { set; get; }
        public string TitleAction { set; get; }

        public Product Cake { set; get; }
        public event PropertyChangedEventHandler PropertyChanged;
        private DialogCake()
        {
            InitializeComponent();
        }
        public DialogCake(Product product, int action)
        {
            InitializeComponent();
            this.DataContext = this;

            this.Action = action;
            this.Cake = new Product();

            this.listCurrentImage = new BindingList<ProductImage>();
            this.listRemovedImage = new BindingList<ProductImage>();

            if (product != null)
            {
                // clone
                this.Cake.ID = product.ID;
                this.Cake.Name = product.Name;
                this.Cake.Description = product.Description;
                this.Cake.IDTypeCake = product.IDTypeCake;
                this.Cake.Price = product.Price;

                var listQuery = QueryDB.Instance.getListImageOfProduct(this.Cake.ID);

                foreach(var image in listQuery)
                {
                    this.listCurrentImage.Add(image);

                    string ImagePath = $"{Directory.GetCurrentDirectory()}\\Images";
                    FileInfo fileInfo = new FileInfo($"{ImagePath}\\ImagesTemp\\{image.ImageName}");
                    if (!fileInfo.Exists)
                    {
                        File.Copy($"{ImagePath}\\Products\\{image.ImageName}", $"{ImagePath}\\ImagesTemp\\{image.ImageName}");
                    }
                }

                this.listImageOfProduct.ItemsSource = this.listCurrentImage;
            }
            else
            {
                // create new
                this.Cake.ID = QueryDB.Instance.getLastIDCake() + 1;
                this.Cake.Name = string.Empty;
                this.Cake.Description = string.Empty;
                this.Cake.IDTypeCake = null;
                this.Cake.Price = 0;
            }
            if (this.Action == ConstantVariable.ADD_CAKE)
            {
                this.TitleAction = "Thêm Bánh";
                this.ActionName = "Thêm";
                this.deleteBtn.Visibility = Visibility.Collapsed;
            }
            else if (this.Action == ConstantVariable.UPDATE_CAKE)
            {
                this.TitleAction = "Cập Nhật Bánh";
                this.ActionName = "Cập nhật";
            }
            else
            {
                // do nothing
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            handler?.Invoke(this.Cake, this.Action);
        }

        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Action = ConstantVariable.CANCEL_ACTION;
            this.Close();
        }


        private void deleteBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Action = ConstantVariable.DEL_CAKE;
            QueryDB.Instance.deleteCake(this.Cake);
            this.Close();
        }

        private void actionBtn_Click(object sender, RoutedEventArgs e)
        {
            //Debug.WriteLine($"--{this.Cake.NameTypeCake}");
            string CurrentPath = $"{Directory.GetCurrentDirectory()}";

            if (!QueryDB.Instance.hasSameCake(this.Cake))
            {
                if (this.Action == ConstantVariable.ADD_CAKE)
                {
                    this.Cake.Amount = 0;

                    foreach(var nameImage in this.listCurrentImage)
                    {
                        File.Copy($"{CurrentPath}\\Images\\ImagesTemp\\{nameImage.ImageName}", $"{CurrentPath}\\Images\\Products\\{nameImage.ImageName}", true);
                    }

                    QueryDB.Instance.addCake(this.Cake, this.listCurrentImage);
                }
                else if (this.Action == ConstantVariable.UPDATE_CAKE)
                {
                    foreach (var nameImage in this.listCurrentImage)
                    {
                        try
                        {
                            File.Copy($"{CurrentPath}\\Images\\ImagesTemp\\{nameImage.ImageName}", $"{CurrentPath}\\Images\\Products\\{nameImage.ImageName}", true);
                        }
                        catch
                        {

                        }
                    }
                    QueryDB.Instance.updateCake(this.Cake, this.listCurrentImage, this.listRemovedImage);
                }
                this.Close();
            }
            else
            {
                MessageBox.Show("Đã tồn tại tên loại bánh này!");
            }
        }

        private void TextBoxInput_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void addNewImg_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();                       /* mở một dialog để chọn ảnh                                                                */
            openFileDialog.Filter = "Images (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg";
            openFileDialog.Multiselect = true;
            if (openFileDialog.ShowDialog() == true)
            {
                string CurrentPath = $"{Directory.GetCurrentDirectory()}";

                // Create directory if not exist
                Directory.CreateDirectory($"{CurrentPath}\\Images\\Products\\");
                Directory.CreateDirectory($"{CurrentPath}\\Images\\ImagesTemp\\");


                string newImageName = "";       // keep name generated by Guid
                FileInfo info = null;
                string ImagePath = $"{CurrentPath}\\Images";

                foreach(var item in openFileDialog.FileNames)
                {
                    Debug.WriteLine($"--> {item}");

                    info = new FileInfo(item);
                    newImageName = $"{Guid.NewGuid()}{info.Extension}";
                    File.Copy(item, $"{ImagePath}\\ImagesTemp\\{newImageName}");
                    this.listCurrentImage.Add(new ProductImage() { ID_Product = this.Cake.ID, ImageName = newImageName, ID = -1});  // -1 means a new image added

                }

                this.listImageOfProduct.ItemsSource = this.listCurrentImage.ToList();
            }
            else
            {
                // do nothing
            }
        }

        private void DeleteImageOfProduct_Click(object sender, RoutedEventArgs e)
        {
            var removeBtn = (Button)sender;
            //Get current item.
            var senderStackPanel = (StackPanel)((Grid)(removeBtn).Parent).Parent;
            //Get TextBlock contain item's id.
            var NameImage = ((Label)VisualTreeHelper.GetChild(senderStackPanel, 1)).Content as string;
            //MessageBox.Show($"{NameImage}");
            Debug.WriteLine("==============================================");
            ProductImage choosenItem = null;
            foreach (var item in this.listCurrentImage)
            {
                Debug.WriteLine($"{item.ImageName} - {NameImage}");
                if ((item.ImageName).Equals(NameImage))
                {
                    choosenItem = item;
                    break;
                }
                else
                {
                    // do nothing
                }
            }
            if(choosenItem != null)
            {
                this.listRemovedImage.Add(choosenItem);
                this.listCurrentImage.Remove(choosenItem);
                this.listImageOfProduct.ItemsSource = this.listCurrentImage;
            }
            else
            {
                //MessageBox.Show("Khong tim thay");
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {

        }

        private void NameTypeCake_Click(object sender, RoutedEventArgs e)
        {
            ListTypeCake screen = new ListTypeCake();
            screen.Owner = this;
            screen.handler += Screen_handler;
            screen.ShowDialog();
        }

        private void Screen_handler(object sender)
        {
            this.Cake.IDTypeCake = (sender as TypeCake).ID;
            NameTypeCake.Content = QueryDB.Instance.getNameTypeCakeByID(this.Cake.IDTypeCake ?? -1);
        }
    }
}
