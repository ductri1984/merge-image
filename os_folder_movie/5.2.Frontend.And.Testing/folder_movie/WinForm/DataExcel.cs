using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WinForm
{
    public class DataExcel
    {
        public const string SheetMovieName = "Movie";
        public const string SheetNewName = "New";
        public const string Temp = "temp.json";

        public string FileTemplatePath { get; set; }
        public string FileTemplateNewPath { get; set; }
        public string FolderMoviePath { get; set; }
        public string FileJsonPath { get; set; }

        public string FolderAPIPath { get; set; }
        public string FileAPIConfigPath { get; set; }
        public string FileAPIHelpPath { get; set; }
        public string APIVersion { get; set; }
        public string FileCollectionPath { get; set; }
        public string FileEnvironmentPath { get; set; }
        private List<FileTemp> _lstTemp = new List<FileTemp>();
        private string _fileTemp = "";
        private List<DTO> _lstDTO = new List<DTO>();
        private List<API> _lstAPI = new List<API>();
        private List<APITesting> _lstAPITesting = new List<APITesting>();
        private string _strAPIConfigStart = "";
        private string _strAPIConfigEnd = "";

        public void SetApplicationFolder(string folderapp)
        {
            if (!string.IsNullOrEmpty(folderapp))
            {
                if (folderapp[folderapp.Length - 1] != '\\')
                    folderapp += "\\";
                _fileTemp = folderapp + Temp;
                if (System.IO.File.Exists(_fileTemp))
                {
                    var sr = new System.IO.StreamReader(_fileTemp);
                    _lstTemp = Newtonsoft.Json.JsonConvert.DeserializeObject<List<FileTemp>>(sr.ReadToEnd());
                    sr.Close();

                    var last = _lstTemp.Select(c => new
                    {
                        c.FileTemplate,
                        c.FolderDTO,
                        c.FolderAPI,
                        c.FileAPIConfig,
                        c.FileAPIHelp,
                        c.APIVersion,
                        c.FileTestCollection,
                        c.FileTestEnvironment,
                        c.ModifiedDate
                    }).OrderByDescending(c => c.ModifiedDate).FirstOrDefault();
                    if (last != null)
                    {
                        FileTemplatePath = last.FileTemplate;
                        FolderDTOPath = last.FolderDTO;
                        FolderAPIPath = last.FolderAPI;
                        FileAPIConfigPath = last.FileAPIConfig;
                        FileAPIHelpPath = last.FileAPIHelp;
                        APIVersion = last.APIVersion;
                        FileCollectionPath = last.FileTestCollection;
                        FileEnvironmentPath = last.FileTestEnvironment;
                    }
                }
            }
        }

        public void LoadTemplate()
        {
            if (string.IsNullOrEmpty(FileTemplatePath))
                throw new Exception("FileTemplatePath not exists");

            _lstDTO.Clear();
            _lstAPI.Clear();
            _lstAPITesting.Clear();

            using (var package = new OfficeOpenXml.ExcelPackage(new System.IO.FileInfo(FileTemplatePath)))
            {
                var worksheet = HelperExcel.GetWorksheetByName(package, SheetDTOName);
                #region SheetDTOName
                if (worksheet != null)
                {
                    int row = 1, col = 1, rowMax = 1000;
                    var itemAdd = default(DTO);
                    for (row = 3; row <= worksheet.Dimension.End.Row && row < rowMax; row++)
                    {
                        col = 2;
                        var strNameSpace = HelperExcel.GetValue(worksheet, row, col);
                        col++;
                        var strSubFolder = HelperExcel.GetValue(worksheet, row, col);
                        col++;
                        var strObjectName = HelperExcel.GetValue(worksheet, row, col);
                        col++;
                        var strPropertyName = HelperExcel.GetValue(worksheet, row, col);
                        col++;
                        var strPropertyType = HelperExcel.GetValue(worksheet, row, col);
                        col++;
                        var strPropertyNull = HelperExcel.GetValue(worksheet, row, col);
                        col++;
                        var strPropertyMin = HelperExcel.GetValue(worksheet, row, col);
                        col++;
                        var strPropertyMax = HelperExcel.GetValue(worksheet, row, col);
                        col++;
                        var strInheritance = HelperExcel.GetValue(worksheet, row, col);
                        col++;
                        var strObjectDescriptionVN = HelperExcel.GetValue(worksheet, row, col);
                        col++;
                        var strObjectDescriptionEN = HelperExcel.GetValue(worksheet, row, col);
                        col++;
                        var strPropertyDescriptionVN = HelperExcel.GetValue(worksheet, row, col);
                        col++;
                        var strPropertyDescriptionEN = HelperExcel.GetValue(worksheet, row, col);

                        if ((!string.IsNullOrEmpty(strObjectName) || itemAdd != null) && !string.IsNullOrEmpty(strPropertyName))
                        {
                            if (itemAdd == null || (!string.IsNullOrEmpty(strObjectName) && itemAdd.ObjectName != strObjectName))
                            {
                                itemAdd = new DTO();
                                itemAdd.NameSpace = strNameSpace;
                                itemAdd.SubFolder = strSubFolder;
                                itemAdd.ObjectName = strObjectName;
                                itemAdd.Inheritance = strInheritance;
                                itemAdd.DescriptionVN = strObjectDescriptionVN;
                                itemAdd.DescriptionEN = strObjectDescriptionEN;
                                itemAdd.Properties = new List<DTOProperty>();
                                _lstDTO.Add(itemAdd);
                            }
                            var itemProperty = new DTOProperty();
                            itemProperty.Name = strPropertyName;
                            itemProperty.TypeCode = strPropertyType;
                            itemProperty.AllowNull = strPropertyNull.ToLower() == "true";
                            if (!string.IsNullOrEmpty(strPropertyMin))
                                itemProperty.Min = Convert.ToDouble(strPropertyMin);
                            if (!string.IsNullOrEmpty(strPropertyMax))
                                itemProperty.Max = Convert.ToDouble(strPropertyMax);
                            itemProperty.DescriptionVN = strPropertyDescriptionVN;
                            itemProperty.DescriptionEN = strPropertyDescriptionEN;
                            itemAdd.Properties.Add(itemProperty);
                        }
                    }
                }
                #endregion

                worksheet = HelperExcel.GetWorksheetByName(package, SheetAPIName);
                #region SheetAPIName
                if (worksheet != null)
                {
                    int row = 1, col = 1, rowMax = 1000;
                    for (row = 3; row <= worksheet.Dimension.End.Row && row < rowMax; row++)
                    {
                        col = 2;
                        var strNameSpace = HelperExcel.GetValue(worksheet, row, col);
                        col++;
                        var strSubFolder = HelperExcel.GetValue(worksheet, row, col);
                        col++;
                        var strCode = HelperExcel.GetValue(worksheet, row, col);
                        col++;
                        var strController = HelperExcel.GetValue(worksheet, row, col);
                        col++;
                        var strMethod = HelperExcel.GetValue(worksheet, row, col);
                        col++;
                        var strIsGet = HelperExcel.GetValue(worksheet, row, col);
                        col++;
                        var strParamObject = HelperExcel.GetValue(worksheet, row, col);
                        col++;
                        var strParamList = HelperExcel.GetValue(worksheet, row, col);
                        col++;
                        var strResultObject = HelperExcel.GetValue(worksheet, row, col);
                        col++;
                        var strResultList = HelperExcel.GetValue(worksheet, row, col);
                        col++;
                        var strTables = HelperExcel.GetValue(worksheet, row, col);
                        col++;
                        var strWCF = HelperExcel.GetValue(worksheet, row, col);
                        col++;
                        var strDescriptionVN = HelperExcel.GetValue(worksheet, row, col);
                        col++;
                        var strDescriptionEN = HelperExcel.GetValue(worksheet, row, col);

                        if (!string.IsNullOrEmpty(strCode) && !string.IsNullOrEmpty(strController) && !string.IsNullOrEmpty(strMethod))
                        {
                            var itemAdd = new API();
                            itemAdd.NameSpace = strNameSpace;
                            itemAdd.SubFolder = strSubFolder;
                            itemAdd.Code = strCode;
                            itemAdd.Controller = strController;
                            itemAdd.MethodName = strMethod;
                            itemAdd.IsGet = strIsGet.ToLower() == "true";
                            itemAdd.ParamObject = strParamObject;
                            itemAdd.ParamList = strParamList;
                            itemAdd.ResultObject = strResultObject;
                            itemAdd.ResultList = strResultList;
                            if (!string.IsNullOrEmpty(strTables))
                                itemAdd.Tables = strTables.Split(',').ToList();
                            else
                                itemAdd.Tables = new List<string>();
                            itemAdd.IsWCF = strWCF.ToLower() == "true";
                            itemAdd.DescriptionVN = strDescriptionVN;
                            itemAdd.DescriptionEN = strDescriptionEN;
                            _lstAPI.Add(itemAdd);
                        }
                    }
                }
                #endregion

                worksheet = HelperExcel.GetWorksheetByName(package, SheetAPITestingName);
                #region SheetAPITestingName
                if (worksheet != null)
                {
                    int row = 1, col = 1, rowMax = 1000;
                    for (row = 3; row <= worksheet.Dimension.End.Row && row < rowMax; row++)
                    {
                        col = 2;
                        var strCode = HelperExcel.GetValue(worksheet, row, col);
                        col++;
                        var strAPICode = HelperExcel.GetValue(worksheet, row, col);
                        col++;
                        var strParam = HelperExcel.GetValue(worksheet, row, col);
                        col++;
                        var strResult = HelperExcel.GetValue(worksheet, row, col);
                        col++;
                        var strReturnLength = HelperExcel.GetValue(worksheet, row, col);
                        col++;
                        var strReturnProperty = HelperExcel.GetValue(worksheet, row, col);
                        col++;
                        var strReturnPropertyValue = HelperExcel.GetValue(worksheet, row, col);
                        col++;
                        var strExceptionMessage = HelperExcel.GetValue(worksheet, row, col);
                        col++;
                        var strPostmanResult = HelperExcel.GetValue(worksheet, row, col);
                        col++;
                        var strPostmanScript = HelperExcel.GetValue(worksheet, row, col);
                        col++;
                        var strDescription = HelperExcel.GetValue(worksheet, row, col);

                        if (!string.IsNullOrEmpty(strCode) && !string.IsNullOrEmpty(strAPICode))
                        {
                            var itemAdd = new APITesting();
                            itemAdd.Code = strCode;
                            itemAdd.APICode = strAPICode;
                            itemAdd.Param = strParam;
                            itemAdd.Result = strResult;
                            itemAdd.ReturnProperty = strReturnProperty;
                            itemAdd.ReturnPropertyValue = strReturnPropertyValue;
                            itemAdd.ExceptionMessage = strExceptionMessage;
                            itemAdd.PostmanResult = strPostmanResult;
                            itemAdd.PostmanScript = strPostmanScript;
                            itemAdd.Description = strDescription;

                            _lstAPITesting.Add(itemAdd);
                        }
                    }
                }
                #endregion
            }
        }

        public System.Data.DataTable DTO_GetTable()
        {
            var dt = new System.Data.DataTable();

            dt.Columns.Add(new System.Data.DataColumn("ObjectName", typeof(string)));
            dt.Columns.Add(new System.Data.DataColumn("PropertyName", typeof(string)));
            dt.Columns.Add(new System.Data.DataColumn("TypeCode", typeof(string)));
            dt.Columns.Add(new System.Data.DataColumn("AllowNull", typeof(bool)));
            dt.Columns.Add(new System.Data.DataColumn("Min", typeof(double)));
            dt.Columns.Add(new System.Data.DataColumn("Max", typeof(double)));
            dt.Columns.Add(new System.Data.DataColumn("DescriptionVN", typeof(string)));
            dt.Columns.Add(new System.Data.DataColumn("DescriptionEN", typeof(string)));

            foreach (var item in _lstDTO)
            {
                foreach (var detail in item.Properties)
                {
                    var dr = dt.NewRow();
                    dr["ObjectName"] = item.ObjectName;
                    dr["PropertyName"] = detail.Name;
                    dr["TypeCode"] = detail.TypeCode;
                    dr["AllowNull"] = detail.AllowNull;
                    if (detail.Min != null)
                        dr["Min"] = detail.Min;
                    if (detail.Max != null)
                        dr["Max"] = detail.Max;
                    dr["DescriptionVN"] = detail.DescriptionVN;
                    dr["DescriptionEN"] = detail.DescriptionEN;
                    dt.Rows.Add(dr);
                }
            }

            return dt;
        }

        public System.Data.DataTable API_GetTable()
        {
            var dt = new System.Data.DataTable();

            dt.Columns.Add(new System.Data.DataColumn("Code", typeof(string)));
            dt.Columns.Add(new System.Data.DataColumn("Controller", typeof(string)));
            dt.Columns.Add(new System.Data.DataColumn("MethodName", typeof(string)));
            dt.Columns.Add(new System.Data.DataColumn("IsGet", typeof(bool)));
            dt.Columns.Add(new System.Data.DataColumn("ParamObject", typeof(string)));
            dt.Columns.Add(new System.Data.DataColumn("ParamList", typeof(string)));
            dt.Columns.Add(new System.Data.DataColumn("ResultObject", typeof(string)));
            dt.Columns.Add(new System.Data.DataColumn("ResultList", typeof(string)));
            dt.Columns.Add(new System.Data.DataColumn("Tables", typeof(string)));

            foreach (var item in _lstAPI)
            {
                var dr = dt.NewRow();
                dr["Code"] = item.Code;
                dr["Controller"] = item.Controller;
                dr["MethodName"] = item.MethodName;
                dr["IsGet"] = item.IsGet;
                dr["ParamObject"] = item.ParamObject;
                dr["ParamList"] = item.ParamList;
                dr["ResultObject"] = item.ResultObject;
                dr["ResultList"] = item.ResultList;
                dr["Tables"] = string.Join(",", item.Tables);
                dt.Rows.Add(dr);
            }

            return dt;
        }

        public void GenData()
        {
            if (string.IsNullOrEmpty(FolderDTOPath) || string.IsNullOrEmpty(FolderAPIPath))
                throw new Exception("FolderDTOPath,FolderAPIPath not exists");

            var itemTemp = _lstTemp.FirstOrDefault(c => c.FileTemplate == FileTemplatePath);
            if (itemTemp == null)
            {
                itemTemp = new FileTemp();
                itemTemp.FileTemplate = FileTemplatePath;
                _lstTemp.Add(itemTemp);
            }
            itemTemp.FolderDTO = FolderDTOPath;
            itemTemp.FolderAPI = FolderAPIPath;
            itemTemp.FileAPIConfig = FileAPIConfigPath;
            itemTemp.FileTestCollection = FileCollectionPath;
            itemTemp.FileTestEnvironment = FileEnvironmentPath;
            itemTemp.FileAPIHelp = FileAPIHelpPath;
            itemTemp.APIVersion = APIVersion;
            itemTemp.ModifiedDate = DateTime.Now;
            if (_lstTemp != null && _lstTemp.Count > 0 && !string.IsNullOrEmpty(_fileTemp))
            {
                var sw = new System.IO.StreamWriter(_fileTemp, false);
                sw.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(_lstTemp));
                sw.Close();
            }


            StringBuilder sbAPIConfig = new StringBuilder();
            _strAPIConfigStart = string.Empty;
            _strAPIConfigEnd = string.Empty;
            if (!string.IsNullOrEmpty(FileAPIConfigPath))
            {
                if (System.IO.File.Exists(FileAPIConfigPath))
                {
                    string strAPIConfigContent = System.IO.File.ReadAllText(FileAPIConfigPath);
                    _strAPIConfigStart = strAPIConfigContent.Substring(0, strAPIConfigContent.IndexOf(CodingStart));
                    _strAPIConfigEnd = strAPIConfigContent.Substring(strAPIConfigContent.IndexOf(CodingEnd) + CodingEnd.Length);
                }
            }


            foreach (var item in _lstDTO)
            {
                string pathdto = FolderDTOPath + "\\" + item.ObjectName + ".cs";
                if (!string.IsNullOrEmpty(item.SubFolder))
                {
                    string subPath = FolderDTOPath + "\\" + item.SubFolder;
                    if (!System.IO.Directory.Exists(subPath))
                        System.IO.Directory.CreateDirectory(subPath);
                    pathdto = subPath + "\\" + item.ObjectName + ".cs";
                }

                string strDTOContent = "";
                if (System.IO.File.Exists(pathdto))
                    strDTOContent = System.IO.File.ReadAllText(pathdto);

                string strUsing = "";
                if (!string.IsNullOrEmpty(strDTOContent))
                {
                    if (strDTOContent.IndexOf(UsingStart) >= 0 && strDTOContent.IndexOf(UsingEnd) > 0)
                        strUsing = strDTOContent.Substring(strDTOContent.IndexOf(UsingStart) + UsingStart.Length, strDTOContent.IndexOf(UsingEnd) - (strDTOContent.IndexOf(UsingStart) + UsingStart.Length));
                    if (strUsing.Length > 1)
                    {
                        strUsing = UsingStart + strUsing + UsingEnd + nl + nl;
                    }
                }

                StringBuilder sb = new StringBuilder();
                if (!string.IsNullOrEmpty(strUsing))
                    sb.Append(strUsing);
                else
                    GenData_Using(sb, false);
                GenData_DTOStart(sb, item);
                GenData_DTOPropety(sb, item);
                GenData_DTOEnd(sb, item);

                var sw = new System.IO.StreamWriter(pathdto, false);
                sw.WriteLine(sb);
                sw.Close();
            }

            foreach (var itemGroup in _lstAPI.Where(c => c.IsWCF != true).GroupBy(c => c.Controller))
            {
                var itemFirst = itemGroup.FirstOrDefault();

                string pathapi = FolderAPIPath + "\\" + itemFirst.Controller + ".cs";
                if (!string.IsNullOrEmpty(itemFirst.SubFolder))
                {
                    string subPath = FolderAPIPath + "\\" + itemFirst.SubFolder;
                    if (!System.IO.Directory.Exists(subPath))
                        System.IO.Directory.CreateDirectory(subPath);
                    pathapi = subPath + "\\" + itemFirst.Controller + ".cs";
                }

                string strAPIContent = "";
                if (System.IO.File.Exists(pathapi))
                    strAPIContent = System.IO.File.ReadAllText(pathapi);

                string strCoding = "";
                if (!string.IsNullOrEmpty(strAPIContent))
                {
                    if (strAPIContent.IndexOf(CodingStart) >= 0 && strAPIContent.IndexOf(CodingEnd) > 0)
                        strCoding = strAPIContent.Substring(strAPIContent.IndexOf(CodingStart) + CodingStart.Length, strAPIContent.IndexOf(CodingEnd) - (strAPIContent.IndexOf(CodingStart) + CodingStart.Length));
                    else
                        strCoding = "";
                    if (strCoding.Length > 1 && strCoding.Substring(strCoding.Length - t2.Length) == t2)
                    {
                        strCoding = strCoding.Substring(0, strCoding.Length - t2.Length);
                        strCoding = t2 + CodingStart + strCoding + t2 + CodingEnd + nl + nl;
                    }
                }

                string strUsing = "";
                if (!string.IsNullOrEmpty(strAPIContent))
                {
                    if (strAPIContent.IndexOf(UsingStart) >= 0 && strAPIContent.IndexOf(UsingEnd) > 0)
                        strUsing = strAPIContent.Substring(strAPIContent.IndexOf(UsingStart) + UsingStart.Length, strAPIContent.IndexOf(UsingEnd) - (strAPIContent.IndexOf(UsingStart) + UsingStart.Length));
                    if (strUsing.Length > 1)
                    {
                        strUsing = UsingStart + strUsing + UsingEnd + nl + nl;
                    }
                }

                StringBuilder sb = new StringBuilder();
                if (!string.IsNullOrEmpty(strUsing))
                    sb.Append(strUsing);
                else
                    GenData_Using(sb, true);
                GenData_APIStart(sb, itemFirst);
                int indexCoding = sb.Length;
                GenData_APIBody(sb, itemGroup.ToList(), ref strCoding);
                if (string.IsNullOrEmpty(strCoding))
                    strCoding = t2 + CodingStart + nl + t2 + CodingEnd + nl + nl;
                sb.Insert(indexCoding, strCoding);
                GenData_APIEnd(sb, itemFirst);

                var sw = new System.IO.StreamWriter(pathapi, false);
                sw.WriteLine(sb);
                sw.Close();

                GenData_APIConfig(sbAPIConfig, itemGroup.ToList());
            }

            if (!string.IsNullOrEmpty(FileAPIConfigPath))
            {
                sbAPIConfig.Insert(0,
                    _strAPIConfigStart +
                    CodingStart + nl
                    );
                sbAPIConfig.AppendLine(t3 + CodingEnd + _strAPIConfigEnd);
                var sw = new System.IO.StreamWriter(FileAPIConfigPath, false);
                sw.WriteLine(sbAPIConfig);
                sw.Close();
            }

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
            var objEnvir = new PMEnvir
            {
                id = "99149ee0-5ddc-4166-b454-f5d5345b621e",
                name = "test",
                values = new List<PMEnvir_Value>(),
                _postman_variable_scope = "environment",
                _postman_exported_at = "2018-04-26T08:50:11.540Z",
                _postman_exported_using = "Postman/6.0.10"
            };
            objEnvir.values.Add(new PMEnvir_Value
            {
                key = "URL",
                value = "localhost:15001/APICurrent",
                enabled = true,
                type = "text"
            });
            foreach (var item in _lstAPITesting)
            {
                var itemAPI = _lstAPI.FirstOrDefault(c => c.Code == item.APICode);
                var itemAdd = new PMCollection_Item();
                itemAdd.name = item.Code;
                itemAdd.response = new List<string>();
                itemAdd._event = new List<PMCollection_Event>();
                Gen_APITestingCollectionEvent(itemAdd, item);
                string strEnvirKeyParam = item.Code + "_param";
                itemAdd.request = new PMCollection_Request
                {
                    method = "POST",
                    header = new List<PMCollection_RequestHeader>(),
                    body = new PMCollection_RequestBody
                    {
                        mode = "raw",
                        raw = "{{" + strEnvirKeyParam + "}}"
                    },
                    url = new PMCollection_RequestURL
                    {
                        raw = "http://{{URL}}/api/" + itemAPI.Controller + "/" + itemAPI.MethodName,
                        protocol = "http",
                        host = new List<string>() { "{{URL}}" },
                        path = new List<string>() { "api", itemAPI.Controller, itemAPI.MethodName }
                    }
                };
                itemAdd.request.header.Add(new PMCollection_RequestHeader
                {
                    key = "Content-Type",
                    value = "application/json"
                });
                objCollect.item.Add(itemAdd);

                Gen_APITestingEnvironmentValue(objEnvir, item);
            }

            if (!string.IsNullOrEmpty(FileCollectionPath) && !string.IsNullOrEmpty(FileEnvironmentPath))
            {
                var sw = new System.IO.StreamWriter(FileCollectionPath, false);
                sw.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(objCollect));
                sw.Close();

                sw = new System.IO.StreamWriter(FileEnvironmentPath, false);
                sw.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(objEnvir));
                sw.Close();
            }

            if (!string.IsNullOrEmpty(FileAPIHelpPath))
            {
                var objHelp = new APIHelp
                {
                    Version = APIVersion,
                    ListAPI = _lstAPI.Select(c => new APIHelp_URL
                    {
                        URL = c.Code,
                        Type = "POST",
                        DescriptionVN = c.DescriptionVN,
                        DescriptionEN = c.DescriptionEN,
                        ListRequest = new List<APIHelp_Request>()
                    }).ToList()
                };
                foreach (var item in objHelp.ListAPI)
                {
                    var itemAPI = _lstAPI.FirstOrDefault(c => c.Code == item.URL);
                    if (itemAPI != null)
                    {
                        item.Sample = "{}";
                        if (!string.IsNullOrEmpty(itemAPI.ParamObject))
                        {
                            var dto = _lstDTO.FirstOrDefault(c => c.ObjectName == itemAPI.ParamObject);
                            if (dto != null)
                            {
                                item.RequestDescriptionVN = dto.DescriptionVN;
                                item.RequestDescriptionEN = dto.DescriptionEN;

                                item.ListRequest = dto.Properties.Select(c => new APIHelp_Request
                                {
                                    Name = c.Name,
                                    Type = c.TypeCode,
                                    DescriptionVN = c.DescriptionVN,
                                    DescriptionEN = c.DescriptionEN
                                }).ToList();
                            }
                        }
                    }
                }
                var sw = new System.IO.StreamWriter(FileAPIHelpPath, false);
                sw.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(objHelp));
                sw.Close();
            }
        }

        private void GenData_Using(StringBuilder sb, bool isAPI)
        {
            if (!isAPI)
            {
                sb.AppendLine(
                    UsingStart + nl +
                    "using System;" + nl +
                    "using System.Collections.Generic;" + nl +
                    "using System.Linq;" + nl +
                    "using System.Text;" + nl +
                    UsingEnd + nl + nl
                );
            }
            else
            {
                sb.AppendLine(
                    UsingStart + nl +
                    "using System;" + nl +
                    "using System.Collections.Generic;" + nl +
                    "using System.Linq;" + nl +
                    "using System.Web;" + nl +
                    "using System.Web.Http;" + nl +
                    "using System.Reflection;" + nl +
                    "using Newtonsoft.Json;" + nl +
                    "using System.Text;" + nl +
                    "using Kendo.Mvc.UI;" + nl +
                    "using Kendo.Mvc.Extensions;" + nl +
                    "using System.Data.Services.Client;" + nl +
                    "using APIBusiness.ModelReference;" + nl +
                    "using DTO;" + nl +
                    "using APIBusiness.Helper;" + nl +
                    UsingEnd + nl + nl
                );
            }
        }

        private void GenData_DTOStart(StringBuilder sb, DTO item)
        {
            string strNS = item.NameSpace;
            string strInheritance = !string.IsNullOrEmpty(item.Inheritance) ? " : " + item.Inheritance : "";
            sb.AppendLine(
                "namespace " + strNS + nl +
                "{" + nl +
                t1 + "public class " + item.ObjectName + strInheritance + nl +
                t1 + "{"
            );
        }

        private void GenData_DTOEnd(StringBuilder sb, DTO item)
        {
            sb.AppendLine(
                t1 + "}" + nl +
                "}"
            );
        }

        private void GenData_DTOPropety(StringBuilder sb, DTO item)
        {
            foreach (var detail in item.Properties)
            {
                string strNull = "";
                if (detail.AllowNull == true && detail.TypeCode != "string")
                    strNull = "?";
                sb.AppendLine(t2 + "public " + detail.TypeCode + strNull + " " + detail.Name + " { get; set; }");
            }
        }

        private void GenData_APIStart(StringBuilder sb, API item)
        {
            string strNS = item.NameSpace;
            string strBase = "Base";
            string strController = item.Controller + "Controller";

            sb.AppendLine(
                "namespace " + strNS + nl +
                "{" + nl +
                t1 + "public class " + strController + " : " + strBase + "\r\n" +
                t1 + "{"
            );
        }

        private void GenData_APIEnd(StringBuilder sb, API item)
        {
            sb.AppendLine(
                t1 + "}" + nl +
                "}" + nl
            );
        }

        private void GenData_APIBody(StringBuilder sb, List<API> lst, ref string strCoding)
        {
            foreach (var item in lst)
            {
                string strAPIReturn = "void";
                string strAPIThrow = "";
                string strCacheReturn = t4 + "HelperCache.APISet(() =>" + nl;
                if (!string.IsNullOrEmpty(item.ResultObject))
                {
                    strAPIReturn = item.ResultObject;
                    strAPIThrow = nl + t5 + "throw new Exception(\"no case\");" + nl;
                    strCacheReturn = item.IsGet ?
                        t4 + "return HelperCache.APIGet<" + item.ResultObject + ">(() =>" + nl :
                        t4 + "return HelperCache.APISet<" + item.ResultObject + ">(() =>" + nl;
                }
                else if (!string.IsNullOrEmpty(item.ResultList))
                {
                    strAPIReturn = "List<" + item.ResultList + ">";
                    strAPIThrow = nl + t5 + "throw new Exception(\"no case\");" + nl;
                    strCacheReturn = item.IsGet ?
                        t4 + "return HelperCache.APIGet<List<" + item.ResultList + ">>(() =>" + nl :
                        t4 + "return HelperCache.APISet<List<" + item.ResultList + ">>(() =>" + nl;
                }
                string strParam = !string.IsNullOrEmpty(item.ParamObject) ?
                    t5 + item.ParamObject + " reqdata = JsonConvert.DeserializeObject<" + item.ParamObject + ">(dynParam.reqdata.ToString());" + nl + nl :
                    !string.IsNullOrEmpty(item.ParamList) ?
                    t5 + "List<" + item.ParamList + "> reqdata = JsonConvert.DeserializeObject<List<" + item.ParamList + ">>(dynParam.reqdata.ToString());" + nl + nl : "";

                int codeIndex = strCoding.IndexOf(" " + item.MethodName + "(dynamic dynParam)");
                if (codeIndex > 0)
                {
                    string strCacheFind = "HelperCache.APISet";
                    strCacheReturn = "HelperCache.APISet(() =>";
                    if (!string.IsNullOrEmpty(item.ResultObject))
                    {
                        strCacheFind = "return HelperCache.";
                        strCacheReturn = item.IsGet ?
                            "return HelperCache.APIGet<" + item.ResultObject + ">(() =>" :
                            "return HelperCache.APISet<" + item.ResultObject + ">(() =>";
                    }
                    else if (!string.IsNullOrEmpty(item.ResultList))
                    {
                        strCacheFind = "return HelperCache.";
                        strCacheReturn = item.IsGet ?
                            "return HelperCache.APIGet<List<" + item.ResultList + ">>(() =>" :
                            "return HelperCache.APISet<List<" + item.ResultList + ">>(() =>";
                    }

                    string strCodeStart = strCoding.Substring(0, codeIndex + 1);
                    string strCodeEnd = strCoding.Substring(codeIndex + (" " + item.MethodName + "(dynamic dynParam)").Length);
                    strCodeStart = strCodeStart.Substring(0, strCodeStart.LastIndexOf("public "));
                    strCodeStart += "public " + strAPIReturn + " " + item.MethodName + "(dynamic dynParam)";

                    strCodeStart += strCodeEnd.Substring(0, strCodeEnd.IndexOf(strCacheFind));
                    strCodeEnd = strCodeEnd.Substring(strCodeEnd.IndexOf(strCacheFind) + strCacheFind.Length);
                    strCodeEnd = strCodeEnd.Substring(strCodeEnd.IndexOf(" =>") + " =>".Length);

                    strCoding = strCodeStart + strCacheReturn + strCodeEnd;
                }
                else
                {
                    sb.AppendLine(
                        t2 + "[HttpPost]" + nl +
                        t2 + "public " + strAPIReturn + " " + item.MethodName + "(dynamic dynParam)" + nl +
                        t2 + "{" + nl +
                        t3 + "try" + nl +
                        t3 + "{" + nl +

                        t4 + "string strParam = dynParam.ToString();" + nl +
                        strCacheReturn +
                        t4 + "{" + nl +

                        strParam +
                        GenData_APIValidate(item) +
                        GenData_APIDataReturn(item) +
                        strAPIThrow +

                        t4 + "}, strParam, SubID, Domain, MethodBase.GetCurrentMethod());" + nl +

                        t3 + "}" + nl +
                        t3 + "catch (Exception ex)" + nl +
                        t3 + "{" + nl +

                        t4 + "HelperLog.MethodEx(ex);" + nl +
                        t4 + "throw ex;" + nl +

                        t3 + "}" + nl +
                        t3 + "finally" + nl +
                        t3 + "{" + nl +

                        t4 + "HelperLog.MethodEnd();" + nl +

                        t3 + "}" + nl +
                        t2 + "}"
                    );
                }
            }
        }

        private string GenData_APIValidate(API item)
        {
            string result = string.Empty;

            if (!string.IsNullOrEmpty(item.ParamObject))
            {
                var itemDTO = _lstDTO.FirstOrDefault(c => c.ObjectName == item.ParamObject);
                if (itemDTO != null)
                {

                }
            }

            return result;
        }

        private string GenData_APIDataReturn(API item)
        {
            bool hasrequest = !string.IsNullOrEmpty(item.ParamObject) || !string.IsNullOrEmpty(item.ParamObject);
            var lstTest = _lstAPITesting.Where(c => c.APICode == item.Code).ToList();
            if (lstTest.Count > 0)
            {
                var lstCheck = new List<string>();
                if (hasrequest)
                {
                    foreach (var itemTest in lstTest)
                    {
                        string strParam = itemTest.Param.Replace("\"", "\\\"").Replace("\r\n", "").Replace("\r", "").Replace("\n", "");
                        string strResult = itemTest.Result.Replace("\"", "\\\"").Replace("\r\n", "").Replace("\r", "").Replace("\n", "");
                        string strCompare = !string.IsNullOrEmpty(item.ParamObject) ?
                            "JsonConvert.SerializeObject(JsonConvert.DeserializeObject<" + item.ParamObject + ">(\"" + strParam + "\"))" :
                            !string.IsNullOrEmpty(item.ParamList) ?
                            "JsonConvert.SerializeObject(JsonConvert.DeserializeObject<List<" + item.ParamList + ">>(\"" + strParam + "\"))" : "";
                        string str = t5 + "if (JsonConvert.SerializeObject(reqdata) == " + strCompare + ")" + nl +
                            t5 + "{" + nl;
                        if (!string.IsNullOrEmpty(itemTest.Result))
                        {
                            if (!string.IsNullOrEmpty(item.ResultObject))
                            {
                                if (_lstDTO.Where(c => c.ObjectName == item.ResultObject).Count() > 0)
                                {
                                    str += t6 + "return JsonConvert.DeserializeObject<" + item.ResultObject + ">(\"" + strResult + "\");" + nl;
                                }
                                else
                                {
                                    switch (item.ResultObject)
                                    {
                                        case "int":
                                            str += t6 + "return Convert.ToInt32(\"" + itemTest.Result + "\");" + nl;
                                            break;
                                        case "long":
                                            str += t6 + "return Convert.ToInt64(\"" + itemTest.Result + "\");" + nl;
                                            break;
                                        case "decimal":
                                            str += t6 + "return Convert.ToDecimal(\"" + itemTest.Result + "\");" + nl;
                                            break;
                                        case "double":
                                            str += t6 + "return Convert.ToDouble(\"" + itemTest.Result + "\");" + nl;
                                            break;
                                        case "DateTime":
                                            str += t6 + "return Convert.ToDateTime(\"" + itemTest.Result + "\");" + nl;
                                            break;
                                    }
                                }
                            }
                            else if (!string.IsNullOrEmpty(item.ResultList))
                            {
                                str += t6 + "return JsonConvert.DeserializeObject<List<" + item.ResultList + ">>(\"" + strResult + "\");" + nl;
                            }
                        }
                        else if (!string.IsNullOrEmpty(itemTest.ExceptionMessage))
                        {
                            str += t6 + "throw new Exception(\"" + itemTest.ExceptionMessage + "\");";
                        }
                        str += t5 + "}" + nl;
                        lstCheck.Add(str);
                    }
                }
                else
                {
                    foreach (var itemTest in lstTest)
                    {
                        string strResult = itemTest.Result.Replace("\"", "\\\"").Replace("\r\n", "").Replace("\r", "").Replace("\n", "");
                        string str = string.Empty;
                        if (!string.IsNullOrEmpty(itemTest.Result))
                        {
                            if (!string.IsNullOrEmpty(item.ResultObject))
                            {
                                if (_lstDTO.Where(c => c.ObjectName == item.ResultObject).Count() > 0)
                                {
                                    str += t5 + "return JsonConvert.DeserializeObject<" + item.ResultObject + ">(\"" + strResult + "\");" + nl;
                                }
                                else
                                {
                                    switch (item.ResultObject)
                                    {
                                        case "int":
                                            str += t5 + "Convert.ToInt32(\"" + itemTest.Result + "\");" + nl;
                                            break;
                                        case "long":
                                            str += t5 + "Convert.ToInt64(\"" + itemTest.Result + "\");" + nl;
                                            break;
                                        case "datetime":
                                            str += t5 + "Convert.ToDateTime(\"" + itemTest.Result + "\");" + nl;
                                            break;
                                    }
                                }
                            }
                            else if (!string.IsNullOrEmpty(item.ResultList))
                            {
                                str += t5 + "return JsonConvert.DeserializeObject<List<" + item.ResultList + ">>(\"" + strResult + "\");" + nl;
                            }
                        }
                        else if (!string.IsNullOrEmpty(itemTest.ExceptionMessage))
                        {
                            str += t6 + "throw new Exception(\"" + itemTest.ExceptionMessage + "\");";
                        }
                        lstCheck.Add(str);
                    }
                }
                return string.Join("", lstCheck);
            }
            else
                throw new Exception("fail data return");
        }

        private void GenData_APIConfig(StringBuilder sb, List<API> lst)
        {
            foreach (var item in lst)
            {
                string strTables = string.Empty;
                foreach (var tb in item.Tables)
                {
                    strTables += ",\"" + tb + "\"";
                }
                if (!string.IsNullOrEmpty(strTables))
                    strTables = strTables.Substring(1);
                sb.AppendLine(t3 + "_lstAPI.Add(new DTOCacheAPI { APILink = \"api/" + item.Controller + "/" + item.MethodName + "\", IsGet = " + item.IsGet + ", DeclaringName = \"" + item.Controller + "Controller\", MethodName = \"" + item.MethodName + "\", Tables = new List<string> { " + strTables + " } });");
            }
        }

        private void Gen_APITestingCollectionEvent(PMCollection_Item collect, APITesting item)
        {
            string strEnvirKeyProperty = item.Code + "_returnp";
            string strEnvirKeyValue = item.Code + "_returnv";
            string strEnvirKeyException = item.Code + "_returnex";
            string strSpace = "     ";
            var itemAdd = new PMCollection_Event
            {
                listen = "test",
                script = new PMCollection_EventScript
                {
                    id = Guid.NewGuid().ToString(),
                    type = "text/javascript",
                    exec = new List<string>()
                }
            };
            if (!string.IsNullOrEmpty(item.ReturnProperty) && !string.IsNullOrEmpty(item.ReturnPropertyCondition) && !string.IsNullOrEmpty(item.ReturnPropertyValue))
            {
                string strCondition = string.Empty;
                switch (item.ReturnPropertyCondition)
                {
                    case "eq": strCondition = "="; break;
                    case "neq": strCondition = "!="; break;
                    case "lt": strCondition = "<"; break;
                    case "lte": strCondition = "<="; break;
                    case "gt": strCondition = ">"; break;
                    case "gte": strCondition = ">="; break;
                }

                if (!string.IsNullOrEmpty(strCondition))
                {
                    string strCompare = string.Empty;
                    if (item.ReturnPropertyCondition == "eq" || item.ReturnPropertyCondition == "neq")
                    {
                        strCompare = "data[strp]+\"\" " + strCondition + " pm.environment.get(\"" + strEnvirKeyValue + "\")+\"\"";
                        //strValue = t1 + "var val = pm.environment.get(\"" + strEnvirKeyValue + "\")+\"\";";
                    }
                    else
                    {
                        strCompare = "parseFloat(data[strp]) " + strCondition + " parseFloat(pm.environment.get(\"" + strEnvirKeyValue + "\"))";
                        //strValue = t1 + "var val = parseFloat(pm.environment.get(\"" + strEnvirKeyValue + "\"));";
                    }

                    itemAdd.script.exec = new List<string>()
                    {
                        "pm.test(\"response must be valid and have a body\", function () {",
                        strSpace + "pm.response.to.be.ok;",
                        strSpace + "pm.response.to.be.withBody;",
                        strSpace + "pm.response.to.be.json;",
                        strSpace + "var data = pm.response.json();",
                        strSpace + "var strp = pm.environment.get(\"" + strEnvirKeyProperty + "\");",
                        strSpace + "var flag = data[strp] !== undefined && " + strCompare + ";",
                        strSpace + "pm.expect(flag).to.equal(true);",
                        "});"
                    };
                }
            }
            else if (!string.IsNullOrEmpty(item.ReturnPropertyCondition) && !string.IsNullOrEmpty(item.ReturnPropertyValue))
            {
                string strCondition = string.Empty;
                switch (item.ReturnPropertyCondition)
                {
                    case "eq": strCondition = "="; break;
                    case "neq": strCondition = "!="; break;
                    case "lt": strCondition = "<"; break;
                    case "lte": strCondition = "<="; break;
                    case "gt": strCondition = ">"; break;
                    case "gte": strCondition = ">="; break;
                }

                if (!string.IsNullOrEmpty(strCondition))
                {
                    string strCompare = string.Empty;
                    if (item.ReturnPropertyCondition == "eq" || item.ReturnPropertyCondition == "neq")
                    {
                        strCompare = "data+\"\" " + strCondition + " pm.environment.get(\"" + strEnvirKeyValue + "\")+\"\"";
                        //strValue = t1 + "var val = pm.environment.get(\"" + strEnvirKeyValue + "\")+\"\";";
                    }
                    else
                    {
                        strCompare = "parseFloat(data) " + strCondition + " parseFloat(pm.environment.get(\"" + strEnvirKeyValue + "\"))";
                        //strValue = t1 + "var val = parseFloat(pm.environment.get(\"" + strEnvirKeyValue + "\"));";
                    }

                    itemAdd.script.exec = new List<string>()
                    {
                        "pm.test(\"response must be valid and have a body\", function () {",
                        strSpace + "pm.response.to.be.ok;",
                        strSpace + "pm.response.to.be.withBody;",
                        strSpace + "pm.response.to.be.json;",
                        strSpace + "var data = pm.response.json();",
                        strSpace + "var flag = data !== undefined && " + strCompare + ";",
                        strSpace + "pm.expect(flag).to.equal(true);",
                        "});"
                    };
                }
            }
            else if (!string.IsNullOrEmpty(item.ReturnProperty))
            {
                itemAdd.script.exec = new List<string>()
                {
                    "pm.test(\"response must be valid and have a body\", function () {",
                    strSpace + "pm.response.to.be.ok;",
                    strSpace + "pm.response.to.be.withBody;",
                    strSpace + "pm.response.to.be.json;",
                    strSpace + "var data = pm.response.json();",
                    strSpace + "var strp = pm.environment.get(\"" + strEnvirKeyProperty + "\");",
                    strSpace + "var flag = data[strp] !== undefined && data[strp] !== null;",
                    strSpace + "pm.expect(flag).to.equal(true);",
                    "});"
                };
            }
            else if (!string.IsNullOrEmpty(item.ExceptionMessage))
            {
                itemAdd.script.exec = new List<string>()
                {
                    "pm.test(\"response must be valid and have a body\", function () {",
                    strSpace + "pm.response.to.be.ok;",
                    strSpace + "pm.response.to.be.withBody;",
                    strSpace + "pm.response.to.be.json;",
                    strSpace + "var data = pm.response.json();",
                    strSpace + "pm.expect(data.ExceptionMessage).to.equal(pm.environment.get(\"" + strEnvirKeyException + "\"));",
                    "});"
                };
            }
            else
            {
                itemAdd.script.exec = new List<string>()
                {
                    "pm.test(\"response must be valid and have a body\", function () {",
                    strSpace + "pm.response.to.be.ok;",
                    strSpace + "pm.response.to.be.withBody;",
                    strSpace + "pm.response.to.be.json;",
                    "});"
                };
            }
        }

        private void Gen_APITestingEnvironmentValue(PMEnvir envir, APITesting item)
        {
            string strEnvirKeyParam = item.Code + "_param";
            string strEnvirKeyProperty = item.Code + "_returnp";
            string strEnvirKeyValue = item.Code + "_returnv";
            string strEnvirKeyException = item.Code + "_returnex";

            envir.values.Add(new PMEnvir_Value
            {
                key = strEnvirKeyParam,
                value = string.IsNullOrEmpty(item.Param) ? "" : item.Param.Trim(),
                enabled = true,
                type = "text"
            });

            envir.values.Add(new PMEnvir_Value
            {
                key = strEnvirKeyProperty,
                value = string.IsNullOrEmpty(item.ReturnProperty) ? "" : item.ReturnProperty.Trim(),
                enabled = true,
                type = "text"
            });

            envir.values.Add(new PMEnvir_Value
            {
                key = strEnvirKeyValue,
                value = string.IsNullOrEmpty(item.ReturnPropertyValue) ? "" : item.ReturnPropertyValue.Trim(),
                enabled = true,
                type = "text"
            });

            envir.values.Add(new PMEnvir_Value
            {
                key = strEnvirKeyException,
                value = string.IsNullOrEmpty(item.ExceptionMessage) ? "" : item.ExceptionMessage.Trim(),
                enabled = true,
                type = "text"
            });
        }
    }

    public class HelperExcel
    {
        private static string[] _excelColumn = new string[] {
            "","A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z",
            "AA", "AB", "AC", "AD", "AE", "AF", "AG", "AH", "AI", "AJ", "AK", "AL", "AM", "AN", "AO", "AP", "AQ", "AR", "AS", "AT", "AU", "AV", "AW", "AX", "AY", "AZ",
            "BA", "BB", "BC", "BD", "BE", "BF", "BG", "BH", "BI", "BJ", "BK", "BL", "BM", "BN", "BO", "BP", "BQ", "BR", "BS", "BT", "BU", "BV", "BW", "BX", "BY", "BZ",
            "CA", "CB", "CC", "CD", "CE", "CF", "CG", "CH", "CI", "CJ", "CK", "CL", "CM", "CN", "CO", "CP", "CQ", "CR", "CS", "CT", "CU", "CV", "CW", "CX", "CY", "CZ"};

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

        //private string GetColumnName(int col)
        //{
        //    return _excelColumn[col];
        //}
    }
}
