using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenData
{
    class Program
    {
        static string FileCredential = "credentials.json";
        static string FileToken = "token.json";
        static string FileSpreadsheet = "temp.xlsx";
        const string GoogleSheetID = "19KVSeofQRjeGL8iG_LQjEoCEWj0shIzaoLQ0I6qlIqM";
        const string SheetContainer = "Container";
        const string SheetContainerData = "ContainerData";

        static void Main(string[] args)
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
                    using (var package = new OfficeOpenXml.ExcelPackage(new System.IO.FileInfo(FileSpreadsheet)))
                    {
                        var worksheet = HelperExcel.GetWorksheetByName(package, SheetContainer);
                        #region SheetContainer
                        if (worksheet != null && worksheet.Dimension != null)
                        {
                            //int row = 1, col = 1, rowMax = 1000;
                            //var itemAdd = default(DTO);
                            //for (row = 3; row <= worksheet.Dimension.End.Row && row < rowMax; row++)
                            //{
                            //    col = 2;
                            //    var strNameSpace = HelperExcel.GetValue(worksheet, row, col);
                            //    col++;
                            //    var strSubFolder = HelperExcel.GetValue(worksheet, row, col);
                            //    col++;
                            //    var strObjectName = HelperExcel.GetValue(worksheet, row, col);
                            //    col++;
                            //    var strPropertyName = HelperExcel.GetValue(worksheet, row, col);
                            //    col++;
                            //    var strPropertyType = HelperExcel.GetValue(worksheet, row, col);
                            //    col++;
                            //    var strPropertyNull = HelperExcel.GetValue(worksheet, row, col);
                            //    col++;
                            //    var strPropertyNotEmpty = HelperExcel.GetValue(worksheet, row, col);
                            //    col++;
                            //    var strPropertyMin = HelperExcel.GetValue(worksheet, row, col);
                            //    col++;
                            //    var strPropertyMax = HelperExcel.GetValue(worksheet, row, col);
                            //    col++;
                            //    var strExpression = HelperExcel.GetValue(worksheet, row, col);
                            //    col++;
                            //    var strExpressionCode = HelperExcel.GetValue(worksheet, row, col);
                            //    col++;
                            //    var strInheritance = HelperExcel.GetValue(worksheet, row, col);
                            //    col++;
                            //    var strObjectDescriptionVN = HelperExcel.GetValue(worksheet, row, col);
                            //    col++;
                            //    var strObjectDescriptionEN = HelperExcel.GetValue(worksheet, row, col);
                            //    col++;
                            //    var strObjectDescriptionJA = HelperExcel.GetValue(worksheet, row, col);
                            //    col++;
                            //    var strObjectDescriptionFR = HelperExcel.GetValue(worksheet, row, col);
                            //    col++;
                            //    var strObjectDescriptionZN = HelperExcel.GetValue(worksheet, row, col);
                            //    col++;
                            //    var strPropertyDescriptionVN = HelperExcel.GetValue(worksheet, row, col);
                            //    col++;
                            //    var strPropertyDescriptionEN = HelperExcel.GetValue(worksheet, row, col);
                            //    col++;
                            //    var strPropertyDescriptionJA = HelperExcel.GetValue(worksheet, row, col);
                            //    col++;
                            //    var strPropertyDescriptionFR = HelperExcel.GetValue(worksheet, row, col);
                            //    col++;
                            //    var strPropertyDescriptionZN = HelperExcel.GetValue(worksheet, row, col);

                            //    if ((!string.IsNullOrEmpty(strObjectName) || itemAdd != null) && (!string.IsNullOrEmpty(strPropertyName) || !string.IsNullOrEmpty(strInheritance)))
                            //    {
                            //        if (itemAdd == null || (!string.IsNullOrEmpty(strObjectName) && itemAdd.ObjectName != strObjectName))
                            //        {
                            //            itemAdd = new DTO();
                            //            itemAdd.NameSpace = strNameSpace;
                            //            itemAdd.SubFolder = strSubFolder;
                            //            itemAdd.ObjectName = strObjectName;
                            //            itemAdd.Inheritance = strInheritance;
                            //            itemAdd.DescriptionVI = strObjectDescriptionVN;
                            //            itemAdd.DescriptionEN = strObjectDescriptionEN;
                            //            itemAdd.DescriptionJA = strObjectDescriptionJA;
                            //            itemAdd.DescriptionFR = strObjectDescriptionFR;
                            //            itemAdd.DescriptionZN = strObjectDescriptionZN;
                            //            itemAdd.Properties = new List<DTOProperty>();
                            //            _lstDTO.Add(itemAdd);
                            //        }
                            //        var itemProperty = new DTOProperty();
                            //        itemProperty.Name = strPropertyName;
                            //        itemProperty.TypeCode = strPropertyType;
                            //        itemProperty.AllowNull = strPropertyNull.ToLower() == "true";
                            //        itemProperty.NotEmpty = strPropertyNotEmpty.ToLower() == "true";
                            //        if (!string.IsNullOrEmpty(strPropertyMin))
                            //            itemProperty.Min = Convert.ToDouble(strPropertyMin);
                            //        if (!string.IsNullOrEmpty(strPropertyMax))
                            //            itemProperty.Max = Convert.ToDouble(strPropertyMax);
                            //        itemProperty.Expression = strExpression;
                            //        itemProperty.ExpressionCode = strExpressionCode;
                            //        itemProperty.DescriptionVI = strPropertyDescriptionVN;
                            //        itemProperty.DescriptionEN = strPropertyDescriptionEN;
                            //        itemProperty.DescriptionJA = strPropertyDescriptionJA;
                            //        itemProperty.DescriptionFR = strPropertyDescriptionFR;
                            //        itemProperty.DescriptionZN = strPropertyDescriptionZN;
                            //        itemAdd.Properties.Add(itemProperty);
                            //    }
                            //}
                        }
                        #endregion
                    }
                }
            }
            else
                throw new Exception("GoogleSheetID not exists in Sheets");
        }
    }
}
