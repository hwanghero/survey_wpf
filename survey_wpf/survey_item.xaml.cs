using System;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace survey_wpf
{
    /// <summary>
    /// survey_item.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class survey_item : Page
    {
        survey_sql s_sql = new survey_sql();
        String select_title;
        String app_top_idx = "";

        public survey_item()
        {
            InitializeComponent();
            
            InputMethod.SetIsInputMethodEnabled(mheight, false);

            // 데이터그리드 데이터 생성
            if ((App.Current as App).SubNumber != null)
            {
                app_top_idx = (App.Current as App).SubNumber.ToString();
                String[] getapp = s_sql.getMainSurveyList(int.Parse(app_top_idx));
            }
            datagrid_load("전체", null, app_top_idx);
            (App.Current as App).Pagemode = "item";
        }

        public void itemgrid_load(int index)
        {
            DataTable dataTable = new DataTable();

            dataTable.Columns.Add("형태", typeof(string));
            dataTable.Columns.Add("순서", typeof(string));
            dataTable.Columns.Add("고유번호", typeof(string));
            dataTable.Columns.Add("질문내용", typeof(string));
            dataTable.Columns.Add("타입", typeof(string));
            dataTable.Columns.Add("별명", typeof(string));
            dataTable.Columns.Add("뷰", typeof(string));
            dataTable.Columns.Add("박스 넓기", typeof(string));

            int allnumber = 1;

            // 객관식 리스트 뽑아오기
            int a = s_sql.getSurveyItemCount(index);
            List<String[,]> b = s_sql.survey_item_get(index);
            String[,] getb = new String[a, 7];

            if (b != null)
            {
                for (int i = a - 1; i >= 0; i--)
                {
                    getb = b[i];
                    dataTable.Rows.Add(new string[] {"객관식", allnumber.ToString(), getb[i, 6], getb[i, 3], getb[i, 1], getb[i, 5], getb[i, 2]});
                    allnumber++;
                }
            }

            // 주관식 리스트 뽑아오기
            int c = s_sql.getSurveyShortCount(index);
            List<String[,]> d = s_sql.survey_short_item_get(index);
            String[,] getd = new String[c, 6];

            if (d != null)
            {
                for (int i = c - 1; i >= 0; i--)
                {
                    getd = d[i];
                    dataTable.Rows.Add(new string[] {"주관식", allnumber.ToString(), getd[i, 4], getd[i, 2], getd[i, 0], "", getd[i, 1], getd[i, 3] });
                    allnumber++;
                }
            }

            itemlist.ItemsSource = dataTable.DefaultView;
        }

        public void datagrid_load(String mode, String input, String top_idx)
        {
            // 데이터그리드 데이터 생성
            DataTable dataTable = new DataTable();

            // 컬럼 생성
            dataTable.Columns.Add("메인 설문조사", typeof(string));
            dataTable.Columns.Add("고유번호", typeof(string));
            dataTable.Columns.Add("제목", typeof(string));
            dataTable.Columns.Add("보기", typeof(string));
            dataTable.Columns.Add("타입", typeof(string));

            // 목록 바로 띄워주기
            if (app_top_idx.Equals(""))
            {
                dataTable.Rows.Add(new string[] { "메인 설문조사를 선택해주세요", "", "", "", "" });
            }
            else
            {
                int a = s_sql.getSurveyCount_grid(app_top_idx);
                List<String[,]> b = s_sql.getSurveyList_grid(app_top_idx);
                Console.WriteLine(a);
                String[,] getb = new String[a, 5];
                if (b != null)
                {
                    for (int i = a - 1; i >= 0; i--)
                    {
                        getb = b[i];
                        if (mode.Equals("전체"))
                        {
                            dataTable.Rows.Add(new string[] { getb[i, 3], getb[i, 0], getb[i, 1], getb[i, 2], getb[i, 4] });
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
            surveylist.ItemsSource = dataTable.DefaultView;
        }

        private void Searchmode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox a = sender as ComboBox;
            string selected_text = a.SelectedValue.ToString();

            if (searchbox != null)
            {
                if (selected_text.Contains("전체"))
                {
                    searchbox.IsEnabled = false;
                }
                else
                {
                    searchbox.IsEnabled = true;
                    
                }
            }
        }

        /*
         * 누를 경우 만족도 활성화 이미 활성화일경우 패스
        */
        private void Surveylist_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (surveylist.SelectedItem != null)
            {
                if (itembox.IsEnabled == false)
                {
                    itembox.IsEnabled = true;
                    // 확인/취소버튼은 비활성화 시켜줌.
                    successB.IsEnabled = false;
                    cancelB.IsEnabled = false;
                }

                DataRowView row = (DataRowView)surveylist.SelectedItems[0];
                
                // 설문조사 정보값 입력
                String[] survey_data = s_sql.surveyGet(row["제목"].ToString());
                survey_number.Text = "번호: " + survey_data[0];
                survey_title.Text = "제목: " + survey_data[1];
                survey_info.Text = "설명: " + survey_data[2];
                type.Text = row["타입"].ToString();
                // 전역변수로 설정
                select_title = survey_data[1];

                int item_count = s_sql.getSurveyItemCount(int.Parse(survey_data[0]));
                itemgrid_load(int.Parse(survey_data[0]));
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            String[] getbutton = sender.ToString().Split(' ');
            if (sender.ToString().Contains("입력") || sender.ToString().Contains("수정") || sender.ToString().Contains("삭제"))
            {
                subbutton_click(getbutton[1]);
                modestatus.Text = getbutton[1];
            }

            if (sender.ToString().Contains("확인") || sender.ToString().Contains("취소"))
            {
                mainbutton_click(getbutton[1]);
            }

            // 검색은 보류
            if (sender.ToString().Contains("검색"))
            {
                //datagrid_load(searchmode.Text, searchbox.Text);
            }
        }

        private void subbutton_click(String sender)
        {
            insertB.IsEnabled = false;
            updateB.IsEnabled = false;
            deleteB.IsEnabled = false;
            successB.IsEnabled = true;
            cancelB.IsEnabled = true;

            if (sender.Equals("입력"))
            {
                number.IsEnabled = false;
                type.IsEnabled = false;
                Content.IsEnabled = true;
                viewbox.IsEnabled = true;
                Nickname.IsEnabled = true;
            }

            if (sender.Equals("수정"))
            {
                number.IsEnabled = false;
                type.IsEnabled = false;
                Content.IsEnabled = false;
                viewbox.IsEnabled = false;
                Nickname.IsEnabled = false;
                Message("밑에 수정을 원하는 설문조사를 선택해주세요");
            }

            if (sender.Equals("삭제"))
            {
                number.IsEnabled = true;
                type.IsEnabled = false;
                Content.IsEnabled = false;
                viewbox.IsEnabled = false;
                Nickname.IsEnabled = false;
            }
        }

        private void mainbutton_click(String sender)
        {
            insertB.IsEnabled = true;
            updateB.IsEnabled = true;
            deleteB.IsEnabled = true;
            successB.IsEnabled = false;
            cancelB.IsEnabled = false;

            number.IsEnabled = false;
            type.IsEnabled = false;
            Content.IsEnabled = false;
            viewbox.IsEnabled = false;
            Nickname.IsEnabled = false;

            if (sender.Equals("확인"))
            {
                String[] selectitem = s_sql.surveyGet(select_title);
                if (modestatus.Text.Equals("입력"))
                {
                    // 비효율적이긴한데 귀찮아서 이렇게 씀
                    if (type.Text.Equals("Text"))
                    {
                        if (String.IsNullOrWhiteSpace(type.Text) || String.IsNullOrWhiteSpace(Content.Text) || String.IsNullOrWhiteSpace(viewbox.Text))
                        {
                            Message("정보를 전부다 입력해주세요");
                        }
                        else
                        {
                            int ok = s_sql.survey_short_item_insert(int.Parse(selectitem[0]), type.Text, viewbox.Text, Content.Text, mheight.Text);
                            if (ok == 0) Message("주관식 항목을 추가하였습니다.");
                            else Message("중복되는 항목이 있습니다.");
                        }
                    }
                    else
                    {
                        if (String.IsNullOrWhiteSpace(type.Text) || String.IsNullOrWhiteSpace(Content.Text) || String.IsNullOrWhiteSpace(viewbox.Text) || String.IsNullOrWhiteSpace(Nickname.Text))
                        {
                            Message("정보를 전부다 입력해주세요");
                        }
                        else
                        {
                            int ok = s_sql.survey_mulit_item_insert(int.Parse(selectitem[0]), type.Text, viewbox.Text, Content.Text, Nickname.Text);
                            if (ok == 0) Message("객관식 항목을 추가하였습니다.");
                            else Message("중복되는 항목이 있습니다.");
                        }
                    }
                }
                else if (modestatus.Text.Equals("수정"))
                {
                    if (type.Text.Equals("Text"))
                    {
                        if (String.IsNullOrWhiteSpace(type.Text) || String.IsNullOrWhiteSpace(Content.Text) || String.IsNullOrWhiteSpace(viewbox.Text) || String.IsNullOrWhiteSpace(mheight.Text))
                        {
                            Message("정보를 전부다 입력해주세요");
                        }
                        else
                        {
                            int ok = s_sql.survey_short_update(int.Parse(selectitem[0]), viewbox.Text, Content.Text, mheight.Text, number.Text);
                            Message("주관식 항목을 수정였습니다.");
                        }
                    }
                    else
                    {
                        if (String.IsNullOrWhiteSpace(number.Text) || String.IsNullOrWhiteSpace(type.Text) || String.IsNullOrWhiteSpace(Content.Text) || String.IsNullOrWhiteSpace(viewbox.Text) || String.IsNullOrWhiteSpace(Nickname.Text))
                        {
                            Message("정보를 전부다 입력해주세요");
                        }
                        else
                        {
                            int ok = s_sql.survey_mulit_update(int.Parse(selectitem[0]), type.Text, viewbox.Text, Content.Text, Nickname.Text, number.Text);
                            Message("객관식 항목을 수정하였습니다.");
                        }
                    }
                }
                else if (modestatus.Text.Equals("삭제"))
                {
                    if (type.Text.Equals("Text"))
                    {
                        int ok = s_sql.survey_short_item_delete(selectitem[0], number.Text);
                        if (ok == 1) Message("객관식 항목을 삭제하였습니다.");
                        else Message("항목을 찾을수없습니다.");
                    }
                    else
                    {
                        int ok = s_sql.survey_mulit_item_delete(selectitem[0], number.Text);
                        if (ok == 1) Message("주관식 항목을 삭제하였습니다.");
                        else Message("항목을 찾을수없습니다.");
                    }
                }
                itemgrid_load(int.Parse(selectitem[0]));
            }
            if (sender.Equals("취소"))
            {
                
            }

            number.Text = "";
            Content.Text = "";
            viewbox.Text = "";
            Nickname.Text = "";
            mheight.Text = "";
            modestatus.Text = "";
            textbox_height.Visibility = Visibility.Hidden;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox a = sender as ComboBox;

            if(a != null)
            {
                if(a.SelectedValue != null)
                {
                    String selected_text = a.SelectedValue.ToString();
                    if (searchbox != null)
                    {
                        if (selected_text.Contains("Text"))
                        {
                            // 텍스트박스 넓이 쓸필요가없음..;
                            textbox_height.Visibility = Visibility.Hidden; 
                            Nickname.Visibility = Visibility.Hidden;
                            Nickname_label.Visibility = Visibility.Hidden;
                        }
                        else
                        {
                            textbox_height.Visibility = Visibility.Hidden;
                            Nickname.Visibility = Visibility.Visible;
                            Nickname_label.Visibility = Visibility.Visible;
                        }
                    }
                }
            }

        }

        public void Message(String input)
        {
            MessageBox.Show(input, "만족도 관리");
        }

        private void itemlist_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (itemlist.SelectedItem != null)
            {
                DataRowView row = (DataRowView)itemlist.SelectedItems[0];
                if (modestatus.Text.Equals("수정"))
                {
                    // 값 불러서 텍스트박스에 저장
                    number.Text = row["고유번호"].ToString();
                    type.Text = row["타입"].ToString();
                    Content.Text = row["질문내용"].ToString();
                    viewbox.Text = row["뷰"].ToString();
                    Nickname.Text = row["별명"].ToString();
                    mheight.Text = row["박스 넓기"].ToString();
                    // 목록 눌러서 텍스트박스 다 활성화
                    number.IsEnabled = true;
                    type.IsEnabled = false;
                    if (type.Text.Equals("Text"))
                        type.IsEnabled = false;

                    Content.IsEnabled = true;
                    viewbox.IsEnabled = true;
                    Nickname.IsEnabled = true;
                }

                if (modestatus.Text.Equals("삭제"))
                {
                    number.Text = row["고유번호"].ToString();
                    type.Text = row["타입"].ToString();
                    Content.Text = row["질문내용"].ToString();
                    viewbox.Text = row["뷰"].ToString();
                    Nickname.Text = row["별명"].ToString();
                    mheight.Text = row["박스 넓기"].ToString();
                }
            }
        }
        public bool IsNumeric(string source)
        {
            Regex regex = new Regex("[^0-9.-]+");
            return !regex.IsMatch(source);
        }
        private void mheight_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = !IsNumeric(e.Text);
        }

        // 메인설문조사 찾기
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("survey_main_search.xaml", UriKind.Relative));
        }
    }
}
