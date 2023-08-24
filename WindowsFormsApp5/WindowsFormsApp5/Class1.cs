using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp5
{
    class Get_ID_Pass
    {
        /// <summary>
        /// 入力チェックを行う
        /// </summary>
        /// <param name="userID">userIDが入ります</param>
        /// <param name="userpassword">userpasswordが入ります</param>
        /// <returns>true:どちらか(もしくは両方)の入力がない　false:どちらも入力がある</returns>
        public static bool Check_Id_Pass(string userID, string userpassword)
        {
            if (string.IsNullOrEmpty(userID) || string.IsNullOrEmpty(userpassword))
            {
              return false;
            }
            return true; 
        }
    }
}
