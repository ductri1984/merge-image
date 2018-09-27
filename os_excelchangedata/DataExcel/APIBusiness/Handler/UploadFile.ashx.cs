using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APIBusiness.Handler
{
    /// <summary>
    /// Summary description for UploadFile
    /// </summary>
    public class UploadFile : IHttpHandler
    {
        const string FolderUploads = "/Uploads";

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            try
            {
                if (context.Request.Files.Count <= 0)
                {
                    throw new Exception("No file");
                }
                else
                {
                    HttpPostedFile file = context.Request.Files[0];
                    string ext = System.IO.Path.GetExtension(file.FileName);
                    string filename = System.IO.Path.GetFileName(file.FileName).Replace(ext, "") + "_" +
                        DateTime.Now.ToString("yyyyMMddHHmm") + ext;
                    string filepath = System.IO.Path.Combine(FolderUploads, filename);
                    file.SaveAs(context.Server.MapPath(filepath));
                    context.Response.Write(filename);
                }
            }
            catch (Exception ex)
            {
                context.Response.Write(ex.Message);
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}