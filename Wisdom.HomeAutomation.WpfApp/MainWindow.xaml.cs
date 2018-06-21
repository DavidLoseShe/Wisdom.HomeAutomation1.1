using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Wisdom.HomeAutomation.WpfApp.ViewModel;
using Wisdom.QuartzJob.Cong.Utility;

namespace Wisdom.HomeAutomation.WpfApp
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

           // DataGrid.LoadingRow += DataGridSoftware_LoadingRow;

            #region 默认TabControl

            RadioButton1.IsChecked=true;
            TabControl.SelectedIndex = 0;

            #endregion
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var radioButton = (RadioButton) sender;
            TabControl.SelectedIndex =  Convert.ToInt32(radioButton.Tag.ToString());

        }

        private void MainWindow_OnClosed(object sender, CancelEventArgs e)
        {
            if (!QuartzHelper.GetInstanceScheduler().IsStarted)
            {
                QuartzHelper.GetInstanceScheduler().Shutdown();
                return;
            }
            var result = MessageBox.Show("若后台方案正在执行，方案将被取消，是否退出?", "", MessageBoxButton.YesNo, MessageBoxImage.Information);
            if (result == MessageBoxResult.Yes)
            {
                QuartzHelper.GetInstanceScheduler().Shutdown();
                return;
            }
            e.Cancel = true;
            
        }
        #region 标题栏事件

        /// <summary>
        /// 窗口移动事件
        /// </summary>
        private void TitleBar_MouseMove(object sender, MouseEventArgs e)
        {
            
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        int i = 0;
        /// <summary>
        /// 标题栏双击事件
        /// </summary>
        private void TitleBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            i += 1;
            System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 0, 300);
            timer.Tick += (s, e1) => { timer.IsEnabled = false; i = 0; };
            timer.IsEnabled = true;

            if (i % 2 == 0)
            {
                timer.IsEnabled = false;
                i = 0;
                this.WindowState = this.WindowState == WindowState.Maximized ?
                    WindowState.Normal : WindowState.Maximized;
            }
        }

        /// <summary>
        /// 窗口最小化
        /// </summary>
        private void btn_min_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized; //设置窗口最小化
        }

        /// <summary>
        /// 窗口最大化与还原
        /// </summary>
        private void btn_max_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
            {
                this.WindowState = WindowState.Normal; //设置窗口还原
            }
            else
            {
                this.WindowState = WindowState.Maximized; //设置窗口最大化
            }
        }

        /// <summary>
        /// 窗口关闭
        /// </summary>
        private void btn_close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        #endregion 标题栏事件
    }
}
