using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using AgileToDo;
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
                var result = UMessageBox.Show( "请确认启动方案");
                if (result == false|| ComboBox.SelectedItem==null)
                {
                    checkBox.IsChecked = false;
                    return;
                }
                this.RemainTimeRow.Visibility = Visibility.Visible;
                return;
            }

            var result1 = UMessageBox.Show("请确认停止方案");
            if (result1 == false)
            {
                checkBox.IsChecked = true;
                return;
            }
            RemainTimeRow.Visibility = Visibility.Collapsed;
        }
    }
}
