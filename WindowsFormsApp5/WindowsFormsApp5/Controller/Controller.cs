using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp5.Controller
{
    public class Controller
    {
        public static TimeSpan CheckLockOut(List<DateTime> historyList)
        {
            int designationMinutes = 3;
            if (historyList.Count == designationMinutes && (historyList[0] - historyList[2]).TotalMinutes <= designationMinutes)
            {
                TimeSpan remainingLockout = TimeSpan.FromMinutes(designationMinutes) - (DateTime.Now - historyList[0]);
                TimeSpan nowFailed = (DateTime.Now - historyList[0]);
            }
        }

    }

}
