using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Http;

namespace APIBusiness.API
{
    //[EnableCors(origins: "*", headers: "*", methods: "*")]
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
                    dto.CreatedDate = DateTime.Now;
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
        public List<DTOGet> GetData(dynamic dynParam)
        {
            try
            {
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
                List<DTOGet> result = new List<DTOGet>();
                foreach (var item in lst.Where(c => c.CreatedDate != null).OrderByDescending(c => c.CreatedDate))
                {
                    var itemGet = new DTOGet
                    {
                        StringTime = item.CreatedDate.Value.ToString("HH:mm"),
                        FileName = item.FileName,
                        LinkFileName = "http://svn.3ps.vn:15002/Uploads/" + item.FileName,
                        Body = string.Empty
                    };
                    List<int> lstRowID = item.ListCells.Select(c => c.Row).Distinct().ToList();
                    foreach (var rowid in lstRowID)
                    {
                        itemGet.Body += "," + item.ListRowTitle[rowid] + ": ";
                        List<string> lstCell = new List<string>();
                        foreach (var cell in item.ListCells.Where(c => c.Row == rowid))
                        {
                            string title = item.ListColumnTitle[cell.Column];
                            itemGet.Body += string.Format("[{0}: {1}->{2}]", title, cell.ValueFrom, cell.ValueTo);
                        }
                    }
                    if (!string.IsNullOrEmpty(itemGet.Body))
                        itemGet.Body = itemGet.Body.Substring(1);
                    result.Add(itemGet);
                }
                return result;
                //return lst.Where(c => c.CreatedDate != null).OrderByDescending(c => c.CreatedDate).ToList();
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

                    using (var client = new HttpClient())
                    {
                        Uri url = new Uri("https://fcm.googleapis.com/fcm/send");

                        client.BaseAddress = new Uri(url.Scheme + "://" + url.Authority);
                        client.DefaultRequestHeaders.Accept.Clear();

                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        client.DefaultRequestHeaders.Add("Authorization", "key=AAAA5NRCJyY:APA91bHBfzDh6iupDq7CvIw9iaNSstO20flnBWzqwWCbMq_EvHt_fG5gIp4pUfmePz1KvuHMBGFuuYNXxYjrCcnvPWVi_pQ39h5IZuQWN8ikZulEgDgB3WCHuXwW7aQXTUdtiY3ZKXNH");
                        client.Timeout = TimeSpan.FromHours(0.1);
                        dynamic dynItem = new { to = "/topics/" + dto.FileName, notification = new { title = dto.Title, body = dto.Body, click_action = "" } };
                        var content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(dynItem), Encoding.UTF8, "application/json");
                        var response = client.PostAsync(url.AbsolutePath, content);
                        if (response != null && response.Result.IsSuccessStatusCode)
                        {
                            HttpResponseMessage res = response.Result;
                        }
                    }
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

                    using (var client = new HttpClient())
                    {
                        Uri url = new Uri("https://iid.googleapis.com/iid/v1:batchAdd");

                        client.BaseAddress = new Uri(url.Scheme + "://" + url.Authority);
                        client.DefaultRequestHeaders.Accept.Clear();

                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        client.DefaultRequestHeaders.Add("Authorization", "key=AAAA5NRCJyY:APA91bHBfzDh6iupDq7CvIw9iaNSstO20flnBWzqwWCbMq_EvHt_fG5gIp4pUfmePz1KvuHMBGFuuYNXxYjrCcnvPWVi_pQ39h5IZuQWN8ikZulEgDgB3WCHuXwW7aQXTUdtiY3ZKXNH");
                        client.Timeout = TimeSpan.FromHours(0.1);
                        dynamic dynItem = new { to = "/topics/" + dto.Topic, registration_tokens = dto.Token };
                        var content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(dynItem), Encoding.UTF8, "application/json");
                        var response = client.PostAsync(url.AbsolutePath, content);
                        if (response != null && response.Result.IsSuccessStatusCode)
                        {
                            HttpResponseMessage res = response.Result;
                        }
                    }
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

                    using (var client = new HttpClient())
                    {
                        Uri url = new Uri("https://iid.googleapis.com/iid/v1:batchRemove");

                        client.BaseAddress = new Uri(url.Scheme + "://" + url.Authority);
                        client.DefaultRequestHeaders.Accept.Clear();

                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        client.DefaultRequestHeaders.Add("Authorization", "key=AAAA5NRCJyY:APA91bHBfzDh6iupDq7CvIw9iaNSstO20flnBWzqwWCbMq_EvHt_fG5gIp4pUfmePz1KvuHMBGFuuYNXxYjrCcnvPWVi_pQ39h5IZuQWN8ikZulEgDgB3WCHuXwW7aQXTUdtiY3ZKXNH");
                        client.Timeout = TimeSpan.FromHours(0.1);
                        dynamic dynItem = new { to = "/topics/" + dto.Topic, registration_tokens = dto.Token };
                        var content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(dynItem), Encoding.UTF8, "application/json");
                        var response = client.PostAsync(url.AbsolutePath, content);
                        if (response != null && response.Result.IsSuccessStatusCode)
                        {
                            HttpResponseMessage res = response.Result;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}