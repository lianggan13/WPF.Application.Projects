using System.Windows;

namespace TIM.Login
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void CloseBtn_Clicked(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
