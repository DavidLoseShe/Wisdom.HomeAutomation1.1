using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Wisdom.HomeAutomation.WpfApp.Controller
{
    /// <summary>
    /// CustomListButton.xaml 的交互逻辑
    /// </summary>
    public partial class CustomListButton : UserControl
    {
        public CustomListButton()
        {
            InitializeComponent();
        }

        private void UIElement_OnMouseEnter(object sender, MouseEventArgs e)
        {
        }

        private void HomeApp_OnMouseEnter(object sender, MouseEventArgs e)
        {
            var grid = (Grid) sender;
            var homeApp = (CheckBox) grid.FindName("homeApp");
            var check1 = (CheckBox) grid.FindName("cmd1");
            var check2 = (CheckBox) grid.FindName("cmd2");
            if (homeApp != null && check1?.Tag != null && homeApp.IsChecked == true)
                check1.Visibility = Visibility.Visible;
            else
            {
                if (check1 != null) check1.Visibility = Visibility.Hidden;
            }
            if (homeApp != null && check2?.Tag != null && homeApp.IsChecked == true)
                check2.Visibility = Visibility.Visible;
            else
            {
                if (check2 != null) check2.Visibility = Visibility.Hidden;
            }
        }

        private void UIElement_OnMouseLeave(object sender, MouseEventArgs e)
        {
            var grid = (Grid) sender;
            var check1 = (CheckBox) grid.FindName("cmd1");
            var check2 = (CheckBox) grid.FindName("cmd2");
            if (check1 != null) check1.Visibility = Visibility.Hidden;
            if (check2 != null) check2.Visibility = Visibility.Hidden;
        }
    }
}