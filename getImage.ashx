<%@ WebHandler Language="C#" Class="getImage" %>

using System;
using System.Data;
using System.Web;
using KHSC;

public class getImage : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        string stdid,col;
        if (context.Request.QueryString["studentid"] != null)
        {
            stdid = context.Request.QueryString["studentid"].ToString();
            col = context.Request.QueryString["col"].ToString();
        }
        else
        {
            throw new ArgumentException("No parameter specified");
        }
        DataTable dt = StudentManager.getStdPhoto(stdid,col);
        if (dt.Rows.Count > 0)
        {
            if (dt.Rows[0][0] != DBNull.Value)
            {
                System.Drawing.Image img = StudentManager.byteArrayToImage((byte[])dt.Rows[0][0]);
                byte[] btp = StudentManager.ConvertImageToByteArray(img, System.Drawing.Imaging.ImageFormat.Jpeg);
                context.Response.ContentType = "image/jpeg";
                context.Response.BinaryWrite(btp);
            }                                   
        }
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }   
}