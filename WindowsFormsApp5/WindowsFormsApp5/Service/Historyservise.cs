using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp5.userservice;

namespace WindowsFormsApp5.service
{
    public class Historyservise
    {
        
        public static List<DateTime> GetHistoryList(string userID)
        {
            List<DateTime> failedLogins = new List<DateTime>();// 失敗したログイン試行の時間を保存するリスト

            string connectionString = "Server=localhost;UID=pol05;Password=pol05;Database=test";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    // 直近の3回のログイン試行を確認　ORDER BY logtime DESC LIMIT 3で直近三回
                    string historyQuery = "SELECT logtime, result FROM login_history WHERE userID = @userID ORDER BY logtime DESC LIMIT 3";
                    using (MySqlCommand historyCommand = new MySqlCommand(historyQuery, connection))
                    {

                        historyCommand.Parameters.AddWithValue("@userID", userID);// 渡すパラメータ

                        //接続開始
                        connection.Open();

                        var reader = historyCommand.ExecuteReader();// SQLクエリを実行

                        while (reader.Read())// クエリの結果を読む
                        {
                            if (reader.GetInt32("result") == 0)
                            {
                                DateTime logtime = reader.GetDateTime("logtime");// 失敗したログインの場合、リストに追加
                                failedLogins.Add(logtime);
                            }
                        }
                        reader.Close();
                        return failedLogins;
                    }

                }
                catch (MySqlException )
                {
                    throw;
                }
                catch (Exception )
                {
                    throw;
                }
                finally
                {
                    //接続解除
                    connection.Close();
                }
            }
        }
    }
}
