using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;

namespace WindowsFormsApp5
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

        }

        private void button1_Click(object sender, EventArgs e)
        {

            //Form2 を表示
            Form2 f2 = new Form2(textBox1.Text);
            f2.ShowDialog();


        }

        private void button2_Click(object sender, EventArgs e)
        {
            string txt = textBox1.Text;
            // 接続文字列
            var connectionString = "Server=localhost;UID=pol05;Password=pol05;Database=test";

            // 実行するSQL
            var sql = "SELECT Name FROM testtable WHERE ID =" + txt; 

           

            using (var connection = new MySqlConnection(connectionString))
            {
                // 接続の確立

                connection.Open();

                using (var command = new MySqlCommand(sql, connection))
                using (var reader = command.ExecuteReader())
                {
                    // SELECT文を実行し、結果を1行ずつコンソールに表示
                    while (reader.Read())
                    {
                        string name = reader["Name"].ToString();

                        if (String.IsNullOrEmpty(name))
                        {
                            MessageBox.Show("no name");
                        }
                        else
                        {
                            MessageBox.Show(name);
                        }
                        
                       
                    }
                }
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            string txt = textBox1.Text;
            // 接続文字列
            var connectionString = "Server=localhost;UID=pol05;Password=pol05;Database=test";

            // 実行するSQL
            var sql = @"SELECT
                            .Name AS Name
                        FROM
                            studytable AS U
                            INNER JOIN authoritytable AS K
                                ON K.ID = K.ID 
                        WHERE
                            U.ID = " + txt;


            using (var connection = new MySqlConnection(connectionString))
            {
                // 接続の確立
                connection.Open();

                using (var command = new MySqlCommand(sql, connection))
                using (var reader = command.ExecuteReader())
                {
                    // SELECT文を実行し、結果を1行ずつコンソールに表示
                    while (reader.Read())
                    {
                        
                        string name = reader["Name"].ToString();

                        if (String.IsNullOrEmpty(name))
                        {
                            MessageBox.Show("no name");
                        }
                        else
                        {
                            MessageBox.Show(name);
                        }


                    }
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string txt = textBox1.Text;
            // 接続文字列
            var connectionString = "Server=localhost;UID=pol05;Password=pol05;Database=test";

            // 実行するSQL
            var sql = @"SELECT
                            .Name AS Name
                        FROM
                            studytable AS U
                            INNER JOIN authoritytable AS K
                                ON K.ID = K.ID 
                        WHERE
                            U.ID = " + txt;


            using (var connection = new MySqlConnection(connectionString))
            {
                // 接続の確立
                connection.Open();

                using (var command = new MySqlCommand(sql, connection))
                using (var reader = command.ExecuteReader())
                {
                    // SELECT文を実行し、結果を1行ずつコンソールに表示
                    while (reader.Read())
                    {

                        string name = reader["Name"].ToString();

                        if (String.IsNullOrEmpty(name))
                        {
                            MessageBox.Show("no name");
                        }
                        else
                        {
                            MessageBox.Show(name);
                        }


                    }
                }
            }
        }
    }
}
//using MySql.Data.MySqlClient;
//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
//using System.Drawing;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Forms;

//namespace WindowsFormsApp5
//{
//    public partial class Form3 : Form
//    {
//        public Form3()
//        {
//            InitializeComponent();
//        }



//        string connectionString = "Server=localhost;UID=pol05;Password=pol05;Database=test";

//        private void button1_Click(object sender, EventArgs e)
//        {

//            #region 値の取得と入力チェック
//            //IDとPWDを受け取る　ここから
//            string userID = textBox1.Text;
//            string userpassword = textBox2.Text;
//            //IDとPWDを受け取る　ここまで

//            //入力チェック　ここから
//            if (string.IsNullOrEmpty(userID) || string.IsNullOrEmpty(userpassword))
//            {
//                MessageBox.Show("入力してください");
//                return;
//            }
//            // 入力チェック　ここまで
//            #endregion

//            #region IDの存在チェック
//            int idCount = 0;
//            // IDがあるか値の受け取り　ここから
//            using (MySqlConnection connection = new MySqlConnection(connectionString))
//            {
//                try
//                {
//                    string idQuery = "SELECT COUNT(*) FROM users WHERE ID = @userID";
//                    using (MySqlCommand idCommand = new MySqlCommand(idQuery, connection))
//                    {
//                        idCommand.Parameters.AddWithValue("@userID", userID);

//                        //接続開始
//                        connection.Open();

//                        idCount = Convert.ToInt32(idCommand.ExecuteScalar());
//                    }
//                }
//                catch (MySqlException ex)
//                {
//                    MessageBox.Show("データベースのエラー: " + ex.Message);
//                }
//                catch (Exception ex)
//                {
//                    MessageBox.Show("エラー: " + ex.Message);
//                }
//                finally
//                {
//                    //接続解除
//                    connection.Close();
//                }

//            }
//            // IDがあるか値の受け取り　ここまで

//            //ID存在チェック　ここから
//            if (idCount == 0)
//            {
//                MessageBox.Show("ユーザーが存在しません");
//                return;
//            }
//            //ID存在チェック　ここまで
//            #endregion

//            int pwdCount = 0;
//            // IDとPWDの紐づきのデータ受け取り　ここから
//            using (MySqlConnection connection = new MySqlConnection(connectionString))
//            {
//                try
//                {
//                    string pwdQuery = "SELECT COUNT(*) FROM users WHERE ID = @userID AND PWD = @userpassword";
//                    using (MySqlCommand pwdCommand = new MySqlCommand(pwdQuery, connection))
//                    {
//                        pwdCommand.Parameters.AddWithValue("@userID", userID);
//                        pwdCommand.Parameters.AddWithValue("@userpassword", userpassword);
//                        //接続開始
//                        connection.Open();

//                        pwdCount = Convert.ToInt32(pwdCommand.ExecuteScalar());

//                    }

//                }
//                catch (MySqlException ex)
//                {
//                    MessageBox.Show("データベースのエラー: " + ex.Message);
//                }
//                catch (Exception ex)
//                {
//                    MessageBox.Show("エラー: " + ex.Message);
//                }
//                finally
//                {
//                    //接続解除
//                    connection.Close();
//                }

//                // IDとPWDの紐づきのデータ受け取り　ここまで

//                //入力チェックここから
//                if (pwdCount == 0)
//                {
//                    MessageBox.Show("パスワードが間違っています");
//                    InsertLoginHistory(userID, false);
//                    return;

//                }
//                //入力チェックここまで

//            }

//            //IDのヒストリー直近3件取得ここから

//            List<DateTime> failedLogins = new List<DateTime>();// 失敗したログイン試行の時間を保存するリスト


//            using (MySqlConnection connection = new MySqlConnection(connectionString))
//            {
//                try
//                {
//                    // 直近の3回のログイン試行を確認　ORDER BY logtime DESC LIMIT 3で直近三回
//                    string historyQuery = "SELECT logtime, result FROM login_history WHERE userID = @userID ORDER BY logtime DESC LIMIT 3";
//                    using (MySqlCommand historyCommand = new MySqlCommand(historyQuery, connection))
//                    {

//                        historyCommand.Parameters.AddWithValue("@userID", userID);// 渡すパラメータ

//                        //接続開始
//                        connection.Open();

//                        var reader = historyCommand.ExecuteReader();// SQLクエリを実行

//                        while (reader.Read())// クエリの結果を読む
//                        {
//                            bool result = reader.GetInt32("result") == 1;
//                            DateTime logtime = reader.GetDateTime("logtime");// 失敗したログインの場合、リストに追加

//                            if (!result)
//                            {
//                                failedLogins.Add(logtime);
//                            }
//                        }
//                        reader.Close();
//                    }

//                }
//                catch (MySqlException ex)
//                {
//                    MessageBox.Show("データベースのエラー: " + ex.Message);
//                }
//                catch (Exception ex)
//                {
//                    MessageBox.Show("エラー: " + ex.Message);
//                }
//                finally
//                {
//                    //接続解除
//                    connection.Close();
//                }


//                if (failedLogins.Count == 3 && (failedLogins[0] - failedLogins[2]).TotalMinutes <= 3)// 直近3回の試行が3分以内に失敗していた場合
//                {


//                    TimeSpan remainingLockout = TimeSpan.FromMinutes(3) - (DateTime.Now - failedLogins[0]);// 残りのロックアウト時間を計算
//                    MessageBox.Show($"あと {remainingLockout.Minutes} 分 {remainingLockout.Seconds} 秒、ログインが禁止されています。");
//                    return;

//                    //IDのヒストリー直近3件取得ここまで
//                }
//                else
//                {
//                    MessageBox.Show("ログイン成功");
//                    InsertLoginHistory(userID, true);
//                }

//            }


//        }

//        private void InsertLoginHistory(string userID, bool loginResult)
//        {
//            using (MySqlConnection connection = new MySqlConnection(connectionString))
//            {
//                string insertQuery = "INSERT INTO login_history (userID, logtime, result) VALUES (@userID, @logtime, @result)";
//                using (MySqlCommand insertCommand = new MySqlCommand(insertQuery, connection))
//                {
//                    try
//                    {
//                        insertCommand.Parameters.AddWithValue("@userID", userID);
//                        insertCommand.Parameters.AddWithValue("@logtime", DateTime.Now);
//                        insertCommand.Parameters.AddWithValue("@result", loginResult ? 1 : 0);

//                        connection.Open();  // 接続を開始します。
//                        insertCommand.ExecuteNonQuery();
//                    }
//                    catch (Exception ex)
//                    {
//                        MessageBox.Show("エラー: " + ex.Message);
//                    }
//                    finally
//                    {
//                        connection.Close();
//                    }
//                }
//            }
//        }


//    }
//}
