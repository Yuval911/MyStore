using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyStore
{
    public class LogRecord
    {
        public int ID { get; set; }
        public DateTime Date { get; set; }
        public string Action { get; set; }
        public string Succeeded { get; set; }
        public string FailCause { get; set; }

        public LogRecord()
        {

        }

        public LogRecord(DateTime date, string action, string succeeded, string fail_Cause)
        {
            Date = date;
            Action = action;
            Succeeded = succeeded;
            FailCause = fail_Cause;
        }
    }
}
