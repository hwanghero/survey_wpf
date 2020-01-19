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
    /// survey_list.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class survey_list : Page
    {
        survey_sql s_sql = new survey_sql();
        public survey_list()
        {
            InitializeComponent();
            datagrid_load();
        }

        /*
         * 누를경우 새로운 폼창떠서 무슨 항목이 있는지 볼수있게끔
         */

        public void datagrid_load()
        {
            // 데이터그리드 데이터 생성
            DataTable dataTable = new DataTable();

            // 컬럼 생성
            dataTable.Columns.Add("번호", typeof(string));
            dataTable.Columns.Add("제목", typeof(string));
            dataTable.Columns.Add("보기", typeof(string));

            // 목록 바로 띄워주기
            int a = s_sql.getSurveyListCount();
            List<String[,]> b = s_sql.getSurveyList();
            String[,] getb = new String[a, 4];
            if (b != null)
            {
                for (int i = a - 1; i >= 0; i--)
                {
                    getb = b[i];
                    dataTable.Rows.Add(new string[] { getb[i, 0], getb[i, 1], getb[i, 2]});
                }
            }
            dataGrid1.ItemsSource = dataTable.DefaultView;
        }
    }
}
