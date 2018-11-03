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

        public static bool CreateSheetFile(string pathCredential, string pathToken, string pathSpreadsheet, string spreadsheetId, List<string> lstSheetGet)
        {
            //return true;

            var credential = GetCredential(pathCredential, pathToken);

            if (credential != null)
            {
                if (string.IsNullOrEmpty(pathSpreadsheet))
                    throw new Exception("Not found pathSpreadsheet");
                if (string.IsNullOrEmpty(spreadsheetId))
                    throw new Exception("Not found spreadsheetId");
                if (lstSheetGet == null || lstSheetGet.Count == 0)
                    throw new Exception("lstSheetGet is empty");

                SheetsService sheetsService = new SheetsService(new BaseClientService.Initializer
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "Google-SheetsSample/0.1",
                });

                // The ranges to retrieve from the spreadsheet.
                List<string> ranges = new List<string>();  // TODO: Update placeholder value.

                // True if grid data should be returned.
                // This parameter is ignored if a field mask was set in the request.
                bool includeGridData = false;  // TODO: Update placeholder value.

                SpreadsheetsResource.GetRequest request = sheetsService.Spreadsheets.Get(spreadsheetId);
                request.Ranges = ranges;
                request.IncludeGridData = includeGridData;

                // To execute asynchronously in an async method, replace `request.Execute()` as shown:
                Spreadsheet response = request.Execute();

                Dictionary<string, IList<RowData>> dic = new Dictionary<string, IList<RowData>>();
                foreach (var item in lstSheetGet)
                {
                    if (!dic.ContainsKey(item))
                    {
                        dic.Add(item, new List<RowData>());
                    }
                }

                foreach (var sheet in response.Sheets)
                {
                    if (dic.ContainsKey(sheet.Properties.Title))
                    {
                        ranges.Add(sheet.Properties.Title + "!A1:BA2000");
                    }
                }

                includeGridData = true;
                request.Ranges = ranges;
                request.IncludeGridData = includeGridData;

                response = request.Execute();
                foreach (var sheet in response.Sheets)
                {
                    if (dic.ContainsKey(sheet.Properties.Title) && sheet.Data.Count == 1)
                    {
                        dic[sheet.Properties.Title] = sheet.Data.FirstOrDefault().RowData;
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
