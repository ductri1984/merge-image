using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessBackground
{
    public class DTODataDetail
    {
        public string ID { get; set; }

        public string FileName { get; set; }
        public string UploadFolder { get; set; }
        public bool IsFile { get; set; }
        public string HandlerLink { get; set; }
        public string HandlerKey { get; set; }
        public int ColumnValueStart { get; set; }
        public int ColumnValueEnd { get; set; }
        public int RowValueStart { get; set; }
        public int RowValueEnd { get; set; }
        public int ColumnTitle { get; set; }
        public int RowTitle { get; set; }
        public string LinkData { get; set; }
        public string LinkPush { get; set; }
        public string FormatPushTitle { get; set; }
        public string FormatPushBody { get; set; }
    }
}
