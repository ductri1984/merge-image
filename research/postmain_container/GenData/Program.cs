using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenData
{
    class Program
    {
        //[DllImport("user32.dll", EntryPoint = "SetCursorPos")]
        //public static extern int SetCursorPos(int x, int y);
        //[DllImport("user32.dll")]
        //public static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        //public enum MouseEventType : int
        //{
        //    LeftDown = 0x02,
        //    LeftUp = 0x04,
        //    RightDown = 0x08,
        //    RightUp = 0x10
        //}

        static string FileCredential = "credentials.json";
        static string FileToken = "token.json";
        static string FileSpreadsheet = "temp.xlsx";
        const string GoogleSheetID = "19KVSeofQRjeGL8iG_LQjEoCEWj0shIzaoLQ0I6qlIqM";
        const string SheetContainer = "Container";
        const string SheetContainerData = "ContainerData";

        static void Main(string[] args)
        {
            try
            {
                var folderapp = AppDomain.CurrentDomain.BaseDirectory;
                if (folderapp[folderapp.Length - 1] != '\\')
                    folderapp += "\\";
                FileCredential = folderapp + FileCredential;
                FileToken = folderapp + FileToken;
                FileSpreadsheet = folderapp + FileSpreadsheet;

                //if (System.IO.File.Exists(FileSpreadsheet))
                //    System.IO.File.Delete(FileSpreadsheet);
                if (GoogleSheets.V4.CreateSheetFile(FileCredential, FileToken, FileSpreadsheet, GoogleSheetID, new List<string> {
                    SheetContainer,
                    SheetContainerData
                }))
                {
                    if (System.IO.File.Exists(FileSpreadsheet))
                    {
                        var lstCase = new List<Case>();

                        using (var package = new OfficeOpenXml.ExcelPackage(new System.IO.FileInfo(FileSpreadsheet)))
                        {
                            var worksheet = HelperExcel.GetWorksheetByName(package, SheetContainer);
                            #region SheetContainer
                            if (worksheet != null && worksheet.Dimension != null)
                            {
                                int row = 1, col = 1, rowMax = 2000;
                                var itemAdd = default(Case);
                                for (row = 4; row <= worksheet.Dimension.End.Row && row < rowMax; row++)
                                {
                                    col = 2;
                                    var strCaseCode = HelperExcel.GetValue(worksheet, row, col);
                                    col++;
                                    var strCaseDescription = HelperExcel.GetValue(worksheet, row, col);
                                    if (!string.IsNullOrEmpty(strCaseCode))
                                    {
                                        if (itemAdd != null)
                                            itemAdd.RowEnd = row - 1;
                                        itemAdd = new Case();
                                        itemAdd.Code = strCaseCode;
                                        itemAdd.Description = strCaseDescription;
                                        itemAdd.RowStart = row;
                                        lstCase.Add(itemAdd);
                                    }
                                }

                                foreach (var item in lstCase)
                                {
                                    item.ListOrder = new List<CaseOrder>();
                                    item.ListMaster = new List<CaseMaster>();

                                    col = 5;
                                    int add = 3;
                                    int current = 0;
                                    row = item.RowStart + (current * add);
                                    var strOrderCode = HelperExcel.GetValue(worksheet, row, col);
                                    while (!string.IsNullOrEmpty(strOrderCode))
                                    {
                                        var strOrderID = HelperExcel.GetValue(worksheet, row + 1, col);
                                        var strOPSContainerID = HelperExcel.GetValue(worksheet, row + 2, col);
                                        item.ListOrder.Add(new CaseOrder
                                        {
                                            Code = strOrderCode,
                                            ID = strOrderID,
                                            OPSContainerID = strOPSContainerID
                                        });
                                        current++;
                                        row = item.RowStart + (current * add);
                                        strOrderCode = HelperExcel.GetValue(worksheet, row, col);
                                    }

                                    col = 20;
                                    int colMax = col + 12;
                                    row = item.RowStart;
                                    var itemMaster = default(CaseMaster);
                                    var strMasterCheck = HelperExcel.GetValue(worksheet, row, col);
                                    var strMasterCode = HelperExcel.GetValue(worksheet, row + 1, col);
                                    var strMasterID = HelperExcel.GetValue(worksheet, row + 2, col);
                                    var strVehicleID = HelperExcel.GetValue(worksheet, row + 3, col);
                                    var strRomoocID = HelperExcel.GetValue(worksheet, row + 4, col);
                                    while (string.IsNullOrEmpty(strMasterCheck) && !string.IsNullOrEmpty(strMasterCode))
                                    {
                                        itemMaster = new CaseMaster
                                        {
                                            Code = strMasterCode,
                                            ID = strMasterID,
                                            VehicleID = strVehicleID,
                                            RomoocID = strRomoocID,
                                            RowStart = row,
                                            ColStart = col + 1
                                        };
                                        item.ListMaster.Add(itemMaster);
                                        bool hasNext = false;
                                        for (int i = col + 1; i <= colMax; i++)
                                        {
                                            strMasterCheck = HelperExcel.GetValue(worksheet, row, i);
                                            if (string.IsNullOrEmpty(strMasterCheck))
                                            {
                                                col = i;
                                                itemMaster.ColEnd = col - 1;
                                                strMasterCode = HelperExcel.GetValue(worksheet, row + 1, col);
                                                strMasterID = HelperExcel.GetValue(worksheet, row + 2, col);
                                                strVehicleID = HelperExcel.GetValue(worksheet, row + 3, col);
                                                strRomoocID = HelperExcel.GetValue(worksheet, row + 4, col);
                                                hasNext = true;
                                                break;
                                            }
                                        }
                                        if (!hasNext)
                                            break;
                                    }

                                    col = 33;
                                    row = item.RowStart;
                                    itemMaster = default(CaseMaster);
                                    strMasterCheck = HelperExcel.GetValue(worksheet, row, col);
                                    colMax = col + 12;
                                    while (!string.IsNullOrEmpty(strMasterCheck))
                                    {
                                        itemMaster = item.ListMaster.FirstOrDefault(c => c.Code == strMasterCheck);
                                        if (itemMaster != null)
                                        {
                                            itemMaster.StateColStart = col;

                                            bool hasNext = false;
                                            for (int i = col + 1; i <= colMax; i++)
                                            {
                                                strMasterCheck = HelperExcel.GetValue(worksheet, row, i);
                                                if (!string.IsNullOrEmpty(strMasterCheck))
                                                {
                                                    col = i;
                                                    itemMaster.StateColEnd = col - 1;
                                                    hasNext = true;
                                                    break;
                                                }
                                            }
                                            if (!hasNext)
                                            {
                                                itemMaster.StateColEnd = colMax;
                                                break;
                                            }
                                        }
                                        else
                                            break;
                                    }
                                }

                                foreach (var item in lstCase)
                                {
                                    foreach (var itemMaster in item.ListMaster)
                                    {
                                        if (itemMaster.RowStart < 1 || itemMaster.ColStart < 1 || itemMaster.ColEnd < 1 || itemMaster.StateColStart < 1 || itemMaster.StateColEnd < 1)
                                            throw new Exception("check data master " + itemMaster.Code);

                                        itemMaster.ListLocation = new List<CaseMasterLocation>();
                                        itemMaster.ListOrder = new List<CaseMasterOrder>();

                                        row = itemMaster.RowStart;
                                        for (int i = itemMaster.ColStart; i <= itemMaster.ColEnd; i++)
                                        {
                                            var strLocationCode = HelperExcel.GetValue(worksheet, row, i);
                                            var strLocationID = HelperExcel.GetValue(worksheet, row + 1, i);
                                            var strTypeCode = HelperExcel.GetValue(worksheet, row + 2, i);
                                            var strTypeID = HelperExcel.GetValue(worksheet, row + 3, i);
                                            var strSortPrev = HelperExcel.GetValue(worksheet, row + 4, i);
                                            var strSortReal = HelperExcel.GetValue(worksheet, row + 5, i);
                                            var strBreakmooc = HelperExcel.GetValue(worksheet, row + 6, i);
                                            if (!string.IsNullOrEmpty(strLocationCode))
                                            {
                                                var itemLocation = new CaseMasterLocation
                                                {
                                                    Code = strLocationCode,
                                                    ID = strLocationID,
                                                    TypeCode = strTypeCode,
                                                    TypeID = strTypeID,
                                                    SortOrder = (itemMaster.ColStart - i).ToString(),
                                                    SortPrev = strSortPrev,
                                                    SortReal = strSortReal
                                                };
                                                if (strBreakmooc.ToLower() == "true")
                                                    itemLocation.IsBreakmooc = true;
                                                else if (strBreakmooc.ToLower() == "false")
                                                    itemLocation.IsBreakmooc = false;
                                                itemMaster.ListLocation.Add(itemLocation);
                                            }
                                        }

                                        row = itemMaster.RowStart + 1;
                                        int add = 2;
                                        int current = 0;
                                        row = row + (current * add);
                                        var strOrderCode = HelperExcel.GetValue(worksheet, row, itemMaster.StateColStart);
                                        while (!string.IsNullOrEmpty(strOrderCode))
                                        {
                                            var itemOrder = item.ListOrder.FirstOrDefault(c => c.Code == strOrderCode);
                                            if (itemOrder != null)
                                            {
                                                for (int i = itemMaster.StateColStart + 1; i <= itemMaster.StateColEnd; i++)
                                                {
                                                    var strLocationCode = HelperExcel.GetValue(worksheet, row, i);
                                                    var strLocationID = HelperExcel.GetValue(worksheet, row + 1, i);
                                                    itemMaster.ListOrder.Add(new CaseMasterOrder
                                                    {
                                                        OrderCode = itemOrder.Code,
                                                        OrderID = itemOrder.ID,
                                                        OPSContainerID = itemOrder.OPSContainerID,
                                                        LocationCode = strLocationCode,
                                                        LocationID = strLocationID,
                                                        SortOrder = (i - itemMaster.StateColStart).ToString()
                                                    });
                                                }
                                                current++;
                                                row = row + (current * add);
                                                strOrderCode = HelperExcel.GetValue(worksheet, row, itemMaster.StateColStart);
                                            }
                                            else
                                                break;
                                        }
                                    }
                                }
                            }
                            #endregion
                        }

                        if (lstCase.Count > 0)
                        {
                            #region GenData
                            string hosttest = string.Empty;
                            string strSpace = string.Empty;
                            var objCollectToken = default(PMCollection);

                            var objCollect = new PMCollection
                            {
                                info = new PMCollection_Info
                                {
                                    _postman_id = "b3f46c76-0d0a-4a46-8704-02b9a7c37df3",
                                    name = "test",
                                    schema = "https://schema.getpostman.com/json/collection/v2.1.0/collection.json"
                                },
                                item = new List<PMCollection_Item>()
                            };

                            var host = "";
                            var port = "";
                            string[] strs = hosttest.Split(':');
                            if (strs.Length > 1)
                            {
                                host = strs[1].Replace("//", "");
                            }
                            if (strs.Length > 2)
                            {
                                port = strs[2];
                            }

                            if (objCollectToken != null && objCollectToken.item.Count > 0)
                            {
                                objCollect.item.Add(objCollectToken.item[0]);
                            }

                            #region gui xuong dieu phoi
                            var lstOrderID = new List<string>();
                            foreach (var itemCase in lstCase)
                            {
                                lstOrderID.AddRange(itemCase.ListOrder.Select(c => c.ID).ToList());
                            }
                            if (lstOrderID.Count > 0)
                            {
                                lstOrderID = lstOrderID.Distinct().OrderBy(c => c).ToList();

                                GenOrderToOPS(objCollect, hosttest, strSpace, lstOrderID, "ORDOrderContainer_ToOPSCheck");
                                GenOrderToOPS(objCollect, hosttest, strSpace, lstOrderID, "ORDOrder_ToOPSCheck");
                                GenOrderToOPS(objCollect, hosttest, strSpace, lstOrderID, "ORDOrder_ToOPS");
                                GenOrderToOPS(objCollect, hosttest, strSpace, lstOrderID, "ORDOrder_UpdateStatus", "204");
                            }
                            #endregion

                            foreach (var itemCase in lstCase)
                            {
                                //var itemAdd = new PMCollection_Item();
                                //itemAdd.name = itemCase.Code;
                                //itemAdd.response = new List<string>();
                                //itemAdd._event = new List<PMCollection_Event>();

                                //var itemEvent = new PMCollection_Event
                                //{
                                //    listen = "test",
                                //    script = new PMCollection_EventScript
                                //    {
                                //        id = Guid.NewGuid().ToString(),
                                //        type = "text/javascript",
                                //        exec = new List<string>()
                                //    }
                                //};

                                //string strCompare = string.Empty;
                                //if (item.ReturnPropertyCondition == "eq" || item.ReturnPropertyCondition == "neq")
                                //{
                                //    strCompare = "data+\"\" " + strCondition + " \"" + item.ReturnPropertyValue + "\"";
                                //    //strValue = t1 + "var val = pm.environment.get(\"" + strEnvirKeyValue + "\")+\"\";";
                                //}
                                //else
                                //{
                                //    strCompare = "parseFloat(data) " + strCondition + " parseFloat(\"" + item.ReturnPropertyValue + "\")";
                                //    //strValue = t1 + "var val = parseFloat(pm.environment.get(\"" + strEnvirKeyValue + "\"));";
                                //}

                                //itemEvent.script.exec = new List<string>()
                                //{
                                //    "pm.test(\"Authorized " + item.Code + "\", function () {",
                                //    strSpace + "pm.response.to.not.have.status(401);",
                                //    "});",
                                //    "pm.test(\"API " + item.Code + "\", function () {",
                                //    //strSpace + "pm.response.to.be.ok;",
                                //    //strSpace + "pm.response.to.be.withBody;",
                                //    //strSpace + "pm.response.to.be.json;",
                                //    strSpace + "var data = pm.response.json();",
                                //    strSpace + "var flag = data !== undefined && " + strCompare + ";",
                                //    strSpace + "pm.expect(flag).to.equal(true);",
                                //    "});"
                                //};

                                //itemAdd._event.Add(itemEvent);

                                //itemAdd.request = new PMCollection_Request
                                //{
                                //    method = "POST",
                                //    header = new List<PMCollection_RequestHeader>(),
                                //    body = new PMCollection_RequestBody
                                //    {
                                //        mode = "raw",
                                //        raw = "{ \"data\": [" + string.Join(",", lstOrderID) + "] }"
                                //    },
                                //    url = new PMCollection_RequestURL
                                //    {
                                //        raw = hosttest + "/api/ORD/ORDOrderContainer_ToOPSCheck",
                                //        protocol = "http",
                                //        port = port,
                                //        host = new List<string>() { host },
                                //        path = new List<string>() { "api", "ORD", "ORDOrderContainer_ToOPSCheck" }
                                //    }
                                //};
                                //if (!string.IsNullOrEmpty(port))
                                //{
                                //    itemAdd.request.url.port = port;
                                //}
                                //itemAdd.request.header.Add(new PMCollection_RequestHeader
                                //{
                                //    key = "Content-Type",
                                //    value = "application/json"
                                //});
                                //itemAdd.request.header.Add(new PMCollection_RequestHeader
                                //{
                                //    key = "k",
                                //    value = "{{k}}"
                                //});
                                //itemAdd.request.header.Add(new PMCollection_RequestHeader
                                //{
                                //    key = "d",
                                //    value = "{{d}}"
                                //});
                                //itemAdd.request.header.Add(new PMCollection_RequestHeader
                                //{
                                //    key = "ListActionCode",
                                //    value = "ActApproved,ActContainer,ActDel,ActEdit,ActExcel,ActOPS,ViewAdmin"
                                //});

                            }

                            //if (!string.IsNullOrEmpty(FileCollectionPath))
                            //{
                            //    var sw = new System.IO.StreamWriter(FileCollectionPath, false);
                            //    sw.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(objCollect));
                            //    sw.Close();
                            //}

                            #endregion
                        }
                    }
                }
                else
                    throw new Exception("GoogleSheetID not exists in Sheets");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Console.ReadLine();
            }
        }

        private static void GenOrderToOPS(PMCollection objCollect, string hosttest, string strSpace, List<string> lstOrderID, string method, string statuscode = null)
        {
            var host = "";
            var port = "";
            string[] strs = hosttest.Split(':');
            if (strs.Length > 1)
            {
                host = strs[1].Replace("//", "");
            }
            if (strs.Length > 2)
            {
                port = strs[2];
            }

            var itemAdd = new PMCollection_Item();
            itemAdd.name = method;
            itemAdd.response = new List<string>();
            itemAdd._event = new List<PMCollection_Event>();

            var itemEvent = new PMCollection_Event
            {
                listen = "test",
                script = new PMCollection_EventScript
                {
                    id = Guid.NewGuid().ToString(),
                    type = "text/javascript",
                    exec = new List<string>()
                }
            };
            if (!string.IsNullOrEmpty(statuscode))
            {
                itemEvent.script.exec = new List<string>()
                {
                    "pm.test(\"" + method + "\", function () {",
                    strSpace + "pm.response.to.have.status(" + statuscode + ");",
                    "});"
                };
            }
            else
            {
                itemEvent.script.exec = new List<string>()
                {
                    "pm.test(\"" + method + "\", function () {",
                    strSpace + "pm.response.to.be.ok;",
                    strSpace + "pm.response.to.be.withBody;",
                    strSpace + "pm.response.to.be.json;",
                    "});"
                };
            }
            itemAdd._event.Add(itemEvent);

            itemAdd.request = new PMCollection_Request
            {
                method = "POST",
                header = new List<PMCollection_RequestHeader>(),
                body = new PMCollection_RequestBody
                {
                    mode = "raw",
                    raw = method == "ORDOrderContainer_ToOPSCheck" ? "{ \"data\": [" + string.Join(",", lstOrderID) + "] }" :
                        method == "ORDOrder_ToOPSCheck" ? "{ \"lst\": [" + string.Join(",", lstOrderID) + "] }" :
                        method == "ORDOrder_ToOPS" ? "{ \"lst\": [" + string.Join(",", lstOrderID) + "], \"IsStatus\":false,\"IsThread\":true }" :
                        method == "ORDOrder_UpdateStatus" ? "{ \"data\": [" + string.Join(",", lstOrderID) + "] }" : ""
                },
                url = new PMCollection_RequestURL
                {
                    raw = hosttest + "/api/ORD/" + method,
                    protocol = "http",
                    port = port,
                    host = new List<string>() { host },
                    path = new List<string>() { "api", "ORD", method }
                }
            };
            if (!string.IsNullOrEmpty(port))
            {
                itemAdd.request.url.port = port;
            }
            itemAdd.request.header.Add(new PMCollection_RequestHeader
            {
                key = "Content-Type",
                value = "application/json"
            });
            itemAdd.request.header.Add(new PMCollection_RequestHeader
            {
                key = "k",
                value = "{{k}}"
            });
            itemAdd.request.header.Add(new PMCollection_RequestHeader
            {
                key = "d",
                value = "{{d}}"
            });
            itemAdd.request.header.Add(new PMCollection_RequestHeader
            {
                key = "ListActionCode",
                value = "ActApproved,ActContainer,ActDel,ActEdit,ActExcel,ActOPS,ViewAdmin"
            });

            objCollect.item.Add(itemAdd);
        }

        private static void GenCreateMaster(PMCollection objCollect, string hosttest, string strSpace, CaseMaster itemMaster)
        {
            itemMaster.ListOrder
        }
    }
}
