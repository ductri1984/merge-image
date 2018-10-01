using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APIBusiness.API
{
    public class HelperExcel
    {
        private static List<string> _excelColumn = new List<string> {
            "","A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z",
            "AA", "AB", "AC", "AD", "AE", "AF", "AG", "AH", "AI", "AJ", "AK", "AL", "AM", "AN", "AO", "AP", "AQ", "AR", "AS", "AT", "AU", "AV", "AW", "AX", "AY", "AZ",
            "BA", "BB", "BC", "BD", "BE", "BF", "BG", "BH", "BI", "BJ", "BK", "BL", "BM", "BN", "BO", "BP", "BQ", "BR", "BS", "BT", "BU", "BV", "BW", "BX", "BY", "BZ",
            "CA", "CB", "CC", "CD", "CE", "CF", "CG", "CH", "CI", "CJ", "CK", "CL", "CM", "CN", "CO", "CP", "CQ", "CR", "CS", "CT", "CU", "CV", "CW", "CX", "CY", "CZ"};

        public static int GetColumnFromByRange(string range)
        {
            string[] strs = range.Split(':');
            return GetColumnByCell(strs[0]);
        }

        public static int GetRowFromByRange(string range)
        {
            string[] strs = range.Split(':');
            return GetRowByCell(strs[0]);
        }

        public static int GetColumnToByRange(string range)
        {
            string[] strs = range.Split(':');
            return GetColumnByCell(strs[1]);
        }

        public static int GetRowToByRange(string range)
        {
            string[] strs = range.Split(':');
            return GetRowByCell(strs[1]);
        }

        public static string GetRangeName(int rowfrom, int columnfrom, int rowto, int columnto)
        {
            if (rowfrom > 0 && columnfrom > 0 && rowto > 0 && columnto > 0)
                return GetCellName(rowfrom, columnfrom) + ":" + GetCellName(rowto, columnto);
            else
                return string.Empty;
        }

        public static int GetColumnByCell(string cell)
        {
            if (!string.IsNullOrEmpty(cell))
            {
                int index = cell.IndexOfAny("0123456789".ToCharArray());
                string str = cell.Substring(0, index);
                return _excelColumn.IndexOf(str);
            }
            else
                return -1;
        }

        public static int GetRowByCell(string cell)
        {
            if (!string.IsNullOrEmpty(cell))
            {
                int index = cell.IndexOfAny("0123456789".ToCharArray());
                string str = cell.Substring(index);
                return Convert.ToInt32(str);
            }
            else
                return -1;
        }

        public static string GetCellName(int row, int column)
        {
            if (column > 0 && row > 0)
                return _excelColumn[column] + row;
            else
                return string.Empty;
        }

        public static OfficeOpenXml.ExcelWorksheet GetWorksheetByIndex(OfficeOpenXml.ExcelPackage package, int index)
        {
            try
            {
                OfficeOpenXml.ExcelWorkbook workBook = package.Workbook;
                if (workBook != null)
                    if (workBook.Worksheets.Count > 0)
                        return workBook.Worksheets[index];
            }
            catch { }
            return null;
        }

        public static OfficeOpenXml.ExcelWorksheet GetWorksheetFirst(OfficeOpenXml.ExcelPackage package)
        {
            try
            {
                OfficeOpenXml.ExcelWorkbook workBook = package.Workbook;
                if (workBook != null)
                    if (workBook.Worksheets.Count > 0)
                        return workBook.Worksheets.FirstOrDefault();
            }
            catch { }
            return null;
        }

        public static OfficeOpenXml.ExcelWorksheet GetWorksheetByName(OfficeOpenXml.ExcelPackage package, string name)
        {
            try
            {
                OfficeOpenXml.ExcelWorkbook workBook = package.Workbook;
                if (workBook != null)
                    if (workBook.Worksheets.Count > 0)
                        return workBook.Worksheets[name];
            }
            catch { }
            return null;
        }

        public static DateTime ValueToDate(string value)
        {
            return DateTime.FromOADate(Convert.ToDouble(value));
        }

        public static DateTime ValueToDate(double value)
        {
            return DateTime.FromOADate(Convert.ToDouble(value));
        }

        public static string GetValue(OfficeOpenXml.ExcelWorksheet worksheet, int row, int col)
        {
            if (worksheet.Cells[row, col].Value != null)
            {
                var value = worksheet.Cells[row, col].Value;
                Type t = value.GetType();
                if (t.Equals(typeof(DateTime)))
                {
                    DateTime date = (DateTime)value;
                    var strDate = date.Day + "/" + date.Month + "/" + date.Year + " " + date.Hour + ":" + date.Minute;
                    return strDate;
                }
                else
                    return worksheet.Cells[row, col].Value.ToString().Trim();
            }
            return string.Empty;
        }
    }
}