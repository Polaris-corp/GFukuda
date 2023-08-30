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
using WindowsFormsApp5.Constant;

namespace WindowsFormsApp5.service
{
    public class Historyservise
    {

        /// <summary>
        /// login_historyから、直近三回の失敗logを降順で保存するリストを作成するメソッドです。
        /// </summary>
        /// <param name="userID">userIDが入ります</param>
        /// <returns>作成したリスト</returns>
        public static List<DateTime> GetHistoryList(string userID)
        {
            List<DateTime> failedLogins = new List<DateTime>();
            using (MySqlConnection connection = new MySqlConnection(Constants.ConnectionString))
            {
                // 直近の3回のログイン試行を確認　ORDER BY logtime DESC LIMIT 3で直近三回
                string historyQuery = @"
                           SELECT 
                                logtime
                                , result 
                           FROM 
                                login_history 
                           WHERE 
                                userID = @userID 
                           ORDER BY 
                                logtime DESC 
                           LIMIT 
                                3";
                using (MySqlCommand historyCommand = new MySqlCommand(historyQuery, connection))
                {
                    historyCommand.Parameters.AddWithValue("@userID", userID);

                    //接続開始
                    connection.Open();
                    // SQLクエリを実行
                    var reader = historyCommand.ExecuteReader();
                    // クエリの結果を読む
                    while (reader.Read())
                    {
                        if (reader.GetInt32("result") == 0)
                        {
                            // 失敗したログインの場合、リストに追加
                            DateTime logtime = reader.GetDateTime("logtime");
                            failedLogins.Add(logtime);
                        }
                    }
                    reader.Close();
                    return failedLogins;
                }
            }
        }

        /// <summary>
        /// ロックアウトになるか判断するメソッドです。
        /// </summary>
        /// <param name="historyList">直近三回の失敗logが入ったリストです。</param>
        /// <returns>trueの場合、ロックアウト：falseの場合、ログイン成功。</returns>
        public static bool LockoutJudgement(List<DateTime> historyList, string userID)
        {
           
            if (!(historyList.Count == Constants.ListElementsCount))
            {
                return true;
            }

            if ((historyList[0] - historyList[2]).TotalMinutes <= Constants.LockoutTime &&
                (DateTime.Now - historyList[0]).Minutes <= Constants.LockoutTime)
            {
                return false;
            }

            return true;

        }

        /// <summary>
        /// ログイン試行のログをlogin_historyテーブルに残すメソッド
        /// </summary>
        /// <param name="userID">userIDがはいります。</param>
        /// <param name="loginResult">loginResultがはいります</param>
        public static void InsertLoginHistory(string userID, bool loginResult)
        {
            using (MySqlConnection connection = new MySqlConnection(Constants.ConnectionString))
            {
                string insertQuery = @"
                       INSERT 
                       INTO 
                            login_history 
                            (
                            userID
                            , logtime
                            , result
                            ) 
                       VALUES 
                            (
                            @userID
                            , @logtime
                            , @result
                            )";
                using (MySqlCommand insertCommand = new MySqlCommand(insertQuery, connection))
                {
                    insertCommand.Parameters.AddWithValue("@userID", userID);
                    insertCommand.Parameters.AddWithValue("@logtime", DateTime.Now);
                    insertCommand.Parameters.AddWithValue("@result", loginResult ? 1 : 0);
                    // 接続を開始します。
                    connection.Open();
                    insertCommand.ExecuteNonQuery();
                }
            }
        }

    }
}
