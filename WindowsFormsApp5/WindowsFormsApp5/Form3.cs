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
using WindowsFormsApp5.Constant;


namespace WindowsFormsApp5
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            #region 値の取得と入力チェック
            string userID = textBox1.Text;
            string userPassword = textBox2.Text;

            if (!GetIDPass.CheckIdPass(userID, userPassword))
            {
                MessageBox.Show("入力してください");
                return;
            }
            #endregion

            #region IDの存在チェック
            //ID存在チェック　ここから
            try
            {


                int checkId = Userservice.CheckID(userID);
                if (checkId == 0)
                {
                    MessageBox.Show("ユーザーが存在しません");
                    return;
                }
                //ID存在チェック　ここまで
                #endregion
                // IDとPWDの紐づきのデータ受け取り　ここから
                int checkIdPwd = Userservice.CheckIdPwd(userID, userPassword);
                // IDとPWDの紐づきのデータ受け取り　ここまで

                //入力チェックここから
                if (checkIdPwd == 0)
                {
                    MessageBox.Show("パスワードが間違っています");
                    Historyservise.InsertLoginHistory(userID, false);
                    return;
                }
                //入力チェックここまで


                //IDのヒストリー直近3件取得ここから
                List<DateTime> historyList = Historyservise.GetHistoryList(userID);
                //IDのヒストリー直近3件取得ここまで

                // ロックアウトの判断
                TimeSpan remainingLockout = TimeSpan.FromMinutes(Constants.LockoutTime) - (DateTime.Now - historyList[0]);

                if (Historyservise.LockoutJudgement(historyList, userID))
                {
                    MessageBox.Show($"あと {remainingLockout.Minutes} 分 {remainingLockout.Seconds} 秒、ログインが禁止されています。");
                }
                else
                {
                    MessageBox.Show("ログイン成功");
                    Historyservise.InsertLoginHistory(userID, true);
                }

            }
            catch(Exception ex)
            {
                MessageBox.Show("エラーが発生しました" + ex.Message);
            }
        }
    }
    
}
