using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace BusinessBackground
{
    public class Business
    {
        public static DTOData Data = default(DTOData);
        private static string FolderPath = string.Empty;

        public static void SetFolderPath(string str)
        {
            FolderPath = str;
        }

        public static void ReadFile(string path)
        {
            if (System.IO.File.Exists(path))
            {
                string str = System.IO.File.ReadAllText(path);
                if (!string.IsNullOrEmpty(str))
                {
                    try
                    {
                        Data = Newtonsoft.Json.JsonConvert.DeserializeObject<DTOData>(str);
                        if (string.IsNullOrEmpty(FolderPath))
                            FolderPath = System.IO.Path.GetDirectoryName(str);
                    }
                    catch (Exception ex)
                    {
                        Data = default(DTOData);
                        throw ex;
                    }
                }
            }
        }

        public static void RunData(Action<string> actionlog, bool runtest = false)
        {
            if (Data != null)
            {
                if ((!Data.IsStop || runtest) && Data.ListDetails != null && Data.ListDetails.Count > 0)
                {
                    foreach (var item in Data.ListDetails)
                    {
                        if (item.IsFile)
                        {

                        }
                        else
                        {
                            if (System.IO.Directory.Exists(item.UploadFolder))
                            {
                                string[] files = System.IO.Directory.GetFiles(item.UploadFolder);
                                if (files.Length > 0)
                                {
                                    string filepath = files[0];
                                    string filecurrent = System.IO.Path.Combine(FolderPath, item.FileName + System.IO.Path.GetExtension(filepath));
                                    if (System.IO.File.Exists(filecurrent))
                                    {
                                        var itemCurrent = new DTOAPIData
                                        {
                                            FileName = item.FileName,
                                            ListColumnTitle = new List<string>(),
                                            ListRowTitle = new List<string>(),
                                            ListCells = new List<DTOAPIDataCell>()
                                        };

                                        using (var package = new OfficeOpenXml.ExcelPackage(new System.IO.FileInfo(filecurrent)))
                                        {
                                            var worksheet = HelperExcel.GetWorksheetFirst(package);
                                            #region current
                                            if (worksheet != null && worksheet.Dimension != null)
                                            {
                                                int row = 1, col = 1;
                                                while (itemCurrent.ListColumnTitle.Count <= item.ColumnValueStart)
                                                {
                                                    itemCurrent.ListColumnTitle.Add(string.Empty);
                                                }
                                                while (itemCurrent.ListRowTitle.Count <= item.RowValueStart)
                                                {
                                                    itemCurrent.ListRowTitle.Add(string.Empty);
                                                }

                                                for (row = item.RowValueStart; row <= item.RowValueEnd && row <= worksheet.Dimension.End.Row; row++)
                                                {
                                                    var strTitle = HelperExcel.GetValue(worksheet, row, item.ColumnTitle);
                                                    itemCurrent.ListRowTitle.Add(strTitle);
                                                }

                                                for (col = item.ColumnValueStart; col <= item.ColumnValueEnd && col <= worksheet.Dimension.End.Column; col++)
                                                {
                                                    var strTitle = HelperExcel.GetValue(worksheet, item.RowTitle, col);
                                                    itemCurrent.ListColumnTitle.Add(strTitle);
                                                }

                                                for (row = item.RowValueStart; row <= item.RowValueEnd && row <= worksheet.Dimension.End.Row; row++)
                                                {
                                                    for (col = item.ColumnValueStart; col <= item.ColumnValueEnd && col <= worksheet.Dimension.End.Column; col++)
                                                    {
                                                        var strValue = HelperExcel.GetValue(worksheet, row, col);
                                                        itemCurrent.ListCells.Add(new DTOAPIDataCell
                                                        {
                                                            Row = row,
                                                            Column = col,
                                                            ValueFrom = strValue,
                                                            ValueTo = strValue
                                                        });
                                                    }
                                                }
                                            }
                                            #endregion
                                        }

                                        using (var package = new OfficeOpenXml.ExcelPackage(new System.IO.FileInfo(filepath)))
                                        {
                                            var worksheet = HelperExcel.GetWorksheetFirst(package);
                                            #region next
                                            if (worksheet != null && worksheet.Dimension != null)
                                            {
                                                foreach (var itemCell in itemCurrent.ListCells)
                                                {
                                                    if (itemCell.Row <= worksheet.Dimension.End.Row && itemCell.Column <= worksheet.Dimension.End.Column)
                                                    {
                                                        var strValue = HelperExcel.GetValue(worksheet, itemCell.Row, itemCell.Column);
                                                        if (itemCell.ValueTo != strValue)
                                                            itemCell.ValueTo = strValue;
                                                    }
                                                }
                                            }
                                            #endregion
                                        }

                                        itemCurrent.ListCells = itemCurrent.ListCells.Where(c => c.ValueTo != c.ValueFrom).ToList();
                                        if (itemCurrent.ListCells.Count > 0)
                                        {
                                            System.IO.File.Delete(filecurrent);
                                            System.IO.File.Copy(filepath, filecurrent);
                                            itemCurrent.FileName = SendFile(item, item.FileName, filecurrent);
                                            SendAPIData(item, itemCurrent);
                                            SendAPIPush(item, itemCurrent);
                                        }
                                    }
                                    else
                                    {
                                        System.IO.File.Copy(filepath, filecurrent);
                                        //SendFile(item, item.FileName, filecurrent);
                                    }
                                    //if (System.IO.File.Exists(filepath))
                                    //    System.IO.File.Delete(filepath);
                                }
                            }
                            else
                                actionlog("not found folder " + item.UploadFolder);
                        }
                    }
                }
                else
                    actionlog(string.Format("IsStop: {0}, ListDetails: {1}", Data.IsStop, Data.ListDetails == null ? 0 : Data.ListDetails.Count));
            }
            else
                actionlog("not found data");
        }

        private static string SendFile(DTODataDetail item, string filename, string filecurrent)
        {
            using (var client = new HttpClient())
            {
                Uri url = new Uri(item.HandlerLink);

                client.BaseAddress = new Uri(url.Scheme + "://" + url.Authority);
                client.DefaultRequestHeaders.Accept.Clear();

                client.Timeout = TimeSpan.FromHours(0.1);
                var content = new MultipartFormDataContent();
                System.IO.Stream stream = new System.IO.FileStream(filecurrent, System.IO.FileMode.Open);
                content.Add(new StreamContent(stream), filename, System.IO.Path.GetFileName(filecurrent));
                var response = client.PostAsync(url.AbsolutePath, content);
                if (response != null && response.Result.IsSuccessStatusCode)
                {
                    HttpResponseMessage res = response.Result;
                    string str = res.Content.ReadAsStringAsync().Result;
                    return str;
                }
                return string.Empty;
            }
        }

        private static void SendAPIData(DTODataDetail item, DTOAPIData data)
        {
            using (var client = new HttpClient())
            {
                Uri url = new Uri(item.LinkData);

                client.BaseAddress = new Uri(url.Scheme + "://" + url.Authority);
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.Timeout = TimeSpan.FromHours(0.1);
                var content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
                var response = client.PostAsync(url.AbsolutePath, content);
                if (response != null && response.Result.IsSuccessStatusCode)
                {
                    HttpResponseMessage res = response.Result;
                }
            }
        }

        private static void SendAPIPush(DTODataDetail item, DTOAPIData data)
        {
            using (var client = new HttpClient())
            {
                Uri url = new Uri(item.LinkPush);

                //client.BaseAddress = new Uri(url.Scheme + "://" + url.Authority);
                //client.DefaultRequestHeaders.Accept.Clear();

                //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //client.Timeout = TimeSpan.FromHours(0.1);
                //var content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
                //var response = client.PostAsync(url.AbsolutePath, content);
                //if (response != null && response.Result.IsSuccessStatusCode)
                //{
                //    HttpResponseMessage res = response.Result;
                //}
            }
        }
    }

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
