<%@ WebHandler Language="C#" Class="getEmpImage" %>

using System;
using System.Web;
using System.Data;
using KHSC;

public class getEmpImage : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        string stdid;
        if (context.Request.QueryString["empno"] != null)
        {
            stdid = context.Request.QueryString["empno"].ToString();
        }
        else
        {
            throw new ArgumentException("No parameter specified");
        }
        DataTable dt = StudentManager.getStdPhoto(stdid,"");//EmpManager.getEmpPhoto(stdid);
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