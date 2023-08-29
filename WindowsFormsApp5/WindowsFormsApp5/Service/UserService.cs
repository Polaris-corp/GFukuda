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
using WindowsFormsApp5.Constant;


namespace WindowsFormsApp5.userservice
{
    public class Userservice
    {
        /// <summary>
        /// 入力されたuserIDがusersテーブルに存在するか確認するメソッド
        /// </summary>
        /// <param name="userID">userIDがはいります。</param>
        /// <returns>入力されたIDの存在個数</returns>
        public static int CheckID(string userID)
        {
            int idCount = 0;
            //const string connectionString = "Server=localhost;UID=pol05;Password=pol05;Database=test";
            using (MySqlConnection connection = new MySqlConnection(Constant.Constant.ConnectionString))
            {
                string idQuery = @"
                        SELECT
                            COUNT(*) 
                        FROM    
                            users 
                        WHERE 
                            ID = @userID";
                using (MySqlCommand idCommand = new MySqlCommand(idQuery, connection))
                {
                    idCommand.Parameters.AddWithValue("@userID", userID);

                    //接続開始
                    connection.Open();

                    idCount = Convert.ToInt32(idCommand.ExecuteScalar());
                    return idCount;
                }
            }
        }

        /// <summary>
        /// 入力されたuserPasswordがuserIDと紐づいているか確認するメソッド
        /// </summary>
        /// <param name="userID">userIDがはいります。</param>
        /// <param name="userPassword">userPasswordがはいります。</param>
        /// <returns>userIDと紐づいているuserPasswordの存在個数</returns>
        public static int CheckIdPwd(string userID, string userPassword)
        {
            int pwdCount = 0;
            using (MySqlConnection connection = new MySqlConnection(Constant.Constant.ConnectionString))
            {
                string pwdQuery = @"
                        SELECT 
                            COUNT(*) 
                        FROM 
                            users 
                        WHERE 
                            ID = @userID 
                            AND PWD = @userPassword";
                using (MySqlCommand pwdCommand = new MySqlCommand(pwdQuery, connection))
                {
                    pwdCommand.Parameters.AddWithValue("@userID", userID);
                    pwdCommand.Parameters.AddWithValue("@userpassword", userPassword);
                    //接続開始
                    connection.Open();

                    pwdCount = Convert.ToInt32(pwdCommand.ExecuteScalar());
                    return pwdCount;
                }
                
            }
        }
    }
}
