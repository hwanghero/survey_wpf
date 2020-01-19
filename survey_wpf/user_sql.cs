using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Controls;
using MySql.Data.MySqlClient;
using System.Data;

namespace survey_wpf
{
    class user_sql
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

        public int adminlogin(String id, String pw)
        {
            using (conn = new MySqlConnection(dbInfo()))
            {
                conn.Open();
                if (conn.State == ConnectionState.Open)
                {
                    string sql3 = "select * from admin where id='"+id+"'and pw='"+pw+"'";
                    MySqlCommand cmd3 = new MySqlCommand(sql3, conn);
                    MySqlDataReader rdr;

                    try
                    {
                        rdr = cmd3.ExecuteReader();
                        if (rdr.Read())
                        {
                            return 1;
                        }
                        else
                        {
                            return 0;
                        }
                    }
                    catch
                    {
                        return 0;
                    }
                }
            }
            
            conn.Close();
            return 0;
        }
    }
}
