﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessBackground
{
    public class DTOAPIData
    {
        public string FileName { get; set; }
        public string FileUpload { get; set; }
        public string SpreadsheetID { get; set; }
        public string SpreadsheetName { get; set; }
        public List<string> ListRowTitle { get; set; }
        public List<string> ListColumnTitle { get; set; }
        public List<DTOAPIDataCell> ListCells { get; set; }
    }

    public class DTOAPIDataCell
    {
        public int Row { get; set; }
        public int Column { get; set; }
        public string ValueFrom { get; set; }
        public string ValueTo { get; set; }
    }
}
