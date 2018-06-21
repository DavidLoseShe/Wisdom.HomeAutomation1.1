using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Animation;


namespace AgileToDo
{
    /// <summary>
    /// UMessageBox.xaml 的交互逻辑
    /// </summary>
    public partial class UMessageBox : Window
    {
        /// <summary>
        /// 禁止在外部实例化
        /// </summary>
        private UMessageBox()
        {
            InitializeComponent();
        }

//        public new string Title
//        {
//            get { return this.lblTitle.Text; }
//            set { this.lblTitle.Text = value; }
//        }

        public static bool Result { set; get; } = true;
        public  string Message
        {
            get { return this.Msg.Text; }
            set { this.Msg.Text = value; }
        }
        /// <summary>
        /// 静态方法 模拟MESSAGEBOX.Show方法
        /// </summary>
        /// <param name="msg">消息</param>
        /// <param name="style"></param>
        /// <returns></returns>
        public static bool? Show( string msg,bool style=true)
        {
            var msgBox = new UMessageBox();
          //  msgBox.Title = title;
            msgBox.Message = msg;
            if (style==false)
            {
                msgBox.BtnOk.Visibility=Visibility.Visible;
                msgBox.BtnOk1.Visibility = Visibility.Hidden;
                msgBox.BtnCancel.Visibility = Visibility.Hidden;
            }
            return msgBox.ShowDialog();
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