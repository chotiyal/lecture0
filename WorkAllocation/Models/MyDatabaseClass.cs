using System;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data;

using System.Web.UI;
using System.Web.UI.WebControls;

using System.Net.Mail;

/// <summary>
/// Summary description for MyDatabaseClass
/// </summary>

public class MyDatabaseClass
{
    public MyDatabaseClass()
    {

    }

    public System.Data.DataSet GetDataSet(String sqlQuery, ref bool hasExceptionThrown, ref String errorMessage)
    {
        DataSet ds = new DataSet();
        hasExceptionThrown = false;
        errorMessage = "";
        SqlConnection con = null;

        try
        {
            con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            using (SqlDataAdapter da = new SqlDataAdapter(sqlQuery, con))
            {
                da.Fill(ds);
            }
        }
        catch (Exception ex)
        {
            ds = null;
            hasExceptionThrown = true;
            errorMessage = ex.Message;
        }
        finally
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
        }
        return ds;
    }

    public int GetRowsAffected(String sqlQuery, ref bool hasExceptionThrown, ref String errorMessage)
    {
        int affectedRows = 0;
        hasExceptionThrown = false;
        errorMessage = "";

        SqlConnection con = null;

        try
        {
            con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
            {
                affectedRows = cmd.ExecuteNonQuery();
            }
        }
        catch (Exception ex)
        {
            affectedRows = -1;
            hasExceptionThrown = true;
            errorMessage = ex.Message;
        }
        finally
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
        }

        return affectedRows;
    }

    public int GetRowsAffected(SortedList sortedList, ref bool hasExceptionThrown, ref String errorMessage)
    {
        int affectedRows = 0;
        hasExceptionThrown = false;
        errorMessage = "";

        SqlConnection con = null;
        SqlTransaction transaction = null;
        String sqlQuery = String.Empty;

        try
        {
            con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["conString"].ConnectionString);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            SqlCommand cmd = con.CreateCommand();
            transaction = con.BeginTransaction();
            cmd.Transaction = transaction;

            for (int i = 0; i < sortedList.Count; i++)
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sortedList[i].ToString();
                cmd.ExecuteNonQuery();
            }

            affectedRows = sortedList.Count;
            transaction.Commit();
            cmd.Dispose();
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            affectedRows = -1;
            hasExceptionThrown = true;
            errorMessage = ex.Message;
        }
        finally
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
        }

        return affectedRows;
    }

    public String GetSingleValue(String sqlQuery, ref bool hasExceptionThrown, ref String errorMessage)
    {
        String singleValue = null;
        hasExceptionThrown = false;
        errorMessage = "";

        try
        {
            DataSet ds = GetDataSet(sqlQuery, ref hasExceptionThrown, ref errorMessage);

            if (ds != null)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    singleValue = ds.Tables[0].Rows[0][0].ToString();
                }
            }
        }
        catch (Exception ex)
        {
            singleValue = null;
            hasExceptionThrown = true;
            errorMessage = ex.Message;
        }

        return singleValue;
    }

    public String GetNewID(String tableName, String columnName, String startCharacter, ref bool hasExceptionThrown, ref String errorMessage)
    {
        String maxId = String.Empty;
        String sqlQuery = "SELECT UPPER(DATA_TYPE) OK FROM user_tab_columns WHERE TABLE_NAME = UPPER('" + tableName + "') AND COLUMN_NAME = UPPER('" + columnName + "')";
        String dataType = GetSingleValue(sqlQuery, ref hasExceptionThrown, ref errorMessage);

        if (dataType != null)
        {
            switch (dataType)
            {
                case "NUMBER":
                    sqlQuery = "SELECT (NVL(MAX(" + columnName + "), 0) + 1) OK FROM " + tableName;
                    maxId = GetSingleValue(sqlQuery, ref hasExceptionThrown, ref errorMessage);
                    break;

                default:
                    sqlQuery = "SELECT " + columnName + " FROM " + tableName + " where rownum<=1 ORDER BY " + columnName + " DESC";

                    String tmp = GetSingleValue(sqlQuery, ref hasExceptionThrown, ref errorMessage);

                    if (tmp == null)
                    {
                        if (startCharacter != "" && startCharacter != null && startCharacter != String.Empty)
                        {
                            maxId = startCharacter + "000001";
                        }
                        else
                        {
                            maxId = "A000001";
                        }
                    }
                    else
                    {
                        char startChar = tmp.ToCharArray()[0];
                        int index = 0;
                        tmp = tmp.Substring(1);
                        int.TryParse(tmp, out index);

                        if (index == 999999)
                        {
                            startChar = (char)((int)startChar + 1);
                            index = 1;
                        }
                        else
                        {
                            index++;
                        }

                        maxId = startChar.ToString() + index.ToString().PadLeft(6, '0');
                    }
                    break;
            }
        }

        return maxId;
    }
    //public String SendEmail(String from, String to, String subject, String body)
    //{
    //    bool sendSuccess = false;
    //    try
    //    {
    //        System.Net.Mail.MailMessage mailMessage = new System.Net.Mail.MailMessage(from, to, subject, body);
    //        mailMessage.IsBodyHtml = true;
    //        System.Net.Mail.SmtpClient smtpClient = new System.Net.Mail.SmtpClient("smtp.rediffmailpro.com");
    //        smtpClient.UseDefaultCredentials = false;
    //        smtpClient.Credentials = new System.Net.NetworkCredential("hr.induction@ceinsys.com", "HR_INDUC*777");
    //        smtpClient.EnableSsl = true;
    //        System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object s, System.Security.Cryptography.X509Certificates.X509Certificate certificate, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors) { return true; };
    //        smtpClient.Send(mailMessage);

    //        sendSuccess = true;
    //    }
    //    catch
    //    {
    //        sendSuccess = false;
    //    }
    //    return sendSuccess.ToString();
    //}
    public string sendEMail(string sender, string receiver, string subject, string msg)
    {
        string retval = "done";
        try
        {
            MailMessage msg1 = new MailMessage(sender, receiver, subject, msg);
            SmtpClient client = new SmtpClient("smtp.rediffmailpro.com");
            client.Port = 587;
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential("wms@ceinsys.com", "Ceinsys@1234"); 
            client.EnableSsl = false;
            msg1.IsBodyHtml = true;
            client.Send(msg1);
            return retval;
        }
        catch(Exception ex)
        {
            return ex.Message;
        }
    }
    public string sendEMailWithAttachment(string sender, string receiver, string subject, string msg, string attachment1, string attachment2, string attachment3)
    {
        string retval = "done";
        try
        {
            MailMessage msg1 = new MailMessage(sender, receiver, subject, msg);
            msg1.Attachments.Add(new Attachment(attachment1));
            if(attachment2 !="")
                msg1.Attachments.Add(new Attachment(attachment2));
            if (attachment3 != "")
                msg1.Attachments.Add(new Attachment(attachment3));
            SmtpClient client = new SmtpClient("smtp.rediffmailpro.com");
            client.Port = 587;
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential("wms@ceinsys.com", "Ceinsys@1234"); 
            client.EnableSsl = false;
            msg1.IsBodyHtml = true;
            client.Send(msg1);
            return retval;
        }
        catch
        {
            return receiver;
        }
    }
    public DataSet readExcel(string filepath, string sheetname)
    {//Microsoft.ACE.Sql.12.0;Data Source=" + path + ";Extended Properties=Excel 12.0;";
        string connstr = "Provider=Microsoft.Jet.Sql.4.0;Data Source='" + filepath + "';Extended Properties=Excel 8.0";
        SqlConnection conn = new SqlConnection(connstr);
        string strSQL = "SELECT * FROM [" + sheetname + "$]";
        SqlCommand cmd = new SqlCommand(strSQL, conn);
        DataSet ds = new DataSet();
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        da.Fill(ds);
        return ds;
    }
    public void ClearControls(Control c)
    {
        try
        {
            foreach (Control c1 in c.Controls)
            {
                if (c1.GetType() == typeof(TextBox))
                {
                    ((TextBox)c1).Text = "";
                }
                if (c1.GetType() == typeof(DropDownList))
                {
                    ((DropDownList)c1).SelectedIndex = 0;
                }
                if (c1.HasControls())
                {
                    ClearControls(c1);
                }
            }
        }
        catch
        { }
    }
    public string getMonth(int M)
    {
        switch (M)
        {
            case 1:
                return "Jan";
            case 2:
                return "Feb";
            case 3:
                return "Mar";
            case 4:
                return "Apr";
            case 5:
                return "May";
            case 6:
                return "Jun";
            case 7:
                return "Jul";
            case 8:
                return "Aug";
            case 9:
                return "Sep";
            case 10:
                return "Oct";
            case 11:
                return "Nov";
            case 12:
                return "Dec";
        }
        return "";
    }
}

