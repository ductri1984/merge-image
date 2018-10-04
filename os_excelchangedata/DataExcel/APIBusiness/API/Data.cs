using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Http;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;

namespace APIBusiness.API
{
    //[EnableCors(origins: "*", headers: "*", methods: "*")]
    public class DataController : ApiController
    {
        const string FileData = "/Uploads/data.json";
        const string FileTopic = "/Uploads/topic.json";
        const string FilePush = "/Uploads/push.json";
        const string FilePushData = "/Uploads/pushdata.json";
        const string FileClientId = "/Uploads/client_id.json";
        const string FileToken = "/Uploads/token";
        const int RowMax = 2900;
        const int ColMax = 52;

        [HttpPost]
        public void SendData(dynamic dynParam)
        {
            try
            {
                string strParam = dynParam != null ? dynParam.ToString() : string.Empty;
                if (!string.IsNullOrEmpty(strParam))
                {
                    var dto = Newtonsoft.Json.JsonConvert.DeserializeObject<DTOAPIData>(strParam);
                    string filepath = HttpContext.Current.Server.MapPath(FileData);
                    if (!System.IO.File.Exists(filepath))
                        System.IO.File.Create(filepath);

                    List<DTOAPIData> lst = new List<DTOAPIData>();
                    string str = string.Empty;
                    if (System.IO.File.Exists(filepath))
                    {
                        str = System.IO.File.ReadAllText(filepath);
                        if (!string.IsNullOrEmpty(str))
                        {
                            try
                            {
                                lst = Newtonsoft.Json.JsonConvert.DeserializeObject<List<DTOAPIData>>(str);
                            }
                            catch
                            {
                                lst = new List<DTOAPIData>();
                            }
                        }
                    }
                    dto.CreatedDate = DateTime.Now;
                    lst.Add(dto);
                    str = Newtonsoft.Json.JsonConvert.SerializeObject(lst);
                    System.IO.File.WriteAllText(filepath, str, System.Text.Encoding.UTF8);

                    //1Ln6hwAS31EII2XdlWN9PN0kaNEt4S3RIZewf81w-ePQ
                    try
                    {
                        SendData_ClearData(dto);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("fail clear " + ex.Message);
                    }
                    try
                    {
                        SendData_CreateData(dto);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("fail create " + ex.Message);
                    }

                    //UpdateData();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void SendData_ClearData(DTOAPIData item)
        {
            if (!string.IsNullOrEmpty(item.FileName) && !string.IsNullOrEmpty(item.SpreadsheetID) && !string.IsNullOrEmpty(item.SpreadsheetName))
            {
                string[] Scopes = { SheetsService.Scope.Spreadsheets }; //delete token folder to refresh scope

                UserCredential credential;

                using (var stream =
                    new System.IO.FileStream(HttpContext.Current.Server.MapPath(FileClientId), System.IO.FileMode.Open, System.IO.FileAccess.Read))
                {
                    string credPath = HttpContext.Current.Server.MapPath(FileToken);
                    credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                        GoogleClientSecrets.Load(stream).Secrets,
                        Scopes,
                        "user",
                        System.Threading.CancellationToken.None,
                        new FileDataStore(credPath, true)).Result;
                    //Console.WriteLine("Credential file saved to: " + credPath);
                }

                if (credential != null)
                {
                    // Create Google Sheets API service.
                    var service = new SheetsService(new BaseClientService.Initializer()
                    {
                        HttpClientInitializer = credential,
                        ApplicationName = "Google Sheets API",
                    });

                    //SheetsService sheetsService = new SheetsService(new BaseClientService.Initializer
                    //{
                    //    HttpClientInitializer = credential,
                    //    ApplicationName = "Google-SheetsSample/0.1",
                    //});

                    string range = item.SpreadsheetName + "!A1:BA" + RowMax;  // update cell F5 
                    var requestBody = new ClearValuesRequest();

                    var deleteRequest = service.Spreadsheets.Values.Clear(requestBody, item.SpreadsheetID, range);
                    var deleteReponse = deleteRequest.Execute();
                }
                else
                    throw new Exception("Credential file fail");
            }


        }

        private void SendData_CreateData(DTOAPIData item)
        {
            if (!string.IsNullOrEmpty(item.FileUpload) && !string.IsNullOrEmpty(item.SpreadsheetID) && !string.IsNullOrEmpty(item.SpreadsheetName))
            {
                string fileupload = HttpContext.Current.Server.MapPath("/Uploads/" + item.FileUpload);
                if (System.IO.File.Exists(fileupload))
                {
                    ValueRange valueRange = new ValueRange();
                    valueRange.MajorDimension = "ROWS";//"ROWS";//COLUMNS                
                    valueRange.Values = new List<IList<object>>();

                    if (System.IO.Path.GetExtension(fileupload).ToLower() == ".csv")
                    {
                        #region csv
                        using (var textReader = new System.IO.StreamReader(fileupload))
                        {
                            int row, col;

                            var csv = new CsvHelper.CsvReader(textReader);
                            row = 0;
                            while (csv.Read())
                            {
                                if (row <= RowMax)
                                {
                                    var objrow = new List<object>();
                                    for (col = 0; col < ColMax; col++)
                                    {
                                        try
                                        {
                                            objrow.Add(csv[col]);
                                        }
                                        catch 
                                        {
                                            break;
                                        }                                        
                                    }
                                    valueRange.Values.Add(objrow);
                                }
                                else
                                    break;

                                row++;
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        #region excel
                        using (var package = new OfficeOpenXml.ExcelPackage(new System.IO.FileInfo(fileupload)))
                        {
                            var worksheet = HelperExcel.GetWorksheetFirst(package);
                            #region current
                            if (worksheet != null && worksheet.Dimension != null)
                            {
                                int row, col;

                                for (row = 1; row <= worksheet.Dimension.End.Row && row < RowMax; row++)
                                {
                                    var objrow = new List<object>();
                                    for (col = 1; col <= worksheet.Dimension.End.Column && col < ColMax; col++)
                                    {
                                        var str = HelperExcel.GetValue(worksheet, row, col);
                                        objrow.Add(str);
                                    }
                                    valueRange.Values.Add(objrow);
                                }
                            }
                            #endregion
                        }
                        #endregion
                    }

                    string[] Scopes = { SheetsService.Scope.Spreadsheets }; //delete token folder to refresh scope
                    UserCredential credential;
                    using (var stream =
                        new System.IO.FileStream(HttpContext.Current.Server.MapPath(FileClientId), System.IO.FileMode.Open, System.IO.FileAccess.Read))
                    {
                        string credPath = HttpContext.Current.Server.MapPath(FileToken);
                        credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                            GoogleClientSecrets.Load(stream).Secrets,
                            Scopes,
                            "user",
                            System.Threading.CancellationToken.None,
                            new FileDataStore(credPath, true)).Result;
                        //Console.WriteLine("Credential file saved to: " + credPath);
                    }

                    if (credential != null)
                    {
                        // Create Google Sheets API service.
                        var service = new SheetsService(new BaseClientService.Initializer()
                        {
                            HttpClientInitializer = credential,
                            ApplicationName = "Google Sheets API",
                        });

                        string range = item.SpreadsheetName + "!A1";

                        SpreadsheetsResource.ValuesResource.UpdateRequest update = service.Spreadsheets.Values.Update(valueRange, item.SpreadsheetID, range);
                        update.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.RAW;
                        var result = update.Execute();
                    }
                    else
                        throw new Exception("Credential file fail");
                }
            }
        }

        //private void UpdateData()
        //{
        //    string[] Scopes = { SheetsService.Scope.Spreadsheets }; //delete token folder to refresh scope

        //    UserCredential credential;

        //    using (var stream =
        //        new System.IO.FileStream(HttpContext.Current.Server.MapPath(FileClientId), System.IO.FileMode.Open, System.IO.FileAccess.Read))
        //    {
        //        string credPath = HttpContext.Current.Server.MapPath(FileToken);
        //        credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
        //            GoogleClientSecrets.Load(stream).Secrets,
        //            Scopes,
        //            "user",
        //            System.Threading.CancellationToken.None,
        //            new FileDataStore(credPath, true)).Result;
        //        Console.WriteLine("Credential file saved to: " + credPath);
        //    }

        //    // Create Google Sheets API service.
        //    var service = new SheetsService(new BaseClientService.Initializer()
        //    {
        //        HttpClientInitializer = credential,
        //        ApplicationName = "Google Sheets API",
        //    });

        //    if (credential != null)
        //    {
        //        SheetsService sheetsService = new SheetsService(new BaseClientService.Initializer
        //        {
        //            HttpClientInitializer = credential,
        //            ApplicationName = "Google-SheetsSample/0.1",
        //        });

        //        // The spreadsheet to request.
        //        string spreadsheetId = "1Ecm_3kKV4Wgz8BpZhPeMez5fy2pZuNSr_BNojTmOPwU";  // TODO: Update placeholder value.

        //        String range2 = "Info!F5";  // update cell F5 
        //        ValueRange valueRange = new ValueRange();
        //        valueRange.MajorDimension = "ROWS";//"ROWS";//COLUMNS

        //        var oblist = new List<object>() { "1", 2 };
        //        valueRange.Values = new List<IList<object>> { oblist };

        //        SpreadsheetsResource.ValuesResource.UpdateRequest update = service.Spreadsheets.Values.Update(valueRange, spreadsheetId, range2);
        //        update.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.RAW;
        //        UpdateValuesResponse result2 = update.Execute();

        //    }

        //    //Console.ReadLine();
        //}

        [HttpPost]
        public List<DTOGet> GetData(dynamic dynParam)
        {
            try
            {
                string filepath = HttpContext.Current.Server.MapPath(FileData);
                List<DTOAPIData> lst = new List<DTOAPIData>();
                string str = string.Empty;
                if (System.IO.File.Exists(filepath))
                {
                    str = System.IO.File.ReadAllText(filepath);
                    if (!string.IsNullOrEmpty(str))
                    {
                        try
                        {
                            lst = Newtonsoft.Json.JsonConvert.DeserializeObject<List<DTOAPIData>>(str);
                        }
                        catch
                        {
                            lst = new List<DTOAPIData>();
                        }
                    }
                }
                List<DTOGet> result = new List<DTOGet>();
                foreach (var item in lst.Where(c => c.CreatedDate != null).OrderByDescending(c => c.CreatedDate))
                {
                    var itemGet = new DTOGet
                    {
                        StringTime = item.CreatedDate.Value.ToString("HH:mm"),
                        FileName = item.FileName,
                        LinkFileName = "http://svn.3ps.vn:15002/Uploads/" + item.FileName,
                        Body = string.Empty
                    };
                    List<int> lstRowID = item.ListCells.Select(c => c.Row).Distinct().ToList();
                    foreach (var rowid in lstRowID)
                    {
                        itemGet.Body += "," + item.ListRowTitle[rowid] + ": ";
                        List<string> lstCell = new List<string>();
                        foreach (var cell in item.ListCells.Where(c => c.Row == rowid))
                        {
                            string title = item.ListColumnTitle[cell.Column];
                            itemGet.Body += string.Format("[{0}: {1}->{2}]", title, cell.ValueFrom, cell.ValueTo);
                        }
                    }
                    if (!string.IsNullOrEmpty(itemGet.Body))
                        itemGet.Body = itemGet.Body.Substring(1);
                    result.Add(itemGet);
                }
                return result;
                //return lst.Where(c => c.CreatedDate != null).OrderByDescending(c => c.CreatedDate).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public void SendPush(dynamic dynParam)
        {
            try
            {
                string strParam = dynParam != null ? dynParam.ToString() : string.Empty;
                if (!string.IsNullOrEmpty(strParam))
                {
                    var dto = Newtonsoft.Json.JsonConvert.DeserializeObject<DTOAPIPush>(strParam);
                    string filepath = HttpContext.Current.Server.MapPath(FilePush);

                    List<DTOAPIPush> lst = new List<DTOAPIPush>();
                    string str = string.Empty;
                    if (System.IO.File.Exists(filepath))
                    {
                        str = System.IO.File.ReadAllText(filepath);
                        if (!string.IsNullOrEmpty(str))
                        {
                            try
                            {
                                lst = Newtonsoft.Json.JsonConvert.DeserializeObject<List<DTOAPIPush>>(str);
                            }
                            catch
                            {
                                lst = new List<DTOAPIPush>();
                            }
                        }
                    }
                    lst.Add(dto);
                    str = Newtonsoft.Json.JsonConvert.SerializeObject(lst);
                    System.IO.File.WriteAllText(filepath, str, System.Text.Encoding.UTF8);

                    using (var client = new HttpClient())
                    {
                        Uri url = new Uri("https://fcm.googleapis.com/fcm/send");

                        client.BaseAddress = new Uri(url.Scheme + "://" + url.Authority);
                        client.DefaultRequestHeaders.Accept.Clear();

                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "key=AAAA5NRCJyY:APA91bHBfzDh6iupDq7CvIw9iaNSstO20flnBWzqwWCbMq_EvHt_fG5gIp4pUfmePz1KvuHMBGFuuYNXxYjrCcnvPWVi_pQ39h5IZuQWN8ikZulEgDgB3WCHuXwW7aQXTUdtiY3ZKXNH");
                        client.Timeout = TimeSpan.FromHours(0.1);
                        dynamic dynItem = new { to = "/topics/" + dto.FileName, notification = new { title = dto.Title, body = dto.Body, click_action = "" } };
                        var content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(dynItem), Encoding.UTF8, "application/json");
                        var response = client.PostAsync(url.AbsolutePath, content);
                        if (response != null && response.Result.IsSuccessStatusCode)
                        {
                            HttpResponseMessage res = response.Result;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public void SendPushByData(dynamic dynParam)
        {
            try
            {
                string strParam = dynParam != null ? dynParam.ToString() : string.Empty;
                if (!string.IsNullOrEmpty(strParam))
                {
                    var dto = Newtonsoft.Json.JsonConvert.DeserializeObject<DTOAPIPush>(strParam);
                    string filePushPath = HttpContext.Current.Server.MapPath(FilePushData);
                    string fileDataPath = HttpContext.Current.Server.MapPath(FileData);

                    if (!System.IO.File.Exists(filePushPath))
                        System.IO.File.Create(filePushPath);
                    if (!System.IO.File.Exists(fileDataPath))
                        System.IO.File.Create(fileDataPath);

                    if (System.IO.File.Exists(filePushPath) && System.IO.File.Exists(fileDataPath))
                    {
                        List<DTOAPIData> lstData = new List<DTOAPIData>();
                        List<DTOAPIPush> lstPush = new List<DTOAPIPush>();
                        string str = string.Empty;
                        str = System.IO.File.ReadAllText(fileDataPath);
                        if (!string.IsNullOrEmpty(str))
                        {
                            try
                            {
                                lstData = Newtonsoft.Json.JsonConvert.DeserializeObject<List<DTOAPIData>>(str);
                            }
                            catch
                            {
                                lstData = new List<DTOAPIData>();
                            }
                        }
                        DTOAPIData itemData = lstData.Where(c => c.FileName == dto.FileName && c.CreatedDate != null).OrderByDescending(c => c.CreatedDate.Value).FirstOrDefault();
                        if (itemData != null)
                        {
                            str = System.IO.File.ReadAllText(filePushPath);
                            if (!string.IsNullOrEmpty(str))
                            {
                                try
                                {
                                    lstPush = Newtonsoft.Json.JsonConvert.DeserializeObject<List<DTOAPIPush>>(str);
                                }
                                catch
                                {
                                    lstPush = new List<DTOAPIPush>();
                                }
                            }

                            List<int> lstRowID = itemData.ListCells.Select(c => c.Row).Distinct().ToList();
                            List<string> lstRowTitle = new List<string>();
                            Dictionary<int, List<string>> dicRowValue = new Dictionary<int, List<string>>();
                            foreach (var rowid in lstRowID)
                            {
                                lstRowTitle.Add(itemData.ListRowTitle[rowid]);
                                List<string> lstCell = new List<string>();
                                foreach (var cell in itemData.ListCells.Where(c => c.Row == rowid))
                                {
                                    string title = itemData.ListColumnTitle[cell.Column];
                                    lstCell.Add(string.Format("[{0}: {1}->{2}]", title, cell.ValueFrom, cell.ValueTo));
                                }
                                dicRowValue.Add(rowid, lstCell);
                            }
                            dto.Title = DateTime.Now.ToString("HH:mm") + " " + string.Join(",", lstRowTitle);
                            dto.Body = string.Empty;
                            foreach (var itemRow in dicRowValue)
                            {
                                string title = itemData.ListRowTitle[itemRow.Key];
                                dto.Body += "," + title + string.Join("", itemRow.Value);
                            }
                            if (!string.IsNullOrEmpty(dto.Body))
                                dto.Body = dto.Body.Substring(1);

                            lstPush.Add(dto);
                            str = Newtonsoft.Json.JsonConvert.SerializeObject(lstPush);
                            System.IO.File.WriteAllText(filePushPath, str, System.Text.Encoding.UTF8);

                            using (var client = new HttpClient())
                            {
                                Uri url = new Uri("https://fcm.googleapis.com/fcm/send");

                                client.BaseAddress = new Uri(url.Scheme + "://" + url.Authority);
                                client.DefaultRequestHeaders.Accept.Clear();

                                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                                client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "key=AAAA5NRCJyY:APA91bHBfzDh6iupDq7CvIw9iaNSstO20flnBWzqwWCbMq_EvHt_fG5gIp4pUfmePz1KvuHMBGFuuYNXxYjrCcnvPWVi_pQ39h5IZuQWN8ikZulEgDgB3WCHuXwW7aQXTUdtiY3ZKXNH");
                                client.Timeout = TimeSpan.FromHours(0.1);
                                dynamic dynItem = new { to = "/topics/" + dto.FileName, notification = new { title = dto.Title, body = dto.Body, click_action = "" } };
                                var content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(dynItem), Encoding.UTF8, "application/json");
                                var response = client.PostAsync(url.AbsolutePath, content);
                                if (response != null && response.Result.IsSuccessStatusCode)
                                {
                                    HttpResponseMessage res = response.Result;
                                }
                            }
                        }
                        else
                            throw new Exception("not found filename in data change");
                    }
                    else
                        throw new Exception("not found data and push path");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public void SendJoin(dynamic dynParam)
        {
            try
            {
                string strParam = dynParam != null ? dynParam.ToString() : string.Empty;
                if (!string.IsNullOrEmpty(strParam))
                {
                    var dto = Newtonsoft.Json.JsonConvert.DeserializeObject<DTOTopic>(strParam);
                    if (!string.IsNullOrEmpty(dto.Topic))
                    {
                        string filepath = HttpContext.Current.Server.MapPath(FileTopic);
                        List<DTOTopic> lst = new List<DTOTopic>();
                        string str = string.Empty;
                        if (System.IO.File.Exists(filepath))
                        {
                            str = System.IO.File.ReadAllText(filepath);
                            if (!string.IsNullOrEmpty(str))
                            {
                                try
                                {
                                    lst = Newtonsoft.Json.JsonConvert.DeserializeObject<List<DTOTopic>>(str);
                                }
                                catch
                                {
                                    lst = new List<DTOTopic>();
                                }
                            }
                        }
                        dto.Action = "join";
                        lst.Add(dto);
                        str = Newtonsoft.Json.JsonConvert.SerializeObject(lst);
                        System.IO.File.WriteAllText(filepath, str, System.Text.Encoding.UTF8);

                        using (var client = new HttpClient())
                        {
                            Uri url = new Uri("https://iid.googleapis.com/iid/v1:batchAdd");

                            client.BaseAddress = new Uri(url.Scheme + "://" + url.Authority);
                            client.DefaultRequestHeaders.Accept.Clear();

                            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                            client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "key=AAAA5NRCJyY:APA91bHBfzDh6iupDq7CvIw9iaNSstO20flnBWzqwWCbMq_EvHt_fG5gIp4pUfmePz1KvuHMBGFuuYNXxYjrCcnvPWVi_pQ39h5IZuQWN8ikZulEgDgB3WCHuXwW7aQXTUdtiY3ZKXNH");
                            client.Timeout = TimeSpan.FromHours(0.1);
                            dynamic dynItem = new { to = "/topics/" + dto.Topic, registration_tokens = new List<string> { dto.Token } };
                            var content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(dynItem), Encoding.UTF8, "application/json");
                            var response = client.PostAsync(url.AbsolutePath, content);
                            if (response != null && response.Result.IsSuccessStatusCode)
                            {
                                HttpResponseMessage res = response.Result;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public void SendLeave(dynamic dynParam)
        {
            try
            {
                string strParam = dynParam != null ? dynParam.ToString() : string.Empty;
                if (!string.IsNullOrEmpty(strParam))
                {
                    var dto = Newtonsoft.Json.JsonConvert.DeserializeObject<DTOTopic>(strParam);
                    if (!string.IsNullOrEmpty(dto.Topic))
                    {
                        string filepath = HttpContext.Current.Server.MapPath(FileTopic);
                        List<DTOTopic> lst = new List<DTOTopic>();
                        string str = string.Empty;
                        if (System.IO.File.Exists(filepath))
                        {
                            str = System.IO.File.ReadAllText(filepath);
                            if (!string.IsNullOrEmpty(str))
                            {
                                try
                                {
                                    lst = Newtonsoft.Json.JsonConvert.DeserializeObject<List<DTOTopic>>(str);
                                }
                                catch
                                {
                                    lst = new List<DTOTopic>();
                                }
                            }
                        }
                        dto.Action = "leave";
                        lst.Add(dto);
                        str = Newtonsoft.Json.JsonConvert.SerializeObject(lst);
                        System.IO.File.WriteAllText(filepath, str, System.Text.Encoding.UTF8);

                        using (var client = new HttpClient())
                        {
                            Uri url = new Uri("https://iid.googleapis.com/iid/v1:batchRemove");

                            client.BaseAddress = new Uri(url.Scheme + "://" + url.Authority);
                            client.DefaultRequestHeaders.Accept.Clear();

                            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                            client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "key=AAAA5NRCJyY:APA91bHBfzDh6iupDq7CvIw9iaNSstO20flnBWzqwWCbMq_EvHt_fG5gIp4pUfmePz1KvuHMBGFuuYNXxYjrCcnvPWVi_pQ39h5IZuQWN8ikZulEgDgB3WCHuXwW7aQXTUdtiY3ZKXNH");
                            client.Timeout = TimeSpan.FromHours(0.1);
                            dynamic dynItem = new { to = "/topics/" + dto.Topic, registration_tokens = new List<string> { dto.Token } };
                            var content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(dynItem), Encoding.UTF8, "application/json");
                            var response = client.PostAsync(url.AbsolutePath, content);
                            if (response != null && response.Result.IsSuccessStatusCode)
                            {
                                HttpResponseMessage res = response.Result;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}