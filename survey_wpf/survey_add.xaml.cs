using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace survey_wpf
{
    public partial class survey_add : Page
    {
        // textbox로 바로 해줘도 됨,
        String mode = "없음";
        survey_sql s_sql = new survey_sql();
        String app_top_idx = "";

        public survey_add()
        {
            InitializeComponent();
            // 첫 모드상태 표시
            modestatus.Text = mode;
            // 데이터그리드 데이터 생성
            if ((App.Current as App).MainNumber != null)
            {
                app_top_idx = (App.Current as App).MainNumber.ToString();
                String[] getapp = s_sql.getMainSurveyList(int.Parse(app_top_idx));
                top_idx.Text = getapp[1];
                insert.IsEnabled = true;
                update.IsEnabled = true;
                delete.IsEnabled = true;
            }
            datagrid_load("전체", null, app_top_idx);

            (App.Current as App).Pagemode = "add";
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
            dataGrid1.ItemsSource = dataTable.DefaultView;
        }

        // 버튼 클릭시
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (sender.ToString().Contains("입력") || sender.ToString().Contains("수정") || sender.ToString().Contains("삭제"))
            {
                subbutton_click(sender.ToString());
            }

            if (sender.ToString().Contains("확인") || sender.ToString().Contains("취소"))
            {
                mainbutton_click(sender.ToString());
            }

            if (sender.ToString().Contains("검색"))
            {
                datagrid_load(searchmode.Text, search_text.Text, app_top_idx);
            }

            if (sender.ToString().Contains("찾기"))
            {
                NavigationService.Navigate(new Uri("survey_main_search.xaml", UriKind.Relative));
            }
        }

        private void subbutton_click(String sender)
        {
            insert.IsEnabled = false;
            update.IsEnabled = false;
            delete.IsEnabled = false;

            success.IsEnabled = true;
            cancel.IsEnabled = true;

            if (sender.Contains("입력"))
            {
                // 고유번호는 자동으로 추가되니까 false 그 외에는 입력해야할 정보들
                number.IsEnabled = false;
                title.IsEnabled = true;
                view.IsEnabled = true;
                type.IsEnabled = true;
                mode = "입력";
            }

            if (sender.Contains("수정"))
            {
                // 값 가져와서 수정하는거는 수정 버튼 누르고 해야함.
                // 밑에 누를경우 텍스트박스 true 시킴
                mode = "수정";
                Message("밑에 수정을 원하는 설문조사를 선택해주세요");
            }

            if (sender.Contains("삭제"))
            {
                // 고유번호로만 삭제시키면됨
                number.IsEnabled = true;
                title.IsEnabled = false;
                view.IsEnabled = false;
                type.IsEnabled = false;
                mode = "삭제";
            }

            // 모드 상태 현황 업데이트
            modestatus.Text = mode;
        }

        private void mainbutton_click(String sender)
        {
            // 확인/취소 작업을 했으니 다시 서브버튼 활성화
            insert.IsEnabled = true;
            update.IsEnabled = true;
            delete.IsEnabled = true;

            // 확인/취소를 했으니 버튼 비활성화
            success.IsEnabled = false;
            cancel.IsEnabled = false;

            // 확인/취소를 했으니 텍스트박스는 다시 비활성화
            number.IsEnabled = false;
            title.IsEnabled = false;
            view.IsEnabled = false;
            type.IsEnabled = false;

            // 버튼눌렀으니까 적용
            if (sender.Contains("확인"))
            {
                if (mode.Equals("입력"))
                {
                    // 공백 체크
                    if(String.IsNullOrWhiteSpace(title.Text) || String.IsNullOrWhiteSpace(view.Text) || String.IsNullOrWhiteSpace(type.Text))
                    {
                        Message("모든 정보를 입력해주세요");
                    }
                    else
                    {
                        // 중복 체크
                        int insert = s_sql.survey_insert(title.Text, view.Text, int.Parse((App.Current as App).MainNumber.ToString()), type.Text);
                        if (insert == -1)
                        {
                            Message("이미 생성한 설문조사가 있습니다");
                        }
                        else
                        {
                            // 생성에 완료했으면 리스트 새로고침
                            Message("설문조사 생성완료");
                            datagrid_load("전체", null, app_top_idx);
                        }
                    }
                }

                if (mode.Equals("수정"))
                {
                    // 공백 체크
                    if (String.IsNullOrWhiteSpace(number.Text) || String.IsNullOrWhiteSpace(title.Text) || String.IsNullOrWhiteSpace(view.Text) || String.IsNullOrWhiteSpace(type.Text))
                    {
                        Message("모든 정보를 입력해주세요");
                    }
                    else
                    {
                        int update = s_sql.survey_update(number.Text, title.Text, view.Text, top_idx.Text, type.Text);
                        if (update == -1)
                            Message("중복되는 설문조사가 있습니다");
                        else
                            Message("설문조사 수정완료"); datagrid_load("전체", null, app_top_idx);

                    }
                }

                if (mode.Equals("삭제"))
                {
                    if (String.IsNullOrWhiteSpace(number.Text))
                    {
                        Message("모든 정보를 입력해주세요");
                    }
                    else
                    {
                        if (MessageBox.Show("설문조사를 정말로 삭제하시겠습니까?", "설문조사 관리", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                        {
                            // 취소
                        }
                        else
                        {
                            int delete = s_sql.survey_delete(number.Text, int.Parse((App.Current as App).MainNumber.ToString()));
                            if (delete == 1)
                            {
                                Message("설문조사를 삭제하였습니다.");
                                datagrid_load("전체", null, app_top_idx);
                            }
                            else
                            {
                                Message("없는 설문조사입니다.");
                            }
                        }
                    }
                }
            }

            if (sender.Contains("취소"))
            {
                Message(mode+" 취소");
            }

            // 확인/취소 했으니 텍스트박스 리셋
            number.Text = "";
            title.Text = "";
            view.Text = "";
            type.Text = "";

            // 모드 초기화
            mode = "없음";

            // 모드 상태 현황 업데이트
            modestatus.Text = mode;
        }

        private void dataGrid1_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (dataGrid1.SelectedItem != null)
            {
                if (mode.Equals("수정"))
                {
                    DataRowView row = (DataRowView)dataGrid1.SelectedItems[0];
                    String[] surveyget = s_sql.surveyGet(row["제목"].ToString());

                    // 값 불러서 텍스트박스에 저장
                    number.Text = surveyget[0];
                    title.Text = surveyget[1];
                    view.Text = surveyget[2];
                    type.Text = surveyget[4];

                    // 목록 눌러서 텍스트박스 다 활성화
                    number.IsEnabled = true;
                    title.IsEnabled = true;
                    view.IsEnabled = true;
                    type.IsEnabled = false;
                }

                if (mode.Equals("삭제"))
                {
                    DataRowView row = (DataRowView)dataGrid1.SelectedItems[0];
                    String[] surveyget = s_sql.surveyGet(row["제목"].ToString());

                    // 값 불러서 텍스트박스에 저장
                    number.Text = surveyget[0];
                    title.Text = surveyget[1];
                    view.Text = surveyget[2];
                    type.Text = surveyget[4];
                }
            }
        }

        public void Message(String input)
        {
            MessageBox.Show(input, "설문조사 관리");
        }

        private void Searchmode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox a = sender as ComboBox;
            string selected_text = a.SelectedValue.ToString();

            if (search_text != null)
            {
                if (selected_text.Contains("전체"))
                {
                    search_text.IsEnabled = false;
                }
                else
                {
                    search_text.IsEnabled = true;
                }
            }
        }
    }
}
