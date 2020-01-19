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
    /// survey_result_window.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class survey_result_window : Window
    {
        String main_number;
        survey_sql s_sql = new survey_sql();

        public survey_result_window()
        {
            InitializeComponent();
            if((App.Current as App).MainNumber != null)
            {
                main_number = (App.Current as App).MainNumber.ToString();
            }
            datagrid_load("전체", null, main_number);
            xLineChart.Visibility = Visibility.Hidden;
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void ButtonFechar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public void datagrid_load(String mode, String input, String top_idx)
        {
            // 데이터그리드 데이터 생성
            DataTable dataTable = new DataTable();

            // 컬럼 생성
            dataTable.Columns.Add("제목", typeof(string));

            // 목록 바로 띄워주기
            if (main_number.Equals(""))
            {
                dataTable.Rows.Add(new string[] { "메인 설문조사를 선택해주세요", "", "", "", "" });
            }
            else
            {
                int a = s_sql.getSurveyCount_grid(main_number);
                List<String[,]> b = s_sql.getSurveyList_grid(main_number);
                Console.WriteLine(a);
                String[,] getb = new String[a, 5];
                if (b != null)
                {
                    for (int i = a - 1; i >= 0; i--)
                    {
                        getb = b[i];
                        if (mode.Equals("전체"))
                        {
                            dataTable.Rows.Add(new string[] { getb[i, 1]});
                        }
                        if (mode.Equals("번호"))
                        {
                            if (getb[i, 0].Equals(input))
                            {
                                dataTable.Rows.Add(new string[] { getb[i, 3], getb[i, 0], getb[i, 1], getb[i, 2], getb[i, 4] });
                            }
                        }
                        if (mode.Equals("제목"))
                        {
                            if (getb[i, 1].Equals(input))
                            {
                                dataTable.Rows.Add(new string[] { getb[i, 3], getb[i, 0], getb[i, 1], getb[i, 2], getb[i, 4] });
                            }
                        }
                        if (mode.Equals("설명"))
                        {
                            if (getb[i, 2].Equals(input))
                            {
                                dataTable.Rows.Add(new string[] { getb[i, 3], getb[i, 0], getb[i, 1], getb[i, 2], getb[i, 4] });
                            }
                        }
                        if (mode.Equals("VIEW"))
                        {
                            if (getb[i, 3].Equals(input))
                            {
                                dataTable.Rows.Add(new string[] { getb[i, 3], getb[i, 0], getb[i, 1], getb[i, 2], getb[i, 4] });
                            }
                        }
                    }
                }
            }
            sublist.ItemsSource = dataTable.DefaultView;
        }

        private void Sublist_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataRowView row = (DataRowView)sublist.SelectedItems[0];
            String subtitle = row["제목"].ToString();
            String[] sub_survey = s_sql.surveyGet(subtitle);

            if (!sub_survey[4].Equals("Text"))
            {
                userlist.Visibility = Visibility.Hidden;

                int mulitcount = s_sql.getSurveyItemCount(int.Parse(sub_survey[0]));
                s_sql.survey_item_get(int.Parse(sub_survey[0]));

                List<String[,]> survey_item = s_sql.survey_item_get(int.Parse(sub_survey[0]));
                String[,] get_item = new String[mulitcount, 7];

                xLineChart.Visibility = Visibility.Visible;
                xLineChart.Title = subtitle + " 통계";
                List<KeyValuePair<string, int>> valueList = new List<KeyValuePair<string, int>>();

                for (int i=0; i<mulitcount; i++)
                {
                    get_item = survey_item[i];
                    int count = s_sql.getResult(int.Parse(sub_survey[0]), get_item[i,3]);
                    valueList.Add(new KeyValuePair<string, int>(get_item[i,3], count));
                }

                xLineChart.DataContext = valueList;
            }
            else
            {
                // 데이터그리드 데이터 생성
                DataTable dataTable = new DataTable();

                // 컬럼 생성
                dataTable.Columns.Add("아이디", typeof(string));
                dataTable.Columns.Add("답변", typeof(string));

                int count = s_sql.getShortResult(int.Parse(sub_survey[0]));
                List<String[,]> list = s_sql.getShortList(int.Parse(sub_survey[0]));
                String[,] getlist = new string[count, 6];
                for (int i = 0; i < count; i++)
                {
                    getlist = list[i];
                    dataTable.Rows.Add(new string[] { getlist[i,1], getlist[i, 4] });
                }
     
                userlist.ItemsSource = dataTable.DefaultView;
                userlist.Visibility = Visibility.Visible;
                xLineChart.Visibility = Visibility.Hidden;
            }
        }
    }
}
