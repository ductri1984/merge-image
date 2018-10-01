using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessBackground
{
    public class DTOAPIPush
    {
        public string FileName { get; set; }
        public string SpreadsheetID { get; set; }
        public string SpreadsheetName { get; set; }
        public string FormatPushTitle { get; set; }
        public string FormatPushBody { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string Action { get; set; }
    }
}
