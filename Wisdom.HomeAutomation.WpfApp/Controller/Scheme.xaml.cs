using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
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
using Wisdom.HomeAutomation.WpfApp.ViewModel;

namespace Wisdom.HomeAutomation.WpfApp.Controller
{
    /// <summary>
    /// Scheme.xaml 的交互逻辑
    /// </summary>
    public partial class Scheme : UserControl
    {
        public Scheme()
        {
            InitializeComponent();
        }

        private void FrameworkElement_OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var se = (Border) sender;
            if (se.IsEnabled == false)
            {
                var style = Resources["StopBackground"] as LinearGradientBrush;
                se.Background = style;

                var image = (Image) se.FindName("Image");
                var imgSource = new BitmapImage(new Uri("../Img/stop.png", UriKind.Relative));
                if (image != null) image.Source = imgSource;
            }
            else
            {
                var style = Resources["RunBackground"] as LinearGradientBrush;
                se.Background = style;
                var image = (Image) se.FindName("Image");
                var imgSource = new BitmapImage(new Uri("../Img/running.png", UriKind.Relative));
                if (image != null) image.Source = imgSource;
            }

        }


  

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var checkBox = (CheckBox)sender;

            

            if ( checkBox.IsChecked == true)
            {
                var result = UMessageBox.Show( "确认启动当前方案?");
                if (result == false|| ComboBox.SelectedItem==null)
                {
                    checkBox.IsChecked = false;
                    return;
                }
                Control.Visibility = Visibility.Collapsed;
                this.RemainTimeRow.Visibility = Visibility.Visible;
                return;
            }

            var result1 = UMessageBox.Show("是否立即停止所有方案？");
            if (result1 == false)
            {
                checkBox.IsChecked = true;
                return;
            }
            Control.Visibility = Visibility.Visible;
            RemainTimeRow.Visibility = Visibility.Collapsed;
        }
    }

    public class TimeConverter : IValueConverter
    {
        /* 数据从Source到Targe时，Convert被调用 */
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value==null)
            {
                return null;
            }
            var secends = (int)value;
            var timeSpan =new TimeSpan(0,0,secends);
            return $"{timeSpan.Hours} 时{timeSpan.Minutes}分{timeSpan.Seconds}秒";
        }
        //数据从Targe到Source时，ConvertBack被调用
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
