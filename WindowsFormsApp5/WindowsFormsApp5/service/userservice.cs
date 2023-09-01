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
        /// <param name="userID">userID</param>
        /// <returns>入力されたIDのデータ個数</returns>
        public static bool CheckID(string userID)
        {
            using (MySqlConnection connection = new MySqlConnection(Constants.ConnectionString))
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
                   return Convert.ToInt32(idCommand.ExecuteScalar()) == 0;
                }
            }
        }

        /// <summary>
        /// 入力されたuserPasswordと入力されたuserIDが紐づいているデータの個数を取得するメソッド
        /// </summary>
        /// <param name="userID">userID</param>
        /// <param name="userPassword">userPassword</param>
        /// <returns>userIDと紐づいているuserPasswordのデータ個数</returns>
        public static bool CheckIdPwd(string userID, string userPassword)
        {
            using (MySqlConnection connection = new MySqlConnection(Constants.ConnectionString))
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
                    return Convert.ToInt32(pwdCommand.ExecuteScalar()) == 0;
                }
            }
        }

        /// <summary>
        /// 入力チェックを行う
        /// </summary>
        /// <param name="userID">userIDが入ります</param>
        /// <param name="userpassword">userpasswordが入ります</param>
        /// <returns>true:どちらか(もしくは両方)の入力がない　false:どちらも入力がある</returns>
        public static bool CheckIdPass(string userID, string userPassword)
        {
            if (string.IsNullOrEmpty(userID) || string.IsNullOrEmpty(userPassword))
            {
                return false;
            }
            return true;
        }
    }
}
