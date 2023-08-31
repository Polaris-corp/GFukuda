using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApp5.userservice;

namespace WindowsFormsApp5.Constant
{
    public class Constants
    {
        //接続文字列
        public const string ConnectionString = "Server=localhost;UID=pol05;Password=pol05;Database=test";

        //指定された定数
        public const int LockoutTime = 3;
        public const int ListElementsCount = 3;

        //各種メッセージ文の定数
        public const string NonIdPwd = "入力してください";
        public const string NonUserId = "ユーザーが存在しません";
        public const string WrongPwd = "パスワードが間違っています";
        public const string Success = "ログイン成功";
        public const string ErrorOccurred = "エラーが発生しました";
        public const string LoginBlock =  "あと {0} 分 {1} 秒、ログインが禁止されています。";

    }
}
