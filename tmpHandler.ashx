<%@ WebHandler Language="C#" Class="tmpHandler" %>

using System;
using System.Web;
using System.IO;
using KHSC;

public class tmpHandler : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        
        string filename = context.Request.QueryString["filename"];
        FileStream fs = new FileStream(context.Server.MapPath(filename), FileMode.Open, FileAccess.Read);
        System.Drawing.Image img = System.Drawing.Image.FromStream(fs);

        byte[] btp = StudentManager.ConvertImageToByteArray(img, System.Drawing.Imaging.ImageFormat.Jpeg);
        context.Response.ContentType = "image/jpeg";
        context.Response.BinaryWrite(btp);
        fs.Close();
        img.Dispose();
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}