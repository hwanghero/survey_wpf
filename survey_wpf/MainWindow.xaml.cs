
using System;
using System.Windows;
using System.Windows.Input;


namespace survey_wpf
{

    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            // 드래그 생성
            this.MouseLeftButtonDown += MainWindow_MouseLeftButtonDown;
        }

        // 드래그
        private void MainWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            user_sql a = new user_sql();
            string username = id.Text;
            string password = pw.Password;
            int b = a.adminlogin(username, pw.Password);

            if(b == 1)
            {
                index w = new index();
                w.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Login failed");
            }
        }

        // 종료
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
