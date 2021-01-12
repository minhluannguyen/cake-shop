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
using System.Windows.Shapes;
using System.Configuration;

namespace CakeShop
{
    /// <summary>
    /// Interaction logic for SplashScreen.xaml
    /// </summary>
    public partial class SplashScreen : Window
    {
        private System.Timers.Timer _timer;
        private double currentTime = 0;


        public int Thumbnail;
        private int rng;
        private string path;
        private const string tab = "    ";
        private string namefood;

        private readonly string[] listFoodImg = new string[]
        {
            "AnGiang","BacGiang","BacKan","BacLieu","BacNinh","BenTre","BinhDinh","BinhDuong","BinhPhuoc","BinhThuan",
            "CaMau","CanTho","CaoBang","DakLak","DakNong","DaNang","DienBien","DongNai","DongThap","GiaLai",
            "HaGiang","HaiDuong","HaiPhong","HaNam","HaNoi","HaTinh","HauGiang","HoaBinh","HungYen","KhanhHoa",
            "KienGiang","KonTum","LaiChau","LamDong","LangSon","LaoCai","LongAn","NamDinh","NgheAn","NinhBinh",
            "NinhThuan","PhuTho","PhuYen","QuangBinh","QuangNam","QuangNgai","QuangNinh","QuangTri","SaiGon","SocTrang",
            "SonLa","TayNinh","ThaiBinh","ThaiNguyen","ThanhHoa","ThuaThienHue","TienGiang","TraVinh","TuyenQuang","VinhLong",
            "VinhPhuc","VungTau","YenBai"
        };

        public SplashScreen()
        {
            this.loading();
            InitializeComponent();
            this.DataContext = this;
        }

        private void loading()
        {
            //var value = ConfigurationManager.AppSettings["ShowSplashScreen"];
            //var showSplash = bool.Parse(value);
            var showSplash = bool.Parse(AppConfiguration.Instance.GetAppSettingsValue("ShowSplashScreen"));
            Debug.WriteLine($"{showSplash}<--");
            if (showSplash == false)
            {
                var screen = new MainWindow();
                screen.Show();

                this.Close();
            }
            else
            {
                _timer = new System.Timers.Timer();
                _timer.Interval = ConstantVariable.DURING_SPLASH_SCREEN;
                _timer.Elapsed += _timer_Elapsed;
                _timer.Start();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.path = Directory.GetCurrentDirectory();

            //Canvas.SetLeft(NameShop, (placeImg.ActualWidth - ConstantVariable.convertDimension(NameShop.ActualWidth)) / 2);
            //Canvas.SetTop(NameShop, 0);

            setRandomImg();
        }

        private void _timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            currentTime += 0.01;

            if (Math.Truncate(currentTime) == ConstantVariable.DURING_SPLASH_SCREEN)
            {
                _timer.Stop();
                Dispatcher.Invoke(() =>
                {
                    //if (isReopenCheckBox.IsChecked.Value)
                    //{
                    //    var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                    //    config.AppSettings.Settings["ShowSplashScreen"].Value = "false";
                    //    config.Save(ConfigurationSaveMode.Minimal);
                    //}

                    AppConfiguration.Instance.UpdateAppSettings("ShowSplashScreen", $"{!isReopenCheckBox.IsChecked.Value}");

                    MainWindow screen = new MainWindow(!isReopenCheckBox.IsChecked.Value);
                    screen.Show();

                    this.Close();
                });
            }

            Dispatcher.Invoke(() =>
            {
                splashProgressBar.Value = currentTime;
            });
        }

        private void continueButton_Click(object sender, RoutedEventArgs e)
        {
            _timer.Stop();

            //if (isReopenCheckBox.IsChecked.Value)
            //{
            //    var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            //    config.AppSettings.Settings["ShowSplashScreen"].Value = "false";
            //    config.Save(ConfigurationSaveMode.Minimal);
            //}

            //var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            //config.AppSettings.Settings["ShowSplashScreen"].Value = $"{!isReopenCheckBox.IsChecked.Value}";
            //config.Save(ConfigurationSaveMode.Minimal);

            AppConfiguration.Instance.UpdateAppSettings("ShowSplashScreen", $"{!isReopenCheckBox.IsChecked.Value}");

            Debug.WriteLine($">>>>{!isReopenCheckBox.IsChecked.Value}");

            MainWindow screen = new MainWindow(!isReopenCheckBox.IsChecked.Value);
            screen.Show();

            this.Close();
        }
        protected void setRandomImg()
        {
            List<Product> listProduct = QueryDB.Instance.getOriginProductList();
            int lengImgArr = listProduct.Count();

            this.rng = GeneratorRng.Instance.Next(lengImgArr);
            Product product = listProduct[rng];

            string nameImage = QueryDB.Instance.getThumbnailOf1Product(product.ID);

            //this.namefood = this.listFoodImg[this.rng];
            //Debug.WriteLine($"{this.path}/Images/splash_places/{this.namefood}.jpg");
            BitmapImage bitImg = new BitmapImage(new Uri($"{this.path}\\Images\\Products\\{nameImage}", UriKind.Absolute));
            splashImg.Source = bitImg;

            Canvas.SetLeft(splashImg, (placeImg.ActualWidth - ConstantVariable.convertDimension(splashImg.Width)) / 2);
            Canvas.SetTop(splashImg, 0);

            //StreamReader sreader = new StreamReader($"{this.path}/info/splash_places/{this.namefood}.txt");

            //nameOfFood.Text = sreader.ReadLine();
            //infoOfFood.Text = tab + sreader.ReadLine();

            nameOfFood.Text = $"/{product.Name}/";
            infoOfFood.Text = product.Description;
            this.Thumbnail = product.ID;
        }
        private void showhiddenInfo(object sender, MouseEventArgs e)
        {
            if (keyHidden.Color == Colors.Transparent || infoFood.Visibility == Visibility.Hidden)
            {
                keyHidden.Color = Colors.Black;
                showingImg.Visibility = Visibility.Visible;
                //keyHidden.Color = Color.FromRgb(168, 105, 80);
                infoFood.Visibility = Visibility.Visible;
            }
            else
            {
                // do nothing
            }
        }
        private void hidehiddenInfo(object sender, MouseEventArgs e)
        {
            if (keyHidden.Color == Colors.Black || infoFood.Visibility == Visibility.Visible)
            //if (keyHidden.Color == Color.FromRgb(253, 187, 45) || infoFood.Visibility == Visibility.Visible)
            {
                keyHidden.Color = Colors.Transparent;
                showingImg.Visibility = Visibility.Collapsed;
                infoFood.Visibility = Visibility.Hidden;
            }
            else
            {
                // do nothing
            }
        }

        #region GeneratorRandomClass
        class GeneratorRng
        {
            private static GeneratorRng _instance = null;
            private Random _rng;

            public static GeneratorRng Instance
            {
                get
                {
                    if (_instance == null)
                    {
                        _instance = new GeneratorRng();
                    }
                    else
                    {
                        // do nothing
                    }
                    return _instance;
                }
            }

            public int Next(int val)
            {
                int result = 0;

                result = this._rng.Next(val);

                return result;
            }

            private GeneratorRng()
            {
                _rng = new Random();
            }
        }
        #endregion
    }
}
