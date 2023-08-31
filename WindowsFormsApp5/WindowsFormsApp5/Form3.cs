﻿using MySql.Data.MySqlClient;
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
                MessageBox.Show(Constants.NonIdPwd);
                return;
            }
            #endregion


            try
            {
                #region IDの存在チェック
                //ID存在チェック　ここから

                int checkId = Userservice.CheckID(userID);
                if (checkId == 0)
                {
                    MessageBox.Show(Constants.NonUserId);
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
                    MessageBox.Show(Constants.WrongPwd);
                    Historyservise.InsertLoginHistory(userID, false);
                    return;
                }
                //入力チェックここまで


                //IDのヒストリー直近3件取得ここから
                List<DateTime> historyList = Historyservise.GetHistoryList(userID);
                //IDのヒストリー直近3件取得ここまで


                // ロックアウトの判断
                if (Historyservise.LockoutJudgement(historyList))
                {
                    MessageBox.Show(Constants.Success);
                    Historyservise.InsertLoginHistory(userID, true); 
                }
                else
                {
                    TimeSpan remainingLockout = TimeSpan.FromMinutes(Constants.LockoutTime) - (DateTime.Now - historyList[0]);
                    string lockoutMessage = string.Format(Constants.LoginBlock, remainingLockout.Minutes, remainingLockout.Seconds);
                    MessageBox.Show(lockoutMessage);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(Constants.ErrorOccurred + ex.Message);
            }
        }
    }
    
}
