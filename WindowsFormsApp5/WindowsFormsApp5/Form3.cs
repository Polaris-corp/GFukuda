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
using WindowsFormsApp5.service;
using WindowsFormsApp5.userservice;


namespace WindowsFormsApp5
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }
        string connectionString = "Server=localhost;UID=pol05;Password=pol05;Database=test";
        Userservice Check_ID_Instance = new Userservice();
        private void button1_Click(object sender, EventArgs e)
        {
            #region 値の取得と入力チェック
            string userID = textBox1.Text;
            string userpassword = textBox2.Text;

            if (!Get_ID_Pass.Check_Id_Pass(userID, userpassword))
            {
                MessageBox.Show("入力してください");
                return;
            }
            #endregion

            #region IDの存在チェック
            //ID存在チェック　ここから
            int check_id = Userservice.Check_ID(userID);
            if (check_id == 0)
            {
                MessageBox.Show("ユーザーが存在しません");
                return;
            }
            //ID存在チェック　ここまで
            #endregion
            // IDとPWDの紐づきのデータ受け取り　ここから
            int check_id_pwd = Userservice.Check_ID_PWD(userID, userpassword);
            // IDとPWDの紐づきのデータ受け取り　ここまで

            //入力チェックここから
            if (check_id_pwd == 0)
            {
                MessageBox.Show("パスワードが間違っています");
                InsertLoginHistory(userID, false);
                return;
            }
            //入力チェックここまで


            //IDのヒストリー直近3件取得ここから
            List<DateTime> get_history_list = Historyservise.Get_History_List(userID);
            //IDのヒストリー直近3件取得ここまで



            if (get_history_list.Count == 3 && (get_history_list[0] - get_history_list[2]).TotalMinutes <= 3)// 直近3回の試行が3分以内に失敗していた場合
            {

                TimeSpan remainingLockout = TimeSpan.FromMinutes(3) - (DateTime.Now - get_history_list[0]);// 残りのロックアウト時間を計算
                TimeSpan NowFailed = (DateTime.Now - get_history_list[0]);

                if (NowFailed.Minutes <= 3)//保留
                {
                    MessageBox.Show($"あと {remainingLockout.Minutes} 分 {remainingLockout.Seconds} 秒、ログインが禁止されています。");
                    return;

                }
                else
                {
                    MessageBox.Show("ログイン成功");
                    InsertLoginHistory(userID, true);
                }

                //IDのヒストリー直近3件取得ここまで
            }
            else
            {
                MessageBox.Show("ログイン成功");
                InsertLoginHistory(userID, true);
            }

        }

        private void InsertLoginHistory(string userID, bool loginResult)
        {
            string connectionString = "Server=localhost;UID=pol05;Password=pol05;Database=test";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string insertQuery = "INSERT INTO login_history (userID, logtime, result) VALUES (@userID, @logtime, @result)";
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
