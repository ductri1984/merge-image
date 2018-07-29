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
using System.Net.Http;
using System.Net.Http.Headers;

namespace ConsoleApplication1
{
    class Program
    {
        // If modifying these scopes, delete your previously saved credentials
        // at ~/.credentials/sheets.googleapis.com-dotnet-quickstart.json
        static string[] Scopes = { SheetsService.Scope.SpreadsheetsReadonly };
        static string ApplicationName = "Google Sheets API .NET Quickstart";

        static void Main(string[] args)
        {
            //1Ecm_3kKV4Wgz8BpZhPeMez5fy2pZuNSr_BNojTmOPwU
            UserCredential credential;

            using (var stream =
                new FileStream(@"D:\_Working\_3PSolution\myproject\research\google_sheets_v4\client_id.json", FileMode.Open, FileAccess.Read))
            {
                string credPath = "token.json";
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                Console.WriteLine("Credential file saved to: " + credPath);
            }

            // Create Google Sheets API service.
            var service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            if (credential != null)
            {
                SheetsService sheetsService = new SheetsService(new BaseClientService.Initializer
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "Google-SheetsSample/0.1",
                });

                // The spreadsheet to request.
                string spreadsheetId = "1Ecm_3kKV4Wgz8BpZhPeMez5fy2pZuNSr_BNojTmOPwU";  // TODO: Update placeholder value.

                // The ranges to retrieve from the spreadsheet.
                List<string> ranges = new List<string>();  // TODO: Update placeholder value.
                ranges.Add("Sheet1!A1:F");
                ranges.Add("Sheet2!A1:N");
                ranges.Add("Sheet3!A1:N");

                // True if grid data should be returned.
                // This parameter is ignored if a field mask was set in the request.
                bool includeGridData = false;  // TODO: Update placeholder value.

                SpreadsheetsResource.GetRequest request = sheetsService.Spreadsheets.Get(spreadsheetId);
                request.Ranges = ranges;
                request.IncludeGridData = includeGridData;

                // To execute asynchronously in an async method, replace `request.Execute()` as shown:
                Spreadsheet response = request.Execute();
                // Data.Spreadsheet response = await request.ExecuteAsync();

                string str = JsonConvert.SerializeObject(response);

                // TODO: Change code below to process the `response` object:
                Console.WriteLine(str);


                //String range = "Sheet3!A1:E";
                //SpreadsheetsResource.ValuesResource.GetRequest request =
                //        service.Spreadsheets.Values.Get(spreadsheetId, range);

                //// Prints the names and majors of students in a sample spreadsheet:
                //// https://docs.google.com/spreadsheets/d/1BxiMVs0XRA5nFMdKvBdBZjgmUUqptlbs74OgvE2upms/edit
                //ValueRange response = request.Execute();
                //IList<IList<Object>> values = response.Values;
                //if (values != null && values.Count > 0)
                //{
                //    Console.WriteLine("Name, Major");
                //    foreach (var row in values)
                //    {
                //        // Print columns A and E, which correspond to indices 0 and 4.
                //        Console.WriteLine("{0}, {1}", row[0], row[4]);
                //    }
                //}
                //else
                //{
                //    Console.WriteLine("No data found.");
                //}
            }

            Console.ReadLine();

            //// Define request parameters.
            //String spreadsheetId = "1Ecm_3kKV4Wgz8BpZhPeMez5fy2pZuNSr_BNojTmOPwU";
            //String range = "Class Data!A2:E";
            //SpreadsheetsResource.ValuesResource.GetRequest request =
            //        service.Spreadsheets.Values.Get(spreadsheetId, range);

            //// Prints the names and majors of students in a sample spreadsheet:
            //// https://docs.google.com/spreadsheets/d/1BxiMVs0XRA5nFMdKvBdBZjgmUUqptlbs74OgvE2upms/edit
            //ValueRange response = request.Execute();
            //IList<IList<Object>> values = response.Values;
            //if (values != null && values.Count > 0)
            //{
            //    Console.WriteLine("Name, Major");
            //    foreach (var row in values)
            //    {
            //        // Print columns A and E, which correspond to indices 0 and 4.
            //        Console.WriteLine("{0}, {1}", row[0], row[4]);
            //    }
            //}
            //else
            //{
            //    Console.WriteLine("No data found.");
            //}
            //Console.Read();
        }
    }
}
