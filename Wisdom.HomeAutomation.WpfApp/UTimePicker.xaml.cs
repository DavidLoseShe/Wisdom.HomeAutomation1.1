using System;
using System.Windows;
using System.Windows.Input;

namespace Wisdom.HomeAutomation.WpfApp
{
    /// <summary>
    /// UMessageBox.xaml 的交互逻辑
    /// </summary>
    public partial class UTimePiker : Window
    {
        /// <summary>
        /// 禁止在外部实例化
        /// </summary>
        private UTimePiker()
        {
            InitializeComponent();

        }

        public static bool Result { set; get; } = true;
        /// <summary>
        /// 设置时间
        /// </summary>
        /// <param name="msg">秒</param>
        /// <param name="style">样式</param>
        /// <returns></returns>
        public static int ChooseEquals(int msg, bool style = true)
        {
            var msgBox = new UTimePiker();
            msgBox.ComboBox1.ItemsSource = new[] {0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23};
            msgBox.ComboBox2.ItemsSource = new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23,24,25,26,27,28,29,30,31,32,33,34,35,36,37,38,39,40,41,42,43,44,45,46,47,48,49,50,51,52,53,54,55,56,57,58,59 };
            msgBox.ComboBox3.ItemsSource = new[] { 0,10,20,30,40,50};
            if (style == false)
            {
                msgBox.BtnOk.Visibility = Visibility.Visible;
                msgBox.BtnOk1.Visibility = Visibility.Hidden;
                msgBox.BtnCancel.Visibility = Visibility.Hidden;
            }
            var timeSpan = new TimeSpan(0,0,msg);
            msgBox.ComboBox1.SelectedItem = timeSpan.Hours;
            msgBox.ComboBox2.SelectedItem = timeSpan.Minutes;
            msgBox.ComboBox3.SelectedItem = timeSpan.Seconds;
            if (msgBox.ShowDialog()==true)
            {
                timeSpan =new TimeSpan((int)msgBox.ComboBox1.SelectedItem,(int)msgBox.ComboBox2.SelectedItem,(int)msgBox.ComboBox3.SelectedItem);
                return (int)timeSpan.TotalSeconds;
            }
            else
            {
                return msg;
            }
        }


        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            Result = true;
            this.Close();
        }

        private void ButtonBase_OnClick1(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            Result = false;
            this.Close();
        }
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
    }
}