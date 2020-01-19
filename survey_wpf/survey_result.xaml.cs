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

namespace survey_wpf
{
    /// <summary>
    /// survey_result.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class survey_result : Page
    {
        public survey_result()
        {
            InitializeComponent();
            ShowColumnChart();
        }

        private void ShowColumnChart()
        {
            List<KeyValuePair<string, int>> valueList = new List<KeyValuePair<string, int>>();

            valueList.Add(new KeyValuePair<string, int>("개발자", 83));
            valueList.Add(new KeyValuePair<string, int>("기타", 17));
            valueList.Add(new KeyValuePair<string, int>("테스터", 36));
            valueList.Add(new KeyValuePair<string, int>("품질", 28));
            valueList.Add(new KeyValuePair<string, int>("관리자", 39));

            xColumnChart.DataContext = valueList;
            xPieChart.DataContext = valueList;
            xAreaChart.DataContext = valueList;
            xBarChart.DataContext = valueList;
            xLineChart.DataContext = valueList;
        }
    }
}
