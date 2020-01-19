using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace survey_wpf
{
    class survey_sql
    {
        MySqlConnection conn;
        string dbInfo()
        {
            string dbServer = "localhost";
            string dbDatabase = "survey";
            string dbUid = "root";
            string dbPwd = "1234";
            string dbSslMode = "none";
            string Conn = "Server=" + dbServer + ";" + "Database=" + dbDatabase + ";" + "Uid=" + dbUid + ";" + "Pwd=" + dbPwd + ";" + "SslMode=" + dbSslMode;
            return Conn;
        }
        MySqlDataReader rdr;

        // 메인 설문조사 개수
        public int getSurveyMainCount()
        {
            int i = 0;
            using (conn = new MySqlConnection(dbInfo()))
            {
                conn.Open();
                if (conn.State == ConnectionState.Open)
                {
                    string sql = "select count(*) from main_survey";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);

                    try
                    {
                        rdr = cmd.ExecuteReader();
                        if (rdr.Read())
                        {
                            i = int.Parse(rdr[0].ToString());
                        }
                    }
                    catch
                    {
                        return -1;
                    }
                }
                rdr.Close();
                conn.Close();
            }
            return i;
        }

        // 메인 설문조사 리스트
        public List<String[,]> getMainSurveyList()
        {
            int count = getSurveyMainCount();
            String[,] itemlist = new string[count, 4];
            List<String[,]> getitem = new List<string[,]>();

            using (conn = new MySqlConnection(dbInfo()))
            {
                conn.Open();
                if (conn.State == ConnectionState.Open)
                {
                    string sql = "select * from main_survey";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);

                    try
                    {
                        rdr = cmd.ExecuteReader();
                        int i = 0;
                        if (rdr.Read())
                        {
                            do
                            {
                                itemlist[i, 0] = rdr[0].ToString();
                                itemlist[i, 1] = rdr[1].ToString();
                                itemlist[i, 2] = rdr[2].ToString();
                                itemlist[i, 3] = rdr[3].ToString();
                                getitem.Add(itemlist);
                                i++;
                            } while (rdr.Read());
                        }
                    }
                    catch
                    {
                        return getitem;
                    }
                }
                rdr.Close();
                conn.Close();
            }
            return getitem;
        }

        // 메인 설문조사 리스트 검색
        public String[] getMainSurveyList(int idx)
        {
            int count = getSurveyMainCount();
            String[] itemlist = new string[4];
            using (conn = new MySqlConnection(dbInfo()))
            {
                conn.Open();
                if (conn.State == ConnectionState.Open)
                {
                    string sql = "select * from main_survey where idx="+idx;
                    MySqlCommand cmd = new MySqlCommand(sql, conn);

                    try
                    {
                        rdr = cmd.ExecuteReader();
                        int i = 0;
                        if (rdr.Read())
                        {
                            do
                            {
                                itemlist[0] = rdr[0].ToString();
                                itemlist[1] = rdr[1].ToString();
                                itemlist[2] = rdr[2].ToString();
                                itemlist[3] = rdr[3].ToString();
                                i++;
                            } while (rdr.Read());
                        }
                    }
                    catch
                    {
                        return itemlist;
                    }
                }
                rdr.Close();
                conn.Close();
            }
            return itemlist;
        }

        // 메인 설문조사 입력
        public int main_survey_insert(String title, String view, String info)
        {
            int check = 0;

            // 중복체크
            List<String[,]> survey_check = getMainSurveyList();
            String[,] getitem = new String[getSurveyMainCount(), 4];

            if (survey_check != null)
            {
                for (int x = 0; x < getSurveyMainCount(); x++)
                {
                    getitem = survey_check[x];
                    if (getitem[x, 1].Equals(title) || title.Equals(""))
                    {
                        check = -1;
                    }
                }
            }

            if (check == 0)
            {
                using (conn = new MySqlConnection(dbInfo()))
                {
                    String query = "INSERT INTO main_survey (title, view, info) VALUES (@title, @view, @info)";

                    using (MySqlCommand command = new MySqlCommand(query, conn))
                    {
                        command.Parameters.AddWithValue("@title", title);
                        command.Parameters.AddWithValue("@view", view);
                        command.Parameters.AddWithValue("@info", info);

                        conn.Open();
                        int result = command.ExecuteNonQuery();
                        if (result < 0)
                            Console.WriteLine("Error inserting data into Database!");
                    }
                }
            }
            rdr.Close();
            conn.Close();
            return check;
        }

        // 메인 설문조사 수정하기
        public int main_survey_update(String number, String title, String view, String info)
        {
            int check = 0;

            if (check == 0)
            {
                using (conn = new MySqlConnection(dbInfo()))
                {
                    String query = "update main_survey set title=@title, view=@view, info=@info where idx='" + number + "'";

                    using (MySqlCommand command = new MySqlCommand(query, conn))
                    {
                        command.Parameters.AddWithValue("@title", title);
                        command.Parameters.AddWithValue("@view", view);
                        command.Parameters.AddWithValue("@info", info);

                        conn.Open();
                        int result = command.ExecuteNonQuery();
                        if (result < 0)
                            Console.WriteLine("Error update failed");
                    }
                }
            }
            rdr.Close();
            conn.Close();
            return check;
        }

        // 메인조사 삭제하기
        public int main_survey_delete(String number)
        {
            int check = 0;

            // 없는값체크
            List<String[,]> survey_check = getMainSurveyList();
            String[,] getitem = new String[getSurveyMainCount(), 4];

            if (survey_check != null)
            {
                for (int x = 0; x < getSurveyMainCount(); x++)
                {
                    getitem = survey_check[x];
                    if (getitem[x, 0].Equals(number) || String.IsNullOrWhiteSpace(number))
                    {
                        check = 1;
                    }
                }
            }

            if (check == 1)
            {
                using (conn = new MySqlConnection(dbInfo()))
                {
                    String query = "delete from main_survey where idx='" + number + "'";

                    using (MySqlCommand command = new MySqlCommand(query, conn))
                    {
                        conn.Open();
                        int result = command.ExecuteNonQuery();
                        if (result < 0)
                            Console.WriteLine("Error delete failed");
                    }
                }
            }
            rdr.Close();
            conn.Close();
            return check;
        }

        // 특정 설문조사 목록 가져오기
        public List<String[,]> getSurveyList_grid(String top_idx)
        {
            int count = getSurveyListCount();
            String[,] itemlist = new String[count, 5];
            List<String[,]> getitem = new List<string[,]>();

            using (conn = new MySqlConnection(dbInfo()))
            {
                conn.Open();
                if (conn.State == ConnectionState.Open)
                {
                    string sql = "select * from survey where top_idx="+top_idx;
                    MySqlCommand cmd = new MySqlCommand(sql, conn);

                    try
                    {
                        rdr = cmd.ExecuteReader();
                        int i = 0;
                        if (rdr.Read())
                        {
                            do
                            {
                                itemlist[i, 0] = rdr[0].ToString();
                                itemlist[i, 1] = rdr[1].ToString();
                                itemlist[i, 2] = rdr[2].ToString();
                                itemlist[i, 3] = rdr[3].ToString();
                                itemlist[i, 4] = rdr[4].ToString();
                                getitem.Add(itemlist);
                                i++;
                            } while (rdr.Read());
                        }
                    }
                    catch
                    {
                        return getitem;
                    }
                }
                rdr.Close();
                conn.Close();
            }
            return getitem;
        }

        // 설문조사 전체 목록 가져오기
        public List<String[,]> getSurveyList()
        {
            int count = getSurveyListCount();
            String[,] itemlist = new String[count, 5];
            List<String[,]> getitem = new List<string[,]>();

            using (conn = new MySqlConnection(dbInfo()))
            {
                conn.Open();
                if (conn.State == ConnectionState.Open)
                {
                    string sql = "select * from survey";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);

                    try
                    {
                        rdr = cmd.ExecuteReader();
                        int i = 0;
                        if (rdr.Read())
                        {
                            do
                            {
                                itemlist[i, 0] = rdr[0].ToString();
                                itemlist[i, 1] = rdr[1].ToString();
                                itemlist[i, 2] = rdr[2].ToString();
                                itemlist[i, 3] = rdr[3].ToString();
                                itemlist[i, 4] = rdr[4].ToString();
                                getitem.Add(itemlist);
                                i++;
                            } while (rdr.Read());
                        }
                    }
                    catch
                    {
                        return getitem;
                    }
                }
                rdr.Close();
                conn.Close();
            }
            return getitem;
        }

        // 설문조사 개수 확인 count 방식이아님 비효율적.
        public int getSurveyListCount()
        {
            int i = 0;
            using (conn = new MySqlConnection(dbInfo()))
            {
                conn.Open();
                if (conn.State == ConnectionState.Open)
                {
                    string sql = "select * from survey";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    
                    try
                    {
                        rdr = cmd.ExecuteReader();
                        if (rdr.Read())
                        {
                            do
                            {
                                i++;
                            }
                            while (rdr.Read());
                        }
                    }
                    catch
                    {
                        return -1;
                    }
                }
                rdr.Close();
                conn.Close();
            }
            return i;
        }

        // 특정 설문조사 개수 찾기
        public int getSurveyCount_grid(String top_idx)
        {
            int i = 0;
            using (conn = new MySqlConnection(dbInfo()))
            {
                conn.Open();
                if (conn.State == ConnectionState.Open)
                {
                    string sql = "select count(*) from survey where top_idx="+top_idx;
                    MySqlCommand cmd = new MySqlCommand(sql, conn);

                    try
                    {
                        rdr = cmd.ExecuteReader();
                        if (rdr.Read())
                        {
                            i = int.Parse(rdr[0].ToString());
                        }
                    }
                    catch
                    {
                        return 0;
                    }
                }
                rdr.Close();
                conn.Close();
            }
            return i;
        }

        // 설문조사 입력
        public int survey_insert(String title, String view, int top_idx, String type)
        {
            int check = 0;

            // 중복체크
            List<String[,]> survey_check = getSurveyList();
            String[,] getitem = new String[getSurveyListCount(), 5];

            if(survey_check != null)
            {
                for (int x = 0; x < getSurveyListCount(); x++)
                {
                    getitem = survey_check[x];
                    if (getitem[x, 1].Equals(title) || title.Equals(""))
                    {
                        check = -1;
                    }
                }
            }


            if(check == 0)
            {
                using (conn = new MySqlConnection(dbInfo()))
                {
                    String query = "INSERT INTO survey (title, view, top_idx, type) VALUES (@title, @view, @top_idx, @type)";

                    using (MySqlCommand command = new MySqlCommand(query, conn))
                    {
                        command.Parameters.AddWithValue("@title", title);
                        command.Parameters.AddWithValue("@view", view);
                        command.Parameters.AddWithValue("@top_idx", top_idx);
                        command.Parameters.AddWithValue("@type", type);

                        conn.Open();
                        int result = command.ExecuteNonQuery();
                        if (result < 0)
                            Console.WriteLine("Error inserting data into Database!");
                    }
                }
            }
            rdr.Close();
            conn.Close();
            return check;
        }

        // 수정시에 필요한 설문조사 값 배열에 담기.
        public String[] surveyGet(String title)
        {
            String[] surveyData = new String[5];
            using (conn = new MySqlConnection(dbInfo()))
            {
                conn.Open();
                if (conn.State == ConnectionState.Open)
                {
                    string sql = "select * from survey where title='"+title+"'";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    try
                    {
                        rdr = cmd.ExecuteReader();
                        if (rdr.Read())
                        {
                            surveyData[0] = rdr[0].ToString();
                            surveyData[1] = rdr[1].ToString();
                            surveyData[2] = rdr[2].ToString();
                            surveyData[3] = rdr[3].ToString();
                            surveyData[4] = rdr[4].ToString();
                        }
                    }
                    catch
                    {
                        Console.WriteLine("surveyGet rdr failed");
                    }
                }
            }
            rdr.Close();
            conn.Close();
            return surveyData;
        }

        // 수정하기
        public int survey_update(String number, String title, String view, String top_idx, String type)
        {
            int check = 0;

            if (check == 0)
            {
                using (conn = new MySqlConnection(dbInfo()))
                {
                    String query = "update survey set title=@title, view=@view, type=@type where idx='"+number+"'";

                    using (MySqlCommand command = new MySqlCommand(query, conn))
                    {
                        command.Parameters.AddWithValue("@title", title);
                        command.Parameters.AddWithValue("@view", view);
                        command.Parameters.AddWithValue("@type", type);

                        conn.Open();
                        int result = command.ExecuteNonQuery();
                        if (result < 0)
                            Console.WriteLine("Error update failed");
                    }
                }
            }
            rdr.Close();
            conn.Close();
            return check;
        }

        // 삭제하기
        public int survey_delete(String number, int top_idx)
        {
            int check = 0;

            // 없는값체크
            List<String[,]> survey_check = getSurveyList();
            String[,] getitem = new String[getSurveyListCount(), 3];

            if (survey_check != null)
            {
                for (int x = 0; x < getSurveyListCount(); x++)
                {
                    getitem = survey_check[x];
                    if (getitem[x, 0].Equals(number) || String.IsNullOrWhiteSpace(number))
                    {
                        check = 1;
                    }
                }
            }

            if (check == 1)
            {
                using (conn = new MySqlConnection(dbInfo()))
                {
                    String query = "delete from survey where idx='" + number + "'";

                    using (MySqlCommand command = new MySqlCommand(query, conn))
                    {
                        conn.Open();
                        int result = command.ExecuteNonQuery();
                        if (result < 0)
                            Console.WriteLine("Error delete failed");
                    }
                }
            }
            rdr.Close();
            conn.Close();
            return check;
        }

        // 객관식 설문조사 항목 개수 확인
        public int getSurveyItemCount(int index)
        {
            int i = 0;
            using (conn = new MySqlConnection(dbInfo()))
            {
                conn.Open();
                if (conn.State == ConnectionState.Open)
                {
                    string sql = "select count(*) from mulit_survey_item where top_idx="+index;
                    MySqlCommand cmd = new MySqlCommand(sql, conn);

                    try
                    {
                        rdr = cmd.ExecuteReader();
                        if (rdr.Read())
                        {
                            i =  int.Parse(rdr[0].ToString());
                        }
                    }
                    catch
                    {
                        return -1;
                    }
                }
                rdr.Close();
                conn.Close();
            }
            return i;
        }

        // 객관식 설문조사 항목 가져오기
        public List<String[,]> survey_item_get(int index)
        {
            int item_count = getSurveyItemCount(index);
            String[,] item = new String[item_count, 7];
            List<String[,]> getitem = new List<string[,]>();

            using (conn = new MySqlConnection(dbInfo()))
            {
                conn.Open();
                if (conn.State == ConnectionState.Open)
                {
                    string sql = "select * from mulit_survey_item where top_idx=" + index;
                    MySqlCommand cmd = new MySqlCommand(sql, conn);

                    try
                    {
                        rdr = cmd.ExecuteReader();
                        int i = 0;
                        if (rdr.Read())
                        {
                            do
                            {
                                item[i, 0] = rdr[0].ToString();
                                item[i, 1] = rdr[1].ToString();
                                item[i, 2] = rdr[2].ToString();
                                item[i, 3] = rdr[3].ToString();
                                item[i, 4] = rdr[4].ToString();
                                item[i, 5] = rdr[5].ToString();
                                item[i, 6] = rdr[6].ToString();
                                getitem.Add(item);
                                i++;
                            } while (rdr.Read());
                        }
                    }
                    catch
                    {
                        return getitem;
                    }
                }
                rdr.Close();
                conn.Close();
            }
            return getitem;
        }

        // 객관식 추가
        public int survey_mulit_item_insert(int index, String type, String view, String question, String name)
        {
            int check = 0;

            // 중복체크
            List<String[,]> survey_check = survey_item_get(index);
            String[,] getitem = new String[getSurveyItemCount(index), 7];

            if (survey_check != null)
            {
                for (int x = 0; x < getSurveyItemCount(index); x++)
                {
                    getitem = survey_check[x];
                    if (getitem[x, 3].Equals(question) || question.Equals(""))
                    {
                        check = -1;
                    }
                }
            }

            if (check == 0)
            {
                using (conn = new MySqlConnection(dbInfo()))
                {
                    String query = "INSERT INTO mulit_survey_item VALUES (@index, @type, @view, @question, 0, @name, null)";

                    using (MySqlCommand command = new MySqlCommand(query, conn))
                    {
                        command.Parameters.AddWithValue("@index", index);
                        command.Parameters.AddWithValue("@type", type);
                        command.Parameters.AddWithValue("@view", view);
                        command.Parameters.AddWithValue("@question", question);
                        command.Parameters.AddWithValue("@name", name);

                        conn.Open();
                        int result = command.ExecuteNonQuery();
                        if (result < 0)
                            Console.WriteLine("Error inserting data into Database!");
                    }
                }
            }
            rdr.Close();
            conn.Close();
            return check;
        }

        // 객관식 수정하기
        public int survey_mulit_update(int index, String type, String view, String question, String name, String item_idx)
        {
            int check = 0;

            if (check == 0)
            {
                using (conn = new MySqlConnection(dbInfo()))
                {
                    String query = "update mulit_survey_item set type=@type, view=@view, question=@question, name=@name where idx='" + item_idx + "' and top_idx='" + index +"'";

                    using (MySqlCommand command = new MySqlCommand(query, conn))
                    {
                        command.Parameters.AddWithValue("@type", type);
                        command.Parameters.AddWithValue("@view", view);
                        command.Parameters.AddWithValue("@question", question);
                        command.Parameters.AddWithValue("@name", name);

                        conn.Open();
                        int result = command.ExecuteNonQuery();
                        if (result < 0)
                            Console.WriteLine("Error update failed");
                    }
                }
            }
            rdr.Close();
            conn.Close();
            return check;
        }
        
        // 객관식 항목 삭제
        public int survey_mulit_item_delete(String top_idx, String index)
        {
            int check = 0;

            // 없는값체크
            List<String[,]> survey_check = survey_item_get(int.Parse(top_idx));
            String[,] getitem = new String[getSurveyItemCount(int.Parse(top_idx)), 7];

            if (survey_check != null)
            {
                for (int x = 0; x < getSurveyItemCount(int.Parse(top_idx)); x++)
                {
                    getitem = survey_check[x];
                    if (getitem[x, 6].Equals(index) || !String.IsNullOrWhiteSpace(index))
                    {
                        check = 1;
                    }
                }
            }

            if (check == 1)
            {
                using (conn = new MySqlConnection(dbInfo()))
                {
                    String query = "delete from mulit_survey_item where top_idx="+top_idx+" and idx=" + index;

                    using (MySqlCommand command = new MySqlCommand(query, conn))
                    {
                        conn.Open();
                        int result = command.ExecuteNonQuery();
                        if (result < 0)
                            Console.WriteLine("Error delete failed");
                    }
                }
            }
            rdr.Close();
            conn.Close();
            return check;
        }

        // 주관식 몇개인지
        public int getSurveyShortCount(int top_idx)
        {
            int i = 0;
            using (conn = new MySqlConnection(dbInfo()))
            {
                conn.Open();
                if (conn.State == ConnectionState.Open)
                {
                    string sql = "select count(*) from short_survey_item where top_idx=" + top_idx;
                    MySqlCommand cmd = new MySqlCommand(sql, conn);

                    try
                    {
                        rdr = cmd.ExecuteReader();
                        if (rdr.Read())
                        {
                            i = int.Parse(rdr[0].ToString());
                        }
                    }
                    catch
                    {
                        return -1;
                    }
                }
                rdr.Close();
                conn.Close();
            }
            return i;
        }

        // 주관식 설문조사 가져오기
        public List<String[,]> survey_short_item_get(int top_idx)
        {
            int item_count = getSurveyShortCount(top_idx);
            String[,] item = new String[item_count, 6];
            List<String[,]> getitem = new List<string[,]>();

            using (conn = new MySqlConnection(dbInfo()))
            {
                conn.Open();
                if (conn.State == ConnectionState.Open)
                {
                    string sql = "select * from short_survey_item where top_idx=" + top_idx;
                    MySqlCommand cmd = new MySqlCommand(sql, conn);

                    try
                    {
                        rdr = cmd.ExecuteReader();
                        int i = 0;
                        if (rdr.Read())
                        {
                            do
                            {
                                item[i, 0] = rdr[0].ToString();
                                item[i, 1] = rdr[1].ToString();
                                item[i, 2] = rdr[2].ToString();
                                item[i, 3] = rdr[3].ToString();
                                item[i, 4] = rdr[4].ToString();
                                item[i, 5] = rdr[5].ToString();
                                getitem.Add(item);
                                i++;
                            } while (rdr.Read());
                        }
                    }
                    catch
                    {
                        return getitem;
                    }
                }
                rdr.Close();
                conn.Close();
            }
            return getitem;
        }

        // 주관식 추가
        public int survey_short_item_insert(int top_idx, String type, String view, String question, String m_height)
        {
            int check = 0;

            // 중복체크
            List<String[,]> survey_check = survey_short_item_get(top_idx);
            String[,] getitem = new String[getSurveyShortCount(top_idx), 6];

            if (survey_check != null)
            {
                for (int x = 0; x < getSurveyShortCount(top_idx); x++)
                {
                    getitem = survey_check[x];
                    if (getitem[x, 3].Equals(question) || question.Equals(""))
                    {
                        check = -1;
                    }
                }
            }

            if (check == 0)
            {
                using (conn = new MySqlConnection(dbInfo()))
                {
                    String query = "INSERT INTO short_survey_item VALUES (@type, @view, @question, @m_height, null, @index)";

                    using (MySqlCommand command = new MySqlCommand(query, conn))
                    {
                        command.Parameters.AddWithValue("@index", top_idx);
                        command.Parameters.AddWithValue("@type", type);
                        command.Parameters.AddWithValue("@view", view);
                        command.Parameters.AddWithValue("@question", question);
                        command.Parameters.AddWithValue("@m_height", m_height);

                        conn.Open();
                        int result = command.ExecuteNonQuery();
                        if (result < 0)
                            Console.WriteLine("Error inserting data into Database!");
                    }
                }
            }
            rdr.Close();
            conn.Close();
            return check;
        }

        // 주관식 수정하기
        public int survey_short_update(int top_idx, String view, String question, String mheight, String item_idx)
        {
            int check = 0;
            if (check == 0)
            {
                using (conn = new MySqlConnection(dbInfo()))
                {
                    String query = "update short_survey_item set view=@view, question=@question, m_height=@mheight where idx='" + item_idx + "' and top_idx='" + top_idx + "'";

                    using (MySqlCommand command = new MySqlCommand(query, conn))
                    {
                        command.Parameters.AddWithValue("@view", view);
                        command.Parameters.AddWithValue("@question", question);
                        command.Parameters.AddWithValue("@mheight", mheight);

                        conn.Open();
                        int result = command.ExecuteNonQuery();
                        if (result < 0)
                            Console.WriteLine("Error update failed");
                    }
                }
            }
            rdr.Close();
            conn.Close();
            return check;
        }

        // 주관식 항목 삭제
        public int survey_short_item_delete(String top_idx, String index)
        {
            int check = 0;

            // 없는값체크
            List<String[,]> survey_check = survey_short_item_get(int.Parse(top_idx));
            String[,] getitem = new String[getSurveyShortCount(int.Parse(top_idx)), 6];

            if (survey_check != null)
            {
                for (int x = 0; x < getSurveyShortCount(int.Parse(top_idx)); x++)
                {
                    getitem = survey_check[x];
                    if (getitem[x, 5].Equals(index) || !String.IsNullOrWhiteSpace(index))
                    {
                        check = 1;
                    }
                }
            }

            if (check == 1)
            {
                using (conn = new MySqlConnection(dbInfo()))
                {
                    String query = "delete from short_survey_item where top_idx=" + top_idx + " and idx=" + index;

                    using (MySqlCommand command = new MySqlCommand(query, conn))
                    {
                        conn.Open();
                        int result = command.ExecuteNonQuery();
                        if (result < 0)
                            Console.WriteLine("Error delete failed");
                    }
                }
            }
            rdr.Close();
            conn.Close();
            return check;
        }

        // 객관식 통계 개수 구하기
        public int getResult(int sub_idx, String answer)
        {
            int i = 0;
            using (conn = new MySqlConnection(dbInfo()))
            {
                conn.Open();
                if (conn.State == ConnectionState.Open)
                {
                    string sql = "select count(*) from user_answer where sub_idx=" + sub_idx +" and answer like '"+answer+"'";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);

                    try
                    {
                        rdr = cmd.ExecuteReader();
                        if (rdr.Read())
                        {
                            i = int.Parse(rdr[0].ToString());
                        }
                    }
                    catch
                    {
                        return -1;
                    }
                }
                rdr.Close();
                conn.Close();
            }
            return i;
        }

        // 주관식 통계 개수 구하기
        public int getShortResult(int sub_idx)
        {
            int i = 0;
            using (conn = new MySqlConnection(dbInfo()))
            {
                conn.Open();
                if (conn.State == ConnectionState.Open)
                {
                    string sql = "select count(*) from user_answer where sub_idx=" + sub_idx;
                    MySqlCommand cmd = new MySqlCommand(sql, conn);

                    try
                    {
                        rdr = cmd.ExecuteReader();
                        if (rdr.Read())
                        {
                            i = int.Parse(rdr[0].ToString());
                        }
                    }
                    catch
                    {
                        return -1;
                    }
                }
                rdr.Close();
                conn.Close();
            }
            return i;
        }

        // 주관식 결과값 가져오기
        public List<String[,]> getShortList(int sub_idx)
        {
            int item_count = getShortResult(sub_idx);
            String[,] item = new String[item_count, 6];
            List<String[,]> getitem = new List<string[,]>();

            using (conn = new MySqlConnection(dbInfo()))
            {
                conn.Open();
                if (conn.State == ConnectionState.Open)
                {
                    string sql = "select * from user_answer where sub_idx=" + sub_idx;
                    MySqlCommand cmd = new MySqlCommand(sql, conn);

                    try
                    {
                        rdr = cmd.ExecuteReader();
                        int i = 0;
                        if (rdr.Read())
                        {
                            do
                            {
                                item[i, 0] = rdr[0].ToString();
                                item[i, 1] = rdr[1].ToString();
                                item[i, 2] = rdr[2].ToString();
                                item[i, 3] = rdr[3].ToString();
                                item[i, 4] = rdr[4].ToString();
                                item[i, 5] = rdr[5].ToString();
                                getitem.Add(item);
                                i++;
                            } while (rdr.Read());
                        }
                    }
                    catch
                    {
                        return getitem;
                    }
                }
                rdr.Close();
                conn.Close();
            }
            return getitem;
        }
    }
}
