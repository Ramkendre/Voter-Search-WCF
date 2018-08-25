using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LatestVoterSearch
{
    public partial class ControlChart : System.Web.UI.Page
    {
        string excelSubject = string.Empty;
        string fileExtension = string.Empty;
        OleDbConnection conn = new OleDbConnection();
        SqlCommand cmd = new SqlCommand();
        string conPath = "";
        int count;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                
            }
        }

        public DataSet GetDataTable(string strQuery)
        {

            DataSet tempDs = null;
            string filePath = Server.MapPath("File_Upload\\" + upldVoterList.FileName);
            fileExtension = Path.GetExtension(filePath);

            if (this.fileExtension == ".xls")
            {
                conPath = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filePath + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=1;TypeGuessRows=0;ImportMixedTypes=Text\"";
            }
            else
            {
                conPath = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=1;TypeGuessRows=0;ImportMixedTypes=Text\"";
            }
            conn = new OleDbConnection(conPath);
            try
            {
                conn.Open();

                OleDbDataAdapter adapter = new OleDbDataAdapter(strQuery, conn);
                tempDs = new DataSet();
                adapter.Fill(tempDs);
            }
            catch (Exception ex)
            {
                Response.Write("<Script>alert('" + ex.Message + "')</Script>");
            }
            conn.Close();

            return tempDs;
        }

        public void Addexcel(string SRNO, string ACNO, string PARTNO, string SRNO_FROM, string SRNO_TO, string WARDNUMBER) //, string LOCALBODYID, string LOCALBODYTYPE)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VoterSearchConStr"].ConnectionString))

                try
                {
                    if (SRNO != "" && ACNO != "" && PARTNO != "" && SRNO_FROM != "" && SRNO_TO != "" && WARDNUMBER != "")
                    {
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Message", "alert('Excel File does not Upload')", true);
                    }
                    else
                    {
                        cmd = new SqlCommand();
                        cmd.CommandText = "Sp_InsertVoterList";
                        cmd.Connection = con;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@SRNO", SRNO);
                        cmd.Parameters.AddWithValue("@ACNO", ACNO);
                        cmd.Parameters.AddWithValue("@PARTNO", PARTNO);
                        cmd.Parameters.AddWithValue("@SRNO_FROM", SRNO_FROM);
                        cmd.Parameters.AddWithValue("@SRNO_TO", SRNO_TO);
                        cmd.Parameters.AddWithValue("@WARDNUMBER", WARDNUMBER);
                        // cmd.Parameters.AddWithValue("@LOCALBODYID", LOCALBODYID);
                        //cmd.Parameters.AddWithValue("@LOCALBODYTYPE", LOCALBODYTYPE);
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
                catch
                {

                }
        }

        string srno = ""; string AcNo = ""; string PartNo = ""; string LocalBodyType = "";
        string Srno_From = ""; string Srno_To = ""; string WardNumber = ""; string LocalBodyId = "";
        public void FetchQuestion(DataSet ExcelDs)
        {
            int countVal = ExcelDs.Tables[0].Rows.Count;

            for (int i = 0; i < countVal; i++)
            {
                srno = Convert.ToString(ExcelDs.Tables[0].Rows[i]["SrNo"]);    //Excel Sheet Column name
                AcNo = Convert.ToString(ExcelDs.Tables[0].Rows[i]["AcNo"]);
                PartNo = Convert.ToString(ExcelDs.Tables[0].Rows[i]["PartNo"]);
                Srno_From = Convert.ToString(ExcelDs.Tables[0].Rows[i]["Srno_From"]);
                Srno_To = Convert.ToString(ExcelDs.Tables[0].Rows[i]["Srno_To"]);
                WardNumber = Convert.ToString(ExcelDs.Tables[0].Rows[i]["WardNumber"]);
                //LocalBodyId = Convert.ToString(ExcelDs.Tables[0].Rows[i]["LocalBodyId"]);
                //LocalBodyType = Convert.ToString(ExcelDs.Tables[0].Rows[i]["LocalBodyType"]);
                //if (rdoControlChartList.SelectedValue == "1")
                //{
                Addexcel(srno, AcNo, PartNo, Srno_From, Srno_To, WardNumber);   //, LocalBodyId, LocalBodyType);
                //}
                //else
                //{
                //  Addexcel(srno, AcNo, PartNo, Srno_From, Srno_To, WardNumber, LocalBodyId, LocalBodyType);
                //}
                count++;
            }
        }

        DataSet ExcelDB;

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (upldVoterList.HasFile)
            {
                string path = "";
                path = Server.MapPath("File_Upload");
                path = path + "\\" + upldVoterList.FileName;
                string aa = upldVoterList.FileName;

                if (File.Exists(path))
                {
                    File.Delete(path);
                    upldVoterList.SaveAs(path);
                }
                else
                {
                    upldVoterList.SaveAs(path);
                }

                excelSubject = "UPLOAD";

                string strQuery = "SELECT * FROM [" + excelSubject + "$]";
                DataSet dscount = GetDataTable(strQuery);

                FetchQuestion(dscount);
            }
        }
    }
}