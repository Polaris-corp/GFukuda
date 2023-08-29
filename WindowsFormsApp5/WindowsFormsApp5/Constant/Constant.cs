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

        //LockoutJudgementで使用される定数
        public const int LockoutTime = 3;
        public const int ListElementsCount = 3;
    }
}
