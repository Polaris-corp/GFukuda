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

        /// <summary>
        /// login_historyから、直近三回の失敗logを保存するリストを作成するメソッドです。
        /// </summary>
        /// <param name="userID">userIDが入ります</param>
        /// <returns>作成したリスト</returns>
        public static List<DateTime> GetHistoryList(string userID)
        {
            List<DateTime> failedLogins = new List<DateTime>();// 失敗したログイン試行の時間を保存するリスト

            string connectionString = "Server=localhost;UID=pol05;Password=pol05;Database=test";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
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

        /// <summary>
        /// ロックアウトになるか判断するメソッドです。
        /// </summary>
        /// <param name="historyList">直近三回の失敗logが入ったリストです。</param>
        /// <returns>trueの場合、ロックアウト：falseの場合、ログイン成功。</returns>
        public static bool LockoutJudgement(List<DateTime> historyList,string userID)
        {
            int designationMinutes = 3;

            //getHistoryList[0]が直近のミス、getHistoryList[2]が最初のミスなので負の値にはならない
            if (historyList.Count == designationMinutes && (historyList[0] - historyList[2]).TotalMinutes <= designationMinutes)// 直近3回の試行が3分以内に失敗していた場合
            {

                TimeSpan remainingLockout = TimeSpan.FromMinutes(designationMinutes) - (DateTime.Now - historyList[0]);// 残りのロックアウト時間を計算
                TimeSpan nowFailed = (DateTime.Now - historyList[0]);

                if (nowFailed.Minutes <= designationMinutes)
                {
                    MessageBox.Show($"あと {remainingLockout.Minutes} 分 {remainingLockout.Seconds} 秒、ログインが禁止されています。");
                    return true; // 保留

                }
                else
                {
                    MessageBox.Show("ログイン成功");
                    InsertLoginHistory(userID, true);
                    return false;//保留
                }

                //IDのヒストリー直近3件取得ここまで
            }
            else
            {
                MessageBox.Show("ログイン成功");
                InsertLoginHistory(userID, true);
                return false; //保留
            }
        }

        /// <summary>
        /// ログイン試行のログをlogin_historyテーブルに残すメソッド
        /// </summary>
        /// <param name="userID">userIDがはいります。</param>
        /// <param name="loginResult">loginResultがはいります</param>
        public static void InsertLoginHistory(string userID, bool loginResult)
        {
            string connectionString = "Server=localhost;UID=pol05;Password=pol05;Database=test";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string insertQuery = @"
                       INSERT 
                       INTO 
                            login_history 
                            (
                            userID
                            , logtime, result
                            ) 
                       VALUES 
                            (
                            @userID
                            , @logtime
                            , @result
                            )";
                using (MySqlCommand insertCommand = new MySqlCommand(insertQuery, connection))
                {
                    try
                    {
                        insertCommand.Parameters.AddWithValue("@userID", userID);
                        insertCommand.Parameters.AddWithValue("@logtime", DateTime.Now);
                        insertCommand.Parameters.AddWithValue("@result", loginResult ? 1 : 0);

                        connection.Open();  // 接続を開始します。
                        insertCommand.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("エラー: " + ex.Message);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }

    }

}
