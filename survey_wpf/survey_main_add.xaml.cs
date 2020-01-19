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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace survey_wpf
{
    /// <summary>
    /// survey_main_add.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class survey_main_add : Page
    {
        String mode = "";
        survey_sql s_sql = new survey_sql();
        public survey_main_add()
        {
            InitializeComponent();
            datagrid_load();
        }

        public void datagrid_load()
        {
            // 데이터그리드 데이터 생성
            DataTable dataTable = new DataTable();

            // 컬럼 생성
            dataTable.Columns.Add("번호", typeof(string));
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
            dataGrid1.ItemsSource = dataTable.DefaultView;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            String[] getbutton = sender.ToString().Split(' ');
            if (sender.ToString().Contains("입력") || sender.ToString().Contains("수정") || sender.ToString().Contains("삭제"))
            {
                subbutton_click(getbutton[1]);
            }

            if (sender.ToString().Contains("확인") || sender.ToString().Contains("취소"))
            {
                mainbutton_click(getbutton[1]);
            }
        }

        private void subbutton_click(String sender)
        {
            if (sender.Equals("입력"))
            {
                number.IsEnabled = false;
                title.IsEnabled = true;
                view.IsEnabled = true;
                info.IsEnabled = true;
                mode = "입력";
            }

            if (sender.Equals("수정"))
            {
                number.IsEnabled = false;
                title.IsEnabled = false;
                view.IsEnabled = false;
                info.IsEnabled = false;
                mode = "수정";
                Message("밑에 수정을 원하는 설문조사를 선택해주세요");
            }

            if (sender.Equals("삭제"))
            {
                number.IsEnabled = true;
                title.IsEnabled = false;
                view.IsEnabled = false;
                info.IsEnabled = false;
                mode = "삭제";
            }

            insert.IsEnabled = false;
            update.IsEnabled = false;
            delete.IsEnabled = false;
            success.IsEnabled = true;
            cancel.IsEnabled = true;
        }

        private void mainbutton_click(String sender)
        {
            if (sender.Equals("확인"))
            {
                if (mode.Equals("입력"))
                {
                    s_sql.main_survey_insert(title.Text, view.Text, info.Text);
                }
                if (mode.Equals("수정"))
                {
                    s_sql.main_survey_update(number.Text, title.Text, view.Text, info.Text);
                }
                if (mode.Equals("삭제"))
                {
                    s_sql.main_survey_delete(number.Text);
                    MessageBox.Show("삭제");
                }
            }

            if (sender.Equals("취소"))
            {
                Message("취소하였습니다.");
            }

            datagrid_load();

            number.IsEnabled = false;
            title.IsEnabled = false;
            view.IsEnabled = false;
            info.IsEnabled = false;

            number.Text = "";
            title.Text = "";
            info.Text = "";
            view.Text = "";

            insert.IsEnabled = true;
            update.IsEnabled = true;
            delete.IsEnabled = true;
            success.IsEnabled = false;
            cancel.IsEnabled = false;
        }

        private void DataGrid1_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dataGrid1.SelectedItem != null)
            {
                DataRowView row = (DataRowView)dataGrid1.SelectedItems[0];

                if (mode.Equals("수정"))
                {
                    number.Text = row["번호"].ToString();
                    title.Text = row["제목"].ToString();
                    view.Text = row["보기"].ToString();
                    info.Text = row["설명"].ToString();
                    number.IsEnabled = true;
                    title.IsEnabled = true;
                    view.IsEnabled = true;
                    info.IsEnabled = true;
                }

                if (mode.Equals("삭제"))
                {
                    number.Text = row["번호"].ToString();
                    title.Text = row["제목"].ToString();
                    view.Text = row["보기"].ToString();
                    info.Text = row["설명"].ToString();
                }
            }
        }

        public void Message(String input)
        {
            MessageBox.Show(input, "메인 설문조사");
        }
    }
}
