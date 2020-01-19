using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace survey_wpf
{
    /// <summary>
    /// App.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class App : Application
    {
        public string MainNumber { get; set; }
        public string SubNumber { get; set; }
        public string SelectNumber { get; set; }
        public string Pagemode { get; set; }
    }
}
