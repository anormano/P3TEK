using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P3TEK.Module.BusinessObjects.General
{
    public class Enums
    {
        public enum LedgerStatus
        {
            Open=0,
            Closed=1
        }
        public enum AccountType
        {
            Asset,
            Liability,
            Equity,
            Income,
            Expense
        }

        public enum MonthName
        {
            January=1,
            February=2,
            March=3,
            April=4,
            May=5,
            June=6,
            July=7,
            August=8,
            September=9,
            October=10,
            November=11,
            December=12
        }
    }
}
