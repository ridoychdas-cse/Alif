<%@ WebHandler Language="C#" Class="getSign" %>

using System;
using System.Web;
using KHSC;
using System.Data;

public class getSign : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        string empno;
        if (context.Request.QueryString["empno"] != null)
        {
            empno = context.Request.QueryString["empno"].ToString();
        }
        else
        {
            throw new ArgumentException("No parameter specified");
        }
        DataTable dt = StudentManager.getStdPhoto(empno,"");// EmpManager.getEmpSign(empno);
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