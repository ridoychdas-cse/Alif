using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using KHSC;

/// <summary>
/// Summary description for StudentAccountGateway
/// </summary>
public class StudentAccountGateway
{

    SqlConnection connection = new SqlConnection(DataManager.OraConnString());
    SqlTransaction transection;
    internal string GetShowAutoId()
    {
        try
        {
            connection.Open();
            string selectQuery = @"SELECT 'Recipt-'+RIGHT('000000'+CONVERT(VARCHAR,ISNULL(MAX(CONVERT(INTEGER,RIGHT([accri_money_rct],6))),0)+1),6) FROM [tbl_acc_recipt_info]";
            SqlCommand command = new SqlCommand(selectQuery, connection);
            return command.ExecuteScalar().ToString();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        finally
        {
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
        }
    }

    internal void SaveAccountInformation(StudentPayment aStudentPayment1, List<StudentPayment> studentPaymentList)
    {
        try
        {
            connection.Open();
            transection = connection.BeginTransaction();
            SqlCommand command = new SqlCommand();
            command.Connection = connection;
            command.Transaction = transection;
            command.CommandText = @"SELECT COUNT (*) FROM [tbl_acc_recipt_info_details] WHERE [accrid_student_id]='" + aStudentPayment1.Id + "' AND [accrid_class]='" + aStudentPayment1.Class + "' AND [accrid_year]='" + aStudentPayment1.Year + "' AND [accrid_section]='" + aStudentPayment1.Section + "'";
           int ReciptCount=Convert.ToInt32( command.ExecuteScalar());
           if (ReciptCount > 0)
           {
               command.CommandText = @"UPDATE [tbl_acc_recipt_info_details]
   SET [accrid_total_paid_amount] =[accrid_total_paid_amount]+'" + aStudentPayment1.TotalPaidAmount + "' ,[accrid_total_due_amount] =[accrid_total_due_amount]-'" + aStudentPayment1.TotalDueAmount + "' WHERE [accrid_student_id]='" + aStudentPayment1.Id + "' AND [accrid_class]='" + aStudentPayment1.Class + "' AND [accrid_year]='" + aStudentPayment1.Year + "' AND [accrid_section]='" + aStudentPayment1.Section + "' ";
            }
            else
            {
                command.CommandText = @"INSERT INTO [tbl_acc_recipt_info_details]
           ([accrid_money_rct]
           ,[accrid_book_page]
           ,[accrid_recipt_date]
           ,[accrid_remark]
           ,[accrid_total_amount]
           ,[accrid_total_late_fee]
           ,[accrid_grand_total]
           ,[accrid_paid_by_cash]
           ,[accrid_paid_by_bank]
           ,[accrid_total_paid_amount]
           ,[accrid_total_due_amount]
           ,[accrid_bank_name]
           ,[accrid_branch_name]
           ,[accrid_account_no]
           ,[accrid_payment_mod]
           ,[accrid_cheque_no]
           ,[accrid_cheque_posting_date]
           ,[accrid_posted_on]
           ,[accrid_posted_by]
           ,[accrid_student_id]
           ,[accrid_class]
           ,[accrid_year]
           ,[accrid_section]
           ,[accrid_roll]
           ,[accrid_transport])
     VALUES
           ( '" + aStudentPayment1.MoneyReciptNo + "','" + aStudentPayment1.BookPageNo + "' ,'" + aStudentPayment1.Date + "','" + aStudentPayment1.Remarks + "','" + aStudentPayment1.TotalpayAmount + "','" + aStudentPayment1.TotalLatefee + "','" + aStudentPayment1.GrandTotal + "','" + aStudentPayment1.PaidByCash + "','" + aStudentPayment1.PaidByBank + "','" + aStudentPayment1.TotalPaidAmount + "','" + aStudentPayment1.TotalDueAmount + "','" + aStudentPayment1.BankName + "','" + aStudentPayment1.BranchName + "','" + aStudentPayment1.AccountNo + "','" + aStudentPayment1.PaymentMod + "','" + aStudentPayment1.ChequeNo + "','" + aStudentPayment1.ChequePostingDate + "',GETDATE() ,'Admin','" + aStudentPayment1.Id + "','" + aStudentPayment1.Class + "','" + aStudentPayment1.Year + "','" + aStudentPayment1.Section + "','" + aStudentPayment1.Roll + "','" + aStudentPayment1.Transportfee + "' )";
                command.ExecuteNonQuery();
            }
            //&************************ Old ****************//
            command.CommandText = @"INSERT INTO [tbl_acc_recipt_info]
           ([accri_money_rct]
           ,[accri_book_page]
           ,[accri_recipt_date]
           ,[accri_remark]
           ,[accri_total_amount]
           ,[accri_total_late_fee]
           ,[accri_grand_total]
           ,[accri_paid_by_cash]
           ,[accri_paid_by_bank]
           ,[accri_total_paid_amount]
           ,[accri_total_due_amount]
           ,[accri_bank_name]
           ,[accri_branch_name]
           ,[accri_account_no]
           ,[accri_payment_mod]
           ,[accri_cheque_no]
           ,[accri_cheque_posting_date]
           ,[accri_posted_on]
           ,[accri_posted_by],[accri_student_id],[accri_class],[accri_year],[accri_section],[accri_roll],[accri_transport])
     VALUES
           ( '" + aStudentPayment1.MoneyReciptNo + "','" + aStudentPayment1.BookPageNo + "' ,'" + aStudentPayment1.Date + "','" + aStudentPayment1.Remarks + "','" + aStudentPayment1.TotalpayAmount + "','" + aStudentPayment1.TotalLatefee + "','" + aStudentPayment1.GrandTotal + "','" + aStudentPayment1.PaidByCash + "','" + aStudentPayment1.PaidByBank + "','" + aStudentPayment1.TotalPaidAmount + "','" + aStudentPayment1.TotalDueAmount + "','" + aStudentPayment1.BankName + "','" + aStudentPayment1.BranchName + "','" + aStudentPayment1.AccountNo + "','" + aStudentPayment1.PaymentMod + "','" + aStudentPayment1.ChequeNo + "','" + aStudentPayment1.ChequePostingDate + "',GETDATE() ,'Admin','" + aStudentPayment1.Id + "','" + aStudentPayment1.Class + "','" + aStudentPayment1.Year + "','" + aStudentPayment1.Section + "','" + aStudentPayment1.Roll + "','"+aStudentPayment1.Transportfee+"' )";
            command.ExecuteNonQuery();
            //2222222222
            foreach (StudentPayment aStudentPayment in studentPaymentList)
            {
                if (aStudentPayment.PaidAmount > 0)
                {
                    if (aStudentPayment.PaymentName == "Admission Fee")
                    {

                        command.CommandText =
                            @"SELECT COUNT(*)  FROM [tbl_account_admission_fee] WHERE [acc_ad_ledger_id]='" +
                            aStudentPayment.HeadId + "' AND [acc_ad_student_id]='" + aStudentPayment1.Id +
                            "' AND [acc_ad_class]='" + aStudentPayment1.Class + "' AND [acc_ad_year]='" +
                            aStudentPayment1.Year + "'";
                        int count = Convert.ToInt32(command.ExecuteScalar());
                        if (count > 0)
                        {
                            command.CommandText =
                                @"UPDATE [tbl_account_admission_fee] SET  [acc_ad_total_paid_amount] =[acc_ad_total_paid_amount]+'" +
                                aStudentPayment.PaidAmount + "',[acc_ad_due_amount] ='" + aStudentPayment.DueAmount +
                                "',[acc_ad_update_on] =GETDATE()  ,[acc_ad_update_by] ='Admin' WHERE [acc_ad_ledger_id]='" +
                                aStudentPayment.HeadId + "' AND [acc_ad_student_id]='" + aStudentPayment1.Id +
                                "' AND [acc_ad_class]='" + aStudentPayment1.Class + "' AND [acc_ad_year]='" +
                                aStudentPayment1.Year + "'";
                            command.ExecuteNonQuery();
                        }
                        else
                        {
                            command.CommandText =
                                @"INSERT INTO [tbl_account_admission_fee]
           ([acc_ad_ledger_id]
           ,[acc_ad_student_id]
           ,[acc_ad_class]
           ,[acc_ad_year]
           ,[acc_ad_total_amount]
           ,[acc_ad_total_paid_amount]
           ,[acc_ad_due_amount]
           ,[acc_ad_posted_on]
           ,[acc_ad_posted_by]
           ,[acc_ad_update_on]
           ,[acc_ad_update_by]
           ,[acc_ad_remarks]
           ,[acc_ad_late_fee])
     VALUES
           ('" +
                                aStudentPayment.HeadId + "' ,'" + aStudentPayment1.Id + "' ,'" + aStudentPayment1.Class +
                                "' ,'" + aStudentPayment1.Year + "' ,'" + aStudentPayment.TotalAmount + "' ,'" +
                                aStudentPayment.PaidAmount + "' ,'" + aStudentPayment.DueAmount +
                                "' ,GETDATE(),'Admin' ,'01-01-1990','' ,'','')";
                            command.ExecuteNonQuery();
                        }

                        command.CommandText =
                            @"INSERT INTO [tbl_account_admission_details]
           ([acc_ad_de_sl_no]
           ,[acc_ad_de_ledger_id]
           ,[acc_ad_de_student_id]
           ,[acc_ad_de_class]
           ,[acc_ad_de_year]
           ,[acc_ad_de_total_amount]
           ,[acc_ad_de_total_paid_amount]
           ,[acc_ad_de_due_amount]
           ,[acc_ad_de_posted_on]
           ,[acc_ad_de_posted_by]
           ,[acc_ad_de_update_on]
           ,[acc_ad_de_update_by]
           ,[acc_ad_de_remarks]
           ,[acc_ad_de_late_fee],[acc_ad_de_receipt_no])
     VALUES
           ((SELECT ISNULL(MAX([acc_ad_de_sl_no]),0) FROM [tbl_account_admission_details])+1,'" +
                            aStudentPayment.HeadId + "','" + aStudentPayment1.Id + "','" + aStudentPayment1.Class + "','" +
                            aStudentPayment1.Year + "','" + aStudentPayment.TotalAmount + "','" +
                            aStudentPayment.PaidAmount + "' ,'" + aStudentPayment.DueAmount +
                            "' ,GETDATE() ,'Admin' ,'01-01-1990','' ,'' ,'','"+aStudentPayment1.MoneyReciptNo+"' )";
                        command.ExecuteNonQuery();
                    }//done
                    else if (aStudentPayment.PaymentName.Contains("Monthly Tuition Fee"))
                    {
                        int count;
                        command.CommandText =
                            @"SELECT COUNT(*)  FROM [tbl_acc_monthly_tution_fee] WHERE [mo_tu_fee_ledger_id]='" +
                            aStudentPayment.HeadId + "' AND [mo_tu_fee_student_id]='" + aStudentPayment1.Id +
                            "' AND [mo_tu_fee_class]='" + aStudentPayment1.Class + "' AND [mo_tu_fee_year]='" +
                            aStudentPayment1.Year + "'";
                        count = Convert.ToInt32(command.ExecuteScalar());
                        if (count > 0)
                        {
                            command.CommandText =
                                @"UPDATE [tbl_acc_monthly_tution_fee] SET  [mo_tu_fee_total_paid_amount] =[mo_tu_fee_total_paid_amount]+'" +
                                aStudentPayment.PaidAmount + "',[mo_tu_fee_due_amount] ='" + aStudentPayment.DueAmount +
                                "',[mo_tu_fee_update_on] =GETDATE()  ,[mo_tu_fee_update_by] ='Admin' WHERE [mo_tu_fee_ledger_id]='" +
                                aStudentPayment.HeadId + "' AND [mo_tu_fee_student_id]='" + aStudentPayment1.Id +
                                "' AND [mo_tu_fee_class]='" + aStudentPayment1.Class + "' AND [mo_tu_fee_year]='" +
                                aStudentPayment1.Year + "'";
                            command.ExecuteNonQuery();
                        }
                        else
                        {
                            command.CommandText =
                                @"INSERT INTO [tbl_acc_monthly_tution_fee]
           ([mo_tu_fee_ledger_id]
           ,[mo_tu_fee_student_id]
           ,[mo_tu_fee_class]
           ,[mo_tu_fee_year]
           ,[mo_tu_fee_total_amount]
           ,[mo_tu_fee_total_paid_amount]
           ,[mo_tu_fee_due_amount]
           ,[mo_tu_fee_posted_on]
           ,[mo_tu_fee_posted_by]
           ,[mo_tu_fee_update_on]
           ,[mo_tu_fee_update_by]
           ,[mo_tu_fee_remarks]
           ,[mo_tu_fee_late_fee])
     VALUES
           ('" +
                                aStudentPayment.HeadId + "' ,'" + aStudentPayment1.Id + "' ,'" + aStudentPayment1.Class +
                                "' ,'" + aStudentPayment1.Year + "' ,'" + aStudentPayment.TotalAmount + "' ,'" +
                                aStudentPayment.PaidAmount + "' ,'" + aStudentPayment.DueAmount +
                                "' ,GETDATE(),'Admin' ,'01-01-1990','' ,'','')";
                            command.ExecuteNonQuery();
                        }

                        command.CommandText =
                            @"INSERT INTO [tbl_acc_monthly_tution_fee_details]
           ([mo_tu_fee_de_sl_no]
           ,[mo_tu_fee_de_ledger_id]
           ,[mo_tu_fee_de_student_id]
           ,[mo_tu_fee_de_class]
           ,[mo_tu_fee_de_year]
           ,[mo_tu_fee_de_total_amount]
           ,[mo_tu_fee_de_total_paid_amount]
           ,[mo_tu_fee_de_due_amount]
           ,[mo_tu_fee_de_posted_on]
           ,[mo_tu_fee_de_posted_by]
           ,[mo_tu_fee_de_update_on]
           ,[mo_tu_fee_de_update_by]
           ,[mo_tu_fee_de_remarks]
           ,[mo_tu_fee_de_late_fee],[mo_tu_fee_de__receipt_no])
     VALUES
           ((SELECT ISNULL(MAX([mo_tu_fee_de_sl_no]),0) FROM [tbl_acc_monthly_tution_fee_details])+1,'" +
                            aStudentPayment.HeadId + "','" + aStudentPayment1.Id + "','" + aStudentPayment1.Class + "','" +
                            aStudentPayment1.Year + "','" + aStudentPayment.TotalAmount + "','" +
                            aStudentPayment.PaidAmount + "' ,'" + aStudentPayment.DueAmount +
                            "' ,GETDATE() ,'Admin' ,'01-01-1990','' ,'' ,'','"+aStudentPayment1.MoneyReciptNo+"' )";
                        command.ExecuteNonQuery();
                    }
                    else if (aStudentPayment.PaymentName == "Yearly Computer Fee")
                    {
                        int count;
                        command.CommandText =
                            @"SELECT COUNT(*)  FROM [tbl_acc_yearly_computer_fee] WHERE [year_co_fee_ledger_id]='" +
                            aStudentPayment.HeadId + "' AND [year_co_fee_student_id]='" + aStudentPayment1.Id +
                            "' AND [year_co_fee_class]='" + aStudentPayment1.Class + "' AND [year_co_fee_year]='" +
                            aStudentPayment1.Year + "'";
                        count = Convert.ToInt32(command.ExecuteScalar());
                        if (count > 0)
                        {
                            command.CommandText =
                                @"UPDATE [tbl_acc_yearly_computer_fee] SET  [year_co_fee_total_paid_amount] =[year_co_fee_total_paid_amount]+'" +
                                aStudentPayment.PaidAmount + "',[year_co_fee_due_amount] ='" + aStudentPayment.DueAmount +
                                "',[year_co_fee_update_on] =GETDATE()  ,[year_co_fee_update_by] ='Admin' WHERE [year_co_fee_ledger_id]='" +
                                aStudentPayment.HeadId + "' AND [year_co_fee_student_id]='" + aStudentPayment1.Id +
                                "' AND [year_co_fee_class]='" + aStudentPayment1.Class + "' AND [year_co_fee_year]='" +
                                aStudentPayment1.Year + "'";
                            command.ExecuteNonQuery();
                        }
                        else
                        {
                            command.CommandText =
                                @"INSERT INTO [tbl_acc_yearly_computer_fee]
           ([year_co_fee_ledger_id]
           ,[year_co_fee_student_id]
           ,[year_co_fee_class]
           ,[year_co_fee_year]
           ,[year_co_fee_total_amount]
           ,[year_co_fee_total_paid_amount]
           ,[year_co_fee_due_amount]
           ,[year_co_fee_posted_on]
           ,[year_co_fee_posted_by]
           ,[year_co_fee_update_on]
           ,[year_co_fee_update_by]
           ,[year_co_fee_remarks]
           ,[year_co_fee_late_fee])
     VALUES
           ('" +
                                aStudentPayment.HeadId + "' ,'" + aStudentPayment1.Id + "' ,'" + aStudentPayment1.Class +
                                "' ,'" + aStudentPayment1.Year + "' ,'" + aStudentPayment.TotalAmount + "' ,'" +
                                aStudentPayment.PaidAmount + "' ,'" + aStudentPayment.DueAmount +
                                "' ,GETDATE(),'Admin' ,'01-01-1990','' ,'','')";
                            command.ExecuteNonQuery();
                        }

                        command.CommandText =
                            @"INSERT INTO [tbl_acc_yearly_computer_fee_details]
           ([year_co_fee_de_sl_no]
           ,[year_co_fee_de_ledger_id]
           ,[year_co_fee_de_student_id]
           ,[year_co_fee_de_class]
           ,[year_co_fee_de_year]
           ,[year_co_fee_de_total_amount]
           ,[year_co_fee_de_total_paid_amount]
           ,[year_co_fee_de_due_amount]
           ,[year_co_fee_de_posted_on]
           ,[year_co_fee_de_posted_by]
           ,[year_co_fee_de_update_on]
           ,[year_co_fee_de_update_by]
           ,[year_co_fee_de_remarks]
           ,[year_co_fee_de_late_fee],[year_co_fee_de_receipt_no])
     VALUES
           ((SELECT ISNULL(MAX([year_co_fee_de_sl_no]),0) FROM [tbl_acc_yearly_computer_fee_details])+1,'" +
                            aStudentPayment.HeadId + "','" + aStudentPayment1.Id + "','" + aStudentPayment1.Class + "','" +
                            aStudentPayment1.Year + "','" + aStudentPayment.TotalAmount + "','" +
                            aStudentPayment.PaidAmount + "' ,'" + aStudentPayment.DueAmount +
                            "' ,GETDATE() ,'Admin' ,'01-01-1990','' ,'' ,'','"+aStudentPayment1.MoneyReciptNo+"' )";
                        command.ExecuteNonQuery();
                    }
                    else if (aStudentPayment.PaymentName == "Yearly Charge")
                    {
                        int count;
                        command.CommandText =
                            @"SELECT COUNT(*) FROM [tbl_acc_yearly_charge] WHERE [acc_year_ch_ledger_id]='" +
                            aStudentPayment.HeadId + "' AND [acc_year_ch_student_id]='" + aStudentPayment1.Id +
                            "' AND [acc_year_ch_class]='" + aStudentPayment1.Class + "' AND [acc_year_ch_year]='" +
                            aStudentPayment1.Year + "'";
                        count = Convert.ToInt32(command.ExecuteScalar());
                        if (count > 0)
                        {
                            command.CommandText =
                                @"UPDATE  [tbl_acc_yearly_charge] SET [acc_year_ch_total_paid_amount] =[acc_year_ch_total_paid_amount]+'" +
                                aStudentPayment.PaidAmount + "',[acc_year_ch_due_amount]='" + aStudentPayment.DueAmount +
                                "',[acc_year_ch_update_on]=GETDATE()  ,[acc_year_ch_update_by]='Admin' WHERE [acc_year_ch_ledger_id]='" +
                                aStudentPayment.HeadId + "' AND [acc_year_ch_student_id]='" + aStudentPayment1.Id +
                                "' AND [acc_year_ch_class]='" + aStudentPayment1.Class + "' AND [acc_year_ch_year]='" +
                                aStudentPayment1.Year + "'";
                            command.ExecuteNonQuery();
                        }
                        else
                        {
                            command.CommandText =
                                @"INSERT INTO [tbl_acc_yearly_charge]
           ([acc_year_ch_ledger_id]
           ,[acc_year_ch_student_id]
           ,[acc_year_ch_class]
           ,[acc_year_ch_year]
           ,[acc_year_ch_total_amount]
           ,[acc_year_ch_total_paid_amount]
           ,[acc_year_ch_due_amount]
           ,[acc_year_ch_posted_on]
           ,[acc_year_ch_posted_by]
           ,[acc_year_ch_update_on]
           ,[acc_year_ch_update_by]
           ,[acc_year_ch_remarks]
           ,[acc_year_ch_late_fee])
     VALUES
           ('" +
                                aStudentPayment.HeadId + "' ,'" + aStudentPayment1.Id + "' ,'" + aStudentPayment1.Class +
                                "' ,'" + aStudentPayment1.Year + "' ,'" + aStudentPayment.TotalAmount + "' ,'" +
                                aStudentPayment.PaidAmount + "' ,'" + aStudentPayment.DueAmount +
                                "' ,GETDATE(),'Admin' ,'01-01-1990','' ,'','')";
                            command.ExecuteNonQuery();
                        }

                        command.CommandText =
                            @"INSERT INTO [tbl_acc_yearly_charge_details]
           ([acc_year_ch_de_sl_no]
           ,[acc_year_ch_de_ledger_id]
           ,[acc_year_ch_de_student_id]
           ,[acc_year_ch_de_class]
           ,[acc_year_ch_de_year]
           ,[acc_year_ch_de_total_amount]
           ,[acc_year_ch_de_total_paid_amount]
           ,[acc_year_ch_de_due_amount]
           ,[acc_year_ch_de_posted_on]
           ,[acc_year_ch_de_posted_by]
           ,[acc_year_ch_de_update_on]
           ,[acc_year_ch_de_update_by]
           ,[acc_year_ch_de_remarks]
           ,[acc_year_ch_de_late_fee],[acc_year_ch_de__receipt_no])
     VALUES
           ((SELECT ISNULL(MAX([acc_year_ch_de_sl_no]),0) FROM [tbl_acc_yearly_charge_details])+1,'" +
                            aStudentPayment.HeadId + "','" + aStudentPayment1.Id + "','" + aStudentPayment1.Class + "','" +
                            aStudentPayment1.Year + "','" + aStudentPayment.TotalAmount + "','" +
                            aStudentPayment.PaidAmount + "' ,'" + aStudentPayment.DueAmount +
                            "' ,GETDATE() ,'Admin' ,'01-01-1990','' ,'' ,'','"+aStudentPayment1.MoneyReciptNo+"' )";
                        command.ExecuteNonQuery();
                    }
                    else if (aStudentPayment.PaymentName == "Yearly Generator Fee")
                    {
                        int count;
                        command.CommandText =
                            @"SELECT COUNT(*)  FROM [tbl_acc_yearly_generator_fee] WHERE [year_gen_fee_ledger_id]='" +
                            aStudentPayment.HeadId + "' AND [year_gen_fee_student_id]='" + aStudentPayment1.Id +
                            "' AND [year_gen_fee_class]='" + aStudentPayment1.Class + "' AND [year_gen_fee_year]='" +
                            aStudentPayment1.Year + "'";
                        count = Convert.ToInt32(command.ExecuteScalar());
                        if (count > 0)
                        {
                            command.CommandText =
                                @"UPDATE [tbl_acc_yearly_generator_fee] SET  [year_gen_fee_total_paid_amount] =[year_gen_fee_total_paid_amount]+'" +
                                aStudentPayment.PaidAmount + "',[year_gen_fee_due_amount] ='" +
                                aStudentPayment.DueAmount +
                                "',[year_gen_fee_update_on]=GETDATE()  ,[year_gen_fee_update_by]='Admin' WHERE [year_gen_fee_ledger_id]='" +
                                aStudentPayment.HeadId + "' AND [year_gen_fee_student_id]='" + aStudentPayment1.Id +
                                "' AND [year_gen_fee_class]='" + aStudentPayment1.Class + "' AND [year_gen_fee_year]='" +
                                aStudentPayment1.Year + "'";
                            command.ExecuteNonQuery();
                        }
                        else
                        {
                            command.CommandText =
                                @"INSERT INTO [tbl_acc_yearly_generator_fee]
           ([year_gen_fee_ledger_id]
           ,[year_gen_fee_student_id]
           ,[year_gen_fee_class]
           ,[year_gen_fee_year]
           ,[year_gen_fee_total_amount]
           ,[year_gen_fee_total_paid_amount]
           ,[year_gen_fee_due_amount]
           ,[year_gen_fee_posted_on]
           ,[year_gen_fee_posted_by]
           ,[year_gen_fee_update_on]
           ,[year_gen_fee_update_by]
           ,[year_gen_fee_remarks]
           ,[year_gen_fee_late_fee])
     VALUES
           ('" +
                                aStudentPayment.HeadId + "' ,'" + aStudentPayment1.Id + "' ,'" + aStudentPayment1.Class +
                                "' ,'" + aStudentPayment1.Year + "' ,'" + aStudentPayment.TotalAmount + "' ,'" +
                                aStudentPayment.PaidAmount + "' ,'" + aStudentPayment.DueAmount +
                                "' ,GETDATE(),'Admin' ,'01-01-1990','' ,'','')";
                            command.ExecuteNonQuery();
                        }

                        command.CommandText =
                            @"INSERT INTO [tbl_acc_yearly_generator_fee_details]
           ([year_gen_fee_de_sl_no]
           ,[year_gen_fee_de_ledger_id]
           ,[year_gen_fee_de_student_id]
           ,[year_gen_fee_de_class]
           ,[year_gen_fee_de_year]
           ,[year_gen_fee_de_total_amount]
           ,[year_gen_fee_de_total_paid_amount]
           ,[year_gen_fee_de_due_amount]
           ,[year_gen_fee_de_posted_on]
           ,[year_gen_fee_de_posted_by]
           ,[year_gen_fee_de_update_on]
           ,[year_gen_fee_de_update_by]
           ,[year_gen_fee_de_remarks]
           ,[year_gen_fee_de_late_fee],[year_gen_fee_de_receipt_no])
     VALUES
           ((SELECT ISNULL(MAX([year_gen_fee_de_sl_no]),0) FROM [tbl_acc_yearly_generator_fee_details])+1,'" +
                            aStudentPayment.HeadId + "','" + aStudentPayment1.Id + "','" + aStudentPayment1.Class + "','" +
                            aStudentPayment1.Year + "','" + aStudentPayment.TotalAmount + "','" +
                            aStudentPayment.PaidAmount + "' ,'" + aStudentPayment.DueAmount +
                            "' ,GETDATE() ,'Admin' ,'01-01-1990','' ,'' ,'','"+aStudentPayment1.MoneyReciptNo+"' )";
                        command.ExecuteNonQuery();
                    }
                    else if (aStudentPayment.PaymentName == "Yearly Library Charge")
                    {
                        int count;
                        command.CommandText =
                            @"SELECT COUNT(*)  FROM [tbl_acc_yearly_library_charge] WHERE [year_lib_cha_fee_ledger_id]='" +
                            aStudentPayment.HeadId + "' AND [year_lib_cha_fee_student_id]='" + aStudentPayment1.Id +
                            "' AND [year_lib_cha_fee_class]='" + aStudentPayment1.Class +
                            "' AND [year_lib_cha_fee_year]='" + aStudentPayment1.Year + "'";
                        count = Convert.ToInt32(command.ExecuteScalar());
                        if (count > 0)
                        {
                            command.CommandText =
                                @"UPDATE [tbl_acc_yearly_library_charge] SET  [year_lib_cha_fee_total_paid_amount] =[year_lib_cha_fee_total_paid_amount]+'" +
                                aStudentPayment.PaidAmount + "',[year_lib_cha_fee_due_amount]='" +
                                aStudentPayment.DueAmount +
                                "',[year_lib_cha_fee_update_on]=GETDATE()  ,[year_lib_cha_fee_update_by]='Admin' WHERE [year_lib_cha_fee_ledger_id]='" +
                                aStudentPayment.HeadId + "' AND [year_lib_cha_fee_student_id]='" + aStudentPayment1.Id +
                                "' AND [year_lib_cha_fee_class]='" + aStudentPayment1.Class +
                                "' AND [year_lib_cha_fee_year]='" + aStudentPayment1.Year + "'";
                            command.ExecuteNonQuery();
                        }
                        else
                        {
                            command.CommandText =
                                @"INSERT INTO [tbl_acc_yearly_library_charge]
           ([year_lib_cha_fee_ledger_id]
           ,[year_lib_cha_fee_student_id]
           ,[year_lib_cha_fee_class]
           ,[year_lib_cha_fee_year]
           ,[year_lib_cha_fee_total_amount]
           ,[year_lib_cha_fee_total_paid_amount]
           ,[year_lib_cha_fee_due_amount]
           ,[year_lib_cha_fee_posted_on]
           ,[year_lib_cha_fee_posted_by]
           ,[year_lib_cha_fee_update_on]
           ,[year_lib_cha_fee_update_by]
           ,[year_lib_cha_fee_remarks]
           ,[year_lib_cha_fee_late_fee])
     VALUES
           ('" +
                                aStudentPayment.HeadId + "' ,'" + aStudentPayment1.Id + "' ,'" + aStudentPayment1.Class +
                                "' ,'" + aStudentPayment1.Year + "' ,'" + aStudentPayment.TotalAmount + "' ,'" +
                                aStudentPayment.PaidAmount + "' ,'" + aStudentPayment.DueAmount +
                                "' ,GETDATE(),'Admin' ,'01-01-1990','' ,'','')";
                            command.ExecuteNonQuery();
                        }

                        command.CommandText =
                            @"INSERT INTO [tbl_acc_yearly_library_charge_details]
           ([year_lib_cha_de_sl_no]
           ,[year_lib_cha_de_ledger_id]
           ,[year_lib_cha_de_student_id]
           ,[year_lib_cha_de_class]
           ,[year_lib_cha_de_year]
           ,[year_lib_cha_de_total_amount]
           ,[year_lib_cha_de_total_paid_amount]
           ,[year_lib_cha_de_due_amount]
           ,[year_lib_cha_de_posted_on]
           ,[year_lib_cha_de_posted_by]
           ,[year_lib_cha_de_update_on]
           ,[year_lib_cha_de_update_by]
           ,[year_lib_cha_de_remarks]
           ,[year_lib_cha_de_late_fee],[year_lib_cha_de_receipt_no])
     VALUES
           ((SELECT ISNULL(MAX([year_lib_cha_de_sl_no]),0) FROM [tbl_acc_yearly_library_charge_details])+1,'" +
                            aStudentPayment.HeadId + "','" + aStudentPayment1.Id + "','" + aStudentPayment1.Class + "','" +
                            aStudentPayment1.Year + "','" + aStudentPayment.TotalAmount + "','" +
                            aStudentPayment.PaidAmount + "' ,'" + aStudentPayment.DueAmount +
                            "' ,GETDATE() ,'Admin' ,'01-01-1990','' ,'' ,'','"+aStudentPayment1.MoneyReciptNo+"' )";
                        command.ExecuteNonQuery();
                    }
                    else if (aStudentPayment.PaymentName == "Yearly Cultural Program")
                    {
                        int count;
                        command.CommandText =
                            @"SELECT COUNT(*) FROM [tbl_acc_yearly_cultural_program_fee] WHERE [year_cul_pro_fee_ledger_id]='" +
                            aStudentPayment.HeadId + "' AND [year_cul_pro_fee_student_id]='" + aStudentPayment1.Id +
                            "' AND [year_cul_pro_fee_class]='" + aStudentPayment1.Class +
                            "' AND [year_cul_pro_fee_year]='" + aStudentPayment1.Year + "'";
                        count = Convert.ToInt32(command.ExecuteScalar());
                        if (count > 0)
                        {
                            command.CommandText =
                                @"UPDATE [tbl_acc_yearly_cultural_program_fee] SET [year_cul_pro_fee_total_paid_amount] =[year_cul_pro_fee_total_paid_amount]+'" +
                                aStudentPayment.PaidAmount + "',[year_cul_pro_fee_due_amount]='" +
                                aStudentPayment.DueAmount +
                                "',[year_cul_pro_fee_update_on]=GETDATE()  ,[year_cul_pro_fee_update_by]='Admin' WHERE [year_cul_pro_fee_ledger_id]='" +
                                aStudentPayment.HeadId + "' AND [year_cul_pro_fee_student_id]='" + aStudentPayment1.Id +
                                "' AND [year_cul_pro_fee_class]='" + aStudentPayment1.Class +
                                "' AND [year_cul_pro_fee_year]='" + aStudentPayment1.Year + "'";
                            command.ExecuteNonQuery();
                        }
                        else
                        {
                            command.CommandText =
                                @"INSERT INTO [tbl_acc_yearly_cultural_program_fee]
           ([year_cul_pro_fee_ledger_id]
           ,[year_cul_pro_fee_student_id]
           ,[year_cul_pro_fee_class]
           ,[year_cul_pro_fee_year]
           ,[year_cul_pro_fee_total_amount]
           ,[year_cul_pro_fee_total_paid_amount]
           ,[year_cul_pro_fee_due_amount]
           ,[year_cul_pro_fee_posted_on]
           ,[year_cul_pro_fee_posted_by]
           ,[year_cul_pro_fee_update_on]
           ,[year_cul_pro_fee_update_by]
           ,[year_cul_pro_fee_remarks]
           ,[year_cul_pro_fee_late_fee])
     VALUES
           ('" +
                                aStudentPayment.HeadId + "' ,'" + aStudentPayment1.Id + "' ,'" + aStudentPayment1.Class +
                                "' ,'" + aStudentPayment1.Year + "' ,'" + aStudentPayment.TotalAmount + "' ,'" +
                                aStudentPayment.PaidAmount + "' ,'" + aStudentPayment.DueAmount +
                                "' ,GETDATE(),'Admin' ,'01-01-1990','' ,'','')";
                            command.ExecuteNonQuery();
                        }

                        command.CommandText =
                            @"INSERT INTO [tbl_acc_yearly_cultural_program_fee_details]
           ([ycp_fee_de_sl_no]
           ,[ycp_fee_de_ledger_id]
           ,[ycp_fee_de_student_id]
           ,[ycp_fee_de_class]
           ,[ycp_fee_de_year]
           ,[ycp_fee_de_total_amount]
           ,[ycp_fee_de_total_paid_amount]
           ,[ycp_fee_de_due_amount]
           ,[ycp_fee_de_posted_on]
           ,[ycp_fee_de_posted_by]
           ,[ycp_fee_de_update_on]
           ,[ycp_fee_de_update_by]
           ,[ycp_fee_de_remarks]
           ,[ycp_fee_de_late_fee],[ycp_fee_de_receipt_no])
     VALUES
           ((SELECT ISNULL(MAX([ycp_fee_de_sl_no]),0) FROM [tbl_acc_yearly_cultural_program_fee_details])+1,'" +
                            aStudentPayment.HeadId + "','" + aStudentPayment1.Id + "','" + aStudentPayment1.Class + "','" +
                            aStudentPayment1.Year + "','" + aStudentPayment.TotalAmount + "','" +
                            aStudentPayment.PaidAmount + "' ,'" + aStudentPayment.DueAmount +
                            "' ,GETDATE() ,'Admin' ,'01-01-1990','' ,'' ,'','"+aStudentPayment1.MoneyReciptNo+"' )";
                        command.ExecuteNonQuery();
                    }
                    else if (aStudentPayment.PaymentName == "Yearly Sports &amp; Prize ")
                    {
                        int count;
                        command.CommandText =
                            @"SELECT COUNT(*) FROM [tbl_acc_yearly_sports_prize_fee] WHERE [ysp_fee_ledger_id]='" +
                            aStudentPayment.HeadId + "' AND [ysp_fee_student_id]='" + aStudentPayment1.Id +
                            "' AND [ysp_fee_class]='" + aStudentPayment1.Class + "' AND [ysp_fee_year]='" +
                            aStudentPayment1.Year + "'";
                        count = Convert.ToInt32(command.ExecuteScalar());
                        if (count > 0)
                        {
                            command.CommandText =
                                @"UPDATE [tbl_acc_yearly_sports_prize_fee] SET [ysp_fee_total_paid_amount] =[ysp_fee_total_paid_amount]+'" +
                                aStudentPayment.PaidAmount + "',[ysp_fee_due_amount]='" + aStudentPayment.DueAmount +
                                "',[ysp_fee_update_on]=GETDATE()  ,[ysp_fee_update_by]='Admin' WHERE [ysp_fee_ledger_id]='" +
                                aStudentPayment.HeadId + "' AND [ysp_fee_student_id]='" + aStudentPayment1.Id +
                                "' AND [ysp_fee_class]='" + aStudentPayment1.Class + "' AND [ysp_fee_year]='" +
                                aStudentPayment1.Year + "'";
                            command.ExecuteNonQuery();
                        }
                        else
                        {
                            command.CommandText =
                                @"INSERT INTO [tbl_acc_yearly_sports_prize_fee]
           ([ysp_fee_ledger_id]
           ,[ysp_fee_student_id]
           ,[ysp_fee_class]
           ,[ysp_fee_year]
           ,[ysp_fee_total_amount]
           ,[ysp_fee_total_paid_amount]
           ,[ysp_fee_due_amount]
           ,[ysp_fee_posted_on]
           ,[ysp_fee_posted_by]
           ,[ysp_fee_update_on]
           ,[ysp_fee_update_by]
           ,[ysp_fee_remarks]
           ,[ysp_fee_late_fee])
     VALUES
           ('" +
                                aStudentPayment.HeadId + "' ,'" + aStudentPayment1.Id + "' ,'" + aStudentPayment1.Class +
                                "' ,'" + aStudentPayment1.Year + "' ,'" + aStudentPayment.TotalAmount + "' ,'" +
                                aStudentPayment.PaidAmount + "' ,'" + aStudentPayment.DueAmount +
                                "' ,GETDATE(),'Admin' ,'01-01-1990','' ,'','')";
                            command.ExecuteNonQuery();
                        }

                        command.CommandText =
                            @"INSERT INTO [tbl_acc_yearly_sports_prize_fee_details]
           ([ysp_fee_de_sl_no]
           ,[ysp_fee_de_ledger_id]
           ,[ysp_fee_de_student_id]
           ,[ysp_fee_de_class]
           ,[ysp_fee_de_year]
           ,[ysp_fee_de_total_amount]
           ,[ysp_fee_de_total_paid_amount]
           ,[ysp_fee_de_due_amount]
           ,[ysp_fee_de_posted_on]
           ,[ysp_fee_de_posted_by]
           ,[ysp_fee_de_update_on]
           ,[ysp_fee_de_update_by]
           ,[ysp_fee_de_remarks]
           ,[ysp_fee_de_late_fee],[ysp_fee_de_receipt_no])
     VALUES
           ((SELECT ISNULL(MAX([ysp_fee_de_sl_no]),0) FROM [tbl_acc_yearly_sports_prize_fee_details])+1,'" +
                            aStudentPayment.HeadId + "','" + aStudentPayment1.Id + "','" + aStudentPayment1.Class + "','" +
                            aStudentPayment1.Year + "','" + aStudentPayment.TotalAmount + "','" +
                            aStudentPayment.PaidAmount + "' ,'" + aStudentPayment.DueAmount +
                            "' ,GETDATE() ,'Admin' ,'01-01-1990','' ,'' ,'','"+aStudentPayment1.MoneyReciptNo+"' )";
                        command.ExecuteNonQuery();
                    }
                    else if (aStudentPayment.PaymentName == "Yearly School Magazine  ")
                    {
                        int count;
                        command.CommandText =
                            @"SELECT COUNT(*) FROM [tbl_acc_yearly_school_magazine_fee] WHERE [ysm_fee_ledger_id]='" +
                            aStudentPayment.HeadId + "' AND [ysm_fee_student_id]='" + aStudentPayment1.Id +
                            "' AND [ysm_fee_class]='" + aStudentPayment1.Class + "' AND [ysm_fee_year]='" +
                            aStudentPayment1.Year + "'";
                        count = Convert.ToInt32(command.ExecuteScalar());
                        if (count > 0)
                        {
                            command.CommandText =
                                @"UPDATE [tbl_acc_yearly_school_magazine_fee] SET [ysm_fee_total_paid_amount] =[ysm_fee_total_paid_amount]+'" +
                                aStudentPayment.PaidAmount + "',[ysm_fee_due_amount]='" + aStudentPayment.DueAmount +
                                "',[ysm_fee_update_on]=GETDATE()  ,[ysm_fee_update_by]='Admin' WHERE [ysm_fee_ledger_id]='" +
                                aStudentPayment.HeadId + "' AND [ysm_fee_student_id]='" + aStudentPayment1.Id +
                                "' AND [ysm_fee_class]='" + aStudentPayment1.Class + "' AND [ysm_fee_year]='" +
                                aStudentPayment1.Year + "'";
                            command.ExecuteNonQuery();
                        }
                        else
                        {
                            command.CommandText =
                                @"INSERT INTO [tbl_acc_yearly_school_magazine_fee]
           ([ysm_fee_ledger_id]
           ,[ysm_fee_student_id]
           ,[ysm_fee_class]
           ,[ysm_fee_year]
           ,[ysm_fee_total_amount]
           ,[ysm_fee_total_paid_amount]
           ,[ysm_fee_due_amount]
           ,[ysm_fee_posted_on]
           ,[ysm_fee_posted_by]
           ,[ysm_fee_update_on]
           ,[ysm_fee_update_by]
           ,[ysm_fee_remarks]
           ,[ysm_fee_late_fee])
     VALUES
           ('" +
                                aStudentPayment.HeadId + "' ,'" + aStudentPayment1.Id + "' ,'" + aStudentPayment1.Class +
                                "' ,'" + aStudentPayment1.Year + "' ,'" + aStudentPayment.TotalAmount + "' ,'" +
                                aStudentPayment.PaidAmount + "' ,'" + aStudentPayment.DueAmount +
                                "' ,GETDATE(),'Admin' ,'01-01-1990','' ,'','')";
                            command.ExecuteNonQuery();
                        }

                        command.CommandText =
                            @"INSERT INTO [tbl_acc_yearly_school_magazine_fee_details]
           ([ysm_fee_de_sl_no]
           ,[ysm_fee_de_ledger_id]
           ,[ysm_fee_de_student_id]
           ,[ysm_fee_de_class]
           ,[ysm_fee_de_year]
           ,[ysm_fee_de_total_amount]
           ,[ysm_fee_de_total_paid_amount]
           ,[ysm_fee_de_due_amount]
           ,[ysm_fee_de_posted_on]
           ,[ysm_fee_de_posted_by]
           ,[ysm_fee_de_update_on]
           ,[ysm_fee_de_update_by]
           ,[ysm_fee_de_remarks]
           ,[ysm_fee_de_late_fee],[ysm_fee_de_receipt_no])
     VALUES
           ((SELECT ISNULL(MAX([ysm_fee_de_sl_no]),0) FROM [tbl_acc_yearly_school_magazine_fee_details])+1,'" +
                            aStudentPayment.HeadId + "','" + aStudentPayment1.Id + "','" + aStudentPayment1.Class + "','" +
                            aStudentPayment1.Year + "','" + aStudentPayment.TotalAmount + "','" +
                            aStudentPayment.PaidAmount + "' ,'" + aStudentPayment.DueAmount +
                            "' ,GETDATE() ,'Admin' ,'01-01-1990','' ,'' ,'','"+aStudentPayment1.MoneyReciptNo+"' )";
                        command.ExecuteNonQuery();
                    }
                    else if (aStudentPayment.PaymentName == "Charity ")
                    {
                        int count;
                        command.CommandText =@"SELECT COUNT(*) FROM [tbl_acc_charity_fee] WHERE [charity_fee_ledger_id]='" +
                            aStudentPayment.HeadId + "' AND [charity_fee_student_id]='" + aStudentPayment1.Id +
                            "' AND [charity_fee_class]='" + aStudentPayment1.Class + "' AND [charity_fee_year]='" +
                            aStudentPayment1.Year + "'";
                        count = Convert.ToInt32(command.ExecuteScalar());
                        if (count > 0)
                        {
                            command.CommandText =
                                @"UPDATE [tbl_acc_charity_fee] SET [charity_fee_total_paid_amount] =[charity_fee_total_paid_amount]+'" +
                                aStudentPayment.PaidAmount + "',[charity_fee_due_amount]='" + aStudentPayment.DueAmount +
                                "',[charity_fee_update_on]=GETDATE()  ,[charity_fee_update_by]='Admin' WHERE [charity_fee_ledger_id]='" +
                                aStudentPayment.HeadId + "' AND [charity_fee_student_id]='" + aStudentPayment1.Id +
                                "' AND [charity_fee_class]='" + aStudentPayment1.Class + "' AND [charity_fee_year]='" +
                                aStudentPayment1.Year + "'";
                            command.ExecuteNonQuery();
                        }
                        else
                        {
                            command.CommandText =
                                @"INSERT INTO [tbl_acc_charity_fee]
           ([charity_fee_ledger_id]
           ,[charity_fee_student_id]
           ,[charity_fee_class]
           ,[charity_fee_year]
           ,[charity_fee_total_amount]
           ,[charity_fee_total_paid_amount]
           ,[charity_fee_due_amount]
           ,[charity_fee_posted_on]
           ,[charity_fee_posted_by]
           ,[charity_fee_update_on]
           ,[charity_fee_update_by]
           ,[charity_fee_remarks]
           ,[charity_fee_late_fee])
     VALUES
           ('" +
                                aStudentPayment.HeadId + "' ,'" + aStudentPayment1.Id + "' ,'" + aStudentPayment1.Class +
                                "' ,'" + aStudentPayment1.Year + "' ,'" + aStudentPayment.TotalAmount + "' ,'" +
                                aStudentPayment.PaidAmount + "' ,'" + aStudentPayment.DueAmount +
                                "' ,GETDATE(),'Admin' ,'01-01-1990','' ,'','')";
                            command.ExecuteNonQuery();
                        }

                        command.CommandText =
                            @"INSERT INTO [tbl_acc_charity_fee_details]
           ([cha_fee_de_sl_no]
           ,[cha_fee_de_ledger_id]
           ,[cha_fee_de_student_id]
           ,[cha_fee_de_class]
           ,[cha_fee_de_year]
           ,[cha_fee_de_total_amount]
           ,[cha_fee_de_total_paid_amount]
           ,[cha_fee_de_due_amount]
           ,[cha_fee_de_posted_on]
           ,[cha_fee_de_posted_by]
           ,[cha_fee_de_update_on]
           ,[cha_fee_de_update_by]
           ,[cha_fee_de_remarks]
           ,[cha_fee_de_late_fee],[cha_fee__de__receipt_no])
     VALUES
           ((SELECT ISNULL(MAX([cha_fee_de_sl_no]),0) FROM [tbl_acc_charity_fee_details])+1,'" +
                            aStudentPayment.HeadId + "','" + aStudentPayment1.Id + "','" + aStudentPayment1.Class + "','" +
                            aStudentPayment1.Year + "','" + aStudentPayment.TotalAmount + "','" +
                            aStudentPayment.PaidAmount + "' ,'" + aStudentPayment.DueAmount +
                            "' ,GETDATE() ,'Admin' ,'01-01-1990','' ,'' ,'','"+aStudentPayment1.MoneyReciptNo+"' )";
                        command.ExecuteNonQuery();
                    }
                    else if (aStudentPayment.PaymentName == "Transport Fee For Two K.M")
                    {
                        int count;
                        command.CommandText =
                            @"SELECT COUNT(*) FROM [tbl_transport_two_km_fee] WHERE [mttkm_fee_ledger_id]='" +
                            aStudentPayment.HeadId + "' AND [mttkm_fee_fee_student_id]='" + aStudentPayment1.Id +
                            "' AND [mttkm_fee_fee_class]='" + aStudentPayment1.Class + "' AND [mttkm_fee_fee_year]='" +
                            aStudentPayment1.Year + "'";
                        count = Convert.ToInt32(command.ExecuteScalar());
                        if (count > 0)
                        {
                            command.CommandText =
                                @"UPDATE [tbl_transport_two_km_fee] SET [mttkm_fee_fee_total_paid_amount] =[mttkm_fee_fee_total_paid_amount]+'" +
                                aStudentPayment.PaidAmount + "',[mttkm_fee_fee_due_amount]='" +
                                aStudentPayment.DueAmount +
                                "',[mttkm_fee_fee_update_on]=GETDATE()  ,[mttkm_fee_fee_update_by]='Admin' WHERE [mttkm_fee_ledger_id]='" +
                                aStudentPayment.HeadId + "' AND [mttkm_fee_fee_student_id]='" + aStudentPayment1.Id +
                                "' AND [mttkm_fee_fee_class]='" + aStudentPayment1.Class + "' AND [mttkm_fee_fee_year]='" +
                                aStudentPayment1.Year + "'";
                            command.ExecuteNonQuery();
                        }
                        else
                        {
                            command.CommandText =
                                @"INSERT INTO [tbl_transport_two_km_fee]
           ([mttkm_fee_ledger_id]
           ,[mttkm_fee_fee_student_id]
           ,[mttkm_fee_fee_class]
           ,[mttkm_fee_fee_year]
           ,[mttkm_fee_fee_total_amount]
           ,[mttkm_fee_fee_total_paid_amount]
           ,[mttkm_fee_fee_due_amount]
           ,[mttkm_fee_fee_posted_on]
           ,[mttkm_fee_fee_posted_by]
           ,[mttkm_fee_fee_update_on]
           ,[mttkm_fee_fee_update_by]
           ,[mttkm_fee_fee_remarks]
           ,[mttkm_fee_fee_late_fee])
     VALUES
           ('" +
                                aStudentPayment.HeadId + "' ,'" + aStudentPayment1.Id + "' ,'" + aStudentPayment1.Class +
                                "' ,'" + aStudentPayment1.Year + "' ,'" + aStudentPayment.TotalAmount + "' ,'" +
                                aStudentPayment.PaidAmount + "' ,'" + aStudentPayment.DueAmount +
                                "' ,GETDATE(),'Admin' ,'01-01-1990','' ,'','')";
                            command.ExecuteNonQuery();
                        }

                        command.CommandText =
                            @"INSERT INTO [tbl_transport_two_km_fee_details]
           ([mottkm_fee_de_sl_no]
           ,[mottkm_fee_de_ledger_id]
           ,[mottkm_fee_de_student_id]
           ,[mottkm_fee_de_class]
           ,[mottkm_fee_de_year]
           ,[mottkm_fee_de_total_amount]
           ,[mottkm_fee_de_total_paid_amount]
           ,[mottkm_fee_de_due_amount]
           ,[mottkm_fee_de_posted_on]
           ,[mottkm_fee_de_posted_by]
           ,[mottkm_fee_de_update_on]
           ,[mottkm_fee_de_update_by]
           ,[mottkm_fee_de_remarks]
           ,[mottkm_fee_de_late_fee],[mottkm_fee_de__receipt_no])
     VALUES
           ((SELECT ISNULL(MAX([mottkm_fee_de_sl_no]),0) FROM [tbl_transport_two_km_fee_details])+1,'" +
                            aStudentPayment.HeadId + "','" + aStudentPayment1.Id + "','" + aStudentPayment1.Class + "','" +
                            aStudentPayment1.Year + "','" + aStudentPayment.TotalAmount + "','" +
                            aStudentPayment.PaidAmount + "' ,'" + aStudentPayment.DueAmount +
                            "' ,GETDATE() ,'Admin' ,'01-01-1990','' ,'' ,'' ,'"+aStudentPayment1.MoneyReciptNo+"')";
                        command.ExecuteNonQuery();
                    }
                    else if (aStudentPayment.PaymentName == "Transport Fee For Others  K.M")
                    {
                        int count;
                        command.CommandText =
                            @"SELECT COUNT(*) FROM [tbl_transport_others_fee] WHERE [mttkmot_fee_ledger_id]='" +
                            aStudentPayment.HeadId + "' AND [mttkmot_fee_fee_student_id]='" + aStudentPayment1.Id +
                            "' AND [mttkmot_fee_fee_class]='" + aStudentPayment1.Class + "' AND [mttkmot_fee_fee_year]='" +
                            aStudentPayment1.Year + "'";
                        count = Convert.ToInt32(command.ExecuteScalar());
                        if (count > 0)
                        {
                            command.CommandText =
                                @"UPDATE [tbl_transport_others_fee] SET [mttkmot_fee_fee_total_paid_amount] =[mttkmot_fee_fee_total_paid_amount]+'" +
                                aStudentPayment.PaidAmount + "',[mttkmot_fee_fee_due_amount]='" +
                                aStudentPayment.DueAmount +
                                "',[mttkmot_fee_fee_update_on]=GETDATE()  ,[mttkmot_fee_fee_update_by]='Admin' WHERE [mttkmot_fee_ledger_id]='" +
                                aStudentPayment.HeadId + "' AND [mttkmot_fee_fee_student_id]='" + aStudentPayment1.Id +
                                "' AND [mttkmot_fee_fee_class]='" + aStudentPayment1.Class +
                                "' AND [mttkmot_fee_fee_year]='" + aStudentPayment1.Year + "'";
                            command.ExecuteNonQuery();
                        }
                        else
                        {
                            command.CommandText =
                                @"INSERT INTO [tbl_transport_others_fee]
           ([mttkmot_fee_ledger_id]
           ,[mttkmot_fee_fee_student_id]
           ,[mttkmot_fee_fee_class]
           ,[mttkmot_fee_fee_year]
           ,[mttkmot_fee_fee_total_amount]
           ,[mttkmot_fee_fee_total_paid_amount]
           ,[mttkmot_fee_fee_due_amount]
           ,[mttkmot_fee_fee_posted_on]
           ,[mttkmot_fee_fee_posted_by]
           ,[mttkmot_fee_fee_update_on]
           ,[mttkmot_fee_fee_update_by]
           ,[mttkmot_fee_fee_remarks]
           ,[mttkmot_fee_fee_late_fee])
     VALUES
           ('" +
                                aStudentPayment.HeadId + "' ,'" + aStudentPayment1.Id + "' ,'" + aStudentPayment1.Class +
                                "' ,'" + aStudentPayment1.Year + "' ,'" + aStudentPayment.TotalAmount + "' ,'" +
                                aStudentPayment.PaidAmount + "' ,'" + aStudentPayment.DueAmount +
                                "' ,GETDATE(),'Admin' ,'01-01-1990','' ,'','')";
                            command.ExecuteNonQuery();
                        }

                        command.CommandText =
                            @"INSERT INTO [tbl_transport_others_fee_details]
           ([motot_fee_de_sl_no]
           ,[motot_fee_de_ledger_id]
           ,[motot_fee_de_student_id]
           ,[motot_fee_de_class]
           ,[motot_fee_de_year]
           ,[motot_fee_de_total_amount]
           ,[motot_fee_de_total_paid_amount]
           ,[motot_fee_de_due_amount]
           ,[motot_fee_de_posted_on]
           ,[motot_fee_de_posted_by]
           ,[motot_fee_de_update_on]
           ,[motot_fee_de_update_by]
           ,[motot_fee_de_remarks]
           ,[motot_fee_de_late_fee],[motot_fee_de__receipt_no])
     VALUES
           ((SELECT ISNULL(MAX([motot_fee_de_sl_no]),0) FROM [tbl_transport_others_fee_details])+1,'" +
                            aStudentPayment.HeadId + "','" + aStudentPayment1.Id + "','" + aStudentPayment1.Class + "','" +
                            aStudentPayment1.Year + "','" + aStudentPayment.TotalAmount + "','" +
                            aStudentPayment.PaidAmount + "' ,'" + aStudentPayment.DueAmount +
                            "' ,GETDATE() ,'Admin' ,'01-01-1990','' ,'' ,'','"+aStudentPayment1.MoneyReciptNo+"' )";
                        command.ExecuteNonQuery();
                    }
                }
            }
            transection.Commit();
        }
        catch (Exception ex)
        {
            transection.Rollback();
            throw new Exception(ex.Message);
        }
        finally
        {
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
        }
    }
}