using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System.IO;

namespace ConsoleGetToken
{
    class Program
    {
        static void Main(string[] args)
        {
            string strClientID = System.Configuration.ConfigurationManager.AppSettings.Get("clientid");
            string strToken = System.Configuration.ConfigurationManager.AppSettings.Get("token");

            string[] Scopes = { SheetsService.Scope.Spreadsheets }; //delete token folder to refresh scope

            UserCredential credential;

            using (var stream =
                new System.IO.FileStream(strClientID, System.IO.FileMode.Open, System.IO.FileAccess.Read))
            {
                string credPath = strToken;
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    System.Threading.CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                Console.WriteLine("Credential file saved to: " + credPath);
            }

            // Create Google Sheets API service.
            var service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "Google Sheets API",
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

            }
        }
    }
}
