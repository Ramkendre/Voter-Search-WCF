using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LatestVoterSearch
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : IService1
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VoterSearchConStr"].ConnectionString);
        //SqlConnection tvcon = new SqlConnection(ConfigurationManager.ConnectionStrings["TrueVoterConnectionString"].ConnectionString);
        SqlConnection seccon = new SqlConnection(ConfigurationManager.ConnectionStrings["SecConStr"].ConnectionString);

        SqlConnection sectempcon = new SqlConnection(ConfigurationManager.ConnectionStrings["SecTempConStr"].ConnectionString);
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();

        List<clsVoterSearchWS> cls = new List<clsVoterSearchWS>();
        clsVoterSearchWS objcls;
        List<WardWiseSearch> lstward = new List<WardWiseSearch>();

        DataSet ds = new DataSet();
        private static JsonSerializerSettings theJsonSerializerSettings = new JsonSerializerSettings();
        private static StringEncryptor myStringEncryptor = StringEncryptor.Instance;


        string data = string.Empty;
        string Sql = string.Empty; 
        string data11 = string.Empty;
       // private static JsonSerializerSettings theJsonSerializerSettings = new JsonSerializerSettings();
        string jsonDataStrngReturn = string.Empty; 
        //private static StringEncryptor myStringEncryptor = StringEncryptor.Instance;

        //public string UploadDataFile(Stream jsData)
        //{
        //    int result = 0;
        //    try
        //    {
        //        using (StreamReader reader = new StreamReader(jsData))
        //        {
        //            data = reader.ReadToEnd();
        //        }
        //        data = data.Replace("\"", "'");

        //      //  data11 = myStringEncryptor.Decrypt(data);

        //        DataContractJsonSerializer obj = new DataContractJsonSerializer(typeof(Employee));

        //        List<Employee> empobj = JsonConvert.DeserializeObject<List<Employee>>(data);

        //        if (con.State == ConnectionState.Closed)
        //        {
        //            con.Open();
        //        }
        //        foreach (Employee classobj in empobj)
        //        {
        //            SqlCommand cmd = new SqlCommand("Insert into [DBVoterSearch].[dbo].[tblDemo]([Name],[Address],[salary],[Department]) values(@name,@address,@salary,@dept)", con);
        //            cmd.Parameters.Add("@name", SqlDbType.NVarChar).Value = classobj.Name.Trim();
        //            cmd.Parameters.Add("@address", SqlDbType.NVarChar).Value = classobj.Address.Trim();
        //            cmd.Parameters.Add("@salary", SqlDbType.NVarChar).Value = classobj.salary.Trim();
        //            cmd.Parameters.Add("@dept", SqlDbType.NVarChar).Value = classobj.Dept.Trim();
        //            result = cmd.ExecuteNonQuery();
        //        }
        //        if (result.Equals(1))
        //        {
        //            jsonDataStrngReturn = "Insert Successful";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        jsonDataStrngReturn = "Fail";
        //    }
        //    finally
        //    {
        //        con.Close();
        //    }
        //    return jsonDataStrngReturn;
        //}

        //public string VoterAnalysisuploadData(Stream js)
        //{
        //    int result = 0;
        //    try
        //    {
        //        using (StreamReader reader = new StreamReader(js))
        //        {
        //            data = reader.ReadToEnd();
        //        }
        //        data = data.Replace("\"", "'");

        //        DataContractJsonSerializer obj = new DataContractJsonSerializer(typeof(VoterAnalysis_Class));
        //        List<VoterAnalysis_Class> empobj = JsonConvert.DeserializeObject<List<VoterAnalysis_Class>>(data);

        //        if (con.State == ConnectionState.Closed)
        //        {
        //            con.Open();
        //        }
        //        foreach (VoterAnalysis_Class voterobj in empobj)
        //        {
        //            cmd.CommandText = "[spUploadVoterAnalysis]";
        //            cmd.CommandType = CommandType.StoredProcedure;
        //            con.Open();
        //            cmd.Parameters.Add("@appId", SqlDbType.Int).Value = voterobj.APPID;
        //            cmd.Parameters.Add("@aCNO", SqlDbType.NVarChar).Value = voterobj.ACNO.Trim();
        //            cmd.Parameters.Add("@pART_No", SqlDbType.NVarChar).Value = voterobj.PART_NO.Trim();
        //            cmd.Parameters.Add("@sRNO", SqlDbType.NVarChar).Value = voterobj.SRNO.Trim();
        //            cmd.Parameters.Add("@hOUSE_No", SqlDbType.NVarChar).Value = voterobj.HOUSE_NO.Trim();
        //            cmd.Parameters.Add("@sECTION_NO", SqlDbType.NVarChar).Value = voterobj.SECTION_NO.Trim();
        //            cmd.Parameters.Add("@fNAME_M", SqlDbType.NVarChar).Value = voterobj.FNAME_M.Trim();
        //            cmd.Parameters.Add("@lNAME_M", SqlDbType.NVarChar).Value = voterobj.LNAME_M.Trim();
        //            cmd.Parameters.Add("@rELATION_TYPE", SqlDbType.NVarChar).Value = voterobj.RELATION_TYPE.Trim();
        //            cmd.Parameters.Add("@rELATION_FNAME_M", SqlDbType.NVarChar).Value = voterobj.RELATION_FNAME_M.Trim();


        //            cmd.Parameters.Add("@rELATION_LNAME_M", SqlDbType.NVarChar).Value = voterobj.RELATION_LNAME_M.Trim();
        //            cmd.Parameters.Add("@iDCARD_ID", SqlDbType.NVarChar).Value = voterobj.IDCARD_ID.Trim();
        //            cmd.Parameters.Add("@sTATUS_TYPE", SqlDbType.NVarChar).Value = voterobj.STATUS_TYPE.Trim();
        //            cmd.Parameters.Add("@sEX", SqlDbType.NVarChar).Value = voterobj.SEX.Trim();
        //            cmd.Parameters.Add("@aGE", SqlDbType.NVarChar).Value = voterobj.AGE.Trim();
        //            cmd.Parameters.Add("@fNAME_E", SqlDbType.NVarChar).Value = voterobj.FNAME_E.Trim();
        //            cmd.Parameters.Add("@lNAME_E", SqlDbType.NVarChar).Value = voterobj.LNAME_E.Trim();
        //            cmd.Parameters.Add("@rELATION_FNAME_E", SqlDbType.NVarChar).Value = voterobj.RELATION_FNAME_E.Trim();
        //            cmd.Parameters.Add("@rELATION_LNAME_E", SqlDbType.NVarChar).Value = voterobj.RELATION_LNAME_E.Trim();
        //            cmd.Parameters.Add("@fULLNAME_E", SqlDbType.NVarChar).Value = voterobj.FULLNAME_E.Trim();

        //            cmd.Parameters.Add("@eB_NO", SqlDbType.NVarChar).Value = voterobj.EB_NO.Trim();
        //            cmd.Parameters.Add("@aLLOCATED_WARD", SqlDbType.NVarChar).Value = voterobj.ALLOCATED_WARD.Trim();
        //            cmd.Parameters.Add("@sERIALNO_INWARD", SqlDbType.NVarChar).Value = voterobj.SERIALNO_INWARD.Trim();
        //            cmd.Parameters.Add("@bOOTH_NO", SqlDbType.NVarChar).Value = voterobj.BOOTH_NO.Trim();
        //            cmd.Parameters.Add("@sERIALNO_FOR_FINAL_LIST", SqlDbType.NVarChar).Value = voterobj.SERIALNO_FOR_FINAL_LIST.Trim();
        //            cmd.Parameters.Add("@oLD_SERIALIN_WARD", SqlDbType.NVarChar).Value = voterobj.OLD_SERIALIN_WARD.Trim();
        //            cmd.Parameters.Add("@pHOTO", SqlDbType.NVarChar).Value = voterobj.PHOTO.Trim();
        //            cmd.Parameters.Add("@vUI_CODE", SqlDbType.NVarChar).Value = voterobj.VUI_CODE.Trim();
        //            cmd.Parameters.Add("@election_ID", SqlDbType.NVarChar).Value = voterobj.Election_ID.Trim();
        //            cmd.Parameters.Add("@sT_CODE", SqlDbType.NVarChar).Value = voterobj.ST_CODE.Trim();


        //            cmd.Parameters.Add("@fVTM_TYPE", SqlDbType.NVarChar).Value = voterobj.FVTM_TYPE.Trim();
        //            cmd.Parameters.Add("@sEGMENT_NO", SqlDbType.NVarChar).Value = voterobj.SEGMENT_NO.Trim();
        //            cmd.Parameters.Add("@fVTM_NO", SqlDbType.NVarChar).Value = voterobj.FVTM_NO.Trim();
        //            cmd.Parameters.Add("@sECN_CATY", SqlDbType.NVarChar).Value = voterobj.SECN_CATY.Trim();
        //            cmd.Parameters.Add("@sTARTSL_NO", SqlDbType.NVarChar).Value = voterobj.STARTSL_NO.Trim();
        //            cmd.Parameters.Add("@hADBAST_NO", SqlDbType.NVarChar).Value = voterobj.HADBAST_NO.Trim();
        //            cmd.Parameters.Add("@pINCODE", SqlDbType.NVarChar).Value = voterobj.PINCODE.Trim();
        //            cmd.Parameters.Add("@dist_no", SqlDbType.NVarChar).Value = voterobj.Dist_no.Trim();
        //            cmd.Parameters.Add("@Section_name_v1", SqlDbType.NVarChar).Value = voterobj.section_name_v1.Trim();
        //            cmd.Parameters.Add("@Section_name_en", SqlDbType.NVarChar).Value = voterobj.section_name_en.Trim();


        //            cmd.Parameters.Add("@Section_id", SqlDbType.NVarChar).Value = voterobj.section_id.Trim();
        //            cmd.Parameters.Add("@aC_Name_V1", SqlDbType.NVarChar).Value = voterobj.AC_Name_V1.Trim();
        //            cmd.Parameters.Add("@aC_Name_En", SqlDbType.NVarChar).Value = voterobj.AC_Name_En.Trim();
        //            cmd.Parameters.Add("@part_Name_V1", SqlDbType.NVarChar).Value = voterobj.Part_Name_V1.Trim();
        //            cmd.Parameters.Add("@part_Name_En", SqlDbType.NVarChar).Value = voterobj.Part_Name_En.Trim();
        //            cmd.Parameters.Add("@district_ID", SqlDbType.NVarChar).Value = voterobj.District_ID.Trim();
        //            cmd.Parameters.Add("@district_Name_En", SqlDbType.NVarChar).Value = voterobj.District_Name_En.Trim();
        //            cmd.Parameters.Add("@district_Name_V1", SqlDbType.NVarChar).Value = voterobj.District_Name_V1.Trim();
        //            cmd.Parameters.Add("@distId", SqlDbType.NVarChar).Value = voterobj.DistId.Trim();
        //            cmd.Parameters.Add("@localBodyType", SqlDbType.NVarChar).Value = voterobj.LocalBodyType.Trim();


        //            cmd.Parameters.Add("@localBodyId", SqlDbType.NVarChar).Value = voterobj.LocalBodyId.Trim();
        //            cmd.Parameters.Add("@cOLOR", SqlDbType.NVarChar).Value = voterobj.COLOR.Trim();
        //            cmd.Parameters.Add("@cASTE", SqlDbType.NVarChar).Value = voterobj.CASTE.Trim();
        //            cmd.Parameters.Add("@mOB_NUM", SqlDbType.NVarChar).Value = voterobj.MOB_NUM.Trim();
        //            cmd.Parameters.Add("@cATEGORY", SqlDbType.NVarChar).Value = voterobj.CATEGORY.Trim();
        //            cmd.Parameters.Add("@qualification", SqlDbType.NVarChar).Value = voterobj.Qualification.Trim();
        //            cmd.Parameters.Add("@occupation", SqlDbType.NVarChar).Value = voterobj.Occupation.Trim();
        //            cmd.Parameters.Add("@createdBy", SqlDbType.NVarChar).Value = voterobj.CreatedBy.Trim();
        //            cmd.Parameters.Add("@createdDate", SqlDbType.NVarChar).Value = voterobj.CreatedDate.Trim();
        //            //cmd.Parameters.Add("", SqlDbType.Int).Value = voterobj.APPID;

        //            //result = cmd.ExecuteNonQuery();
        //        }
        //        if (result.Equals(1))
        //        {
        //            jsonDataStrngReturn = "Insert Successful";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        jsonDataStrngReturn = "Fail";
        //    }
        //    finally
        //    {
        //        con.Close();
        //    }
        //    return jsonDataStrngReturn;
        //}
       
        public string InsertRegRecord(string MaxId)
        {
            SqlCommand cmd = new SqlCommand();
            DataSet ds1 = new DataSet();
            try
            {
                Sql = "select [LBID],[Id],[FirstName],[MiddleName],[LastName],[MobNo],[EDivisionID],[EDivisionName],[ECollegeID],[ECollegeName],[UserName] FROM [RegistrationDtls] " +
                       "WHERE Id > '" + MaxId + "'";
                da = new SqlDataAdapter(Sql, seccon);
                da.Fill(ds);

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string sql = string.Empty;

                    Sql = "Select [LBDistrictID] from [LB_District] where [LBID]='" + ds.Tables[0].Rows[i]["LBID"].ToString() + "'";
                    cmd = new SqlCommand(Sql, seccon);
                    seccon.Open();
                    string districtId = Convert.ToString(cmd.ExecuteScalar());
                    seccon.Close();

                    cmd = new SqlCommand("Select [Districtname] from [mstDistrict] where [Districtcode]='" + districtId + "'", seccon);
                    seccon.Open();
                    string Districtname = Convert.ToString(cmd.ExecuteScalar());
                    seccon.Close();

                    Sql = "Select [LBTalukaID] from [LB_Taluka] where [LBID]='" + ds.Tables[0].Rows[i]["LocalBodyID"].ToString() + "'";
                    cmd = new SqlCommand(Sql, seccon);
                    seccon.Open();
                    string talukaId = Convert.ToString(cmd.ExecuteScalar());
                    seccon.Close();

                    Sql = "Select [SubDistrictname] from [mstTaluka] where [SubDistrictcode]='" + talukaId + "'";
                    cmd = new SqlCommand(Sql, seccon);
                    seccon.Open();
                    string talukaName = Convert.ToString(cmd.ExecuteScalar());
                    seccon.Close();

                    sql = "Insert into [tblRegistrationSEC]([RegIDSEC],[FirstName],[MiddleName],[LastName],[RegMobileNo],[DistrictId],[DistrictName],[TalukaId],[TalukaName],[ElectrolId] " +
                         ",[ElectrolName],[ElectrolClgId],[ElectrolClgName],[PartyName],[SymbolName],[ElectionName]) values(N'" + ds.Tables[0].Rows[i]["Id"].ToString() + "',N'" + ds.Tables[0].Rows[i]["FirstName"].ToString() + "' " +
                         ",N'" + ds.Tables[0].Rows[i]["MiddleName"].ToString() + "',N'" + ds.Tables[0].Rows[i]["LastName"].ToString() + "',N'" + ds.Tables[0].Rows[i]["MobNo"].ToString() + "' " +
                         ",N'" + districtId + "','" + Districtname + "',N'" + talukaId + "',N'" + talukaName + "' " +
                        ",N'" + ds.Tables[0].Rows[i]["EDivisionID"].ToString() + "',N'" + ds.Tables[0].Rows[i]["EDivisionName"].ToString() + "',N'" + ds.Tables[0].Rows[i]["ECollegeID"].ToString() + "',N'" + ds.Tables[0].Rows[i]["ECollegeName"].ToString() + "' " +
                      ",N'" + ds.Tables[0].Rows[i]["UserName"].ToString() + "')";
                    cmd = new SqlCommand(sql, sectempcon);
                    sectempcon.Open();
                    cmd.ExecuteNonQuery();
                    sectempcon.Close();
                    jsonDataStrngReturn += "1";
                }
            }
            catch (Exception ex)
            {
                jsonDataStrngReturn += "0";
            }
            finally
            {
                ds.Clear();
            }
            return jsonDataStrngReturn;
        }

        public string InsertRegRecordMP(string MaxId)
        {
            SqlCommand cmd = new SqlCommand();
            DataSet ds1 = new DataSet();
            try
            {
                Sql = "select [LocalBodyID],[FirstName],[MiddleName],[LastName],[CandidateMob],[Address],[NominationID] FROM  [tblNominationMp] ";//  +
                       //"WHERE NominationID < '" + MaxId + "'";
                da = new SqlDataAdapter(Sql, seccon);
                da.Fill(ds);

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string sql = string.Empty;

                    Sql = "Select [LBDistrictID] from [LB_District] where [LBID]='" + ds.Tables[0].Rows[i]["LocalBodyID"].ToString() + "'";
                    cmd = new SqlCommand(Sql, seccon);
                    seccon.Open();
                    string districtId = Convert.ToString(cmd.ExecuteScalar());
                    seccon.Close();

                    cmd = new SqlCommand("Select [Districtname] from [mstDistrict] where [Districtcode]='" + districtId.ToString() + "'", seccon);
                    seccon.Open();
                    string Districtname = Convert.ToString(cmd.ExecuteScalar());
                    seccon.Close();

                    //Sql = "Select [LBTalukaID] from [LB_Taluka] where [LBID]='" + ds.Tables[0].Rows[i]["LocalBodyID"].ToString() + "'";
                    //cmd = new SqlCommand(Sql, seccon);
                    //seccon.Open();
                    //string talukaId = Convert.ToString(cmd.ExecuteScalar());
                    //seccon.Close();

                    sql = "Select [SubDistrictname] from [mstTaluka] where [SubDistrictcode]='" + districtId.ToString() + "'";      //'" + talukaId + "'"; 
                    //cmd = new SqlCommand("Select [SubDistrictname] from [mstTaluka] where [DistrictCode]='" + districtId.ToString() + "'", seccon);
                    seccon.Open();
                    string talukaName = Convert.ToString(cmd.ExecuteScalar());
                    //DataSet DS = new DataSet();
                    //da = new SqlDataAdapter(sql, seccon);
                    //da.Fill(DS);
                    seccon.Close();

                    sql = "Insert into [tblRegistrationSEC]([NominationID],[FirstName],[MiddleName],[LastName],[RegMobileNo],[DistrictId],[DistrictName],[TalukaName],[Address]) " +
                         " values(N'" + ds.Tables[0].Rows[i]["NominationID"].ToString() + "',N'" + ds.Tables[0].Rows[i]["FirstName"].ToString() + "' " +
                         ",N'" + ds.Tables[0].Rows[i]["MiddleName"].ToString() + "',N'" + ds.Tables[0].Rows[i]["LastName"].ToString() + "',N'" + ds.Tables[0].Rows[i]["CandidateMob"].ToString() + "'" +
                         ",N'" + districtId + "',N'" + Districtname + "',N'" + talukaName +"'" +
                        ",N'" + ds.Tables[0].Rows[i]["Address"].ToString().Replace("'","") + "')";
                    cmd = new SqlCommand(sql, sectempcon);
                    sectempcon.Open();
                    cmd.ExecuteNonQuery();
                    sectempcon.Close();
                    
                }
                jsonDataStrngReturn += "1";
            }
            catch (Exception ex)
            {
                jsonDataStrngReturn += "0";
            }
            finally
            {
                ds.Clear();
            }
            return jsonDataStrngReturn;
        }

        public string InsertRegRecordZP(string MaxId)
        {
            SqlCommand cmd = new SqlCommand();
            DataSet ds1 = new DataSet();
            try
            {
                Sql = "select [LocalBodyID],[FirstName],[MiddleName],[LastName],[CandidateMob],[Address],[Code] FROM  [NominationZP_Reg] " +
                       "WHERE Code > '" + MaxId + "'";
                da = new SqlDataAdapter(Sql, seccon);
                da.Fill(ds);

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string sql = string.Empty;

                    Sql = "Select [LBDistrictID] from [LB_District] where [LBID]='" + ds.Tables[0].Rows[i]["LocalBodyID"].ToString() + "'";
                    cmd = new SqlCommand(Sql, seccon);
                    seccon.Open();
                    string districtId = Convert.ToString(cmd.ExecuteScalar());
                    seccon.Close();

                    cmd = new SqlCommand("Select [Districtname] from [mstDistrict] where [Districtcode]='" + districtId + "'", seccon);
                    seccon.Open();
                    string Districtname = Convert.ToString(cmd.ExecuteScalar());
                    seccon.Close();

                    Sql = "Select [LBTalukaID] from [LB_Taluka] where [LBID]='" + ds.Tables[0].Rows[i]["LocalBodyID"].ToString() + "'";
                    cmd = new SqlCommand(Sql, seccon);
                    seccon.Open();
                    string talukaId = Convert.ToString(cmd.ExecuteScalar());
                    seccon.Close();

                    Sql = "Select [SubDistrictname] from [mstTaluka] where [SubDistrictcode]='" + talukaId + "'";
                    cmd = new SqlCommand(Sql, seccon);
                    seccon.Open();
                    string talukaName = Convert.ToString(cmd.ExecuteScalar());
                    seccon.Close();

                    sql = "Insert into [tblRegistrationSEC]([Code],[FirstName],[MiddleName],[LastName],[RegMobileNo],[DistrictId],[DistrictName],[TalukaId],[TalukaName],[Address]" +
                        // ,[ElectrolId] " +",[ElectrolName],[ElectrolClgId],[ElectrolClgName],[PartyName],[SymbolName],[ElectionName]) 
                         "values(N'" + ds.Tables[0].Rows[i]["Code"].ToString() + "',N'" + ds.Tables[0].Rows[i]["FirstName"].ToString() + "' " +
                         ",N'" + ds.Tables[0].Rows[i]["MiddleName"].ToString() + "',N'" + ds.Tables[0].Rows[i]["LastName"].ToString() + "',N'" + ds.Tables[0].Rows[i]["CandidateMob"].ToString() + "'" +
                         ",N'" + districtId + "','" + Districtname + "',N'" + talukaId + "',N'" + talukaName + "' " +
                       ",N'" + ds.Tables[0].Rows[i]["Address"].ToString() + "')";
                    cmd = new SqlCommand(sql, sectempcon);
                    sectempcon.Open();
                    cmd.ExecuteNonQuery();
                    sectempcon.Close();
                    jsonDataStrngReturn += "1";
                }
            }
            catch (Exception ex)
            {
                jsonDataStrngReturn += "0";
            }
            finally
            {
                ds.Clear();
            }
            return jsonDataStrngReturn;
        }

        public string DownloadRegData(string maxid)
        {
            DataTable dt = new DataTable();
            List<Downloadreg> lstdwn = new List<Downloadreg>();
            try
            {
                Sql = "select [LocalBodyID],[Id],[FirstName],[MiddleName],[LastName],[CandidateMob],[Address],[NominationID] FROM  [NominationMP_Reg] " +
                     "WHERE NominationID ='" + maxid + "'";
                da = new SqlDataAdapter(Sql, seccon);
                da.Fill(ds);

                //if(ds.Tables[0].Rows.Count > 0)
                //{
                //    dt.Columns.Add(new DataColumn("LocalBodyID", typeof(string)));
                //    dt.Columns.Add(new DataColumn("Id", typeof(string)));
                //    dt.Columns.Add(new DataColumn("FirstName", typeof(string)));
                //    dt.Columns.Add(new DataColumn("MiddleName", typeof(string)));
                //    dt.Columns.Add(new DataColumn("LastName", typeof(string)));
                //    dt.Columns.Add(new DataColumn("CandidateMob", typeof(string)));
                //    dt.Columns.Add(new DataColumn("Address", typeof(string)));
                //    dt.Columns.Add(new DataColumn("NominationID", typeof(string)));

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    Sql = "Select [LBDistrictID] from [LB_District] where [LBID]='" + ds.Tables[0].Rows[i]["LocalBodyID"].ToString() + "'";
                    cmd = new SqlCommand(Sql, seccon);
                    seccon.Open();
                    string districtId = Convert.ToString(cmd.ExecuteScalar());
                    seccon.Close();

                    cmd = new SqlCommand("Select [Districtname] from [mstDistrict] where [Districtcode]='" + districtId.ToString() + "'", seccon);
                    seccon.Open();
                    string Districtname = Convert.ToString(cmd.ExecuteScalar());
                    seccon.Close();

                    Sql = "Select [LBTalukaID] from [LB_Taluka] where [LBID]='" + ds.Tables[0].Rows[i]["LocalBodyID"].ToString() + "'";
                    cmd = new SqlCommand(Sql, seccon);
                    seccon.Open();
                    string talukaId = Convert.ToString(cmd.ExecuteScalar());
                    seccon.Close();

                    // Sql = "Select [SubDistrictname] from [mstTaluka] where [Districtcode]='" + districtId.ToString() + "'";      //'" + talukaId + "'"; 
                    //cmd = new SqlCommand(Sql, seccon);
                    cmd = new SqlCommand("Select [SubDistrictname] from [mstTaluka] where [SubDistrictcode]='" + talukaId.ToString() + "'", seccon);
                    seccon.Open();
                    string talukaName = Convert.ToString(cmd.ExecuteScalar());
                    seccon.Close();



                    //        dt.Rows.Add(ds.Tables[0].Rows[i]["LocalBodyID"].ToString(), ds.Tables[0].Rows[i]["Id"].ToString(), ds.Tables[0].Rows[i]["FirstName"].ToString(), ds.Tables[0].Rows[i]["MiddleName"].ToString(), ds.Tables[0].Rows[i]["LastName"].ToString(), ds.Tables[0].Rows[i]["CandidateMob"].ToString(), ds.Tables[0].Rows[i]["Address"].ToString(), ds.Tables[0].Rows[i]["NominationID"].ToString());

                    Downloadreg dwn = new Downloadreg();
                    dwn.LOCALBODYID = ds.Tables[0].Rows[i]["LocalBodyID"].ToString();
                    dwn.ID = ds.Tables[0].Rows[i]["Id"].ToString();
                    dwn.FIRSTNAME = ds.Tables[0].Rows[i]["FirstName"].ToString();
                    dwn.MIDDLENAME = ds.Tables[0].Rows[i]["MiddleName"].ToString();
                    dwn.LASTNAME = ds.Tables[0].Rows[i]["LastName"].ToString();
                    dwn.CANDIDATEMOB = ds.Tables[0].Rows[i]["CandidateMob"].ToString();
                    dwn.ADDRESS = ds.Tables[0].Rows[i]["Address"].ToString();
                    dwn.NOMINATIONID = ds.Tables[0].Rows[i]["NominationID"].ToString();
                    dwn.DISTRICTID = districtId.ToString();
                    dwn.DISTRICTNAME = Districtname.ToString();
                    dwn.TALUKAID = talukaId.ToString();
                    dwn.TALUKANAME = talukaName.ToString();
                    lstdwn.Add(dwn);
                }
                //}

                jsonDataStrngReturn = JsonConvert.SerializeObject(lstdwn, theJsonSerializerSettings); ;
            }
            catch
            {
                jsonDataStrngReturn = "0";
            }
            return jsonDataStrngReturn;
        }

        //public List<Assests> HighAssetsReoprt(string districtid )//, string localbodytype)
        //{
        //    string Nomid = string.Empty; string lbid_str = string.Empty;
        //    string agecunt21to30 = string.Empty; string agecunt31to40 = string.Empty; string agecunt41to50 = string.Empty;
        //    string agecunt51to60 = string.Empty; string agecunt61to70 = string.Empty; string agecunt71to80 = string.Empty;
        //    string agecunt81to90 = string.Empty;
        //    List<Assests> lstAssts = new List<Assests>();
        //    Assests assts = new Assests();
        //    try
        //    {
        //        Sql = "select [LBID] from [SEC].[dbo].[LB_District] where [LB_District]='" + districtid + "'";
        //        da = new SqlDataAdapter(Sql, seccon);
        //        DataSet ds1 = new DataSet();
        //        da.Fill(ds1);
        //        for (int a = 0; a < ds1.Tables[0].Rows.Count; a++)
        //        {
        //            lbid_str = lbid_str + "," + Convert.ToString(ds1.Tables[0].Rows[a]["LBID"]);
        //        }
        //        lbid_str = lbid_str.Substring(1);

        //        //if (localbodytype == "3")
        //        //{
        //        Sql = "select top 10 Name,[Mov],[Immov],[TotalMovImmov],[AffID] FROM [SEC].[dbo].[Aff_AllSummery_2] where LBID in(" + lbid_str + ") order by CONVERT(money,[TotalMovImmov]) Desc";
        //        da = new SqlDataAdapter(Sql, seccon);
        //        da.Fill(ds);
        //        //}
        //        //else if (localbodytype == "5")
        //        //{
        //        //    Sql = "select Count(*) as AgeCount1To20 from [SEC].[dbo].[NominationMP_Reg] where (Age Between 1 and 20) and [Symbols_ID] Not in(0) and [Subchk]=1 and [Withdrawal_status] is null and LocalBodyId In(" + lbid_str + ")";
        //        //    cmd = new SqlCommand(Sql, seccon);
        //        //    seccon.Open();
        //        //    agecunt1to20 = Convert.ToString(cmd.ExecuteScalar());
        //        //    seccon.Close();
        //        //}
        //        //else
        //        //{
        //        //    Sql = "select Count(*) as AgeCount1To20 from [SEC].[dbo].[NominationZP_Reg] where (Age Between 1 and 20) and [Symbols_ID] Not in(0) and [Subchk]=1 and [Withdrawal_status] is null and LocalBodyId In(" + lbid_str + ")";
        //        //    cmd = new SqlCommand(Sql, seccon);
        //        //    seccon.Open();
        //        //    agecunt1to20 = Convert.ToString(cmd.ExecuteScalar());
        //        //    seccon.Close();
        //        //}
        //        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        //        {
        //            //Sql = "select [NominationID] from [SEC].[dbo].[Aff_CandidateDetails_Submit] where [AffID]=" + Convert.ToString(ds.Tables[0].Rows[i]["AffID"]) + "";
        //            //cmd = new SqlCommand(Sql, seccon);
        //            //seccon.Open();
        //            //Nomid = Convert.ToString(cmd.ExecuteScalar());
        //            //seccon.Close();

        //            //Sql = "select [NominationID] from [SEC].[dbo].[Aff_CandidateDetails_Submit] where [AffID]=" + Convert.ToString(ds.Tables[0].Rows[i]["AffID"]) + "";
        //            //cmd = new SqlCommand(Sql, seccon);
        //            //seccon.Open();
        //            //Nomid = Convert.ToString(cmd.ExecuteScalar());
        //            //seccon.Close();

        //            //Sql = "select [NominationID] from [SEC].[dbo].[Aff_CandidateDetails_Submit] where [AffID]=" + Convert.ToString(ds.Tables[0].Rows[i]["AffID"]) + "";
        //            //cmd = new SqlCommand(Sql, seccon);
        //            //seccon.Open();
        //            //Nomid = Convert.ToString(cmd.ExecuteScalar());
        //            //seccon.Close();

        //            assts = new Assests();
        //            assts.Name = Convert.ToString(ds.Tables[0].Rows[0]["Name"]);
        //            // assts.partyName = Convert.ToString(agecunt21to30);
        //            assts.movableAssts = Convert.ToString(ds.Tables[0].Rows[0]["Mov"]);
        //            assts.immovableAssts = Convert.ToString(ds.Tables[0].Rows[0]["Immov"]);
        //            assts.totalassts = Convert.ToString(ds.Tables[0].Rows[0]["TotalMovImmov"]);
        //            // assts.AGECUNT61TO70 = Convert.ToString(agecunt61to70);
        //        }
        //    }
        //    catch
        //    {

        //    }
        //    return lstAssts.ToList();
        //}

        public List<Liablity> HighliablityReoprt(string districtid)//, string localbodytype)
        {
            string agecunt1to20 = string.Empty; string lbid_str = string.Empty;
            string agecunt21to30 = string.Empty; string agecunt31to40 = string.Empty; string agecunt41to50 = string.Empty;
            string agecunt51to60 = string.Empty; string agecunt61to70 = string.Empty; string agecunt71to80 = string.Empty;
            string agecunt81to90 = string.Empty;
            List<Liablity> lstliblity = new List<Liablity>();
            Liablity liablity = new Liablity();
            try
            {
                Sql = "select [LBID] from [SEC].[dbo].[LB_District] where [LB_District]='" + districtid + "'";
                da = new SqlDataAdapter(Sql, seccon);
                DataSet ds1 = new DataSet();
                da.Fill(ds1);
                for (int a = 0; a < ds1.Tables[0].Rows.Count; a++)
                {
                    lbid_str = lbid_str + "," + Convert.ToString(ds1.Tables[0].Rows[a]["LBID"]);
                }
                lbid_str = lbid_str.Substring(1);

                //if (localbodytype == "3")
                //{
                Sql = "select top 10 Name,[Mov],[Immov],[TotalMovImmov] FROM [SEC].[dbo].[Aff_AllSummery_2] where LBID in(" + lbid_str + ") order by CONVERT(money,[TotalMovImmov]) Desc";
                da = new SqlDataAdapter(Sql, seccon);
                da.Fill(ds);
                //}
                //else if (localbodytype == "5")
                //{
                //    Sql = "select Count(*) as AgeCount1To20 from [SEC].[dbo].[NominationMP_Reg] where (Age Between 1 and 20) and [Symbols_ID] Not in(0) and [Subchk]=1 and [Withdrawal_status] is null and LocalBodyId In(" + lbid_str + ")";
                //    cmd = new SqlCommand(Sql, seccon);
                //    seccon.Open();
                //    agecunt1to20 = Convert.ToString(cmd.ExecuteScalar());
                //    seccon.Close();
                //}
                //else
                //{
                //    Sql = "select Count(*) as AgeCount1To20 from [SEC].[dbo].[NominationZP_Reg] where (Age Between 1 and 20) and [Symbols_ID] Not in(0) and [Subchk]=1 and [Withdrawal_status] is null and LocalBodyId In(" + lbid_str + ")";
                //    cmd = new SqlCommand(Sql, seccon);
                //    seccon.Open();
                //    agecunt1to20 = Convert.ToString(cmd.ExecuteScalar());
                //    seccon.Close();
                //}
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    liablity = new Liablity();
                    liablity.Name = Convert.ToString(ds.Tables[0].Rows[0]["Name"]);
                    // assts.partyName = Convert.ToString(agecunt21to30);
                    liablity.liablities = Convert.ToString(ds.Tables[0].Rows[0]["Immov"]);
                    liablity.totalassts = Convert.ToString(ds.Tables[0].Rows[0]["TotalMovImmov"]);
                    // assts.AGECUNT61TO70 = Convert.ToString(agecunt61to70);
                }
            }
            catch
            {

            }
            return lstliblity.ToList();
        }

   

    }
}
