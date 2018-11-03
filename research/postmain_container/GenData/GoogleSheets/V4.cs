using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using Newtonsoft.Json;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using OfficeOpenXml;

namespace GenData.GoogleSheets
{
    public class V4
    {
        public delegate void Step(int stepNum, string stepNote);
        public static event Step OnStep;

        // If modifying these scopes, delete your previously saved credentials
        // at ~/.credentials/sheets.googleapis.com-dotnet-quickstart.json
        public static string[] Scopes = { SheetsService.Scope.SpreadsheetsReadonly };
        public static string ApplicationName = "Google Sheets API .NET Quickstart";

        public static bool CheckCredential(string pathCredential, string pathToken)
        {
            var credential = GetCredential(pathCredential, pathToken);
            return credential != null;
        }

        public static bool CreateSheetFile(string pathCredential, string pathToken, string pathSpreadsheet, string spreadsheetId, Dictionary<string, string> dicSheetGet)
        {
            //return true;

            var credential = GetCredential(pathCredential, pathToken);

            if (credential != null)
            {
                if (string.IsNullOrEmpty(pathSpreadsheet))
                    throw new Exception("Not found pathSpreadsheet");
                if (string.IsNullOrEmpty(spreadsheetId))
                    throw new Exception("Not found spreadsheetId");
                if (dicSheetGet == null || dicSheetGet.Count == 0)
                    throw new Exception("lstSheetGet is empty");

                Dictionary<string, IList<RowData>> dic = new Dictionary<string, IList<RowData>>();

                SheetsService sheetsService = new SheetsService(new BaseClientService.Initializer
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "Google-SheetsSample/0.1",
                });
                
                SpreadsheetsResource.GetRequest request = sheetsService.Spreadsheets.Get(spreadsheetId);
                request.Ranges = new List<string>();
                request.IncludeGridData = false;
                // To execute asynchronously in an async method, replace `request.Execute()` as shown:
                Spreadsheet response = request.Execute();
                var lstSheets = new List<string>();
                foreach (var sheet in response.Sheets)
                {
                    lstSheets.Add(sheet.Properties.Title);
                }
                
                foreach (var item in dicSheetGet)
                {
                    if (!dic.ContainsKey(item.Key) && lstSheets.Contains(item.Key))
                    {
                        request.Ranges = new List<string> { item.Key + "!" + item.Value };
                        request.IncludeGridData = true;
                        // To execute asynchronously in an async method, replace `request.Execute()` as shown:
                        response = request.Execute();
                        dic.Add(item.Key, response.Sheets[0].Data.FirstOrDefault().RowData);
                    }
                }

                if (File.Exists(pathSpreadsheet))
                    File.Delete(pathSpreadsheet);

                using (var package = new ExcelPackage(new FileInfo(pathSpreadsheet)))
                {
                    foreach (var item in dic)
                    {
                        ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(item.Key);
                        if (item.Value != null && item.Value.Count > 0)
                        {
                            for (int r = 0; r < item.Value.Count; r++)
                            {
                                var row = item.Value[r];
                                if (row != null && row.Values != null && row.Values.Count > 0)
                                {
                                    for (int c = 0; c < row.Values.Count; c++)
                                    {
                                        if (row.Values[c] != null && row.Values[c].FormattedValue != null)
                                        {
                                            worksheet.Cells[r + 1, c + 1].Value = row.Values[c].FormattedValue;
                                        }
                                    }
                                }
                            }
                        }
                    }

                    package.Save();
                }
                return true;
            }
            else
                throw new Exception("Not auth 2.0 google");
        }

        private static UserCredential GetCredential(string pathCredential, string pathToken)
        {
            if (string.IsNullOrEmpty(pathCredential))
                throw new Exception("Not found pathCredential");
            if (string.IsNullOrEmpty(pathCredential))
                throw new Exception("Not found pathToken");
            if (!File.Exists(pathCredential))
                throw new Exception("Not found pathCredential");

            UserCredential credential;

            using (var stream = new FileStream(pathCredential, FileMode.Open, FileAccess.Read))
            {
                string credPath = pathToken;
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                if (OnStep != null)
                    OnStep(1, "Credential file saved to: " + credPath);
            }

            // Create Google Sheets API service.
            var service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            return credential;
        }
    }
}
