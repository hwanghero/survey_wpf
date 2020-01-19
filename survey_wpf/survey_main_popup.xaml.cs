using System;
using System.Collections.Generic;
using System.Data;
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
    /// survey_main_popup.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class survey_main_popup : Window
    {
        survey_sql s_sql = new survey_sql();

        public survey_main_popup()
        {
            InitializeComponent();
            datagrid_load();
        }

        // 드래그
        private void MainWindow_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
            e.Handled = true;
        }

        // 취소버튼
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public void datagrid_load()
        {
            // 데이터그리드 데이터 생성
            DataTable dataTable = new DataTable();

            // 컬럼 생성
            dataTable.Columns.Add("고유번호", typeof(string));
            dataTable.Columns.Add("제목", typeof(string));
            dataTable.Columns.Add("보기", typeof(string));
            dataTable.Columns.Add("설명", typeof(string));

            // 목록 바로 띄워주기
            int a = s_sql.getSurveyMainCount();
            List<String[,]> b = s_sql.getMainSurveyList();
            String[,] getb = new String[a, 4];
            if (b != null)
            {
                for (int i = a - 1; i >= 0; i--)
                {
                    getb = b[i];
                    dataTable.Rows.Add(new string[] { getb[i, 0], getb[i, 1], getb[i, 2], getb[i, 3] });
                }
            }
            main_list.ItemsSource = dataTable.DefaultView;
        }

        private void Main_list_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (main_list.SelectedItem != null)
            {
                DataRowView row = (DataRowView)main_list.SelectedItems[0];
                MessageBox.Show(row["고유번호"].ToString());
            }
        }
    }
}
