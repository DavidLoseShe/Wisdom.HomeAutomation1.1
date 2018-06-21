using System;
using System.Collections.Generic;
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

namespace Wisdom.HomeAutomation.WpfApp.Controller
{
    /// <summary>
    /// DateTimePicker.xaml 的交互逻辑
    /// </summary>
    public partial class DateTimePicker : UserControl
    {
        #region 重要变量

        #endregion

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public DateTimePicker()
        {
            InitializeComponent();
            this.initParameters();
            this.textbox_minute.Background = Brushes.White;
            this.textbox_second.Background = Brushes.White;
            this.textbox_hour.Background = Brushes.White;
        }

        #endregion

        #region 业务处理函数
        /// <summary>
        /// 更改选中状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textbox_hour_SelectionChanged(object sender, RoutedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (tb != null)
            {
                switch (tb.Name)
                {
                    case "textbox_hour":
                        tb.Background = Brushes.Gray;
                        this.textbox_minute.Background = Brushes.White;
                        this.textbox_second.Background = Brushes.White;
                        break;
                    case "textbox_minute":
                        tb.Background = Brushes.Gray;
                        this.textbox_hour.Background = Brushes.White;
                        this.textbox_second.Background = Brushes.White;
                        break;
                    case "textbox_second":
                        tb.Background = Brushes.Gray;
                        this.textbox_hour.Background = Brushes.White;
                        this.textbox_minute.Background = Brushes.White;
                        break;
                }
            }
        }

        /// <summary>
        /// 向上加时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_up_Click(object sender, RoutedEventArgs e)
        {
            if (this.textbox_hour.Background == Brushes.Gray)
            {
                int temp = System.Int32.Parse(this.textbox_hour.Text);
                temp++;
                if (temp > 24)
                {
                    temp = 0;
                }
                this.textbox_hour.Text = temp.ToString();
            }
            else if (this.textbox_minute.Background == Brushes.Gray)
            {
                int temp = System.Int32.Parse(this.textbox_minute.Text);
                temp++;
                if (temp > 60)
                {
                    temp = 0;
                }
                this.textbox_minute.Text = temp.ToString();
            }
            else if (this.textbox_second.Background == Brushes.Gray)
            {
                int temp = System.Int32.Parse(this.textbox_second.Text);
                temp++;
                if (temp > 60)
                {
                    temp = 0;
                }
                this.textbox_second.Text = temp.ToString();
            }
        }

        /// <summary>
        /// 向下减时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_down_Click(object sender, RoutedEventArgs e)
        {
            if (this.textbox_hour.Background == Brushes.Gray)
            {
                int temp = System.Int32.Parse(this.textbox_hour.Text);
                temp--;
                if (temp < 0)
                {
                    temp = 24;
                }
                this.textbox_hour.Text = temp.ToString();
            }
            else if (this.textbox_minute.Background == Brushes.Gray)
            {
                int temp = System.Int32.Parse(this.textbox_minute.Text);
                temp--;
                if (temp < 0)
                {
                    temp = 60;
                }
                this.textbox_minute.Text = temp.ToString();
            }
            else if (this.textbox_second.Background == Brushes.Gray)
            {
                int temp = System.Int32.Parse(this.textbox_second.Text);
                temp--;
                if (temp < 0)
                {
                    temp = 60;
                }
                this.textbox_second.Text = temp.ToString();
            }
        }

        /// <summary>
        /// 初始化参数设置
        /// </summary>
        private void initParameters()
        {
            string strt = System.DateTime.Now.ToString("HH:mm:ss");
            this.textbox_hour.Text = strt.Split(':')[0];
            this.textbox_minute.Text = strt.Split(':')[1];
            this.textbox_second.Text = strt.Split(':')[2];
        }

        /// <summary>
        /// 数字标准化处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void numtextboxchanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (tb != null)
            {
                if ((this.isNum(tb.Text) == false) || (tb.Text.Length > 2))
                {
                    tb.Text = "0";
                    MessageBox.Show("请输入正确的时间！", "警告！");
                    return;
                }
            }
        }

        /// <summary>
        /// 判断是否为数字，是--->true，否--->false
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private bool isNum(string str)
        {
            bool ret = true;
            foreach (char c in str)
            {
                if ((c < 48) || (c > 57))
                {
                    return false;
                }
            }

            return ret;
        }

        #endregion

        private void DateTimePicker_OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var user = (UserControl) sender;
            var strt = System.DateTime.Now.ToString("HH:mm:ss");
            this.textbox_hour.Text = strt.Split(':')[0];
            this.textbox_minute.Text = strt.Split(':')[1];
            this.textbox_second.Text = strt.Split(':')[2];
        }
    }

}
