using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Finman.Models
{
    public class RetirementForm
    {
        public DateTime RetirementTime { get; set; }
        public int MonthlyExpenses { get; set; }
        public int ExpectedPeriod { get; set; }
    }
}