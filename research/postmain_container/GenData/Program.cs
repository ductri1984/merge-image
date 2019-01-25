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
        const string SheetCheck = "Check";
        static string FileAuthCollection = "collection.json";
        static string FileAuthEnvironment = "environment.json";
        static string FileCollection = "testing.json";
        static string FileCollection1 = "testing1.json";
        const string ServiceIM = "NK";
        const string ServiceEX = "XK";
        const string ServiceCK = "CK";
        const string ServiceCC = "CC";
        const string ServiceCBT = "CBT";

        static void Main(string[] args)
        {
            try
            {
                string strSplit = string.Empty;
                if (args != null && args.Length > 0)
                    strSplit = args[0];

                //strSplit = "Case004";

                var folderapp = AppDomain.CurrentDomain.BaseDirectory;
                if (folderapp[folderapp.Length - 1] != '\\')
                    folderapp += "\\";
                FileCredential = folderapp + FileCredential;
                FileToken = folderapp + FileToken;
                FileSpreadsheet = folderapp + FileSpreadsheet;
                FileAuthCollection = folderapp + FileAuthCollection;
                FileAuthEnvironment = folderapp + FileAuthEnvironment;
                FileCollection = folderapp + FileCollection;
                FileCollection1 = folderapp + FileCollection1;

                if (System.IO.File.Exists(FileCollection))
                    System.IO.File.Delete(FileCollection);
                if (System.IO.File.Exists(FileCollection1))
                    System.IO.File.Delete(FileCollection1);
                if (System.IO.File.Exists(FileSpreadsheet))
                    System.IO.File.Delete(FileSpreadsheet);
                if (GoogleSheets.V4.CreateSheetFile(FileCredential, FileToken, FileSpreadsheet, GoogleSheetID, new Dictionary<string, string> {
                    { SheetContainer, "A1:BC700" },
                    { SheetContainerData, "A1:AB500" },
                    { SheetCheck, "A1:AV2000" }
                }))
                {
                    if (System.IO.File.Exists(FileSpreadsheet))
                    {
                        var lstCase = new List<Case>();
                        var lstMasterCheck = new List<MasterCheck>();
                        var lstMasterID = new List<string>();

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
                                if (itemAdd != null && itemAdd.RowEnd == 0)
                                    itemAdd.RowEnd = itemAdd.RowStart + 20;

                                foreach (var item in lstCase)
                                {
                                    item.ListOrder = new List<CaseOrder>();
                                    item.ListMaster = new List<CaseMaster>();

                                    col = 5;
                                    int add = 5;
                                    int current = 0;
                                    row = item.RowStart + (current * add);
                                    var strOrderCode = HelperExcel.GetValue(worksheet, row, col);
                                    while (!string.IsNullOrEmpty(strOrderCode))
                                    {
                                        var strOrderID = HelperExcel.GetValue(worksheet, row + 1, col);
                                        var strOPSContainerID = HelperExcel.GetValue(worksheet, row + 2, col);
                                        var strServiceCode = HelperExcel.GetValue(worksheet, row + 3, col);
                                        var strMergerCode = HelperExcel.GetValue(worksheet, row + 4, col);
                                        var strDepotID = HelperExcel.GetValue(worksheet, row + 4, col + 1);
                                        var strDepotReturnID = HelperExcel.GetValue(worksheet, row + 4, col + 2);
                                        item.ListOrder.Add(new CaseOrder
                                        {
                                            Code = strOrderCode,
                                            ID = strOrderID,
                                            OPSContainerID = strOPSContainerID,
                                            ServiceCode = strServiceCode,
                                            MergerCode = strMergerCode,
                                            DepotID = strDepotID,
                                            DepotReturnID = strDepotReturnID
                                        });
                                        current++;
                                        row = item.RowStart + (current * add);
                                        strOrderCode = HelperExcel.GetValue(worksheet, row, col);
                                        if (row >= item.RowEnd)
                                            break;
                                    }

                                    if (item.ListOrder.Count == 0) break;

                                    col = 22;
                                    int colMax = col + 15;
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

                                    if (item.ListMaster.Count == 0) break;

                                    col = 38;
                                    row = item.RowStart;
                                    itemMaster = default(CaseMaster);
                                    strMasterCheck = HelperExcel.GetValue(worksheet, row, col);
                                    colMax = col + 15;
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
                                    if (item.ListOrder.Count == 0 || item.ListMaster.Count == 0)
                                        break;

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
                                                    SortOrder = (i - itemMaster.ColStart + 1).ToString(),
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
                                                    if (!string.IsNullOrEmpty(strLocationCode) && !string.IsNullOrEmpty(strLocationID))
                                                    {
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

                            worksheet = HelperExcel.GetWorksheetByName(package, SheetCheck);
                            #region SheetCheck
                            if (worksheet != null && worksheet.Dimension != null)
                            {
                                int row = 1, col = 1, rowMax = 2000;
                                var itemAdd = default(MasterCheck);
                                for (row = 4; row <= worksheet.Dimension.End.Row && row < rowMax; row++)
                                {
                                    col = 2;
                                    var strMasterCode = HelperExcel.GetValue(worksheet, row, col);
                                    col++;
                                    var strCaseDescription = HelperExcel.GetValue(worksheet, row, col);
                                    if (!string.IsNullOrEmpty(strMasterCode))
                                    {
                                        if (itemAdd != null)
                                            itemAdd.RowEnd = row - 1;
                                        itemAdd = new MasterCheck();
                                        itemAdd.Code = strMasterCode;
                                        itemAdd.RowStart = row;
                                        lstMasterCheck.Add(itemAdd);
                                    }
                                }
                                if (itemAdd != null && itemAdd.RowEnd == 0)
                                    itemAdd.RowEnd = itemAdd.RowStart + 20;

                                var strOrderCode = string.Empty;
                                foreach (var itemCheck in lstMasterCheck)
                                {
                                    itemCheck.ListCOTOContainer = new List<MasterCheck_COTOContainer>();
                                    itemCheck.ListLocation = new List<MasterCheck_Location>();

                                    for (int cotosort = 1; cotosort <= 7; cotosort++)
                                    {
                                        col = 3 + (cotosort - 1) * 3;
                                        int add = 4;
                                        int current = 0;
                                        row = itemCheck.RowStart + (current * add);
                                        strOrderCode = HelperExcel.GetValue(worksheet, row, col);
                                        while (!string.IsNullOrEmpty(strOrderCode))
                                        {
                                            var strOrderID = HelperExcel.GetValue(worksheet, row + 1, col);
                                            var strLocationFromID = HelperExcel.GetValue(worksheet, row + 1, col + 1);
                                            var strLocationToID = HelperExcel.GetValue(worksheet, row + 1, col + 2);
                                            var strOPSContainerID = HelperExcel.GetValue(worksheet, row + 2, col);
                                            var strSortFrom = HelperExcel.GetValue(worksheet, row + 2, col + 1);
                                            var strSortTo = HelperExcel.GetValue(worksheet, row + 2, col + 2);
                                            var strIsSwap = HelperExcel.GetValue(worksheet, row + 3, col + 1);
                                            bool? isswap = null;
                                            if (strIsSwap.Trim().ToLower() == "true")
                                                isswap = true;

                                            itemCheck.ListCOTOContainer.Add(new MasterCheck_COTOContainer
                                            {
                                                OPSContainerID = strOPSContainerID,
                                                COTOSort = cotosort.ToString(),
                                                SortFrom = strSortFrom,
                                                SortTo = strSortTo,
                                                LocationFromID = strLocationFromID,
                                                LocationToID = strLocationToID,
                                                IsSwap = isswap
                                            });
                                            for (int i = 0; i < 10; i++)
                                            {
                                                current++;
                                                row = itemCheck.RowStart + (current * add);
                                                strOrderCode = HelperExcel.GetValue(worksheet, row, col);

                                                if (row >= itemCheck.RowEnd)
                                                    break;

                                                if (string.IsNullOrEmpty(strOrderCode))
                                                {
                                                    strLocationFromID = HelperExcel.GetValue(worksheet, row + 1, col + 1);
                                                    strLocationToID = HelperExcel.GetValue(worksheet, row + 1, col + 2);
                                                    strSortFrom = HelperExcel.GetValue(worksheet, row + 2, col + 1);
                                                    strSortTo = HelperExcel.GetValue(worksheet, row + 2, col + 2);
                                                    var strIsLastEXEmptyStock = HelperExcel.GetValue(worksheet, row + 3, col + 1);
                                                    bool? islastexemptystock = null;
                                                    if (strIsLastEXEmptyStock.Trim().ToLower() == "true")
                                                        islastexemptystock = true;
                                                    if (!string.IsNullOrEmpty(strLocationFromID))
                                                    {
                                                        itemCheck.ListLocation.Add(new MasterCheck_Location
                                                        {
                                                            OPSContainerID = strOPSContainerID,
                                                            COTOSort = cotosort.ToString(),
                                                            SortFrom = strSortFrom,
                                                            SortTo = strSortTo,
                                                            LocationFromID = strLocationFromID,
                                                            LocationToID = strLocationToID,
                                                            IsLastEXEmptyStock = islastexemptystock
                                                        });
                                                    }
                                                }
                                                else
                                                    break;
                                            }

                                            if (row >= itemCheck.RowEnd)
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
                            string hosttest = "http://localhost:2743";
                            string strSpace = "     ";
                            var objCollectAuth = default(PMCollection);

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

                            if (!string.IsNullOrEmpty(FileAuthCollection))
                            {
                                string str = System.IO.File.ReadAllText(FileAuthCollection, Encoding.UTF8);
                                if (!string.IsNullOrEmpty(str))
                                {
                                    objCollectAuth = Newtonsoft.Json.JsonConvert.DeserializeObject<PMCollection>(str);
                                    if (objCollectAuth != null && objCollectAuth.item.Count > 1)
                                    {
                                        //doi du lieu cho get key
                                        var itemEvent = objCollectAuth.item[0]._event[0];
                                        string d = !string.IsNullOrEmpty(port) ? host + ":" + port : host;
                                        itemEvent.script.exec = new List<string>()
                                        {
                                            "pm.test(\"Get key\", function () {",
                                            strSpace + "var data = pm.response.text();",
                                            strSpace + "var flag = false;",
                                            strSpace + "if(data !== undefined && data !== null){",
                                            strSpace + strSpace + "pm.environment.set(\"k\", data);",
                                            strSpace + strSpace + "pm.environment.set(\"d\", \"" + d + "\");",
                                            strSpace + strSpace + "flag = true;",
                                            strSpace + "}",
                                            strSpace + "pm.expect(flag).to.equal(true);",
                                            "});"
                                        };

                                        //cap nhat lai duong dan lay auth
                                        var itemRequest = objCollectAuth.item[1].request;
                                        itemRequest.url = new PMCollection_RequestURL
                                        {
                                            raw = hosttest + "/api/SYS/App_GetAuthorization",
                                            protocol = "http",
                                            port = port,
                                            host = new List<string>() { host },
                                            path = new List<string>() { "api", "SYS", "App_GetAuthorization" }
                                        };
                                    }
                                }
                            }

                            if (objCollectAuth != null && objCollectAuth.item.Count > 0)
                            {
                                objCollect.item.Add(objCollectAuth.item[0]);
                            }

                            #region gui xuong dieu phoi
                            var lstOrderID = new List<long>();
                            //foreach (var itemCase in lstCase)
                            //{
                            //    lstOrderID.AddRange(itemCase.ListOrder.Select(c => Convert.ToInt64(c.ID)).ToList());
                            //}
                            for (long i = 1; i < 100; i++)
                            {
                                lstOrderID.Add(i);
                            }
                            if (lstOrderID.Count > 0)
                            {
                                lstOrderID = lstOrderID.Distinct().OrderBy(c => c).ToList();

                                if (objCollectAuth != null && objCollectAuth.item.Count > 1)
                                    objCollect.item.Add(objCollectAuth.item[1]);
                                GenOrderToOPS(objCollect, hosttest, strSpace, lstOrderID, "ORDOrderContainer_ToOPSCheck");
                                if (objCollectAuth != null && objCollectAuth.item.Count > 1)
                                    objCollect.item.Add(objCollectAuth.item[1]);
                                GenOrderToOPS(objCollect, hosttest, strSpace, lstOrderID, "ORDOrder_ToOPSCheck");
                                if (objCollectAuth != null && objCollectAuth.item.Count > 1)
                                    objCollect.item.Add(objCollectAuth.item[1]);
                                GenOrderToOPS(objCollect, hosttest, strSpace, lstOrderID, "ORDOrder_ToOPS");
                                if (objCollectAuth != null && objCollectAuth.item.Count > 1)
                                    objCollect.item.Add(objCollectAuth.item[1]);
                                GenOrderToOPS(objCollect, hosttest, strSpace, lstOrderID, "ORDOrder_UpdateStatus", "204");
                            }
                            #endregion

                            foreach (var itemCase in lstCase)
                            {
                                if (itemCase.ListOrder.Count == 0 || itemCase.ListMaster.Count == 0)
                                    break;
                                if (itemCase.Code == strSplit)
                                {
                                    if (!string.IsNullOrEmpty(FileCollection))
                                    {
                                        var sw = new System.IO.StreamWriter(FileCollection, false);
                                        sw.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(objCollect));
                                        sw.Close();

                                        FileCollection = FileCollection1;

                                        objCollect = new PMCollection
                                        {
                                            info = new PMCollection_Info
                                            {
                                                _postman_id = "b3f46c76-0d0a-4a46-8704-02b9a7c37df3",
                                                name = "test1",
                                                schema = "https://schema.getpostman.com/json/collection/v2.1.0/collection.json"
                                            },
                                            item = new List<PMCollection_Item>()
                                        };
                                    }
                                }

                                #region kiem tra du lieu
                                var lstMasterCheckID = itemCase.ListMaster.Select(c => c.ID).Distinct().ToList();
                                if (lstMasterID.Where(c => lstMasterCheckID.Contains(c)).Count() > 0)
                                    throw new Exception(itemCase.Code + " list master used");
                                else
                                    lstMasterID.AddRange(lstMasterCheckID);
                                var lstOPSContainerID = itemCase.ListOrder.Select(c => c.OPSContainerID).Distinct().ToList();
                                foreach (var itemCheck in itemCase.ListMaster)
                                {
                                    int sort = 1;
                                    foreach (var itemLocation in itemCheck.ListLocation.Select(c => new { c.SortOrder, c.SortReal, c.SortPrev }).OrderBy(c => c.SortReal))
                                    {
                                        if (itemLocation.SortOrder == itemLocation.SortPrev)
                                            throw new Exception(itemCase.Code + " - " + itemCheck.Code + " check SortPrev");
                                        if (itemLocation.SortReal != sort.ToString())
                                            throw new Exception(itemCase.Code + " - " + itemCheck.Code + " check SortReal");
                                        sort++;
                                    }
                                    if (itemCheck.ListOrder.Where(c => !lstOPSContainerID.Contains(c.OPSContainerID)).Count() > 0)
                                    {
                                        throw new Exception(itemCase.Code + " - " + itemCheck.Code + " check OPSContainerID");
                                    }
                                }
                                #endregion

                                if (objCollectAuth != null && objCollectAuth.item.Count > 1)
                                    objCollect.item.Add(objCollectAuth.item[1]);

                                var itemMaster = itemCase.ListMaster[0];
                                GenEditContainer(objCollect, hosttest, strSpace, itemCase);
                                GenCreateMaster(objCollect, hosttest, strSpace, itemCase);
                                GenCOTOContainerList(objCollect, hosttest, strSpace, itemCase.Code + "_getid" + itemMaster.ID, itemMaster);
                                GenCOTOContainerCheck(objCollect, hosttest, strSpace, itemCase.Code + "_checkdata" + itemMaster.ID, itemMaster, lstMasterCheck);
                                GenCheckMasterLocation(objCollect, hosttest, strSpace, itemCase.Code + "_checklocation" + itemMaster.ID, itemMaster);
                                GenCOTOContainerStart(objCollect, hosttest, strSpace, itemCase.Code + "_start" + itemMaster.ID, itemMaster);
                                GenCOTOContainerList(objCollect, hosttest, strSpace, itemCase.Code + "_getidcheck" + itemMaster.ID, itemMaster, true);
                                if (itemMaster.ListLocation.Where(c => c.IsBreakmooc != null).Count() > 0)
                                    GenCOTOContainerBreakmooc(objCollect, hosttest, strSpace, itemCase.Code + "_breakmooc" + itemMaster.ID, itemMaster);
                                else
                                    GenCOTOContainerComplete(objCollect, hosttest, strSpace, itemCase.Code + "_complete" + itemMaster.ID, itemMaster);

                                if (itemCase.ListMaster.Count > 1)
                                {
                                    itemMaster = itemCase.ListMaster[1];
                                    GenCOTOContainerList(objCollect, hosttest, strSpace, itemCase.Code + "_getid" + itemMaster.ID, itemMaster);
                                    GenCOTOContainerCheck(objCollect, hosttest, strSpace, itemCase.Code + "_checkdata" + itemMaster.ID, itemMaster, lstMasterCheck);
                                    GenCheckMasterLocation(objCollect, hosttest, strSpace, itemCase.Code + "_checklocation" + itemMaster.ID, itemMaster);
                                    GenCOTOContainerStart(objCollect, hosttest, strSpace, itemCase.Code + "_start" + itemMaster.ID, itemMaster);
                                    GenCOTOContainerList(objCollect, hosttest, strSpace, itemCase.Code + "_getidcheck" + itemMaster.ID, itemMaster, true);
                                    if (itemMaster.ListLocation.Where(c => c.IsBreakmooc != null).Count() > 0)
                                        GenCOTOContainerBreakmooc(objCollect, hosttest, strSpace, itemCase.Code + "_breakmooc" + itemMaster.ID, itemMaster);
                                    else
                                        GenCOTOContainerComplete(objCollect, hosttest, strSpace, itemCase.Code + "_complete" + itemMaster.ID, itemMaster);
                                }

                                if (itemCase.ListMaster.Count > 2)
                                {
                                    itemMaster = itemCase.ListMaster[2];
                                    GenCOTOContainerList(objCollect, hosttest, strSpace, itemCase.Code + "_getid" + itemMaster.ID, itemMaster);
                                    GenCOTOContainerCheck(objCollect, hosttest, strSpace, itemCase.Code + "_checkdata" + itemMaster.ID, itemMaster, lstMasterCheck);
                                    GenCheckMasterLocation(objCollect, hosttest, strSpace, itemCase.Code + "_checklocation" + itemMaster.ID, itemMaster);
                                    GenCOTOContainerStart(objCollect, hosttest, strSpace, itemCase.Code + "_start" + itemMaster.ID, itemMaster);
                                    GenCOTOContainerList(objCollect, hosttest, strSpace, itemCase.Code + "_getidcheck" + itemMaster.ID, itemMaster, true);
                                    if (itemMaster.ListLocation.Where(c => c.IsBreakmooc != null).Count() > 0)
                                        GenCOTOContainerBreakmooc(objCollect, hosttest, strSpace, itemCase.Code + "_breakmooc" + itemMaster.ID, itemMaster);
                                    else
                                        GenCOTOContainerComplete(objCollect, hosttest, strSpace, itemCase.Code + "_complete" + itemMaster.ID, itemMaster);
                                }
                            }

                            if (!string.IsNullOrEmpty(FileCollection))
                            {
                                var sw = new System.IO.StreamWriter(FileCollection, false);
                                sw.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(objCollect));
                                sw.Close();
                            }

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

        private static void GenOrderToOPS(PMCollection objCollect, string hosttest, string strSpace, List<long> lstOrderID, string method, string statuscode = null)
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

        private static void GenEditContainer(PMCollection objCollect, string hosttest, string strSpace, Case itemCase)
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

            foreach (var itemOrder in itemCase.ListOrder.Where(c => c.ServiceCode == ServiceCK && string.IsNullOrEmpty(c.MergerCode)))
            {
                if (!string.IsNullOrEmpty(itemOrder.DepotID))
                {
                    var itemAdd = new PMCollection_Item();
                    itemAdd.name = itemCase.Code + "_edit" + itemOrder.OPSContainerID + "depot";
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
                    itemEvent.script.exec = new List<string>()
                    {
                        "pm.test(\"" + itemCase.Code + "_edit\", function () {",
                        strSpace + "pm.expect(pm.response.code).to.be.oneOf([200,204]);",
                        "});"
                    };
                    itemAdd._event.Add(itemEvent);

                    itemAdd.request = new PMCollection_Request
                    {
                        method = "POST",
                        description = itemCase.Description,
                        header = new List<PMCollection_RequestHeader>(),
                        body = new PMCollection_RequestBody
                        {
                            mode = "raw",
                            raw = "{\"action\":{\"OPSContainerID\":" + itemOrder.OPSContainerID + ",\"ActionType\":\"depotget\"," +
                                "\"IsLocationOrder\":false,\"LocationID\":" + itemOrder.DepotID + "}}"
                        },
                        url = new PMCollection_RequestURL
                        {
                            raw = hosttest + "/api/OPS/OPSCON_PopupEditContainer_Action",
                            protocol = "http",
                            port = port,
                            host = new List<string>() { host },
                            path = new List<string>() { "api", "OPS", "OPSCON_PopupEditContainer_Action" }
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
                if (!string.IsNullOrEmpty(itemOrder.DepotReturnID))
                {
                    var itemAdd = new PMCollection_Item();
                    itemAdd.name = itemCase.Code + "_edit" + itemOrder.OPSContainerID + "depotreturn";
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
                    itemEvent.script.exec = new List<string>()
                    {
                        "pm.test(\"" + itemCase.Code + "_edit\", function () {",
                        strSpace + "pm.expect(pm.response.code).to.be.oneOf([200,204]);",
                        "});"
                    };
                    itemAdd._event.Add(itemEvent);

                    itemAdd.request = new PMCollection_Request
                    {
                        method = "POST",
                        description = itemCase.Description,
                        header = new List<PMCollection_RequestHeader>(),
                        body = new PMCollection_RequestBody
                        {
                            mode = "raw",
                            raw = "{\"action\":{\"OPSContainerID\":" + itemOrder.OPSContainerID + ",\"ActionType\":\"depotreturn\"," +
                                "\"IsLocationOrder\":false,\"LocationID\":" + itemOrder.DepotReturnID + "}}"
                        },
                        url = new PMCollection_RequestURL
                        {
                            raw = hosttest + "/api/OPS/OPSCON_PopupEditContainer_Action",
                            protocol = "http",
                            port = port,
                            host = new List<string>() { host },
                            path = new List<string>() { "api", "OPS", "OPSCON_PopupEditContainer_Action" }
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
            }
        }

        private static void GenCreateMaster(PMCollection objCollect, string hosttest, string strSpace, Case itemCase)
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

            var itemMaster = itemCase.ListMaster.FirstOrDefault();

            var itemAdd = new PMCollection_Item();
            itemAdd.name = itemCase.Code + "_create";
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
            itemEvent.script.exec = new List<string>()
            {
                "pm.test(\"" + itemCase.Code + "_create\", function () {",
                strSpace + "pm.expect(pm.response.code).to.be.oneOf([200,204]);",
                "});"
            };
            itemAdd._event.Add(itemEvent);

            string strIsSwap = "false";
            string strIsBorrowEmpty = "false";
            if (itemCase.ListOrder.Where(c => c.ServiceCode == ServiceIM).Count() > 0 && itemCase.ListOrder.Where(c => c.ServiceCode == ServiceEX).Count() > 0 &&
                itemCase.ListOrder.Where(c => (c.ServiceCode == ServiceIM || c.ServiceCode == ServiceEX) && !string.IsNullOrEmpty(c.MergerCode)).Count() > 0)
                strIsSwap = "true";
            if (itemCase.ListOrder.Where(c => c.ServiceCode == ServiceCK && !string.IsNullOrEmpty(c.MergerCode)).Count() > 0)
                strIsBorrowEmpty = "true";

            itemAdd.request = new PMCollection_Request
            {
                method = "POST",
                description = itemCase.Description,
                header = new List<PMCollection_RequestHeader>(),
                body = new PMCollection_RequestBody
                {
                    mode = "raw",
                    raw = "{\"item\":{\"ID\":-1,\"DriverID\":3," +
                        "\"ETD\":\"" + DateTime.UtcNow.ToString("o") + "\",\"ETA\":\"" + DateTime.UtcNow.AddHours(1).ToString("o") + "\"," +
                        "\"ListOPSContainerID\": [" + string.Join(",", itemCase.ListOrder.Select(c => c.OPSContainerID).ToList()) + "],\"ListCOTOMasterID\":[],\"ListDITOGroupProduct\":[]," +
                        "\"VehicleID\":" + itemMaster.VehicleID + ",\"RomoocID\":" + itemMaster.RomoocID + ",\"IsSwap\":" + strIsSwap + ",\"IsNotMergeCont\":false,\"IsBorrowEmpty\":" + strIsBorrowEmpty + "," +
                        "\"DriverName\":\"container1 container1\",\"DriverTel\":\"123456789\"}}"
                },
                url = new PMCollection_RequestURL
                {
                    raw = hosttest + "/api/OPS/OPSCON_View1_Save",
                    protocol = "http",
                    port = port,
                    host = new List<string>() { host },
                    path = new List<string>() { "api", "OPS", "OPSCON_View1_Save" }
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

            //GenCheckMasterLocation(objCollect, hosttest, strSpace, itemCase.Code + "_check" + itemMaster.ID, itemMaster);
            GenApprovedMaster(objCollect, hosttest, strSpace, itemCase.Code + "_approved" + itemMaster.ID, itemMaster);
        }

        private static void GenApprovedMaster(PMCollection objCollect, string hosttest, string strSpace, string name, CaseMaster itemMaster)
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
            itemAdd.name = name;
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
            itemEvent.script.exec = new List<string>()
            {
                "pm.test(\"" + name + "\", function () {",
                strSpace + "pm.response.to.be.ok;",
                strSpace + "pm.response.to.be.withBody;",
                strSpace + "pm.response.to.be.json;",
                "});"
            };
            itemAdd._event.Add(itemEvent);

            itemAdd.request = new PMCollection_Request
            {
                method = "POST",
                header = new List<PMCollection_RequestHeader>(),
                body = new PMCollection_RequestBody
                {
                    mode = "raw",
                    raw = "{\"lstmasterid\":[" + itemMaster.ID + "]}"
                },
                url = new PMCollection_RequestURL
                {
                    raw = hosttest + "/api/OPS/OPSCON_PopupTOMaster_TOContainerApproved",
                    protocol = "http",
                    port = port,
                    host = new List<string>() { host },
                    path = new List<string>() { "api", "OPS", "OPSCON_PopupTOMaster_TOContainerApproved" }
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

        private static void GenCOTOContainerList(PMCollection objCollect, string hosttest, string strSpace, string name, CaseMaster itemMaster, bool checkdata = false)
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
            itemAdd.name = name;
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
            if (checkdata == true)
            {
                var firstLocation = itemMaster.ListLocation.FirstOrDefault(c => c.SortReal == "1");
                var secondLocation = itemMaster.ListLocation.FirstOrDefault(c => c.SortReal == "2");
                if (firstLocation == null || secondLocation == null)
                    throw new Exception(itemMaster.Code + " not found first, second location");

                itemEvent.script.exec = new List<string>()
                {
                    "pm.test(\"" + name + "\", function () {",
                    strSpace + "var result = pm.response.json();",
                    strSpace + "var flag = false;",
                    strSpace + "if(result != undefined && result != null && result.Data != undefined && result.Data != null && result.Data.length > 0){",
                    strSpace + strSpace + "flag = true;",
                    strSpace + strSpace + "var current = null;",
                    strSpace + strSpace + "var breakmooc = null;",
                    strSpace + strSpace + "var sub = 0;",
                    strSpace + strSpace + "current = result.Data.find(function(o){ return o.LocationFromID == " + firstLocation.ID + " && o.LocationToID == " + secondLocation.ID + " && o.COTOSort == 1 ; });",
                    strSpace + strSpace + "if(current == null) current = result.Data.find(function(o){ return o.LocationFromID == " + firstLocation.ID + " && o.LocationToID == " + secondLocation.ID + " && o.COTOSort == 2 ; });",
                    strSpace + strSpace + "if(current == null) current = result.Data.find(function(o){ return o.LocationFromID == " + firstLocation.ID + " && o.LocationToID == " + secondLocation.ID + " && o.COTOSort == 3 ; });",
                    strSpace + strSpace + "if(current != null) sub = current.COTOSort - 1;",
                };

                var lstCheckData = new List<string>();
                var itemOrderCurrent = default(CaseMasterOrder);
                var lstOPSContainerID = itemMaster.ListOrder.Select(c => c.OPSContainerID).Distinct().ToList();
                foreach (var opscontainerid in lstOPSContainerID)
                {
                    itemOrderCurrent = default(CaseMasterOrder);
                    foreach (var itemOrder in itemMaster.ListOrder.Where(c => c.OPSContainerID == opscontainerid))
                    {
                        if (itemOrderCurrent != null)
                        {
                            if (itemMaster.ListLocation.Where(c => c.IsBreakmooc != null && c.SortReal == itemOrderCurrent.SortOrder).Count() > 0)
                            {
                                lstCheckData.AddRange(new List<string>
                                {
                                    strSpace + strSpace + "current = result.Data.find(function(o){ return o.OPSContainerID == " + itemOrderCurrent.OPSContainerID +
                                        " && o.LocationFromID == " + itemOrderCurrent.LocationID + " && o.LocationToID == " + itemOrder.LocationID +
                                        " && o.COTOSort == (" + itemOrderCurrent.SortOrder + "+sub) ; });",
                                    strSpace + strSpace + "if(current == null) flag = false;",
                                    strSpace + strSpace + "else breakmooc = current;",
                                });
                            }
                            else
                            {
                                lstCheckData.AddRange(new List<string>
                            {
                                    strSpace + strSpace + "current = result.Data.find(function(o){ return o.OPSContainerID == " + itemOrderCurrent.OPSContainerID +
                                        " && o.LocationFromID == " + itemOrderCurrent.LocationID + " && o.LocationToID == " + itemOrder.LocationID +
                                        " && o.COTOSort == (" + itemOrderCurrent.SortOrder + "+sub) ; });",
                                strSpace + strSpace + "if(current == null) flag = false;",
                            });
                            }
                        }
                        itemOrderCurrent = itemOrder;
                    }
                }


                itemEvent.script.exec.AddRange(lstCheckData);

                itemEvent.script.exec.AddRange(new List<string>()
                {
                    strSpace + strSpace + "if(flag == true){",
                    strSpace + strSpace + strSpace + "pm.environment.set(\"actionComplete\", JSON.stringify({action:{COTOContainerID:result.Data[result.Data.length - 1].ID, ActionType:\"complete\"}}));",
                    strSpace + strSpace + strSpace + "if(breakmooc != null) pm.environment.set(\"actionBreakmooc\", JSON.stringify({action:{COTOContainerID:breakmooc.ID, IsBreakmoocNext:true, ReasonID:1, ActionType:\"breakmooc\"}}));",
                    strSpace + strSpace + "}",
                    strSpace + "}",
                    strSpace + "pm.expect(flag).to.equal(true);",
                    "});"
                });
            }
            else
            {
                itemEvent.script.exec = new List<string>()
                {
                    "pm.test(\"" + name + "\", function () {",
                    strSpace + "var result = pm.response.json();",
                    strSpace + "var flag = false;",
                    strSpace + "pm.environment.set(\"id\", \"-1\");",
                    strSpace + "if(result != undefined && result != null && result.Data != undefined && result.Data != null && result.Data.length > 0){",
                    strSpace + strSpace + "pm.environment.set(\"id\", result.Data[0].ID + \"\");",
                    strSpace + strSpace + "flag = true;",
                    strSpace + "}",
                    strSpace + "pm.expect(flag).to.equal(true);",
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
                    raw = "{\"masterid\":" + itemMaster.ID + "}"
                },
                url = new PMCollection_RequestURL
                {
                    raw = hosttest + "/api/MON/MONCON_PopupInfo_COTOContainer_List",
                    protocol = "http",
                    port = port,
                    host = new List<string>() { host },
                    path = new List<string>() { "api", "MON", "MONCON_PopupInfo_COTOContainer_List" }
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

        private static void GenCOTOContainerCheck(PMCollection objCollect, string hosttest, string strSpace, string name, CaseMaster itemMaster, List<MasterCheck> lstMasterCheck)
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

            var itemCheck = lstMasterCheck.FirstOrDefault(c => c.Code == itemMaster.Code);
            if (itemCheck != null && itemCheck.ListCOTOContainer != null && itemCheck.ListCOTOContainer.Count > 0)
            {
                var itemAdd = new PMCollection_Item();
                itemAdd.name = name;
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

                itemEvent.script.exec = new List<string>()
                {
                    "pm.test(\"" + name + "\", function () {",
                    strSpace + "var data = pm.response.json();",
                    strSpace + "var flag = false;",
                    strSpace + "if(data != undefined && data != null && data.ListCOTOContainer != undefined && data.ListCOTOContainer != null && data.ListCOTOContainer.length > 0){",
                    strSpace + strSpace + "flag = true;",
                    strSpace + strSpace + "var current = null;",
                    strSpace + strSpace + "var prev = null;",
                };

                var lstCheckData = new List<string>();
                int i = 0;
                #region ListCOTOContainer
                foreach (var cotocontainer in itemCheck.ListCOTOContainer)
                {
                    if (i == 0)
                    {
                        if (cotocontainer.IsSwap == true)
                        {
                            lstCheckData.AddRange(new List<string>
                            {
                                strSpace + strSpace + "current = data.ListCOTOContainer[0];",
                                strSpace + strSpace + "if(current != null && current != undefined){",
                                strSpace + strSpace + strSpace + "var strswap = current.IsSwap + \"\";",
                                strSpace + strSpace + strSpace + "if(current.OPSContainerID == " + cotocontainer.OPSContainerID +
                                    " && current.LocationFromID == " + cotocontainer.LocationFromID + " && current.LocationToID == " + cotocontainer.LocationToID +
                                    " && current.SortFrom == " + cotocontainer.SortFrom + " && current.SortTo == " + cotocontainer.SortTo +
                                    " && current.COTOSort == " + cotocontainer.COTOSort + " && strswap == \"true\")",
                                strSpace + strSpace + strSpace + strSpace + "prev = current;",
                                strSpace + strSpace + strSpace + "if(prev == null) flag = false;",
                                strSpace + strSpace + "}",
                                strSpace + strSpace + "else flag = false;",
                            });
                        }
                        else
                        {
                            lstCheckData.AddRange(new List<string>
                            {
                                strSpace + strSpace + "current = data.ListCOTOContainer[0];",
                                strSpace + strSpace + "if(current != null && current != undefined){",
                                strSpace + strSpace + strSpace + "if(current.OPSContainerID == " + cotocontainer.OPSContainerID +
                                    " && current.LocationFromID == " + cotocontainer.LocationFromID + " && current.LocationToID == " + cotocontainer.LocationToID +
                                    " && current.SortFrom == " + cotocontainer.SortFrom + " && current.SortTo == " + cotocontainer.SortTo +
                                    " && current.COTOSort == " + cotocontainer.COTOSort + ")",
                                strSpace + strSpace + strSpace + strSpace + "prev = current;",
                                strSpace + strSpace + strSpace + "if(prev == null) flag = false;",
                                strSpace + strSpace + "}",
                                strSpace + strSpace + "else flag = false;",
                            });
                        }
                    }
                    else
                    {
                        if (cotocontainer.IsSwap == true)
                        {
                            lstCheckData.AddRange(new List<string>
                            {
                                strSpace + strSpace + "current = data.ListCOTOContainer.find(function(o){ return o.OPSContainerID == " + cotocontainer.OPSContainerID +
                                    " && o.LocationFromID == " + cotocontainer.LocationFromID + " && o.LocationToID == " + cotocontainer.LocationToID +
                                    " && o.SortFrom == " + cotocontainer.SortFrom + " && o.SortTo == " + cotocontainer.SortTo +
                                    " && o.COTOSort == " + cotocontainer.COTOSort + "; });",
                                strSpace + strSpace + "if(current == null) flag = false;",
                                strSpace + strSpace + "else {",
                                strSpace + strSpace + strSpace + "var strswap = current.IsSwap + \"\";",
                                strSpace + strSpace + strSpace + "if(strswap != \"true\") flag = false;",
                                strSpace + strSpace + "}",
                            });
                        }
                        else
                        {
                            lstCheckData.AddRange(new List<string>
                            {
                                strSpace + strSpace + "current = data.ListCOTOContainer.find(function(o){ return o.OPSContainerID == " + cotocontainer.OPSContainerID +
                                    " && o.LocationFromID == " + cotocontainer.LocationFromID + " && o.LocationToID == " + cotocontainer.LocationToID +
                                    " && o.SortFrom == " + cotocontainer.SortFrom + " && o.SortTo == " + cotocontainer.SortTo +
                                    " && o.COTOSort == " + cotocontainer.COTOSort + "; });",
                                strSpace + strSpace + "if(current == null) flag = false;",
                            });
                        }
                    }

                    i++;
                }
                #endregion
                i = 0;
                #region ListLocation
                foreach (var location in itemCheck.ListLocation)
                {
                    if (i == 0)
                    {
                        if (location.IsLastEXEmptyStock == true)
                        {
                            lstCheckData.AddRange(new List<string>
                            {
                                strSpace + strSpace + "current = data.ListLocation[0];",
                                strSpace + strSpace + "if(current != null && current != undefined){",
                                strSpace + strSpace + strSpace + "var strswap = current.IsLastEXEmptyStock + \"\";",
                                strSpace + strSpace + strSpace + "if(current.OPSContainerID == " + location.OPSContainerID +
                                    " && current.LocationFromID == " + location.LocationFromID + " && current.LocationToID == " + location.LocationToID +
                                    " && current.SortFrom == " + location.SortFrom + " && current.SortTo == " + location.SortTo +
                                    " && current.COTOSort == " + location.COTOSort + " && strswap == \"true\")",
                                strSpace + strSpace + strSpace + strSpace + "prev = current;",
                                strSpace + strSpace + strSpace + "if(prev == null) flag = false;",
                                strSpace + strSpace + "}",
                                strSpace + strSpace + "else flag = false;",
                            });
                        }
                        else
                        {
                            lstCheckData.AddRange(new List<string>
                            {
                                strSpace + strSpace + "current = data.ListLocation[0];",
                                strSpace + strSpace + "if(current != null && current != undefined){",
                                strSpace + strSpace + strSpace + "if(current.OPSContainerID == " + location.OPSContainerID +
                                    " && current.LocationFromID == " + location.LocationFromID + " && current.LocationToID == " + location.LocationToID +
                                    " && current.SortFrom == " + location.SortFrom + " && current.SortTo == " + location.SortTo +
                                    " && current.COTOSort == " + location.COTOSort + ")",
                                strSpace + strSpace + strSpace + strSpace + "prev = current;",
                                strSpace + strSpace + strSpace + "if(prev == null) flag = false;",
                                strSpace + strSpace + "}",
                                strSpace + strSpace + "else flag = false;",
                            });
                        }
                    }
                    else
                    {
                        if (location.IsLastEXEmptyStock == true)
                        {
                            lstCheckData.AddRange(new List<string>
                            {
                                strSpace + strSpace + "current = data.ListLocation.find(function(o){ return o.OPSContainerID == " + location.OPSContainerID +
                                    " && o.LocationFromID == " + location.LocationFromID + " && o.LocationToID == " + location.LocationToID +
                                    " && o.SortFrom == " + location.SortFrom + " && o.SortTo == " + location.SortTo +
                                    " && o.COTOSort == " + location.COTOSort + "; });",
                                strSpace + strSpace + "if(current == null) flag = false;",
                                strSpace + strSpace + "else {",
                                strSpace + strSpace + strSpace + "var strswap = current.IsSwap + \"\";",
                                strSpace + strSpace + strSpace + "if(strswap != \"true\") flag = false;",
                                strSpace + strSpace + "}",
                            });
                        }
                        else
                        {
                            lstCheckData.AddRange(new List<string>
                            {
                                strSpace + strSpace + "current = data.ListLocation.find(function(o){ return o.OPSContainerID == " + location.OPSContainerID +
                                    " && o.LocationFromID == " + location.LocationFromID + " && o.LocationToID == " + location.LocationToID +
                                    " && o.SortFrom == " + location.SortFrom + " && o.SortTo == " + location.SortTo +
                                    " && o.COTOSort == " + location.COTOSort + "; });",
                                strSpace + strSpace + "if(current == null) flag = false;",
                            });
                        }
                    }

                    i++;
                }
                #endregion

                itemEvent.script.exec.AddRange(lstCheckData);

                itemEvent.script.exec.AddRange(new List<string>()
                {
                    strSpace + "}",
                    strSpace + "pm.expect(flag).to.equal(true);",
                    "});"
                });

                itemAdd._event.Add(itemEvent);

                itemAdd.request = new PMCollection_Request
                {
                    method = "POST",
                    header = new List<PMCollection_RequestHeader>(),
                    body = new PMCollection_RequestBody
                    {
                        mode = "raw",
                        raw = "{\"masterid\":" + itemMaster.ID + "}"
                    },
                    url = new PMCollection_RequestURL
                    {
                        raw = hosttest + "/api/MON/MONCON_PopupInfo_COTOContainer_StartCheck",
                        protocol = "http",
                        port = port,
                        host = new List<string>() { host },
                        path = new List<string>() { "api", "MON", "MONCON_PopupInfo_COTOContainer_StartCheck" }
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
        }

        private static void GenCOTOContainerStart(PMCollection objCollect, string hosttest, string strSpace, string name, CaseMaster itemMaster)
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
            itemAdd.name = name;
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
            itemEvent.script.exec = new List<string>()
            {
                "pm.test(\"" + name + "\", function () {",
                strSpace + "pm.expect(pm.response.code).to.be.oneOf([200,204]);",
                "});"
            };
            itemAdd._event.Add(itemEvent);

            itemAdd.request = new PMCollection_Request
            {
                method = "POST",
                header = new List<PMCollection_RequestHeader>(),
                body = new PMCollection_RequestBody
                {
                    mode = "raw",
                    raw = "{{lstLocation}}"
                },
                url = new PMCollection_RequestURL
                {
                    raw = hosttest + "/api/MON/MONCON_PopupInfo_COTOContainer_Start",
                    protocol = "http",
                    port = port,
                    host = new List<string>() { host },
                    path = new List<string>() { "api", "MON", "MONCON_PopupInfo_COTOContainer_Start" }
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

        private static void GenCOTOContainerBreakmooc(PMCollection objCollect, string hosttest, string strSpace, string name, CaseMaster itemMaster)
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
            itemAdd.name = name;
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
            itemEvent.script.exec = new List<string>()
            {
                "pm.test(\"" + name + "\", function () {",
                strSpace + "pm.response.to.have.status(204);",
                "});"
            };
            itemAdd._event.Add(itemEvent);

            itemAdd.request = new PMCollection_Request
            {
                method = "POST",
                header = new List<PMCollection_RequestHeader>(),
                body = new PMCollection_RequestBody
                {
                    mode = "raw",
                    raw = "{{actionBreakmooc}}"
                },
                url = new PMCollection_RequestURL
                {
                    raw = hosttest + "/api/MON/MONCON_PopupInfo_COTOContainer_Action",
                    protocol = "http",
                    port = port,
                    host = new List<string>() { host },
                    path = new List<string>() { "api", "MON", "MONCON_PopupInfo_COTOContainer_Action" }
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

            GenCOTOContainerATD(objCollect, hosttest, strSpace, name + "_atd", itemMaster);
        }

        private static void GenCOTOContainerComplete(PMCollection objCollect, string hosttest, string strSpace, string name, CaseMaster itemMaster)
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
            itemAdd.name = name;
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
            itemEvent.script.exec = new List<string>()
            {
                "pm.test(\"" + name + "\", function () {",
                strSpace + "pm.expect(pm.response.code).to.be.oneOf([200,204]);",
                "});"
            };
            itemAdd._event.Add(itemEvent);

            itemAdd.request = new PMCollection_Request
            {
                method = "POST",
                header = new List<PMCollection_RequestHeader>(),
                body = new PMCollection_RequestBody
                {
                    mode = "raw",
                    raw = "{{actionComplete}}"
                },
                url = new PMCollection_RequestURL
                {
                    raw = hosttest + "/api/MON/MONCON_PopupInfo_COTOContainer_Action",
                    protocol = "http",
                    port = port,
                    host = new List<string>() { host },
                    path = new List<string>() { "api", "MON", "MONCON_PopupInfo_COTOContainer_Action" }
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

            GenCOTOContainerATD(objCollect, hosttest, strSpace, name + "_atd", itemMaster);
        }

        static DateTime ATD = DateTime.Now;

        private static void GenCOTOContainerATD(PMCollection objCollect, string hosttest, string strSpace, string name, CaseMaster itemMaster)
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
            itemAdd.name = name;
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
            itemEvent.script.exec = new List<string>()
            {
                "pm.test(\"" + name + "\", function () {",
                strSpace + "pm.expect(pm.response.code).to.be.oneOf([200,204]);",
                "});"
            };
            itemAdd._event.Add(itemEvent);

            ATD = ATD.AddHours(1);
            itemAdd.request = new PMCollection_Request
            {
                method = "POST",
                header = new List<PMCollection_RequestHeader>(),
                body = new PMCollection_RequestBody
                {
                    mode = "raw",
                    raw = "{\"action\":{\"ActionType\":\"ATD\",\"COTOMasterID\":" + itemMaster.ID + "," +
                        "\"ETD\":\"" + ATD.ToString("yyyy-MM-dd HH:mm:ss") + "\"," +
                        "\"ETA\":\"" + ATD.AddHours(0.5).ToString("yyyy-MM-dd HH:mm:ss") + "\"," +
                        "\"ATD\":\"" + ATD.ToString("yyyy-MM-dd HH:mm:ss") + "\","+
                        "\"ATA\":\"" + ATD.AddHours(0.5).ToString("yyyy-MM-dd HH:mm:ss") + "\"}}"
                },
                url = new PMCollection_RequestURL
                {
                    raw = hosttest + "/api/MON/MON_Container_COM_PopupInfo_Master_Action",
                    protocol = "http",
                    port = port,
                    host = new List<string>() { host },
                    path = new List<string>() { "api", "MON", "MON_Container_COM_PopupInfo_Master_Action" }
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

        private static void GenCheckMasterLocation(PMCollection objCollect, string hosttest, string strSpace, string name, CaseMaster itemMaster)
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
            itemAdd.name = name;
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
            itemEvent.script.exec = new List<string>()
            {
                "pm.test(\"" + name + "\", function () {",
                strSpace + "var data = pm.response.json();",
                strSpace + "var flag = false;",
                strSpace + "pm.environment.set(\"lstLocation\",\"{}\");",
                strSpace + "if(data != undefined && data != null && data.length > 0){",
                strSpace + strSpace + "data = data.map(function(o){ return {ID:o.ID,LocationID:o.LocationID,SortOrder:o.SortOrder,SortPrev:o.SortPrev,TypeOfTOLocationID:o.TypeOfTOLocationID,SortOrderReal:-1 }; });",
                //strSpace + strSpace + "console.log(JSON.stringify(data));",
                strSpace + strSpace + "flag = true;",
                strSpace + strSpace + "var i = 0;",
                strSpace + strSpace + "var current = null;",
            };
            foreach (var itemLocation in itemMaster.ListLocation)
            {

                itemEvent.script.exec.AddRange(new List<string>
                {
                    strSpace + strSpace + "current = data.find(function(o){ return o.LocationID == " + itemLocation.ID + " && o.SortOrder == " + itemLocation.SortOrder + " && o.SortPrev == " + itemLocation.SortPrev + " && o.TypeOfTOLocationID == " + itemLocation.TypeID + "; });",
                    strSpace + strSpace + "if(current != null) current.SortOrderReal = " + itemLocation.SortReal + ";",
                    strSpace + strSpace + "else flag = false;",

                    //strSpace + strSpace + "current = data[i];i++;",
                    //strSpace + strSpace + "if(current.LocationID !== " + itemLocation.ID + " || current.SortOrder !== " + itemLocation.SortOrder + 
                    //    " || current.SortPrev !== " + itemLocation.SortPrev + " || current.TypeOfTOLocationID !== " + itemLocation.TypeID + ")",
                    //strSpace + strSpace + strSpace + "flag = false;",
                });
            }
            itemEvent.script.exec.AddRange(new List<string>
            {
                strSpace + strSpace + "if(flag == true) pm.environment.set(\"lstLocation\", JSON.stringify({ tocontainerid:pm.environment.get(\"id\"), lstlocation:data }));",
                strSpace + "}",
                strSpace + "pm.expect(flag).to.equal(true);",
                "});"
            });
            itemAdd._event.Add(itemEvent);

            itemAdd.request = new PMCollection_Request
            {
                method = "POST",
                header = new List<PMCollection_RequestHeader>(),
                body = new PMCollection_RequestBody
                {
                    mode = "raw",
                    raw = "{\"masterid\":" + itemMaster.ID + "}"
                },
                url = new PMCollection_RequestURL
                {
                    raw = hosttest + "/api/OPS/OPSCON_PopupTOMaster_ListLocation",
                    protocol = "http",
                    port = port,
                    host = new List<string>() { host },
                    path = new List<string>() { "api", "OPS", "OPSCON_PopupTOMaster_ListLocation" }
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
    }
}
