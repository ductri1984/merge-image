using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace APIBusiness.API
{
    public class DataController : ApiController
    {
        const string FileData = "/Uploads/data.json";
        const string FileTopic = "/Uploads/topic.json";
        const string FilePush = "/Uploads/push.json";

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
                    lst.Add(dto);
                    str = Newtonsoft.Json.JsonConvert.SerializeObject(lst);
                    System.IO.File.WriteAllText(filepath, str, System.Text.Encoding.UTF8);
                }
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
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}