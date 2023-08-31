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
        /// login_historyから、直近三回のログイン失敗時のログを降順で最大3件保存するリストを作成するメソッド
        /// </summary>
        /// <param name="userID">userID</param>
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
        /// historyListが3件あるか
        /// 直近のログイン失敗から3分以内に連続3回のログイン失敗をしているか
        /// 直近のログイン失敗から3分経過しているか
        /// を判定するメソッド
        /// </summary>
        /// <param name="historyList">直近三回の失敗ログが入ったリスト</param>
        /// <returns>trueの場合、ログイン成功：falseの場合、ロックアウト</returns>
        public static bool LockoutJudgement(List<DateTime> historyList, DateTime clickTime)
        {
            if (historyList.Count < Constants.ListElementsCount)
            {
                return true;
            }

            TimeSpan missTimeDifference = historyList[0] - historyList[2];
            if (Constants.LockoutTime < missTimeDifference.TotalMinutes)
            {
                return true;
            }

            TimeSpan nowFailed = (clickTime - historyList[0]);
            if (Constants.LockoutTime < nowFailed.TotalMinutes)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// ログイン試行のログをlogin_historyテーブルにインサートするメソッド
        /// </summary>
        /// <param name="userID">userID</param>
        /// <param name="loginResult">loginResult</param>
        public static void InsertLoginHistory(string userID, bool loginResult,DateTime clickTime)
        {
            using (MySqlConnection connection = new MySqlConnection(Constants.ConnectionString))
            {
                string insertQuery = @"
                       INSERT INTO 
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
                    insertCommand.Parameters.AddWithValue("@logtime", clickTime);
                    insertCommand.Parameters.AddWithValue("@result", loginResult ? 1 : 0);
                    // 接続を開始します。
                    connection.Open();
                    insertCommand.ExecuteNonQuery();
                }
            }
        }

    }
}
