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
        static string ApplicationName = "Google Sheets API .NET Quickstart";

        static void Main(string[] args)
        {
            //1Ecm_3kKV4Wgz8BpZhPeMez5fy2pZuNSr_BNojTmOPwU
            //GetData();
            //UpdateData();
            DeleteData();

            //UserCredential credential;

            //using (var stream =
            //    new FileStream(@"D:\_Working\_3PSolution\myproject\research\google_sheets_v4\client_id.json", FileMode.Open, FileAccess.Read))
            //{
            //    string credPath = "token.json";
            //    credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
            //        GoogleClientSecrets.Load(stream).Secrets,
            //        Scopes,
            //        "user",
            //        CancellationToken.None,
            //        new FileDataStore(credPath, true)).Result;
            //    Console.WriteLine("Credential file saved to: " + credPath);
            //}

            //// Create Google Sheets API service.
            //var service = new SheetsService(new BaseClientService.Initializer()
            //{
            //    HttpClientInitializer = credential,
            //    ApplicationName = ApplicationName,
            //});

            //if (credential != null)
            //{
            //    SheetsService sheetsService = new SheetsService(new BaseClientService.Initializer
            //    {
            //        HttpClientInitializer = credential,
            //        ApplicationName = "Google-SheetsSample/0.1",
            //    });

            //    // The spreadsheet to request.
            //    string spreadsheetId = "1Ecm_3kKV4Wgz8BpZhPeMez5fy2pZuNSr_BNojTmOPwU";  // TODO: Update placeholder value.

            //    // The ranges to retrieve from the spreadsheet.
            //    List<string> ranges = new List<string>();  // TODO: Update placeholder value.
            //    ranges.Add("Sheet1!A1:F");
            //    ranges.Add("Sheet2!A1:N");
            //    ranges.Add("Sheet3!A1:N");

            //    // True if grid data should be returned.
            //    // This parameter is ignored if a field mask was set in the request.
            //    bool includeGridData = false;  // TODO: Update placeholder value.

            //    SpreadsheetsResource.GetRequest request = sheetsService.Spreadsheets.Get(spreadsheetId);
            //    request.Ranges = ranges;
            //    request.IncludeGridData = includeGridData;

            //    // To execute asynchronously in an async method, replace `request.Execute()` as shown:
            //    Spreadsheet response = request.Execute();
            //    // Data.Spreadsheet response = await request.ExecuteAsync();

            //    string str = JsonConvert.SerializeObject(response);

            //    // TODO: Change code below to process the `response` object:
            //    Console.WriteLine(str);


            //    //String range = "Sheet3!A1:E";
            //    //SpreadsheetsResource.ValuesResource.GetRequest request =
            //    //        service.Spreadsheets.Values.Get(spreadsheetId, range);

            //    //// Prints the names and majors of students in a sample spreadsheet:
            //    //// https://docs.google.com/spreadsheets/d/1BxiMVs0XRA5nFMdKvBdBZjgmUUqptlbs74OgvE2upms/edit
            //    //ValueRange response = request.Execute();
            //    //IList<IList<Object>> values = response.Values;
            //    //if (values != null && values.Count > 0)
            //    //{
            //    //    Console.WriteLine("Name, Major");
            //    //    foreach (var row in values)
            //    //    {
            //    //        // Print columns A and E, which correspond to indices 0 and 4.
            //    //        Console.WriteLine("{0}, {1}", row[0], row[4]);
            //    //    }
            //    //}
            //    //else
            //    //{
            //    //    Console.WriteLine("No data found.");
            //    //}
            //}

            //Console.ReadLine();

            ////// Define request parameters.
            ////String spreadsheetId = "1Ecm_3kKV4Wgz8BpZhPeMez5fy2pZuNSr_BNojTmOPwU";
            ////String range = "Class Data!A2:E";
            ////SpreadsheetsResource.ValuesResource.GetRequest request =
            ////        service.Spreadsheets.Values.Get(spreadsheetId, range);

            ////// Prints the names and majors of students in a sample spreadsheet:
            ////// https://docs.google.com/spreadsheets/d/1BxiMVs0XRA5nFMdKvBdBZjgmUUqptlbs74OgvE2upms/edit
            ////ValueRange response = request.Execute();
            ////IList<IList<Object>> values = response.Values;
            ////if (values != null && values.Count > 0)
            ////{
            ////    Console.WriteLine("Name, Major");
            ////    foreach (var row in values)
            ////    {
            ////        // Print columns A and E, which correspond to indices 0 and 4.
            ////        Console.WriteLine("{0}, {1}", row[0], row[4]);
            ////    }
            ////}
            ////else
            ////{
            ////    Console.WriteLine("No data found.");
            ////}
            ////Console.Read();
        }

        static void GetData()
        {
            string[] Scopes = { SheetsService.Scope.SpreadsheetsReadonly }; //delete token folder to refresh scope

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
            }

            Console.ReadLine();
        }

        static void UpdateData()
        {
            string[] Scopes = { SheetsService.Scope.Spreadsheets }; //delete token folder to refresh scope

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
                string spreadsheetId = "1Ln6hwAS31EII2XdlWN9PN0kaNEt4S3RIZewf81w-ePQ";  // TODO: Update placeholder value.
                
                String range2 = "Sheet1!F5";  // update cell F5 
                ValueRange valueRange = new ValueRange();
                valueRange.MajorDimension = "ROWS";//"ROWS";//COLUMNS

                var oblist = new List<object>() { "1", 2 };
                valueRange.Values = new List<IList<object>> { oblist, oblist };
                
                SpreadsheetsResource.ValuesResource.UpdateRequest update = service.Spreadsheets.Values.Update(valueRange, spreadsheetId, range2);
                update.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.RAW;
                UpdateValuesResponse result2 = update.Execute();

            }

            Console.ReadLine();
        }

        static void ChangeColor()
        {
            string[] Scopes = { SheetsService.Scope.Spreadsheets }; //delete token folder to refresh scope

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
                //SheetsService sheetsService = new SheetsService(new BaseClientService.Initializer
                //{
                //    HttpClientInitializer = credential,
                //    ApplicationName = "Google-SheetsSample/0.1",
                //});

                //// The ID of the spreadsheet to update.
                //string spreadsheetId = "my-spreadsheet-id";  // TODO: Update placeholder value.

                //// How the input data should be interpreted.
                //string valueInputOption = "";  // TODO: Update placeholder value.

                //// The new values to apply to the spreadsheet.
                //List<ValueRange> data = new List<ValueRange>();  // TODO: Update placeholder value.
                //CellData cl = new CellData();
                //cl.UserEnteredValue = new ExtendedValue();
                //cl.UserEnteredValue.StringValue = "1";
                ////cl.UserEnteredFormat.BackgroundColor=
                //var range = new ValueRange();
                //range.Values.Add(data);

                


                //// TODO: Assign values to desired properties of `requestBody`:
                //BatchUpdateValuesRequest requestBody = new BatchUpdateValuesRequest();
                //requestBody.ValueInputOption = valueInputOption;
                //requestBody.Data = data;
                
                //SpreadsheetsResource.ValuesResource.BatchUpdateRequest request = sheetsService.Spreadsheets.Values.BatchUpdate(requestBody, spreadsheetId);
                

                //// To execute asynchronously in an async method, replace `request.Execute()` as shown:
                //BatchUpdateValuesResponse response = request.Execute();
                //// Data.BatchUpdateValuesResponse response = await request.ExecuteAsync();

                //// TODO: Change code below to process the `response` object:
                //Console.WriteLine(JsonConvert.SerializeObject(response));
            }
        }

        static void DeleteData()
        {
            //var range = $"{sheet}!A543:F";
            //var requestBody = new ClearValuesRequest();

            //var deleteRequest = service.Spreadsheets.Values.Clear(requestBody, SpreadsheetId, range);
            //var deleteReponse = deleteRequest.Execute();

            string[] Scopes = { SheetsService.Scope.Spreadsheets }; //delete token folder to refresh scope

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
                string spreadsheetId = "1Ln6hwAS31EII2XdlWN9PN0kaNEt4S3RIZewf81w-ePQ";  // TODO: Update placeholder value.

                String range2 = "Sheet1!A1:L20";  // update cell F5 
                var requestBody = new ClearValuesRequest();

                var deleteRequest = service.Spreadsheets.Values.Clear(requestBody, spreadsheetId, range2);
                var deleteReponse = deleteRequest.Execute();
            }

            Console.ReadLine();
        }
    }
}
