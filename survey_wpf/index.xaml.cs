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
using System.Windows.Shapes;

namespace survey_wpf
{
    /// <summary>
    /// Window1.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class index : Window
    {

        public index()
        {
            InitializeComponent();

            /*
            // 목록 바로 띄워주기
            survey_sql s_sql = new survey_sql();
            int a = s_sql.getSurveyListCount();
            List<String> b = s_sql.getSurveyList();
            if(b != null){
                for (int i = 0; i < a; i++){
                    surveylist.Items.Add(b[i]);
                }
            }

            surveytitle.Text = "설문조사 목록 (" + a + "개)";
            */
        }

        private void ButtonFechar_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void GridTitle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
        

        // 목록
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            changeFrame.Source = new Uri("survey_list.xaml", UriKind.Relative);
        }
        // 생성
        private void createbutton_Click(object sender, RoutedEventArgs e)
        {
            changeFrame.Source = new Uri("survey_add.xaml", UriKind.Relative);
        }

        private void create_mutli_Click(object sender, RoutedEventArgs e)
        {
            changeFrame.Source = new Uri("survey_item.xaml", UriKind.Relative);
        }

        private void Maincreatebutton_Click(object sender, RoutedEventArgs e)
        {
            changeFrame.Source = new Uri("survey_main_add.xaml", UriKind.Relative);
        }

        private void Survey_result_Click(object sender, RoutedEventArgs e)
        {
            changeFrame.Source = new Uri("survey_result_select.xaml", UriKind.Relative);
        }
    }
}
