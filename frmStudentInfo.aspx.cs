using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using KHSC;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.draw;

public partial class frmStudentInfo : System.Web.UI.Page
{
    StudentInfoManager studentMg=new StudentInfoManager();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {

                txtDBO.Attributes.Add("onBlur", "formatdate('" + txtDBO.ClientID + "')");
                txtAdmissionDate.Attributes.Add("onBlur", "formatdate('" + txtAdmissionDate.ClientID + "')");
                txtCertificationDate.Attributes.Add("onBlur", "formatdate('" + txtCertificationDate.ClientID + "')");

          txtDiscount.Text = txtWaiver.Text = txtTotalAmount.Text = txtPayAmount.Text = txtDueAmount.Text = "0.00";
                imgStd.ImageUrl = "~/tmpHandler.ashx?filename=img/noimage.jpg";
                ViewState["stdPhoto"] = null;

          
                
            }
            catch (FormatException fex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('" + fex.Message + "');", true);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Database"))
                    ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('Database Maintain Error. Contact to the Software Provider..!!');", true);
                else
                    ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('There is some problem to do the task. Try again properly.!!');", true);
            }

        }
    }

    protected void lbImgUpload_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtName.Text != "" && imgUpload.HasFile)
            {
                int width = 145;
                int height = 165;
                byte[] stdphoto;
                byte[] std_cur_photo;
                using (System.Drawing.Bitmap img = StudentManager.ResizeImage(new System.Drawing.Bitmap(imgUpload.PostedFile.InputStream), width, height, StudentManager.ResizeOptions.ExactWidthAndHeight))
                {
                    imgUpload.PostedFile.InputStream.Close();
                    stdphoto = StudentManager.ConvertImageToByteArray(img, System.Drawing.Imaging.ImageFormat.Png);
                    std_cur_photo = StudentManager.ConvertImageToByteArray(img, System.Drawing.Imaging.ImageFormat.Png);
                    ViewState["stdPhoto"] = stdphoto;
                    img.Dispose();
                }
                string base64String = Convert.ToBase64String(stdphoto, 0, stdphoto.Length);
                imgStd.ImageUrl = "data:image/png;base64," + base64String;

                string base64String1 = Convert.ToBase64String(std_cur_photo, 0, std_cur_photo.Length);
                
            }

            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "ale", "alert('Please input student campus, semester, admission date and then browse a photograph image!!');", true);
            }
        }
        catch (FormatException fex)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('" + fex.Message + "');", true);
        }
        catch (Exception ex)
        {
            if (ex.Message.Contains("Database"))
                ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('Database Maintain Error. Contact to the Software Provider..!!');", true);
            else
                ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('There is some problem to do the task. Try again properly.!!');", true);
        }
    }

    public void Clear()
    {
        txtSlNo.Text = "";
        txtNid.Text = "";
        txtName.Text = "";
        txtStdPhoneNo.Text = "";
        txtFatehrName.Text = "";
        txtFthPhoneNo.Text = "";
        txtMthName.Text = "";
        txtMthPhoneNo.Text = "";
        txtDBO.Text = "";
        txtEmail.Text = "";
        txtNationality.Text = "";
        txtReligion.Text = "";
        txtMaritalStatus.Text = "";
        txtLastEducation.Text = "";
        txtBord.Text = "";
        txtResult.Text = "";
        txtPresentAddress.Text = "";
        txtVillage.Text = "";
        txtPostOffice.Text = "";
        txtPoliceStation.Text = "";
        txtDistrict.Text = "";
        txtCourseName.Text = "";
        txtTrainerName.Text = "";
       
        txtBatchNo.Text = "";
        txtClassTime.Text = "";
        txtAdmissionDate.Text = "";
        rbStartAmPm.SelectedValue = null;
       txtDiscount.Text = txtWaiver.Text = txtTotalAmount.Text = txtPayAmount.Text = txtDueAmount.Text = "0.00";
       txtCourseFee.Text = "";
       txtAddmissionYear.Text = "";
        txtCertificationDate.Text = "";
        SatCheckBox.Checked = false;
        SatCheckBox.Text = "Staturday";
        SunCheckBox.Text = "SunDay";
        MonCheckBox.Text = "MonDay";
        TueCheckBox.Text = "TuesDay";
        WedCheckBox.Text = "WednessDay";
        ThusCheckBox.Text = "Thusday";
        FriCheckBox.Text = "Friday";
        SunCheckBox.Checked = false;
        MonCheckBox.Checked = false;
        TueCheckBox.Checked = false;
        WedCheckBox.Checked = false;
        ThusCheckBox.Checked = false;
        FriCheckBox.Checked = false;
        //Image1.ImageUrl = "images/noimage.jpeg";
        imgStd.ImageUrl = "~/tmpHandler.ashx?filename=img/noimage.jpg";
        ViewState["stdPhoto"] = null;
        txtSearch.Text = "";
        lblTimeAM.Text = "";
        idHiddenField.Value = "";
        PaidHiddenField.Value = "";
        txtSlNo.ReadOnly = false;
        txtCourseFee.ReadOnly = false;
        txtWaiver.ReadOnly = false;
        txtDiscount.ReadOnly = false;
      

    }
    public StudentInfoModel GetAll()

    {
        StudentInfoModel Student=new StudentInfoModel();
        Student.SlNo = txtSlNo.Text;
        Student.NID = txtNid.Text;
        Student.Name = txtName.Text;
        Student.StdPhone = txtStdPhoneNo.Text;
        Student.FthName = txtFatehrName.Text;
        Student.FthPhone = txtFthPhoneNo.Text;
        Student.MthName = txtMthName.Text;
        Student.MthPhone = txtMthPhoneNo.Text;
        Student.DBO = txtDBO.Text;
        Student.Email = txtEmail.Text;
        Student.Nationality = txtNationality.Text;
        Student.Religion = txtReligion.Text;
        Student.MaritalStatus = txtMaritalStatus.Text;
        Student.LastEducation = txtLastEducation.Text;
        Student.Bord = txtBord.Text;
        if (!string.IsNullOrEmpty(txtResult.Text))
        {
            Student.Result = Convert.ToDecimal(txtResult.Text);  
        }
        else
        {
            Student.Result = 0;
        }
        Student.PresentAddress = txtPresentAddress.Text;
        Student.ParVill = txtVillage.Text;
        Student.ParPO = txtPoliceStation.Text;
        Student.ParPS = txtPoliceStation.Text;
        Student.ParDis = txtDistrict.Text;
        Student.CourseName = txtCourseName.Text;
        Student.TrainerName = txtTrainerName.Text;
        Student.BatchNo = txtBatchNo.Text;
        Student.ClassTime = txtClassTime.Text;
        Student.AdmissionDate = txtAdmissionDate.Text;
        Student.APM = lblTimeAM.Text;
        if (ViewState["stdPhoto"] !="")
        {
            Student.StdPhoto = (byte[]) ViewState["stdPhoto"];
        }
        if (string.IsNullOrEmpty(txtCourseFee.Text))
        {
            Student.CourseFee = 0;
        }
        else
        {
            Student.CourseFee = Convert.ToDecimal(txtCourseFee.Text);
        }
        if (string.IsNullOrEmpty(txtWaiver.Text))
        {
            Student.Waiver = 0;
        }
        else
        {
            Student.Waiver = Convert.ToDecimal(txtWaiver.Text);
        }

        if (string.IsNullOrEmpty(txtDiscount.Text))
        {
            Student.DisCount = 0;
        }
        else
        {
            Student.DisCount = Convert.ToDecimal(txtDiscount.Text);
        }
        if (string.IsNullOrEmpty(txtTotalAmount.Text))
        {
            Student.TotalAmount = 0;
        }
        else
        {
            Student.TotalAmount = Convert.ToDecimal(txtTotalAmount.Text);
        }
        if (string.IsNullOrEmpty(txtPayAmount.Text))
        {
            Student.PayAmount = 0;
        }
        else
        {
            Student.PayAmount = Convert.ToDecimal(txtPayAmount.Text);
        }
        Student.AddmissionYear = txtAddmissionYear.Text;
        Student.CertificationDate = txtCertificationDate.Text;

        if (SatCheckBox.Checked == true)
        {
            Student.SatDay = "1";

        }
        else
        { Student.SatDay = ""; }
        if (SunCheckBox.Checked == true)
        { Student.SunDay = "2"; }
        else
        { Student.SunDay = ""; }
        if (MonCheckBox.Checked == true)
        { Student.MonDay = "3"; }
        else
        { Student.MonDay = ""; }
        if (TueCheckBox.Checked == true)
        { Student.TuesDay ="4"; }
        else
        { Student.TuesDay = ""; }
        if (WedCheckBox.Checked == true)
        { Student.WednessDay = "5"; }
        else
        { Student.WednessDay = ""; }
        if (ThusCheckBox.Checked == true)
        { Student.ThusDay = "6"; }
        else
        { Student.ThusDay = ""; }
        if (FriCheckBox.Checked == true)
        { Student.FriDay = "7"; }
        else
        { Student.FriDay = ""; }
        return Student;
    }

    public void TotaLAmount()
    {
        decimal totalAmount = 0;
       StudentInfoModel student=new StudentInfoModel();
        if (string.IsNullOrEmpty(txtCourseFee.Text))
        {
            student.CourseFee = 0;
        }
        else
        {
            student.CourseFee = Convert.ToDecimal(txtCourseFee.Text);
        }

        if (string.IsNullOrEmpty(txtWaiver.Text))
        {
            student.Waiver = 0;
        }
        else
        {
            student.Waiver = Convert.ToDecimal(txtWaiver.Text);
        }

        if (string.IsNullOrEmpty(txtDiscount.Text))
        {
            student.DisCount = 0;
        }
        else
        {
            student.DisCount = Convert.ToDecimal(txtDiscount.Text);
        }
        if (student.Waiver==0)
        {
            totalAmount = student.CourseFee - student.DisCount;
        }
        else
        {

         totalAmount = ((student.CourseFee - (student.CourseFee * ((student.Waiver) / 100))) - student.DisCount);
                
        }

        if (string.IsNullOrEmpty(txtPayAmount.Text))
        {
            student.PayAmount = 0;
        }
        else
        {
            student.PayAmount = Convert.ToDecimal(txtPayAmount.Text);
        }

        student.Due = totalAmount - student.PayAmount;

        txtTotalAmount.Text = totalAmount.ToString("N2");
        txtDueAmount.Text = student.Due.ToString("N2");

    }

    protected void BtnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrEmpty(idHiddenField.Value))
            {
                if (string.IsNullOrEmpty(txtSlNo.Text))
                {
                    txtSlNo.Text = (studentMg.MstId() + 1).ToString();
                }
                else
                {
                    DataTable data = studentMg.CheckId(txtSlNo.Text);
                    if (data.Rows.Count > 0)
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('The Same Id Already Save..!!');", true);
                    }
                    else if (string.IsNullOrEmpty(txtName.Text))
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('Pleace Insert Student Name ..!!');", true);
                    }
                    else if (string.IsNullOrEmpty(txtStdPhoneNo.Text))
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('Pleace Insert Student Phone NO..!!');", true);
                    }
                    else if (string.IsNullOrEmpty(txtCourseName.Text))
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('Pleace Insert Course Name..!!');", true);
                    }
                    else if (string.IsNullOrEmpty(txtClassTime.Text))
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('Pleace Insert Class Time..!!');", true);
                    }
                    else if (string.IsNullOrEmpty(txtCourseFee.Text))
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('Pleace Course Fee.!!');", true);
                    }
                    else
                    {
                        var student = GetAll();
                        studentMg.SaveMst(student);
                        int success = studentMg.Save(student);
                        if (success > 0)
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('Save Success..!!');", true);
                            Clear();
                        }
                        else
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('Save Fail..!!');", true);
                            Clear();
                        }
                    }
                }
            }
            else
            {
                if (string.IsNullOrEmpty(txtSlNo.Text))
                {
                    txtSlNo.Text = (studentMg.MstId() + 1).ToString();
                }
                else
                {
                     if (string.IsNullOrEmpty(txtName.Text))
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('Pleace Insert Student Name ..!!');", true);
                    }
                    else if (string.IsNullOrEmpty(txtStdPhoneNo.Text))
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('Pleace Insert Student Phone NO..!!');", true);
                    }
                    else if (string.IsNullOrEmpty(txtCourseName.Text))
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('Pleace Insert Course Name..!!');", true);
                    }
                    else if (string.IsNullOrEmpty(txtClassTime.Text))
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('Pleace Insert Class Time..!!');", true);
                    }
                    else if (string.IsNullOrEmpty(txtCourseFee.Text))
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('Pleace Course Fee.!!');", true);
                    }
                    else
                    {
                        var student = GetAll();

                        var MstId = idHiddenField.Value;
                        studentMg.UpdateMst(student,MstId);
                        int success = studentMg.Update(student, MstId);
                        if (success > 0)
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('Update Success..!!');", true);
                            Clear();
                        }
                        else
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('Update Fail..!!');", true);
                            Clear();
                        }
                    }
                }
            }
        }
        catch (FormatException fex)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('" + fex.Message + "');", true);
        }
        catch (Exception ex)
        {
            if (ex.Message.Contains("Database"))
                ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('Database Maintain Error. Contact to the Software Provider..!!');", true);
            else
                ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('There is some problem to do the task. Try again properly.!!');", true);
        }
    }
    protected void rbStartAmPm_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rbStartAmPm.SelectedValue == "AM")
        {
            lblTimeAM.Text = "AM";
        }
        else   if (rbStartAmPm.SelectedValue == "PM")
        {
            lblTimeAM.Text = "PM";
        }
        else
        {
            lblTimeAM.Text = "";
        }
    }
    protected void txtCourseFee_TextChanged(object sender, EventArgs e)
    {
        TotaLAmount();
    }
    protected void txtWaiver_TextChanged(object sender, EventArgs e)
    {
        var waiver = Convert.ToDecimal(txtWaiver.Text);
        if (waiver>100)
        {
            txtWaiver.Text = "0.00";
            ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('Waiver Is Over Limit..!!');", true);
        }
        else
        {
            TotaLAmount();
        }
       
    }
    protected void txtDiscount_TextChanged(object sender, EventArgs e)
    {
        decimal Course, totalAmount, discount;
        if (txtCourseFee.Text=="" || txtCourseFee.Text=="0")
        {
            Course = 0;
        }
        else
        {
            Course = Convert.ToDecimal(txtCourseFee.Text);
        }
        if (txtTotalAmount.Text=="" || txtTotalAmount.Text=="0")
        {
            totalAmount = 0;
        }
        else
        {
            totalAmount = Convert.ToDecimal(txtTotalAmount.Text);
        }
         discount = Convert.ToDecimal(txtDiscount.Text);
        if (discount>Course || discount>totalAmount)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('Discount Amounts is Over Course Fee..!!');", true);
            txtDiscount.Text = "";
        }
        else
        {
            TotaLAmount();
            
        }
    }
    protected void txtPayAmount_TextChanged(object sender, EventArgs e)
    {
        StudentInfoModel student = new StudentInfoModel();
        if (string.IsNullOrEmpty(idHiddenField.Value))
        {
          
            if (string.IsNullOrEmpty(txtTotalAmount.Text))
            {
                student.TotalAmount = 0;
            }
            else
            {
                student.TotalAmount = Convert.ToDecimal(txtTotalAmount.Text);
            }
            if (string.IsNullOrEmpty(txtPayAmount.Text))
            {
                student.PayAmount = 0;

            }
            else
            {
                student.PayAmount = Convert.ToDecimal(txtPayAmount.Text);
            }

            if (student.TotalAmount < student.PayAmount)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('Pay AmountIs Over..!!');", true);
                txtPayAmount.Text = txtTotalAmount.Text;
                student.PayAmount = student.TotalAmount;
            }
            var TotalDue = student.TotalAmount - student.PayAmount;
            txtDueAmount.Text = TotalDue.ToString("N2");
        }
        else
        {
            var paid = Convert.ToDecimal(PaidHiddenField.Value);
            var totalAmount = Convert.ToDecimal(txtTotalAmount.Text);
            var due = totalAmount - paid;
            var payment = Convert.ToDecimal(txtPayAmount.Text);
            if (payment>due)
            {
                txtPayAmount.Text = due.ToString();
                txtDueAmount.Text = "0.00";
                ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('Pay AmountIs Over..!!');", true);
            }
            else
            {
                txtDueAmount.Text = (due - payment).ToString();
            }
        }

    }

    protected void BtnReset_Click(object sender, EventArgs e)
    {
        Clear();
    }



    protected void txtSearch_TextChanged(object sender, EventArgs e)
    {
        try
        {

            DataTable data1 = studentMg.SearchId(txtSearch.Text);
            if (data1.Rows.Count>0)
            {
                idHiddenField.Value = data1.Rows[0]["Id"].ToString(); 
            }
            else
            {
                idHiddenField.Value = "0";
            }

            DataTable data = studentMg.Search(idHiddenField.Value, txtSearch.Text);
            ViewState["Print"] = data;
            txtSearch.Text = "";
            if (data.Rows.Count>0)
            {

              
                idHiddenField.Value = data.Rows[0]["Id"].ToString();
                txtSlNo.Text = data.Rows[0]["Sl_Id"].ToString();
                txtSlNo.ReadOnly = true;
                txtNid.Text = data.Rows[0]["NID"].ToString();
                txtName.Text = data.Rows[0]["StdName"].ToString();
                txtStdPhoneNo.Text = data.Rows[0]["StdPhoneNo"].ToString();
                txtFatehrName.Text = data.Rows[0]["FthName"].ToString();
                txtFthPhoneNo.Text = data.Rows[0]["FthPhoneNo"].ToString();
                txtMthName.Text = data.Rows[0]["MthName"].ToString();
                txtMthPhoneNo.Text = data.Rows[0]["MthPhoneNo"].ToString();
                txtDBO.Text = data.Rows[0]["DBO"].ToString();
                txtEmail.Text = data.Rows[0]["Email"].ToString();
                txtNationality.Text = data.Rows[0]["Nationality"].ToString();
                txtReligion.Text = data.Rows[0]["Religion"].ToString();



                txtMaritalStatus.Text = data.Rows[0]["MaritalStatus"].ToString();
                txtLastEducation.Text = data.Rows[0]["LastEducation"].ToString();
                txtBord.Text = data.Rows[0]["Bord"].ToString();

                txtResult.Text = data.Rows[0]["Result"].ToString();
                txtPresentAddress.Text = data.Rows[0]["PresentAddress"].ToString();
                txtVillage.Text = data.Rows[0]["ParVill"].ToString();
                txtPostOffice.Text = data.Rows[0]["ParPO"].ToString();
                txtPoliceStation.Text = data.Rows[0]["ParPS"].ToString();
                txtDistrict.Text = data.Rows[0]["ParDistrict"].ToString();
                txtCourseName.Text = data.Rows[0]["CourseName"].ToString();
                txtTrainerName.Text = data.Rows[0]["TrainerName"].ToString();
                txtBatchNo.Text = data.Rows[0]["BatchNo"].ToString();
               txtAdmissionDate.Text=data.Rows[0]["AdmissionDate"].ToString();
               txtClassTime.Text=data.Rows[0]["ClassTime"].ToString();
               var apm = data.Rows[0]["APM"].ToString();
              
               if (apm == "AM")
               {
                   rbStartAmPm.SelectedValue = "AM";
               }
               else if (apm=="PM")
               {
                   rbStartAmPm.SelectedValue = "PM";
               }
            
             
               txtCourseFee.Text = data.Rows[0]["CourseFee"].ToString();
               txtWaiver.Text = data.Rows[0]["Waiver"].ToString();
               txtDiscount.Text = data.Rows[0]["DisCount"].ToString();
               txtTotalAmount.Text = data.Rows[0]["TotalAmount"].ToString();
               PaidHiddenField.Value = data.Rows[0]["PayAmount"].ToString();
               var totalAmount = Convert.ToDecimal(txtTotalAmount.Text);
               var Payment = Convert.ToDecimal(data.Rows[0]["PayAmount"].ToString());
               //var due = totalAmount - Payment;
               txtDueAmount.Text = data.Rows[0]["Due"].ToString();
               txtAddmissionYear.Text = data.Rows[0]["AddmissionYear"].ToString();
               txtCertificationDate.Text = data.Rows[0]["CertificationDate"].ToString();
               txtCourseFee.ReadOnly = txtWaiver.ReadOnly = txtDiscount.ReadOnly = txtTotalAmount.ReadOnly = true;
               byte[] image = studentMg.GetStudentPhoto(txtSlNo.Text);

               if (image.Length > 4)
               {
                   MemoryStream ms = new MemoryStream(image);
                   System.Drawing.Image img = System.Drawing.Image.FromStream(ms);
                   ViewState["stdPhoto"] = image;
                   string base64String = Convert.ToBase64String(image, 0, image.Length);
                   imgStd.ImageUrl = "data:image/jpeg;base64," + base64String;
               }
               else
               {
                   imgStd.ImageUrl = "~/tmpHandler.ashx?filename=img/noimage.jpg";
                  
               }

             var StarDay = data.Rows[0]["StarDay"].ToString();
             if (StarDay !="0")
             {
                 SatCheckBox.Checked = true;
             }
             var Sunday = data.Rows[0]["Sunday"].ToString();
             if (Sunday !="0")
             {
                 SunCheckBox.Checked = true;
             }

             var MonDay = data.Rows[0]["MonDay"].ToString();
             if (MonDay !="0")
             {
                 MonCheckBox.Checked = true;
             }
             var TusesDay = data.Rows[0]["TusesDay"].ToString();
             if (TusesDay !="0")
             {
                 TueCheckBox.Checked = true;
             }
             var WednessDay = data.Rows[0]["WednessDay"].ToString();
             if (WednessDay !="0")
             {
                 WedCheckBox.Checked = true;

             }
             var ThusDay = data.Rows[0]["ThusDay"].ToString();
             if (ThusDay !="0")
             {
                 ThusCheckBox.Checked = true;
             }
             var Friday = data.Rows[0]["Friday"].ToString();
             if (Friday !="0")
             {
                 FriCheckBox.Checked = true;
             }
            
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('There is no data match this Item ..!!');", true);
            }


        }
        catch (FormatException fex)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('" + fex.Message + "');", true);
        }
        catch (Exception ex)
        {
            if (ex.Message.Contains("Database"))
                ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('Database Maintain Error. Contact to the Software Provider..!!');", true);
            else
         ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('There is some problem to do the task. Try again properly.!!');", true);
   }
    }
    protected void BtnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            if (!string.IsNullOrEmpty(idHiddenField.Value))
            {
                var MstId = idHiddenField.Value;
                studentMg.DeleteMst( MstId);
                int success = studentMg.DeleteMst( MstId);
                if (success > 0)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('Delete Success..!!');", true);
                    Clear();
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('Delete Fail..!!');", true);
                    Clear();
                }
            }

        }
            catch (FormatException fex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('" + fex.Message + "');", true);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Database"))
                    ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('Database Maintain Error. Contact to the Software Provider..!!');", true);
                else
                    ClientScript.RegisterStartupScript(this.GetType(), "Warning", "alert('There is some problem to do the task. Try again properly.!!');", true);
            }


    }






    private static Phrase FormatPhrase(string value)
    {
        return new Phrase(value, FontFactory.GetFont(FontFactory.TIMES_ROMAN, 8));
    }
    private static Phrase FormatHeaderPhrase(string value)
    {
        return new Phrase(value, FontFactory.GetFont(FontFactory.TIMES_ROMAN, 9, iTextSharp.text.Font.BOLD));
    }
    private static Phrase FormatNormal(string value)
    {
        return new Phrase(value, FontFactory.GetFont(FontFactory.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL));
    }
    private static Phrase FormatHeaderPhrase1(string value)
    {
        return new Phrase(value, FontFactory.GetFont(FontFactory.TIMES_ROMAN, 11, iTextSharp.text.Font.BOLD));
    }
    protected void PrintButton_Click(object sender, EventArgs e)
    {
        var data = (DataTable) ViewState["Print"];
        if (data.Rows.Count>0)
        {
            var Sl_No = data.Rows[0]["Sl_Id"].ToString();
            var Date = data.Rows[0]["AdmissionDate"].ToString();
            var Time = data.Rows[0]["ClassTime"].ToString();
            var APM = data.Rows[0]["APM"].ToString();
            var CourseTime = Time + APM;
            var Result = data.Rows[0]["Result"].ToString();
            var CertificationDate = data.Rows[0]["CertificationDate"].ToString();
            var Name = data.Rows[0]["StdName"].ToString();
            var FatherName = data.Rows[0]["FthName"].ToString();
            var MotherName = data.Rows[0]["MthName"].ToString();
            var PresentAddress = data.Rows[0]["PresentAddress"].ToString();
            var Vill = data.Rows[0]["ParVill"].ToString();
            var P_O = data.Rows[0]["ParPO"].ToString();
            var P_S = data.Rows[0]["ParPS"].ToString();
            var DIST = data.Rows[0]["ParDistrict"].ToString();
            var GurdianPhone = data.Rows[0]["FthPhoneNo"].ToString();
            var PersonalPhone = data.Rows[0]["StdPhoneNo"].ToString();
            var NID = data.Rows[0]["NID"].ToString();
            var MaritalStatus = data.Rows[0]["MaritalStatus"].ToString();
            var Religion = data.Rows[0]["Religion"].ToString();
            var Nationality = data.Rows[0]["Nationality"].ToString();
            var LastEducation = data.Rows[0]["LastEducation"].ToString();
            var DBO = data.Rows[0]["DBO"].ToString();

            
             Response.Clear();
        Response.ContentType = "application/pdf";
        Response.AddHeader("content-disposition", "attachment; filename='Alif Admission Form'.pdf");
        Document document = new Document();
        document = new Document(PageSize.A4);
        MemoryStream ms = new MemoryStream();
        PdfWriter writer = PdfWriter.GetInstance(document, Response.OutputStream);
        pdfPage page = new pdfPage();
        writer.PageEvent = page;
        document.Open();
        PdfPCell cell;
        byte[] logo = GlBookManager.GetGlLogo("AMB");
        iTextSharp.text.Image gif = iTextSharp.text.Image.GetInstance(logo);
        gif.Alignment = iTextSharp.text.Image.MIDDLE_ALIGN;
        gif.ScalePercent(40f);

        float[] titwidth = new float[2] { 10, 200 };
        PdfPTable dth = new PdfPTable(titwidth);
        dth.WidthPercentage = 100;



        cell = new PdfPCell(gif);
        cell.HorizontalAlignment = 1;
        cell.VerticalAlignment = 1;
        cell.Rowspan = 5;
        cell.BorderWidth = 0f;
        cell.FixedHeight = 80f;
        dth.AddCell(cell);

        cell = new PdfPCell(new Phrase("ALIF COMPUTER", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 18, iTextSharp.text.Font.BOLD)));
        cell.HorizontalAlignment = 1;
        cell.VerticalAlignment = 1;
        cell.BorderWidth = 0f;

        dth.AddCell(cell);
        cell = new PdfPCell(new Phrase("We are not Only A King,We are Also A King Maker", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 12, iTextSharp.text.Font.BOLDITALIC)));
        cell.HorizontalAlignment = 1;
        cell.VerticalAlignment = 1;
        cell.BorderWidth = 0f;

        dth.AddCell(cell);
        cell = new PdfPCell(new Phrase("Authorised From Maa Memorial Foundation, Gov't Of The Pupils Republice Of Bangladesh Regd.No. S-7705(895)/08", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL)));
        cell.HorizontalAlignment = 1;
        cell.VerticalAlignment = 1;
        cell.BorderWidth = 0f;

        dth.AddCell(cell);

        cell = new PdfPCell(new Phrase("339-340 Muktobangla Shopping Complex (2nd Floor) Mirpur -1 ,Dhaka-1216", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.BOLD)));
        cell.HorizontalAlignment = 1;
        cell.VerticalAlignment = 1;
        cell.BorderWidth = 0f;

        dth.AddCell(cell);
        cell = new PdfPCell(new Phrase("ADMISSION FORM", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 14, iTextSharp.text.Font.BOLD)));
        cell.HorizontalAlignment = 1;
        cell.VerticalAlignment = 1;
        cell.BorderWidth = 0f;

        dth.AddCell(cell);
        cell.BorderColor = BaseColor.LIGHT_GRAY;
        dth.AddCell(cell);

        document.Add(dth);
        LineSeparator line = new LineSeparator(0f, 100, null, Element.ALIGN_CENTER, -2);
        document.Add(line);
        PdfPTable dtempty = new PdfPTable(1);
        cell.BorderWidth = 0f;
        cell.FixedHeight = 10f;
        dtempty.AddCell(cell);
        document.Add(dtempty);

        float[] width = new float[5] { 20, 15, 20, 35, 20,  };
        PdfPTable pdtc = new PdfPTable(width);
        pdtc.WidthPercentage = 100;
        cell = new PdfPCell(new Phrase("Sl. No:", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.BOLD)));
        cell.HorizontalAlignment = 0;
        cell.VerticalAlignment = 1;
        cell.BorderWidth = 0f;
        pdtc.AddCell(cell);
        cell = new PdfPCell(new Phrase(Sl_No, FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.BOLD)));
        cell.HorizontalAlignment = 0;
        cell.VerticalAlignment = 0;
        cell.BorderWidth = 0f;
        pdtc.AddCell(cell);

        cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.BOLD)));
        cell.HorizontalAlignment = 0;
        cell.VerticalAlignment = 0;
        cell.BorderWidth = 0f;
        pdtc.AddCell(cell);
        cell = new PdfPCell(new Phrase("Date", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.BOLD)));
        cell.HorizontalAlignment = 1;
        cell.VerticalAlignment = 1;
        cell.BorderWidth = 0f;
        pdtc.AddCell(cell);
        cell = new PdfPCell(new Phrase(Date, FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.BOLD)));
        cell.HorizontalAlignment = 0;
        cell.VerticalAlignment = 0;
        cell.BorderWidth = 0f;
        pdtc.AddCell(cell);




        cell = new PdfPCell(new Phrase("To", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.BOLD)));
        cell.HorizontalAlignment = 0;
        cell.VerticalAlignment = 2;
        cell.BorderWidth = 0f;
        pdtc.AddCell(cell);
        cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.BOLD)));
        cell.HorizontalAlignment = 0;
        cell.VerticalAlignment = 0;
        cell.BorderWidth = 0f;
        pdtc.AddCell(cell);

        cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.BOLD)));
        cell.HorizontalAlignment = 0;
        cell.VerticalAlignment = 0;
        cell.BorderWidth = 0f;
        pdtc.AddCell(cell);
        cell = new PdfPCell(new Phrase("Course Time", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.BOLD)));
        cell.HorizontalAlignment = 1;
        cell.VerticalAlignment = 1;
        cell.BorderWidth = 0f;
        pdtc.AddCell(cell);
        cell = new PdfPCell(new Phrase(CourseTime, FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.BOLD)));
        cell.HorizontalAlignment = 0;
        cell.VerticalAlignment = 0;
        cell.BorderWidth = 0f;
        pdtc.AddCell(cell);

        cell = new PdfPCell(new Phrase("The Chairman", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.BOLD)));
        cell.HorizontalAlignment = 0;
        cell.VerticalAlignment = 1;
        cell.Colspan = 2;
        cell.BorderWidth = 0f;
        pdtc.AddCell(cell);
        cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.BOLD)));
        cell.HorizontalAlignment = 0;
        cell.VerticalAlignment = 0;
        cell.BorderWidth = 0f;
        pdtc.AddCell(cell);
        cell = new PdfPCell(new Phrase("Result :", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.BOLD)));
        cell.HorizontalAlignment = 1;
        cell.VerticalAlignment = 1;
        cell.BorderWidth = 0f;
        pdtc.AddCell(cell);
        cell = new PdfPCell(new Phrase(Result, FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.BOLD)));
        cell.HorizontalAlignment = 0;
        cell.VerticalAlignment = 0;
        cell.BorderWidth = 0f;
        pdtc.AddCell(cell);
        cell = new PdfPCell(new Phrase("Alif Coumputer", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.BOLD)));
        cell.HorizontalAlignment = 0;
        cell.VerticalAlignment = 1;
        cell.Colspan = 2;
        cell.BorderWidth = 0f;
        pdtc.AddCell(cell);

        cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.BOLD)));
        cell.HorizontalAlignment = 0;
        cell.VerticalAlignment = 0;
        cell.BorderWidth = 0f;
        pdtc.AddCell(cell);
        cell = new PdfPCell(new Phrase("Certificate Lssue Date :", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.BOLD)));
        cell.HorizontalAlignment = 0;
        cell.VerticalAlignment = 1;
        cell.BorderWidth = 0f;
        pdtc.AddCell(cell);
        cell = new PdfPCell(new Phrase(CertificationDate, FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.BOLD)));
        cell.HorizontalAlignment = 0;
        cell.VerticalAlignment = 0;
        cell.BorderWidth = 0f;
        pdtc.AddCell(cell);
        cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.BOLD)));
        cell.HorizontalAlignment = 0;
        cell.VerticalAlignment = 1;
        cell.Colspan = 5;
        cell.BorderWidth = 0f;
        pdtc.AddCell(cell);

        cell = new PdfPCell(new Phrase("Subject : Application For Admission In your institute for the cource of ..................................................................................", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.BOLD)));
        cell.HorizontalAlignment = 0;
        cell.VerticalAlignment = 1;
        cell.Colspan = 5;
        cell.BorderWidth = 0f;
        pdtc.AddCell(cell);
        cell = new PdfPCell(new Phrase("............................................................................................................................................................................................................", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.BOLD)));
        cell.HorizontalAlignment = 0;
        cell.VerticalAlignment = 1;
        cell.Colspan = 5;
        cell.BorderWidth = 0f;
        pdtc.AddCell(cell);
        cell = new PdfPCell(new Phrase("Sir", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.NORMAL)));
        cell.HorizontalAlignment = 0;
        cell.VerticalAlignment = 1;
        cell.Colspan = 5;
        cell.BorderWidth = 0f;
        pdtc.AddCell(cell);
     
        cell = new PdfPCell(new Phrase("Most respectfully I state that I am interested to get the mentioned Cource from Your institute.My detailes are here given", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.NORMAL)));
        cell.HorizontalAlignment = 0;
        cell.VerticalAlignment = 1;
        cell.Colspan = 5;
        cell.BorderWidth = 0f;
        pdtc.AddCell(cell);

        cell = new PdfPCell(new Phrase("for your permission to get admitted to your training School for my Better Computer Professional Life", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.NORMAL)));
        cell.HorizontalAlignment = 0;
        cell.VerticalAlignment = 1;
        cell.Colspan = 5;
        cell.BorderWidth = 0f;
        pdtc.AddCell(cell);

        cell = new PdfPCell(new Phrase("Name :", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.BOLD)));
        cell.HorizontalAlignment = 0;
        cell.VerticalAlignment = 1;
        cell.BorderWidth = 0f;
        pdtc.AddCell(cell);
        cell = new PdfPCell(new Phrase(Name, FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.NORMAL)));
        cell.HorizontalAlignment = 0;
        cell.VerticalAlignment = 1;
        cell.Colspan = 4;
        cell.BorderWidth = 0f;
        pdtc.AddCell(cell);
        cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.NORMAL)));
        cell.HorizontalAlignment = 0;
        cell.VerticalAlignment = 1;
        cell.Colspan = 5;
        cell.BorderWidth = 0f;
        pdtc.AddCell(cell);
        cell = new PdfPCell(new Phrase("Father :", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.BOLD)));
        cell.HorizontalAlignment = 0;
        cell.VerticalAlignment = 1;
        cell.BorderWidth = 0f;
        pdtc.AddCell(cell);
        cell = new PdfPCell(new Phrase(FatherName, FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.NORMAL)));
        cell.HorizontalAlignment = 0;
        cell.VerticalAlignment = 1;
        cell.Colspan = 4;
        cell.BorderWidth = 0f;
        pdtc.AddCell(cell);
        cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.NORMAL)));
        cell.HorizontalAlignment = 0;
        cell.VerticalAlignment = 1;
        cell.Colspan = 5;
        cell.BorderWidth = 0f;
        pdtc.AddCell(cell);
        cell = new PdfPCell(new Phrase("Mother :", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.BOLD)));
        cell.HorizontalAlignment = 0;
        cell.VerticalAlignment = 1;
        cell.BorderWidth = 0f;
        pdtc.AddCell(cell);
        cell = new PdfPCell(new Phrase(MotherName, FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.NORMAL)));
        cell.HorizontalAlignment = 0;
        cell.VerticalAlignment = 1;
        cell.Colspan = 4;
        cell.BorderWidth = 0f;
        pdtc.AddCell(cell);
        cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.NORMAL)));
        cell.HorizontalAlignment = 0;
        cell.VerticalAlignment = 1;
        cell.Colspan = 5;
        cell.BorderWidth = 0f;
        pdtc.AddCell(cell);
        //cell = new PdfPCell(new Phrase("Present Address :", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.BOLD)));
        //cell.HorizontalAlignment = 0;
        //cell.VerticalAlignment = 1;
        //cell.BorderWidth = 0f;
        //pdtc.AddCell(cell);
        //cell = new PdfPCell(new Phrase(PresentAddress, FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.NORMAL)));
        //cell.HorizontalAlignment = 0;
        //cell.VerticalAlignment = 1;
        //cell.Colspan = 4;
        //cell.BorderWidth = 0f;
        //pdtc.AddCell(cell);
        //cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.NORMAL)));
        //cell.HorizontalAlignment = 0;
        //cell.VerticalAlignment = 1;
        //cell.Colspan = 5;
        //cell.BorderWidth = 0f;
        //pdtc.AddCell(cell);
        //cell = new PdfPCell(new Phrase("Permanent Address:", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.BOLD)));
        //cell.HorizontalAlignment = 0;
        //cell.VerticalAlignment = 1;
        //cell.BorderWidth = 0f;
        //pdtc.AddCell(cell);
        //cell = new PdfPCell(new Phrase("Village :"+Vill+",    Post Office : "+P_O+",   Police Station :"+P_S+",   District :"+DIST+"", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.BOLD)));
        //cell.HorizontalAlignment = 0;
        //cell.VerticalAlignment = 1;
        //cell.Colspan = 4;
        //cell.BorderWidth = 0f;
        //pdtc.AddCell(cell);
        //cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.NORMAL)));
        //cell.HorizontalAlignment = 0;
        //cell.VerticalAlignment = 1;
        //cell.Colspan = 5;
        //cell.BorderWidth = 0f;
        //pdtc.AddCell(cell);
        cell = new PdfPCell(new Phrase("Gurdian Phone :", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.BOLD)));
        cell.HorizontalAlignment = 0;
        cell.VerticalAlignment = 1;
        cell.BorderWidth = 0f;
        pdtc.AddCell(cell);

        cell = new PdfPCell(new Phrase(GurdianPhone, FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.NORMAL)));
        cell.HorizontalAlignment = 0;
        cell.VerticalAlignment = 1;
        cell.BorderWidth = 0f;
        pdtc.AddCell(cell);
        cell = new PdfPCell(new Phrase("Personal Phone :", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.BOLD)));
        cell.HorizontalAlignment = 1;
        cell.VerticalAlignment = 1;
        cell.BorderWidth = 0f;
        pdtc.AddCell(cell);
        cell = new PdfPCell(new Phrase(PersonalPhone, FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.NORMAL)));
        cell.HorizontalAlignment = 0;
        cell.VerticalAlignment = 1;
        cell.BorderWidth = 0f;
        pdtc.AddCell(cell);
        cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.BOLD)));
        cell.HorizontalAlignment = 0;
        cell.VerticalAlignment = 1;
        cell.BorderWidth = 0f;
        pdtc.AddCell(cell);
        cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.NORMAL)));
        cell.HorizontalAlignment = 0;
        cell.VerticalAlignment = 1;
        cell.Colspan = 5;
        cell.BorderWidth = 0f;
        pdtc.AddCell(cell);
        cell = new PdfPCell(new Phrase("Naitonal Id Card No", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.BOLD)));
        cell.HorizontalAlignment = 0;
        cell.VerticalAlignment = 1;
        cell.BorderWidth = 0f;
        pdtc.AddCell(cell);
        cell = new PdfPCell(new Phrase(": " +NID, FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.NORMAL)));
        cell.HorizontalAlignment = 0;
        cell.VerticalAlignment = 1;
        cell.BorderWidth = 0f;
        pdtc.AddCell(cell);
        cell = new PdfPCell(new Phrase("Last Education :", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.BOLD)));
        cell.HorizontalAlignment = 0;
        cell.VerticalAlignment = 1;
        cell.BorderWidth = 0f;
        pdtc.AddCell(cell);
        cell = new PdfPCell(new Phrase(LastEducation, FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.NORMAL)));
        cell.HorizontalAlignment = 0;
        cell.VerticalAlignment = 1;
        cell.BorderWidth = 0f;
        pdtc.AddCell(cell);
        cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.BOLD)));
        cell.HorizontalAlignment = 0;
        cell.VerticalAlignment = 1;
        cell.BorderWidth = 0f;
        pdtc.AddCell(cell);
        cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.NORMAL)));
        cell.HorizontalAlignment = 0;
        cell.VerticalAlignment = 1;
        cell.Colspan = 5;
        cell.BorderWidth = 0f;
        pdtc.AddCell(cell);

        cell = new PdfPCell(new Phrase("Marital Status :", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.BOLD)));
        cell.HorizontalAlignment = 0;
        cell.VerticalAlignment = 1;
        cell.BorderWidth = 0f;
        pdtc.AddCell(cell);
        cell = new PdfPCell(new Phrase(MaritalStatus, FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.NORMAL)));
        cell.HorizontalAlignment = 0;
        cell.VerticalAlignment = 1;
        cell.BorderWidth = 0f;
        pdtc.AddCell(cell);
        cell = new PdfPCell(new Phrase("Nationality :", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.BOLD)));
        cell.HorizontalAlignment = 0;
        cell.VerticalAlignment = 1;
        cell.BorderWidth = 0f;
        pdtc.AddCell(cell);
        cell = new PdfPCell(new Phrase(Nationality, FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.NORMAL)));
        cell.HorizontalAlignment = 0;
        cell.VerticalAlignment = 1;
        cell.BorderWidth = 0f;
        pdtc.AddCell(cell);
        cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.BOLD)));
        cell.HorizontalAlignment = 0;
        cell.VerticalAlignment = 1;
        cell.BorderWidth = 0f;
        pdtc.AddCell(cell);
        cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.NORMAL)));
        cell.HorizontalAlignment = 0;
        cell.VerticalAlignment = 1;
        cell.Colspan = 5;
        cell.BorderWidth = 0f;
        pdtc.AddCell(cell);
        cell = new PdfPCell(new Phrase("Religion :", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.BOLD)));
        cell.HorizontalAlignment = 0;
        cell.VerticalAlignment = 1;
        cell.BorderWidth = 0f;
        pdtc.AddCell(cell);
        cell = new PdfPCell(new Phrase(Religion, FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.NORMAL)));
        cell.HorizontalAlignment = 0;
        cell.VerticalAlignment = 1;
        cell.BorderWidth = 0f;
        pdtc.AddCell(cell);
        cell = new PdfPCell(new Phrase("Date Of Birth :", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.BOLD)));
        cell.HorizontalAlignment = 0;
        cell.VerticalAlignment = 1;
        cell.BorderWidth = 0f;
        pdtc.AddCell(cell);
        cell = new PdfPCell(new Phrase(DBO, FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.NORMAL)));
        cell.HorizontalAlignment = 0;
        cell.VerticalAlignment = 1;
        cell.BorderWidth = 0f;
        pdtc.AddCell(cell);
        cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.BOLD)));
        cell.HorizontalAlignment = 0;
        cell.VerticalAlignment = 1;
        cell.BorderWidth = 0f;
        pdtc.AddCell(cell);
        cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.NORMAL)));
        cell.HorizontalAlignment = 0;
        cell.VerticalAlignment = 1;
        cell.Colspan = 5;
        cell.BorderWidth = 0f;
        pdtc.AddCell(cell);
        cell = new PdfPCell(new Phrase("Present Address :", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.BOLD)));
        cell.HorizontalAlignment = 0;
        cell.VerticalAlignment = 1;
        cell.BorderWidth = 0f;
        pdtc.AddCell(cell);
        cell = new PdfPCell(new Phrase(PresentAddress, FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.NORMAL)));
        cell.HorizontalAlignment = 0;
        cell.VerticalAlignment = 1;
        cell.Colspan = 4;
        cell.BorderWidth = 0f;
        pdtc.AddCell(cell);
        cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.NORMAL)));
        cell.HorizontalAlignment = 0;
        cell.VerticalAlignment = 1;
        cell.Colspan = 5;
        cell.BorderWidth = 0f;
        pdtc.AddCell(cell);
        cell = new PdfPCell(new Phrase("Permanent Address:", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.BOLD)));
        cell.HorizontalAlignment = 0;
        cell.VerticalAlignment = 1;
        cell.BorderWidth = 0f;
        pdtc.AddCell(cell);
        cell = new PdfPCell(new Phrase("Village :" + Vill + ",    Post Office : " + P_O + ",   Police Station :" + P_S + ",   District :" + DIST + "", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.BOLD)));
        cell.HorizontalAlignment = 0;
        cell.VerticalAlignment = 1;
        cell.Colspan = 4;
        cell.BorderWidth = 0f;
        pdtc.AddCell(cell);
        cell = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.NORMAL)));
        cell.HorizontalAlignment = 0;
        cell.VerticalAlignment = 1;
        cell.Colspan = 5;
        cell.BorderWidth = 0f;
        pdtc.AddCell(cell);
        document.Add(pdtc);

        
        PdfPCell cells = new PdfPCell();
        iTextSharp.text.Rectangle page1 = document.PageSize;
        float[] FootWth = new float[] { 5, 30, 10, 30, 10, 30 };
        PdfPTable Fdth = new PdfPTable(FootWth);
        Fdth.TotalWidth = page1.Width - 10;
        Fdth.HorizontalAlignment = Element.ALIGN_CENTER;
        cells = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.BOLD)));
        cells.HorizontalAlignment = 1;
        cells.Border = 0;
        cells.VerticalAlignment = 1;
        Fdth.AddCell(cells);
        cells = new PdfPCell(new Phrase("Signature Of Chairman", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.BOLD)));
        cells.HorizontalAlignment = 1;
        cells.Border = 1;
        cells.VerticalAlignment = 1;
        Fdth.AddCell(cells);
        cells = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.BOLD)));
        cells.HorizontalAlignment = 1;
        cells.VerticalAlignment = 1;
        cells.Border = 0;
        Fdth.AddCell(cells);
        cells = new PdfPCell(new Phrase("Signature Of Instructor", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.BOLD)));
        cells.HorizontalAlignment = 1;
        cells.Border = 1;
        cells.VerticalAlignment = 1;
        Fdth.AddCell(cells);
        cells = new PdfPCell(new Phrase("", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.BOLD)));
        cells.HorizontalAlignment = 1;
        cells.VerticalAlignment = 1;
        cells.Border = 0;
        Fdth.AddCell(cells);
        cells = new PdfPCell(new Phrase("Applicant Signature", FontFactory.GetFont(FontFactory.TIMES_ROMAN, 10, iTextSharp.text.Font.BOLD)));
        cells.HorizontalAlignment = 1;
        cells.Border = 1;
        cells.VerticalAlignment = 1;
        Fdth.AddCell(cells);
        Fdth.WriteSelectedRows(0, 5, 0, 60, writer.DirectContent);

       

        document.Close();
        Response.Flush();
        Response.End();


        }
       
        
       

    }

    protected void btnPrint2_Click(object sender, EventArgs e)
    {

       
    }





    public PdfPCell cell { get; set; }



 
}