using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Newtonsoft.Json;
using System.ServiceModel.Web;
using System.Security.Cryptography;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Web;
using System.Xml;
using System.Web.Services;


namespace LatestVoterSearch
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "WCFVoterSearchWS" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select WCFVoterSearchWS.svc or WCFVoterSearchWS.svc.cs at the Solution Explorer and start debugging.
    public class WCFVoterSearchWS : IWCFVoterSearchWS
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
        string file = string.Empty;
        string data = string.Empty;
        string Sql = string.Empty; string jsonDataStrngReturn = string.Empty;

        //  EncryptDecrypt EncryDecry = new EncryptDecrypt();

        //public List<clsVoterSearchWS> GetVoterDetailslstSearch(string Name, string LName, string MaxId, string acno)
        //{
        //    try
        //    {
        //        //cmd.CommandText = "SELECT TOP 30 [CCODE] AS Sr,[PART_NO] AS WardNo ,[IDCARD_NO] AS IDCARD_NO,[fm_name_v1] AS fm_name_v1,[Lastname_v1] AS Lastname_v1," +
        //        //                              "[RLN_FM_NM_v1],[RLN_L_NM_v1],[FM_NAMEEN],[LASTNAMEEN],[RLN_FM_NMEN],[SEX],[AGE],[AC_NO],[PART_NO],[SLNOINPART],[PART_NO] AS BoothNumber," +
        //        //                              "[PART_NO] AS boothname,[PART_NO] AS BoothAddress FROM [DBVoterSearch].[dbo].[AC174part001] WHERE [FM_NAMEEN] LIKE '" + Name.Trim() + "%' AND [LASTNAMEEN] LIKE '" + LName.Trim() + "%'  AND [CCODE] > '" + MaxId.Trim() + "' ";
        //        cmd.CommandText = "[VoterSearchByName]";
        //        // cmd.CommandType = CommandType.Text;
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.Add("@TableName", SqlDbType.NVarChar).Value = "Ac_" + acno + "_".Trim();                                   //"AC174part001".Trim(); 
        //        cmd.Parameters.Add("@SearchFName", SqlDbType.NVarChar).Value = Name.Trim();
        //        cmd.Parameters.Add("@SearchLName", SqlDbType.NVarChar).Value = LName.Trim();
        //        cmd.Parameters.Add("@ServerMaxId", SqlDbType.Int).Value = MaxId.Trim();
        //        cmd.Connection = con;
        //        da.SelectCommand = cmd;
        //        da.Fill(ds);
        //        if (ds.Tables[0].Rows.Count > 0)
        //        {
        //            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        //            {
        //                objcls = new clsVoterSearchWS();
        //                objcls.Sr = ds.Tables[0].Rows[i]["Sr"].ToString();
        //                objcls.WardNo = ds.Tables[0].Rows[i]["WardNo"].ToString();
        //                objcls.IDCARD_NO = ds.Tables[0].Rows[i]["IDCARD_NO"].ToString();
        //                objcls.fm_name_v1 = ds.Tables[0].Rows[i]["fm_name_v1"].ToString();
        //                objcls.Lastname_v1 = ds.Tables[0].Rows[i]["Lastname_v1"].ToString();
        //                objcls.RLN_FM_NM_v1 = ds.Tables[0].Rows[i]["RLN_FM_NM_v1"].ToString();
        //                objcls.RLN_L_NM_v1 = ds.Tables[0].Rows[i]["RLN_L_NM_v1"].ToString();
        //                objcls.FM_NAMEEN = ds.Tables[0].Rows[i]["FM_NAMEEN"].ToString();
        //                objcls.LASTNAMEEN = ds.Tables[0].Rows[i]["LASTNAMEEN"].ToString();
        //                objcls.RLN_FM_NMEN = ds.Tables[0].Rows[i]["RLN_FM_NMEN"].ToString();
        //                objcls.SEX = ds.Tables[0].Rows[i]["SEX"].ToString();
        //                objcls.AGE = ds.Tables[0].Rows[i]["AGE"].ToString();
        //                objcls.AC_NO = ds.Tables[0].Rows[i]["AC_NO"].ToString();
        //                objcls.PART_NO = ds.Tables[0].Rows[i]["PART_NO"].ToString();
        //                objcls.SLNOINPART = ds.Tables[0].Rows[i]["SLNOINPART"].ToString();
        //                objcls.BoothNumber = ds.Tables[0].Rows[i]["BoothNumber"].ToString();
        //                objcls.boothname = ds.Tables[0].Rows[i]["boothname"].ToString();
        //                objcls.BoothAddress = ds.Tables[0].Rows[i]["BoothAddress"].ToString();
        //                cls.Add(objcls);

        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //  Return any exception messages back to the Response header
        //        OutgoingWebResponseContext response = WebOperationContext.Current.OutgoingResponse;
        //        response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
        //        response.StatusDescription = ex.Message.Replace("\r\n", "");
        //        return null;

        //        // jsonDataStrngReturn = "Error";
        //    }
        //    finally
        //    {
        //        ds.Clear();
        //    }
        //    return cls.ToList();
        //}

        /// <summary>
        /// This method is use for Name,Lanme,Maxid for table and acno wise search record...
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="LName"></param>
        /// <param name="MaxId"></param>
        /// <param name="acno"></param>
        /// <returns></returns>
        public List<clsVoterSearchWS> GetVoterDetailsSearch(string Name, string LName, string MaxId, string acno)
        {
            try
            {
                //cmd.CommandText = "SELECT TOP 30 [CCODE] AS Sr,[PART_NO] AS WardNo ,[IDCARD_NO] AS IDCARD_NO,[fm_name_v1] AS fm_name_v1,[Lastname_v1] AS Lastname_v1," +
                //                  "[RLN_FM_NM_v1],[RLN_L_NM_v1],[FM_NAMEEN],[LASTNAMEEN],[RLN_FM_NMEN],[SEX],[AGE],[AC_NO],[PART_NO],[SLNOINPART],[PART_NO] AS BoothNumber," +
                //                  "[PART_NO] AS boothname,[PART_NO] AS BoothAddress FROM [DBVoterSearch].[dbo].[AC174part001] WHERE [FM_NAMEEN] LIKE '" + Name.Trim() + "%' AND [LASTNAMEEN] LIKE '" + LName.Trim() + "%'  AND [CCODE] > '" + MaxId.Trim() + "' ";
                cmd.CommandText = "VoterSearchByName";
                // cmd.CommandType = CommandType.Text;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@TableName", SqlDbType.NVarChar).Value = "Ac_" + acno + "_";
                cmd.Parameters.Add("@SearchFName", SqlDbType.NVarChar).Value = Name.Trim();
                cmd.Parameters.Add("@SearchLName", SqlDbType.NVarChar).Value = LName.Trim();
                cmd.Parameters.Add("@ServerMaxId", SqlDbType.Int).Value = MaxId.Trim();
                cmd.Connection = con;
                da.SelectCommand = cmd;
                da.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        objcls = new clsVoterSearchWS();
                        objcls.Id = ds.Tables[0].Rows[i]["Sr"] != null ? ds.Tables[0].Rows[i]["Sr"].ToString() : "NA";
                        objcls.Sr = ds.Tables[0].Rows[i]["Sr"] != null ? ds.Tables[0].Rows[i]["Sr"].ToString() : "NA";
                        objcls.WardNo = ds.Tables[0].Rows[i]["WardNo"] != null ? ds.Tables[0].Rows[i]["WardNo"].ToString() : "NA";
                        objcls.IDCARD_NO = ds.Tables[0].Rows[i]["IDCARD_NO"] != null ? ds.Tables[0].Rows[i]["IDCARD_NO"].ToString() : "NA";
                        objcls.fm_name_v1 = ds.Tables[0].Rows[i]["fm_name_v1"] != null ? ds.Tables[0].Rows[i]["fm_name_v1"].ToString() : "NA";
                        objcls.Lastname_v1 = ds.Tables[0].Rows[i]["Lastname_v1"] != null ? ds.Tables[0].Rows[i]["Lastname_v1"].ToString() : "NA";
                        objcls.RLN_FM_NM_v1 = ds.Tables[0].Rows[i]["RLN_FM_NM_v1"] != null ? ds.Tables[0].Rows[i]["RLN_FM_NM_v1"].ToString() : "NA";
                        objcls.RLN_L_NM_v1 = ds.Tables[0].Rows[i]["RLN_L_NM_v1"] != null ? ds.Tables[0].Rows[i]["RLN_L_NM_v1"].ToString() : "NA";
                        objcls.FM_NAMEEN = ds.Tables[0].Rows[i]["FM_NAMEEN"] != null ? ds.Tables[0].Rows[i]["FM_NAMEEN"].ToString() : "NA";
                        objcls.LASTNAMEEN = ds.Tables[0].Rows[i]["LASTNAMEEN"] != null ? ds.Tables[0].Rows[i]["LASTNAMEEN"].ToString() : "NA";
                        objcls.RLN_FM_NMEN = ds.Tables[0].Rows[i]["RLN_FM_NMEN"] != null ? ds.Tables[0].Rows[i]["RLN_FM_NMEN"].ToString() : "NA";
                        objcls.SEX = ds.Tables[0].Rows[i]["SEX"] != null ? ds.Tables[0].Rows[i]["SEX"].ToString() : "NA";
                        objcls.AGE = ds.Tables[0].Rows[i]["AGE"] != null ? ds.Tables[0].Rows[i]["AGE"].ToString() : "NA";
                        objcls.AC_NO = ds.Tables[0].Rows[i]["AC_NO"] != null ? ds.Tables[0].Rows[i]["AC_NO"].ToString() : "NA";
                        objcls.PART_NO = ds.Tables[0].Rows[i]["PART_NO"] != null ? ds.Tables[0].Rows[i]["PART_NO"].ToString() : "NA";
                        objcls.SLNOINPART = ds.Tables[0].Rows[i]["SLNOINPART"] != null ? ds.Tables[0].Rows[i]["SLNOINPART"].ToString() : "NA";
                        objcls.BoothNumber = ds.Tables[0].Rows[i]["BoothNumber"] != null ? Convert.ToString(ds.Tables[0].Rows[i]["BoothNumber"]) : "NA";
                        objcls.boothname = ds.Tables[0].Rows[i]["boothname"] != null ? ds.Tables[0].Rows[i]["boothname"].ToString() : "NA";
                        objcls.BoothAddress = ds.Tables[0].Rows[i]["BoothAddress"] != null ? ds.Tables[0].Rows[i]["BoothAddress"].ToString() : "NA";
                        objcls.house_no = ds.Tables[0].Rows[i]["HouseNo"] != null ? ds.Tables[0].Rows[i]["HouseNo"].ToString() : "NA";
                        objcls.Section_No = ds.Tables[0].Rows[i]["SectionNo"] != null ? ds.Tables[0].Rows[i]["SectionNo"].ToString() : "NA";
                        objcls.section_name_v1 = ds.Tables[0].Rows[i]["section_name_v1"] != null ? ds.Tables[0].Rows[i]["section_name_v1"].ToString() : "NA";
                        objcls.section_name_en = ds.Tables[0].Rows[i]["section_name_en"] != null ? ds.Tables[0].Rows[i]["section_name_en"].ToString() : "NA";
                        if (acno == "071" || acno == "096" || acno == "235" || acno == "114" || acno == "115" || acno == "136" || acno == "137" || acno == "188")
                        {
                            objcls.SerialNoInBooth = ds.Tables[0].Rows[i]["SerialNo_Booth"] != null ? ds.Tables[0].Rows[i]["SerialNo_Booth"].ToString() : "NA";
                        }
                        else
                        {
                            objcls.SerialNoInBooth = "NA";
                        }
                        cls.Add(objcls);

                        // jsonDataStrngReturn = JsonConvert.SerializeObject(cls, theJsonSerializerSettings);

                    }
                }
                else
                {
                    jsonDataStrngReturn = "NoData Found";
                }
                //returnStr = myStringEncryptor.Encrypt(jsonDataStrngReturn);

            }
            catch (Exception ex)
            {
                //  Return any exception messages back to the Response header
                OutgoingWebResponseContext response = WebOperationContext.Current.OutgoingResponse;
                response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                response.StatusDescription = ex.Message.Replace("\r\n", "");
                //return null;

                jsonDataStrngReturn = "Error";
            }
            finally
            {
                ds.Clear();
            }
            return cls.ToList();//jsonDataStrngReturn;
        }

        public List<clsVoterSearchWS> GetEpikNoWiseDetails(string assemblyid, string maxserverid, string epiknoId)
        {
            try
            {
                //cmd.CommandText = "SELECT TOP 30 [CCODE] AS Sr,[PART_NO] AS WardNo ,[IDCARD_NO] AS IDCARD_NO,[fm_name_v1] AS fm_name_v1,[Lastname_v1] AS Lastname_v1," +
                //                  "[RLN_FM_NM_v1],[RLN_L_NM_v1],[FM_NAMEEN],[LASTNAMEEN],[RLN_FM_NMEN],[SEX],[AGE],[AC_NO],[PART_NO],[SLNOINPART],[PART_NO] AS BoothNumber," +
                //                  "[PART_NO] AS boothname,[PART_NO] AS BoothAddress FROM [DBVoterSearch].[dbo].[AC174part001] WHERE [FM_NAMEEN] LIKE '" + Name.Trim() + "%' AND [LASTNAMEEN] LIKE '" + LName.Trim() + "%'  AND [CCODE] > '" + MaxId.Trim() + "' ";
                cmd.CommandText = "VoterSearchByName_EpikIDWise";
                // cmd.CommandType = CommandType.Text;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@TableName", SqlDbType.NVarChar).Value = "Ac_" + assemblyid + "_";
                cmd.Parameters.Add("@EPIKNOID", SqlDbType.NVarChar).Value = epiknoId.Trim();
                cmd.Parameters.Add("@ServerMaxId", SqlDbType.Int).Value = maxserverid.Trim();
                cmd.Connection = con;
                da.SelectCommand = cmd;
                da.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        objcls = new clsVoterSearchWS();
                        objcls.Id = ds.Tables[0].Rows[i]["Sr"] != null ? ds.Tables[0].Rows[i]["Sr"].ToString() : "NA";
                        objcls.Sr = ds.Tables[0].Rows[i]["Sr"] != null ? ds.Tables[0].Rows[i]["Sr"].ToString() : "NA";
                        objcls.WardNo = ds.Tables[0].Rows[i]["WardNo"] != null ? ds.Tables[0].Rows[i]["WardNo"].ToString() : "NA";
                        objcls.IDCARD_NO = ds.Tables[0].Rows[i]["IDCARD_NO"] != null ? ds.Tables[0].Rows[i]["IDCARD_NO"].ToString() : "NA";
                        objcls.fm_name_v1 = ds.Tables[0].Rows[i]["fm_name_v1"] != null ? ds.Tables[0].Rows[i]["fm_name_v1"].ToString() : "NA";
                        objcls.Lastname_v1 = ds.Tables[0].Rows[i]["Lastname_v1"] != null ? ds.Tables[0].Rows[i]["Lastname_v1"].ToString() : "NA";
                        objcls.RLN_FM_NM_v1 = ds.Tables[0].Rows[i]["RLN_FM_NM_v1"] != null ? ds.Tables[0].Rows[i]["RLN_FM_NM_v1"].ToString() : "NA";
                        objcls.RLN_L_NM_v1 = ds.Tables[0].Rows[i]["RLN_L_NM_v1"] != null ? ds.Tables[0].Rows[i]["RLN_L_NM_v1"].ToString() : "NA";
                        objcls.FM_NAMEEN = ds.Tables[0].Rows[i]["FM_NAMEEN"] != null ? ds.Tables[0].Rows[i]["FM_NAMEEN"].ToString() : "NA";
                        objcls.LASTNAMEEN = ds.Tables[0].Rows[i]["LASTNAMEEN"] != null ? ds.Tables[0].Rows[i]["LASTNAMEEN"].ToString() : "NA";
                        objcls.RLN_FM_NMEN = ds.Tables[0].Rows[i]["RLN_FM_NMEN"] != null ? ds.Tables[0].Rows[i]["RLN_FM_NMEN"].ToString() : "NA";
                        objcls.SEX = ds.Tables[0].Rows[i]["SEX"] != null ? ds.Tables[0].Rows[i]["SEX"].ToString() : "NA";
                        objcls.AGE = ds.Tables[0].Rows[i]["AGE"] != null ? ds.Tables[0].Rows[i]["AGE"].ToString() : "NA";
                        objcls.AC_NO = ds.Tables[0].Rows[i]["AC_NO"] != null ? ds.Tables[0].Rows[i]["AC_NO"].ToString() : "NA";
                        objcls.PART_NO = ds.Tables[0].Rows[i]["PART_NO"] != null ? ds.Tables[0].Rows[i]["PART_NO"].ToString() : "NA";
                        objcls.SLNOINPART = ds.Tables[0].Rows[i]["SLNOINPART"] != null ? ds.Tables[0].Rows[i]["SLNOINPART"].ToString() : "NA";
                        objcls.BoothNumber = ds.Tables[0].Rows[i]["BoothNumber"] != null ? Convert.ToString(ds.Tables[0].Rows[i]["BoothNumber"]) : "NA";
                        objcls.boothname = ds.Tables[0].Rows[i]["boothname"] != null ? ds.Tables[0].Rows[i]["boothname"].ToString() : "NA";
                        objcls.BoothAddress = ds.Tables[0].Rows[i]["BoothAddress"] != null ? ds.Tables[0].Rows[i]["BoothAddress"].ToString() : "NA";
                        objcls.house_no = ds.Tables[0].Rows[i]["HouseNo"] != null ? ds.Tables[0].Rows[i]["HouseNo"].ToString() : "NA";
                        objcls.Section_No = ds.Tables[0].Rows[i]["SectionNo"] != null ? ds.Tables[0].Rows[i]["SectionNo"].ToString() : "NA";
                        objcls.section_name_v1 = ds.Tables[0].Rows[i]["section_name_v1"] != null ? ds.Tables[0].Rows[i]["section_name_v1"].ToString() : "NA";
                        objcls.section_name_en = ds.Tables[0].Rows[i]["section_name_en"] != null ? ds.Tables[0].Rows[i]["section_name_en"].ToString() : "NA";
                        if (assemblyid == "071" || assemblyid == "096" || assemblyid == "235" || assemblyid == "114" || assemblyid == "115" || assemblyid == "136" || assemblyid == "137" || assemblyid == "188")
                        {
                            objcls.SerialNoInBooth = ds.Tables[0].Rows[i]["SerialNo_Booth"] != null ? ds.Tables[0].Rows[i]["SerialNo_Booth"].ToString() : "NA";
                        }
                        else
                        {
                            objcls.SerialNoInBooth = "NA";
                        }
                        cls.Add(objcls);

                        // jsonDataStrngReturn = JsonConvert.SerializeObject(cls, theJsonSerializerSettings);
                    }
                }
                else
                {
                    jsonDataStrngReturn = "NoData Found";
                }
                //returnStr = myStringEncryptor.Encrypt(jsonDataStrngReturn);

            }
            catch (Exception ex)
            {
                //  Return any exception messages back to the Response header
                OutgoingWebResponseContext response = WebOperationContext.Current.OutgoingResponse;
                response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                response.StatusDescription = ex.Message.Replace("\r\n", "");
                //return null;

                jsonDataStrngReturn = "Error";
            }
            finally
            {
                ds.Clear();
            }
            return cls.ToList();//jsonDataStrngReturn;
        }

        //public List<clsVoterSearchWS> GetVoterDetailsSearchVoyerNoWise(string Name, string LName, string MaxId, string acno,string voterNo)
        //{
        //    try
        //    {
        //        cmd.CommandText = "VoterSearchByName";
        //        // cmd.CommandType = CommandType.Text;
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.Add("@TableName", SqlDbType.NVarChar).Value = "Ac_" + acno + "_";
        //        cmd.Parameters.Add("@SearchFName", SqlDbType.NVarChar).Value = Name.Trim();
        //        cmd.Parameters.Add("@SearchLName", SqlDbType.NVarChar).Value = LName.Trim();
        //        cmd.Parameters.Add("@ServerMaxId", SqlDbType.Int).Value = MaxId.Trim();
        //        cmd.Connection = con;
        //        da.SelectCommand = cmd;
        //        da.Fill(ds);
        //        if (ds.Tables[0].Rows.Count > 0)
        //        {
        //            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        //            {
        //                objcls = new clsVoterSearchWS();
        //                objcls.Id = ds.Tables[0].Rows[i]["Sr"] != null ? ds.Tables[0].Rows[i]["Sr"].ToString() : "NA";
        //                objcls.Sr = ds.Tables[0].Rows[i]["Sr"] != null ? ds.Tables[0].Rows[i]["Sr"].ToString() : "NA";
        //                objcls.WardNo = ds.Tables[0].Rows[i]["WardNo"] != null ? ds.Tables[0].Rows[i]["WardNo"].ToString() : "NA";
        //                objcls.IDCARD_NO = ds.Tables[0].Rows[i]["IDCARD_NO"] != null ? ds.Tables[0].Rows[i]["IDCARD_NO"].ToString() : "NA";
        //                objcls.fm_name_v1 = ds.Tables[0].Rows[i]["fm_name_v1"] != null ? ds.Tables[0].Rows[i]["fm_name_v1"].ToString() : "NA";
        //                objcls.Lastname_v1 = ds.Tables[0].Rows[i]["Lastname_v1"] != null ? ds.Tables[0].Rows[i]["Lastname_v1"].ToString() : "NA";
        //                objcls.RLN_FM_NM_v1 = ds.Tables[0].Rows[i]["RLN_FM_NM_v1"] != null ? ds.Tables[0].Rows[i]["RLN_FM_NM_v1"].ToString() : "NA";
        //                objcls.RLN_L_NM_v1 = ds.Tables[0].Rows[i]["RLN_L_NM_v1"] != null ? ds.Tables[0].Rows[i]["RLN_L_NM_v1"].ToString() : "NA";
        //                objcls.FM_NAMEEN = ds.Tables[0].Rows[i]["FM_NAMEEN"] != null ? ds.Tables[0].Rows[i]["FM_NAMEEN"].ToString() : "NA";
        //                objcls.LASTNAMEEN = ds.Tables[0].Rows[i]["LASTNAMEEN"] != null ? ds.Tables[0].Rows[i]["LASTNAMEEN"].ToString() : "NA";
        //                objcls.RLN_FM_NMEN = ds.Tables[0].Rows[i]["RLN_FM_NMEN"] != null ? ds.Tables[0].Rows[i]["RLN_FM_NMEN"].ToString() : "NA";
        //                objcls.SEX = ds.Tables[0].Rows[i]["SEX"] != null ? ds.Tables[0].Rows[i]["SEX"].ToString() : "NA";
        //                objcls.AGE = ds.Tables[0].Rows[i]["AGE"] != null ? ds.Tables[0].Rows[i]["AGE"].ToString() : "NA";
        //                objcls.AC_NO = ds.Tables[0].Rows[i]["AC_NO"] != null ? ds.Tables[0].Rows[i]["AC_NO"].ToString() : "NA";
        //                objcls.PART_NO = ds.Tables[0].Rows[i]["PART_NO"] != null ? ds.Tables[0].Rows[i]["PART_NO"].ToString() : "NA";
        //                objcls.SLNOINPART = ds.Tables[0].Rows[i]["SLNOINPART"] != null ? ds.Tables[0].Rows[i]["SLNOINPART"].ToString() : "NA";
        //                objcls.BoothNumber = ds.Tables[0].Rows[i]["BoothNumber"] != null ? Convert.ToString(ds.Tables[0].Rows[i]["BoothNumber"]) : "NA";
        //                objcls.boothname = ds.Tables[0].Rows[i]["boothname"] != null ? ds.Tables[0].Rows[i]["boothname"].ToString() : "NA";
        //                objcls.BoothAddress = ds.Tables[0].Rows[i]["BoothAddress"] != null ? ds.Tables[0].Rows[i]["BoothAddress"].ToString() : "NA";
        //                cls.Add(objcls);

        //                // jsonDataStrngReturn = JsonConvert.SerializeObject(cls, theJsonSerializerSettings);

        //            }
        //        }
        //        else
        //        {
        //            jsonDataStrngReturn = "NoData Found";
        //        }
        //        //returnStr = myStringEncryptor.Encrypt(jsonDataStrngReturn);

        //    }
        //    catch (Exception ex)
        //    {
        //        //  Return any exception messages back to the Response header
        //        OutgoingWebResponseContext response = WebOperationContext.Current.OutgoingResponse;
        //        response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
        //        response.StatusDescription = ex.Message.Replace("\r\n", "");
        //        //return null;

        //        jsonDataStrngReturn = "Error";
        //    }
        //    finally
        //    {
        //        ds.Clear();
        //    }
        //    return cls.ToList();//jsonDataStrngReturn;
        //}


        ///// <summary>
        ///// This method use for PartNo,WardNo,maxid of table and Acno Wise voter Search Recod..
        ///// </summary>
        ///// <param name="partNo"></param>
        ///// <param name="WarNo"></param>
        ///// <param name="MaxId"></param>
        ///// <param name="acno"></param>
        ///// <returns></returns>
        //public string GetVoterDetailsSearchSecondmthd(string partNo, string WarNo, string MaxId, string acno)
        //{
        //    try
        //    {
        //        //cmd.CommandText = "SELECT TOP 30 [CCODE] AS Sr,[PART_NO] AS WardNo ,[IDCARD_NO] AS IDCARD_NO,[fm_name_v1] AS fm_name_v1,[Lastname_v1] AS Lastname_v1," +
        //        //                              "[RLN_FM_NM_v1],[RLN_L_NM_v1],[FM_NAMEEN],[LASTNAMEEN],[RLN_FM_NMEN],[SEX],[AGE],[AC_NO],[PART_NO],[SLNOINPART],[PART_NO] AS BoothNumber," +
        //        //                              "[PART_NO] AS boothname,[PART_NO] AS BoothAddress FROM [DBVoterSearch].[dbo].[AC174part001] WHERE [FM_NAMEEN] LIKE '" + Name.Trim() + "%' AND [LASTNAMEEN] LIKE '" + LName.Trim() + "%'  AND [CCODE] > '" + MaxId.Trim() + "' ";
        //        cmd.CommandText = "";
        //        //cmd.CommandType = CommandType.Text;
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.Add("@TableName", SqlDbType.NVarChar).Value = "Ac_" + acno + "_".Trim();                                   //"AC174part001".Trim(); 
        //        cmd.Parameters.Add("@SearchPartWise", SqlDbType.NVarChar).Value = partNo.Trim();
        //        cmd.Parameters.Add("@SearchwardNoWise", SqlDbType.NVarChar).Value = WarNo.Trim();
        //        cmd.Parameters.Add("@ServerMaxId", SqlDbType.Int).Value = MaxId.Trim();
        //        cmd.Connection = con;
        //        da.SelectCommand = cmd;
        //        da.Fill(ds);
        //        if (ds.Tables[0].Rows.Count > 0)
        //        {
        //            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        //            {
        //                objcls = new clsVoterSearchWS();
        //                objcls.Id = ds.Tables[0].Rows[i]["Sr"].ToString();   
        //                objcls.Sr = ds.Tables[0].Rows[i]["Sr"].ToString();
        //                objcls.WardNo = ds.Tables[0].Rows[i]["WardNo"].ToString();
        //                objcls.IDCARD_NO = ds.Tables[0].Rows[i]["IDCARD_NO"].ToString();
        //                objcls.fm_name_v1 = ds.Tables[0].Rows[i]["fm_name_v1"].ToString();
        //                objcls.Lastname_v1 = ds.Tables[0].Rows[i]["Lastname_v1"].ToString();
        //                objcls.RLN_FM_NM_v1 = ds.Tables[0].Rows[i]["RLN_FM_NM_v1"].ToString();
        //                objcls.RLN_L_NM_v1 = ds.Tables[0].Rows[i]["RLN_L_NM_v1"].ToString();
        //                objcls.FM_NAMEEN = ds.Tables[0].Rows[i]["FM_NAMEEN"].ToString();
        //                objcls.LASTNAMEEN = ds.Tables[0].Rows[i]["LASTNAMEEN"].ToString();
        //                objcls.RLN_FM_NMEN = ds.Tables[0].Rows[i]["RLN_FM_NMEN"].ToString();
        //                objcls.SEX = ds.Tables[0].Rows[i]["SEX"].ToString();
        //                objcls.AGE = ds.Tables[0].Rows[i]["AGE"].ToString();
        //                objcls.AC_NO = ds.Tables[0].Rows[i]["AC_NO"].ToString();
        //                objcls.PART_NO = ds.Tables[0].Rows[i]["PART_NO"].ToString();
        //                objcls.SLNOINPART = ds.Tables[0].Rows[i]["SLNOINPART"].ToString();
        //                objcls.BoothNumber = ds.Tables[0].Rows[i]["BoothNumber"].ToString();
        //                objcls.boothname = ds.Tables[0].Rows[i]["boothname"].ToString();
        //                objcls.BoothAddress = ds.Tables[0].Rows[i]["BoothAddress"].ToString();
        //                cls.Add(objcls);

        //                jsonDataStrngReturn = JsonConvert.SerializeObject(cls);
        //            }
        //        }
        //        else
        //        {
        //            jsonDataStrngReturn = "NoData Found";
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        //  Return any exception messages back to the Response header
        //        OutgoingWebResponseContext response = WebOperationContext.Current.OutgoingResponse;
        //        response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
        //        response.StatusDescription = ex.Message.Replace("\r\n", "");
        //        //return null;

        //        jsonDataStrngReturn = "Error";
        //    }
        //    finally
        //    {
        //        ds.Clear();
        //    }
        //    return jsonDataStrngReturn;
        //}

        /// <summary>
        /// This method use for Acno,partno,wardno,divisionNo,electrolclgid,max id And Acno wise voter search record...
        /// </summary>
        /// <param name="Acno"></param>
        /// <param name="partNo"></param>
        /// <param name="WarNo"></param>
        /// <param name="divisionNo"></param>
        /// <param name="electrolclgid"></param>
        /// <param name="MaxId"></param>
        /// <param name="acno"></param>
        /// <returns></returns>
        public List<clsVoterSearchWS> GetWardWiseDetails(string assemblyid, string wardno, string maxserverid, string localBodyId, string localBodyType)
        {
            try
            {
                if (localBodyType.Equals("1"))
                {
                    cmd.CommandText = "VoterSearchByWard";
                }
                else if (localBodyType.Equals("4"))
                {
                    cmd.CommandText = "VoterSearchByDivision";
                }
                else if (localBodyType.Equals("5"))
                {
                    cmd.CommandText = "VoterSearchByElectoralCollege";
                }
                //using (StreamReader reader = new StreamReader(wardwise))
                //{
                //    data = reader.ReadToEnd();
                //}
                //data = data.Replace("\"", "'");

                //DataContractJsonSerializer obj = new DataContractJsonSerializer(typeof(WardWiseSearch));

                //List<WardWiseSearch> wardwiseobj = JsonConvert.DeserializeObject<List<WardWiseSearch>>(data);

                //if (con.State == ConnectionState.Closed)
                //{
                //    con.Open();
                //}

                //foreach (WardWiseSearch wardws in wardwiseobj)
                //{
                //Sql = "select * from Ac_999_ where [AC_NO]='" + wardws.assemblyId.ToString() + "' and [PART_NO]='"+ wardws.wardNo +"'";
                //da = new SqlDataAdapter(Sql, con);
                // cmd.CommandText = "VoterSearchByWard";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@TableName", SqlDbType.NVarChar).Value = "Ac_" + assemblyid + "_";
                //// cmd.Parameters.Add("@SearchAcnoWise", SqlDbType.NVarChar).Value = wardws.assemblyId.Trim();
                //// cmd.Parameters.Add("@SearchpartNoWise", SqlDbType.NVarChar).Value = wardws.divId.Trim();
                // //cmd.Parameters.Add("@Searchdistid", SqlDbType.NVarChar).Value = wardws.distId.Trim();
                // cmd.Parameters.Add("@Searchlocalbdyid", SqlDbType.NVarChar).Value = localBodyId.Trim();
                //// cmd.Parameters.Add("@Searchlocalbdytype", SqlDbType.NVarChar).Value = wardws.localBodyType.Trim();
                cmd.Parameters.Add("@AllocatedWard", SqlDbType.NVarChar).Value = wardno.Trim();
                // cmd.Parameters.Add("@SearchdivisionNoWise", SqlDbType.NVarChar).Value = divId.Trim();
                // cmd.Parameters.Add("@SearchelectrolclgidWise", SqlDbType.NVarChar).Value = eCollId.Trim();
                cmd.Parameters.Add("@ServerMaxId", SqlDbType.Int).Value = maxserverid.Trim();
                cmd.Connection = con;
                da.SelectCommand = cmd;
                da.Fill(ds);
                // }
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        objcls = new clsVoterSearchWS();
                        objcls.Id = ds.Tables[0].Rows[i]["ID"] != null ? ds.Tables[0].Rows[i]["ID"].ToString() : "NA";
                        objcls.Sr = ds.Tables[0].Rows[i]["Sr"] != null ? ds.Tables[0].Rows[i]["Sr"].ToString() : "NA";
                        objcls.WardNo = ds.Tables[0].Rows[i]["WardNo"] != null ? ds.Tables[0].Rows[i]["WardNo"].ToString() : "NA";
                        objcls.IDCARD_NO = ds.Tables[0].Rows[i]["IDCARD_NO"] != null ? ds.Tables[0].Rows[i]["IDCARD_NO"].ToString() : "NA";
                        objcls.fm_name_v1 = ds.Tables[0].Rows[i]["fm_name_v1"] != null ? ds.Tables[0].Rows[i]["fm_name_v1"].ToString() : "NA";
                        objcls.Lastname_v1 = ds.Tables[0].Rows[i]["Lastname_v1"] != null ? ds.Tables[0].Rows[i]["Lastname_v1"].ToString() : "NA";
                        objcls.RLN_FM_NM_v1 = ds.Tables[0].Rows[i]["RLN_FM_NM_v1"] != null ? ds.Tables[0].Rows[i]["RLN_FM_NM_v1"].ToString() : "NA";
                        objcls.RLN_L_NM_v1 = ds.Tables[0].Rows[i]["RLN_L_NM_v1"] != null ? ds.Tables[0].Rows[i]["RLN_L_NM_v1"].ToString() : "NA";
                        objcls.FM_NAMEEN = ds.Tables[0].Rows[i]["FM_NAMEEN"] != null ? ds.Tables[0].Rows[i]["FM_NAMEEN"].ToString() : "NA";
                        objcls.LASTNAMEEN = ds.Tables[0].Rows[i]["LASTNAMEEN"] != null ? ds.Tables[0].Rows[i]["LASTNAMEEN"].ToString() : "NA";
                        objcls.RLN_FM_NMEN = ds.Tables[0].Rows[i]["RLN_FM_NMEN"] != null ? ds.Tables[0].Rows[i]["RLN_FM_NMEN"].ToString() : "NA";
                        objcls.SEX = ds.Tables[0].Rows[i]["SEX"] != null ? ds.Tables[0].Rows[i]["SEX"].ToString() : "NA";
                        objcls.AGE = ds.Tables[0].Rows[i]["AGE"] != null ? ds.Tables[0].Rows[i]["AGE"].ToString() : "NA";
                        objcls.AC_NO = ds.Tables[0].Rows[i]["AC_NO"] != null ? ds.Tables[0].Rows[i]["AC_NO"].ToString() : "NA";
                        objcls.PART_NO = ds.Tables[0].Rows[i]["PART_NO"] != null ? ds.Tables[0].Rows[i]["PART_NO"].ToString() : "NA";
                        objcls.SLNOINPART = ds.Tables[0].Rows[i]["SLNOINPART"] != null ? ds.Tables[0].Rows[i]["SLNOINPART"].ToString() : "NA";
                        objcls.BoothNumber = ds.Tables[0].Rows[i]["BoothNumber"] != null ? ds.Tables[0].Rows[i]["BoothNumber"].ToString() : "NA";
                        objcls.boothname = ds.Tables[0].Rows[i]["boothname"] != null ? ds.Tables[0].Rows[i]["boothname"].ToString() : "NA";
                        objcls.BoothAddress = ds.Tables[0].Rows[i]["BoothAddress"] != null ? ds.Tables[0].Rows[i]["BoothAddress"].ToString() : "NA";
                        objcls.house_no = ds.Tables[0].Rows[i]["HouseNo"] != null ? ds.Tables[0].Rows[i]["HouseNo"].ToString() : "NA";
                        objcls.Section_No = ds.Tables[0].Rows[i]["SectionNo"] != null ? ds.Tables[0].Rows[i]["SectionNo"].ToString() : "NA";
                        objcls.section_name_v1 = ds.Tables[0].Rows[i]["section_name_v1"] != null ? ds.Tables[0].Rows[i]["section_name_v1"].ToString() : "NA";
                        objcls.section_name_en = ds.Tables[0].Rows[i]["section_name_en"] != null ? ds.Tables[0].Rows[i]["section_name_en"].ToString() : "NA";
                        if (assemblyid == "071" || assemblyid == "096" || assemblyid == "235" || assemblyid == "114" || assemblyid == "115" || assemblyid == "136" || assemblyid == "137" || assemblyid == "188")
                        {
                            objcls.SerialNoInBooth = ds.Tables[0].Rows[i]["SerialNo_Booth"] != null ? ds.Tables[0].Rows[i]["SerialNo_Booth"].ToString() : "NA";
                        }
                        else
                        {
                            objcls.SerialNoInBooth = "NA";
                        }
                        objcls.dob = ds.Tables[0].Rows[i]["dob"] != null ? ds.Tables[0].Rows[i]["dob"].ToString() : "NA";
                        objcls.aadharno = ds.Tables[0].Rows[i]["AadhaarNo"] != null ? ds.Tables[0].Rows[i]["AadhaarNo"].ToString() : "NA";
                        cls.Add(objcls);

                        // jsonDataStrngReturn = JsonConvert.SerializeObject(cls);
                    }
                }
                else
                {
                    jsonDataStrngReturn = "NoData Found";
                }

            }
            catch (Exception ex)
            {
                //  Return any exception messages back to the Response header
                OutgoingWebResponseContext response = WebOperationContext.Current.OutgoingResponse;
                response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                response.StatusDescription = ex.Message.Replace("\r\n", "");
                //return null;

                jsonDataStrngReturn = "Error";
            }
            finally
            {
                ds.Clear();
            }
            return cls.ToList();  //jsonDataStrngReturn;
        }

        public List<clsVoterSearchWS> GetPartWiseDetails(string assemblyid, string partno, string maxserverid, string localBodyId, string localBodyType)
        {
            try
            {
                if (localBodyType.Equals("1"))
                {
                    cmd.CommandText = "VoterSearchByPart";
                }
                else if (localBodyType.Equals("4"))
                {
                    cmd.CommandText = "VoterSearchByDivision_Part";
                }
                else if (localBodyType.Equals("5"))
                {
                    cmd.CommandText = "VoterSearchByElectoralCollege_Part";
                }

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@TableName", SqlDbType.NVarChar).Value = "Ac_" + assemblyid + "_";
                cmd.Parameters.Add("@PartNo", SqlDbType.NVarChar).Value = partno.Trim();
                cmd.Parameters.Add("@ServerMaxId", SqlDbType.Int).Value = maxserverid.Trim();
                cmd.Connection = con;
                da.SelectCommand = cmd;
                da.Fill(ds);
                // }
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        objcls = new clsVoterSearchWS();
                        objcls.Id = ds.Tables[0].Rows[i]["ID"] != null ? ds.Tables[0].Rows[i]["ID"].ToString() : "NA";
                        objcls.Sr = ds.Tables[0].Rows[i]["Sr"] != null ? ds.Tables[0].Rows[i]["Sr"].ToString() : "NA";
                        objcls.WardNo = ds.Tables[0].Rows[i]["WardNo"] != null ? ds.Tables[0].Rows[i]["WardNo"].ToString() : "NA";
                        objcls.IDCARD_NO = ds.Tables[0].Rows[i]["IDCARD_NO"] != null ? ds.Tables[0].Rows[i]["IDCARD_NO"].ToString() : "NA";
                        objcls.fm_name_v1 = ds.Tables[0].Rows[i]["fm_name_v1"] != null ? ds.Tables[0].Rows[i]["fm_name_v1"].ToString() : "NA";
                        objcls.Lastname_v1 = ds.Tables[0].Rows[i]["Lastname_v1"] != null ? ds.Tables[0].Rows[i]["Lastname_v1"].ToString() : "NA";
                        objcls.RLN_FM_NM_v1 = ds.Tables[0].Rows[i]["RLN_FM_NM_v1"] != null ? ds.Tables[0].Rows[i]["RLN_FM_NM_v1"].ToString() : "NA";
                        objcls.RLN_L_NM_v1 = ds.Tables[0].Rows[i]["RLN_L_NM_v1"] != null ? ds.Tables[0].Rows[i]["RLN_L_NM_v1"].ToString() : "NA";
                        objcls.FM_NAMEEN = ds.Tables[0].Rows[i]["FM_NAMEEN"] != null ? ds.Tables[0].Rows[i]["FM_NAMEEN"].ToString() : "NA";
                        objcls.LASTNAMEEN = ds.Tables[0].Rows[i]["LASTNAMEEN"] != null ? ds.Tables[0].Rows[i]["LASTNAMEEN"].ToString() : "NA";
                        objcls.RLN_FM_NMEN = ds.Tables[0].Rows[i]["RLN_FM_NMEN"] != null ? ds.Tables[0].Rows[i]["RLN_FM_NMEN"].ToString() : "NA";
                        objcls.SEX = ds.Tables[0].Rows[i]["SEX"] != null ? ds.Tables[0].Rows[i]["SEX"].ToString() : "NA";
                        objcls.AGE = ds.Tables[0].Rows[i]["AGE"] != null ? ds.Tables[0].Rows[i]["AGE"].ToString() : "NA";
                        objcls.AC_NO = ds.Tables[0].Rows[i]["AC_NO"] != null ? ds.Tables[0].Rows[i]["AC_NO"].ToString() : "NA";
                        objcls.PART_NO = ds.Tables[0].Rows[i]["PART_NO"] != null ? ds.Tables[0].Rows[i]["PART_NO"].ToString() : "NA";
                        objcls.SLNOINPART = ds.Tables[0].Rows[i]["SLNOINPART"] != null ? ds.Tables[0].Rows[i]["SLNOINPART"].ToString() : "NA";
                        objcls.BoothNumber = ds.Tables[0].Rows[i]["BoothNumber"] != null ? ds.Tables[0].Rows[i]["BoothNumber"].ToString() : "NA";
                        objcls.boothname = ds.Tables[0].Rows[i]["boothname"] != null ? ds.Tables[0].Rows[i]["boothname"].ToString() : "NA";
                        objcls.BoothAddress = ds.Tables[0].Rows[i]["BoothAddress"] != null ? ds.Tables[0].Rows[i]["BoothAddress"].ToString() : "NA";
                        objcls.house_no = ds.Tables[0].Rows[i]["HouseNo"] != null ? ds.Tables[0].Rows[i]["HouseNo"].ToString() : "NA";
                        objcls.Section_No = ds.Tables[0].Rows[i]["SectionNo"] != null ? ds.Tables[0].Rows[i]["SectionNo"].ToString() : "NA";
                        objcls.section_name_v1 = ds.Tables[0].Rows[i]["section_name_v1"] != null ? ds.Tables[0].Rows[i]["section_name_v1"].ToString() : "NA";
                        objcls.section_name_en = ds.Tables[0].Rows[i]["section_name_en"] != null ? ds.Tables[0].Rows[i]["section_name_en"].ToString() : "NA";
                        if (assemblyid == "071" || assemblyid == "096" || assemblyid == "235" || assemblyid == "114" || assemblyid == "115" || assemblyid == "136" || assemblyid == "137" || assemblyid == "188")
                        {
                            objcls.SerialNoInBooth = ds.Tables[0].Rows[i]["SerialNo_Booth"] != null ? ds.Tables[0].Rows[i]["SerialNo_Booth"].ToString() : "NA";
                        }
                        else
                        {
                            objcls.SerialNoInBooth = "NA";
                        }
                        objcls.dob = ds.Tables[0].Rows[i]["dob"] != null ? ds.Tables[0].Rows[i]["dob"].ToString() : "NA";
                        objcls.aadharno = ds.Tables[0].Rows[i]["AadhaarNo"] != null ? ds.Tables[0].Rows[i]["AadhaarNo"].ToString() : "NA";
                        cls.Add(objcls);

                        // jsonDataStrngReturn = JsonConvert.SerializeObject(cls);
                    }
                }
                else
                {
                    jsonDataStrngReturn = "NoData Found";
                }

            }
            catch (Exception ex)
            {
                //  Return any exception messages back to the Response header
                OutgoingWebResponseContext response = WebOperationContext.Current.OutgoingResponse;
                response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                response.StatusDescription = ex.Message.Replace("\r\n", "");
                //return null;

                jsonDataStrngReturn = "Error";
            }
            finally
            {
                ds.Clear();
            }
            return cls.ToList();  //jsonDataStrngReturn;
        }

        public string GetStringVoterDetails(string Name, string LName, string MaxId, string acno)
        {
            try
            {
                //cmd.CommandText = "SELECT TOP 30 [CCODE] AS Sr,[PART_NO] AS WardNo ,[IDCARD_NO] AS IDCARD_NO,[fm_name_v1] AS fm_name_v1,[Lastname_v1] AS Lastname_v1," +
                //                  "[RLN_FM_NM_v1],[RLN_L_NM_v1],[FM_NAMEEN],[LASTNAMEEN],[RLN_FM_NMEN],[SEX],[AGE],[AC_NO],[PART_NO],[SLNOINPART],[PART_NO] AS BoothNumber," +
                //                  "[PART_NO] AS boothname,[PART_NO] AS BoothAddress FROM [DBVoterSearch].[dbo].[AC174part001] WHERE [FM_NAMEEN] LIKE '" + Name.Trim() + "%' AND [LASTNAMEEN] LIKE '" + LName.Trim() + "%'  AND [CCODE] > '" + MaxId.Trim() + "' ";
                cmd.CommandText = "VoterSearchByName";
                //cmd.CommandType = CommandType.Text;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@TableName", SqlDbType.NVarChar).Value = "Ac_" + acno + "_";                                   //"AC174part001".Trim(); 
                cmd.Parameters.Add("@SearchFName", SqlDbType.NVarChar).Value = Name.Trim();
                cmd.Parameters.Add("@SearchLName", SqlDbType.NVarChar).Value = LName.Trim();
                cmd.Parameters.Add("@ServerMaxId", SqlDbType.Int).Value = MaxId.Trim();
                cmd.Connection = con;
                da.SelectCommand = cmd;
                da.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        objcls = new clsVoterSearchWS();
                        objcls.Id = ds.Tables[0].Rows[i]["Sr"].ToString();
                        objcls.Sr = ds.Tables[0].Rows[i]["Sr"].ToString();
                        objcls.WardNo = ds.Tables[0].Rows[i]["WardNo"].ToString();
                        objcls.IDCARD_NO = ds.Tables[0].Rows[i]["IDCARD_NO"].ToString();
                        objcls.fm_name_v1 = ds.Tables[0].Rows[i]["fm_name_v1"].ToString();
                        objcls.Lastname_v1 = ds.Tables[0].Rows[i]["Lastname_v1"].ToString();
                        objcls.RLN_FM_NM_v1 = ds.Tables[0].Rows[i]["RLN_FM_NM_v1"].ToString();
                        objcls.RLN_L_NM_v1 = ds.Tables[0].Rows[i]["RLN_L_NM_v1"].ToString();
                        objcls.FM_NAMEEN = ds.Tables[0].Rows[i]["FM_NAMEEN"].ToString();
                        objcls.LASTNAMEEN = ds.Tables[0].Rows[i]["LASTNAMEEN"].ToString();
                        objcls.RLN_FM_NMEN = ds.Tables[0].Rows[i]["RLN_FM_NMEN"].ToString();
                        objcls.SEX = ds.Tables[0].Rows[i]["SEX"].ToString();
                        objcls.AGE = ds.Tables[0].Rows[i]["AGE"].ToString();
                        objcls.AC_NO = ds.Tables[0].Rows[i]["AC_NO"].ToString();
                        objcls.PART_NO = ds.Tables[0].Rows[i]["PART_NO"].ToString();
                        objcls.SLNOINPART = ds.Tables[0].Rows[i]["SLNOINPART"].ToString();
                        objcls.BoothNumber = ds.Tables[0].Rows[i]["BoothNumber"].ToString();
                        objcls.boothname = ds.Tables[0].Rows[i]["boothname"].ToString();
                        objcls.BoothAddress = ds.Tables[0].Rows[i]["BoothAddress"].ToString();
                        cls.Add(objcls);

                        jsonDataStrngReturn = JsonConvert.SerializeObject(cls, theJsonSerializerSettings);
                    }
                }
                else
                {
                    jsonDataStrngReturn = "NoData Found";
                }

            }
            catch (Exception ex)
            {
                //  Return any exception messages back to the Response header
                OutgoingWebResponseContext response = WebOperationContext.Current.OutgoingResponse;
                response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                response.StatusDescription = ex.Message.Replace("\r\n", "");
                //return null;

                jsonDataStrngReturn = "Error";
            }
            finally
            {
                ds.Clear();
            }
            return jsonDataStrngReturn.ToString(); ;
        }

        //public string InsertRegRecord(string MaxId)
        //{
        //    SqlCommand cmd = new SqlCommand();
        //    DataSet ds1 = new DataSet();
        //    try
        //    {
        //        Sql = "select [LBID],[Id],[FirstName],[MiddleName],[LastName],[MobNo],[EDivisionID],[EDivisionName],[ECollegeID],[ECollegeName],[UserName] FROM  [SEC].[dbo].[RegistrationDtls] " +
        //               "WHERE Id > '" + MaxId + "'";
        //        da = new SqlDataAdapter(Sql, seccon);
        //        da.Fill(ds);

        //        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        //        {
        //            string sql = string.Empty;

        //            Sql = "Select [LBDistrictID] from [SEC].[dbo].[LB_District] where [LBID]='" + ds.Tables[0].Rows[i]["LBID"].ToString() + "'";
        //            cmd = new SqlCommand(Sql, seccon);
        //            seccon.Open();
        //            string districtId = Convert.ToString(cmd.ExecuteScalar());

        //            cmd = new SqlCommand("Select [Districtname] from [SEC].[dbo].[mstDistrict] where [Districtcode]='" + districtId + "'", seccon);
        //            seccon.Open();
        //            string Districtname = Convert.ToString(cmd.ExecuteScalar());

        //            Sql = "Select [SubDistrictcode],[SubDistrictname] from [SEC].[dbo].[mstTaluka] where [DistrictCode]='" + districtId + "'";
        //            da = new SqlDataAdapter(Sql, seccon);
        //            da.Fill(ds1);

        //            sql = "Insert into [SEC_TV].[dbo].[tblRegistrationSEC]([RegIDSEC],[FirstName],[MiddleName],[LastName],[RegMobileNo],[DistrictId],[DistrictName],[TalukaId],[TalukaName],[ElectrolId] " +
        //                 ",[ElectrolName],[ElectrolClgId],[ElectrolClgName],[PartyName],[SymbolName],[ElectionName]) values('" + ds.Tables[0].Rows[i]["Id"].ToString() + "','" + ds.Tables[0].Rows[i]["FirstName"].ToString() + "' " +
        //                 ",'" + ds.Tables[0].Rows[i]["MiddleName"].ToString() + "','" + ds.Tables[0].Rows[i]["LastName"].ToString() + "','" + ds.Tables[0].Rows[i]["MobNo"].ToString() + "'," +
        //                 ",'" + districtId + "','" + Districtname + "','" + ds1.Tables[0].Rows[i]["SubDistrictcode"].ToString() + "','" + ds1.Tables[0].Rows[i]["SubDistrictname"].ToString() + "' " +
        //                ",'" + ds.Tables[0].Rows[i]["EDivisionID"].ToString() + "','" + ds.Tables[0].Rows[i]["EDivisionName"].ToString() + "','" + ds.Tables[0].Rows[i]["ECollegeID"].ToString() + "','" + ds.Tables[0].Rows[i]["ECollegeName"].ToString() + "' " +
        //              ",'" + ds.Tables[0].Rows[i]["UserName"].ToString() + "')";
        //            cmd = new SqlCommand(sql, sectempcon);
        //            seccon.Open();
        //            cmd.ExecuteNonQuery();
        //            seccon.Close();
        //            jsonDataStrngReturn += "1";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        jsonDataStrngReturn += "0";
        //    }
        //    finally
        //    {
        //        ds.Clear();
        //    }
        //    return jsonDataStrngReturn;
        //}

        //public string InsertRegRecordMP(string MaxId)
        //{
        //    SqlCommand cmd = new SqlCommand();
        //    DataSet ds1 = new DataSet();
        //    try
        //    {
        //        Sql = "select [LocalBodyID],[Id],[FirstName],[MiddleName],[LastName],[CandidateMob],[Address],[NominationID] FROM  [SEC].[dbo].[NominationMP_Reg] " +
        //               "WHERE NominationID > '" + MaxId + "'";
        //        da = new SqlDataAdapter(Sql, seccon);
        //        da.Fill(ds);

        //        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        //        {
        //            string sql = string.Empty;

        //            Sql = "Select [LBDistrictID] from [SEC].[dbo].[LB_District] where [LBID]='" + ds.Tables[0].Rows[i]["LocalBodyID"].ToString() + "'";
        //            cmd = new SqlCommand(Sql, seccon);
        //            seccon.Open();
        //            string districtId = Convert.ToString(cmd.ExecuteScalar());

        //            cmd = new SqlCommand("Select [Districtname] from [SEC].[dbo].[mstDistrict] where [Districtcode]='" + districtId + "'", seccon);
        //            seccon.Open();
        //            string Districtname = Convert.ToString(cmd.ExecuteScalar());

        //            Sql = "Select [SubDistrictcode],[SubDistrictname] from [SEC].[dbo].[mstTaluka] where [DistrictCode]='" + districtId + "'";
        //            da = new SqlDataAdapter(Sql, seccon);
        //            da.Fill(ds1);

        //            sql = "Insert into [temp].[dbo].[tblRegistrationSEC]([NominationID],[FirstName],[MiddleName],[LastName],[RegMobileNo],[DistrictId],[DistrictName],[TalukaId],[TalukaName],[Address]) " +
        //                 " values('" + ds.Tables[0].Rows[i]["NominationID"].ToString() + "','" + ds.Tables[0].Rows[i]["FirstName"].ToString() + "' " +
        //                 ",'" + ds.Tables[0].Rows[i]["MiddleName"].ToString() + "','" + ds.Tables[0].Rows[i]["LastName"].ToString() + "','" + ds.Tables[0].Rows[i]["CandidateMob"].ToString() + "'," +
        //                 ",'" + districtId + "','" + Districtname + "','" + ds1.Tables[0].Rows[i]["SubDistrictcode"].ToString() + "','" + ds1.Tables[0].Rows[i]["SubDistrictname"].ToString() + "' " +
        //                ",'" + ds.Tables[0].Rows[i]["Address"].ToString() + "')";
        //            cmd = new SqlCommand(sql, sectempcon);
        //            seccon.Open();
        //            cmd.ExecuteNonQuery();
        //            seccon.Close();
        //            jsonDataStrngReturn += "1";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        jsonDataStrngReturn += "0";
        //    }
        //    finally
        //    {
        //        ds.Clear();
        //    }
        //    return jsonDataStrngReturn;
        //}

        //public string InsertRegRecordZP(string MaxId)
        //{
        //    SqlCommand cmd = new SqlCommand();
        //    DataSet ds1 = new DataSet();
        //    try
        //    {
        //        Sql = "select [LocalBodyID],[FirstName],[MiddleName],[LastName],[CandidateMob],[Address],[Code] FROM  [SEC].[dbo].[NominationZP_Reg] " +
        //               "WHERE Code > '" + MaxId + "'";
        //        da = new SqlDataAdapter(Sql, seccon);
        //        da.Fill(ds);

        //        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        //        {
        //            string sql = string.Empty;

        //            Sql = "Select [LBDistrictID] from [SEC].[dbo].[LB_District] where [LBID]='" + ds.Tables[0].Rows[i]["LocalBodyID"].ToString() + "'";
        //            cmd = new SqlCommand(Sql, seccon);
        //            seccon.Open();
        //            string districtId = Convert.ToString(cmd.ExecuteScalar());

        //            cmd = new SqlCommand("Select [Districtname] from [SEC].[dbo].[mstDistrict] where [Districtcode]='" + districtId + "'", seccon);
        //            seccon.Open();
        //            string Districtname = Convert.ToString(cmd.ExecuteScalar());

        //            Sql = "Select [SubDistrictcode],[SubDistrictname] from [SEC].[dbo].[mstTaluka] where [DistrictCode]='" + districtId + "'";
        //            da = new SqlDataAdapter(Sql, seccon);
        //            da.Fill(ds1);

        //            sql = "Insert into [SEC_TV].[dbo].[tblRegistrationSEC]([Code],[FirstName],[MiddleName],[LastName],[RegMobileNo],[DistrictId],[DistrictName],[TalukaId],[TalukaName],[Address]"+
        //                    // ,[ElectrolId] " +",[ElectrolName],[ElectrolClgId],[ElectrolClgName],[PartyName],[SymbolName],[ElectionName]) 
        //                 "values('" + ds.Tables[0].Rows[i]["Code"].ToString() + "','" + ds.Tables[0].Rows[i]["FirstName"].ToString() + "' " +
        //                 ",'" + ds.Tables[0].Rows[i]["MiddleName"].ToString() + "','" + ds.Tables[0].Rows[i]["LastName"].ToString() + "','" + ds.Tables[0].Rows[i]["CandidateMob"].ToString() + "'," +
        //                 ",'" + districtId + "','" + Districtname + "','" + ds1.Tables[0].Rows[i]["SubDistrictcode"].ToString() + "','" + ds1.Tables[0].Rows[i]["SubDistrictname"].ToString() + "' " +
        //               ",'" + ds.Tables[0].Rows[i]["Address"].ToString() + "')";
        //            cmd = new SqlCommand(sql, sectempcon);
        //            seccon.Open();
        //            cmd.ExecuteNonQuery();
        //            seccon.Close();
        //            jsonDataStrngReturn += "1";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        jsonDataStrngReturn += "0";
        //    }
        //    finally
        //    {
        //        ds.Clear();
        //    }
        //    return jsonDataStrngReturn;
        //}

        public List<Downloadreg> DownloadRegDataMP(string localbodyid, string localbodytype, string dateform, string dateto, string reportStatus)
        {
            DataTable dt = new DataTable();
            List<Downloadreg> lstdwn = new List<Downloadreg>();
            try
            {
                if (localbodytype == "5")
                {
                    //Sql = "select [CreatedDate],[WardID],[LocalBodyID],[FirstName],[MiddleName],[LastName],[CandidateMob],[Address],[NominationID],[GroupID],[Pin],[Formtype],[SU_Status],[SubChk],[withdrawal_Status],[Aff_FinalSumbission] FROM  [NominationMP_Reg] " +
                    //     "WHERE  Symbols_ID NOT IN (0) and Subchk=1 and Withdrawal_status is null and [CreatedDate] BETWEEN '2017-01-27' and '2017-02-07'";
                    if (reportStatus == "AL")
                    {
                        if (dateform == "" || dateto == "")
                        {
                            Sql = "select [CreatedDate],[WardID],[LocalBodyID],[FirstName],[MiddleName],[LastName],[CandidateMob],[NominationID],[GroupID],[Pin],[Formtype],[SU_Status],[SubChk],[withdrawal_Status],[Aff_FinalSumbission] FROM  [NominationMP_Reg] " +
                                 "WHERE LocalBodyID='" + localbodyid + "'";
                        }
                        else
                        {
                            Sql = "select [CreatedDate],[WardID],[LocalBodyID],[FirstName],[MiddleName],[LastName],[CandidateMob],[NominationID],[GroupID],[Pin],[Formtype],[SU_Status],[SubChk],[withdrawal_Status],[Aff_FinalSumbission] FROM  [NominationMP_Reg] " +
                               "WHERE LocalBodyID='" + localbodyid + "' and [CreatedDate] BETWEEN '" + dateform + "' AND '" + dateto + "'";
                        }
                    }
                    else if (reportStatus == "F")
                    {
                        if (dateform == "" || dateto == "")
                        {
                            Sql = "select [CreatedDate],[WardID],[LocalBodyID],[FirstName],[MiddleName],[LastName],[CandidateMob],[NominationID],[GroupID],[Pin],[Formtype],[SU_Status],[SubChk],[withdrawal_Status],[Aff_FinalSumbission] FROM  [NominationMP_Reg] " +
                                 "WHERE LocalBodyID='" + localbodyid + "' and Symbols_ID IS NOT NULL and SU_Status='A' and  [withdrawal_Status] IS NULL  ";
                        }
                        else
                        {
                            Sql = "select [CreatedDate],[WardID],[LocalBodyID],[FirstName],[MiddleName],[LastName],[CandidateMob],[NominationID],[GroupID],[Pin],[Formtype],[SU_Status],[SubChk],[withdrawal_Status],[Aff_FinalSumbission] FROM  [NominationMP_Reg] " +
                               "WHERE LocalBodyID='" + localbodyid + "' and Symbols_ID IS NOT NULL and SU_Status='A' and  [withdrawal_Status] IS NULL  and [CreatedDate] BETWEEN '" + dateform + "' AND '" + dateto + "'";
                        }
                    }
                    else if (reportStatus == "P")
                    {
                        if (dateform == "" || dateto == "")
                        {
                            Sql = "select [CreatedDate],[WardID],[LocalBodyID],[FirstName],[MiddleName],[LastName],[CandidateMob],[NominationID],[GroupID],[Pin],[Formtype],[SU_Status],[SubChk],[withdrawal_Status],[Aff_FinalSumbission] FROM  [NominationMP_Reg] " +
                                 "WHERE LocalBodyID='" + localbodyid + "' and SU_Status='P'";
                        }
                        else
                        {
                            Sql = "select [CreatedDate],[WardID],[LocalBodyID],[FirstName],[MiddleName],[LastName],[CandidateMob],[NominationID],[GroupID],[Pin],[Formtype],[SU_Status],[SubChk],[withdrawal_Status],[Aff_FinalSumbission] FROM  [NominationMP_Reg] " +
                               "WHERE LocalBodyID='" + localbodyid + "' and SU_Status='P' and [CreatedDate] BETWEEN '" + dateform + "' AND '" + dateto + "'";
                        }
                    }

                    else if (reportStatus == "R")
                    {
                        if (dateform == "" || dateto == "")
                        {
                            Sql = "select [CreatedDate],[WardID],[LocalBodyID],[FirstName],[MiddleName],[LastName],[CandidateMob],[NominationID],[GroupID],[Pin],[Formtype],[SU_Status],[SubChk],[withdrawal_Status],[Aff_FinalSumbission] FROM  [NominationMP_Reg] " +
                                 "WHERE LocalBodyID='" + localbodyid + "' and SU_Status='R'";
                        }
                        else
                        {
                            Sql = "select [CreatedDate],[WardID],[LocalBodyID],[FirstName],[MiddleName],[LastName],[CandidateMob],[NominationID],[GroupID],[Pin],[Formtype],[SU_Status],[SubChk],[withdrawal_Status],[Aff_FinalSumbission] FROM  [NominationMP_Reg] " +
                               "WHERE LocalBodyID='" + localbodyid + "' and SU_Status='R' and [CreatedDate] BETWEEN '" + dateform + "' AND '" + dateto + "'";
                        }
                    }

                    else if (reportStatus == "WY")
                    {
                        if (dateform == "" || dateto == "")
                        {
                            Sql = "select [CreatedDate],[WardID],[LocalBodyID],[FirstName],[MiddleName],[LastName],[CandidateMob],[NominationID],[GroupID],[Pin],[Formtype],[SU_Status],[SubChk],[withdrawal_Status],[Aff_FinalSumbission] FROM  [NominationMP_Reg] " +
                                 "WHERE LocalBodyID='" + localbodyid + "' and SU_Status='A' and  [withdrawal_Status]='y'";
                        }
                        else
                        {
                            Sql = "select [CreatedDate],[WardID],[LocalBodyID],[FirstName],[MiddleName],[LastName],[CandidateMob],[NominationID],[GroupID],[Pin],[Formtype],[SU_Status],[SubChk],[withdrawal_Status],[Aff_FinalSumbission] FROM  [NominationMP_Reg] " +
                               "WHERE LocalBodyID='" + localbodyid + "' and SU_Status='A' and  [withdrawal_Status]='y' and [CreatedDate] BETWEEN '" + dateform + "' AND '" + dateto + "'";
                        }
                    }
                    else if (reportStatus == "RO")
                    {
                        if (dateform == "" || dateto == "")
                        {
                            Sql = "select [CreatedDate],[WardID],[LocalBodyID],[FirstName],[MiddleName],[LastName],[CandidateMob],[NominationID],[GroupID],[Pin],[Formtype],[SU_Status],[SubChk],[withdrawal_Status],[Aff_FinalSumbission] FROM  [NominationMP_Reg] " +
                                 "WHERE LocalBodyID='" + localbodyid + "' and Subchk=1";
                        }
                        else
                        {
                            Sql = "select [CreatedDate],[WardID],[LocalBodyID],[FirstName],[MiddleName],[LastName],[CandidateMob],[NominationID],[GroupID],[Pin],[Formtype],[SU_Status],[SubChk],[withdrawal_Status],[Aff_FinalSumbission] FROM  [NominationMP_Reg] " +
                               "WHERE LocalBodyID='" + localbodyid + "' and Subchk=1 and [CreatedDate] BETWEEN '" + dateform + "' AND '" + dateto + "'";
                        }
                    }



                    da = new SqlDataAdapter(Sql, seccon);
                    da.Fill(ds);

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

                        cmd = new SqlCommand("Select [LBName] from [LB_LocalBody] where [LBID]='" + Convert.ToString(ds.Tables[0].Rows[i]["LocalBodyID"]) + "'", seccon);
                        seccon.Open();
                        string LocalBodyName = Convert.ToString(cmd.ExecuteScalar());
                        seccon.Close();

                        Sql = "Select [WardID] from [WardSeatDetails] where [Ward_SeatID]='" + ds.Tables[0].Rows[i]["WardID"].ToString() + "'";
                        cmd = new SqlCommand(Sql, seccon);
                        seccon.Open();
                        string wardid = Convert.ToString(cmd.ExecuteScalar());
                        seccon.Close();

                        Downloadreg dwn = new Downloadreg();
                        dwn.LOCALBODYNAME = Convert.ToString(LocalBodyName);
                        dwn.LOCALBODYID = ds.Tables[0].Rows[i]["LocalBodyID"].ToString() != null ? ds.Tables[0].Rows[i]["LocalBodyID"].ToString() : "NA";
                        dwn.FIRSTNAME = ds.Tables[0].Rows[i]["FirstName"].ToString() != null ? ds.Tables[0].Rows[i]["FirstName"].ToString() : "NA";
                        dwn.MIDDLENAME = ds.Tables[0].Rows[i]["MiddleName"].ToString() != null ? ds.Tables[0].Rows[i]["MiddleName"].ToString() : "NA";
                        dwn.LASTNAME = ds.Tables[0].Rows[i]["LastName"].ToString() != null ? ds.Tables[0].Rows[i]["LastName"].ToString() : "NA";
                        dwn.CANDIDATEMOB = ds.Tables[0].Rows[i]["CandidateMob"].ToString() != null ? ds.Tables[0].Rows[i]["CandidateMob"].ToString() : "NA";
                        // dwn.ADDRESS = ds.Tables[0].Rows[i]["Address"].ToString() != null ? ds.Tables[0].Rows[i]["Address"].ToString() : "NA";
                        dwn.NOMINATIONID = ds.Tables[0].Rows[i]["NominationID"].ToString() != null ? ds.Tables[0].Rows[i]["NominationID"].ToString() : "NA";
                        dwn.DISTRICTID = Convert.ToString(districtId);
                        dwn.DISTRICTNAME = Convert.ToString(Districtname);
                        dwn.WARDID = Convert.ToString(wardid);
                        // dwn.TALUKAID = talukaId.ToString();
                        // dwn.TALUKANAME = talukaName.ToString();
                        dwn.GROUPID = ds.Tables[0].Rows[i]["GroupID"].ToString() != null ? ds.Tables[0].Rows[i]["GroupID"].ToString() : "NA";
                        dwn.PIN = ds.Tables[0].Rows[i]["Pin"].ToString() != null ? ds.Tables[0].Rows[i]["Pin"].ToString() : "NA";
                        dwn.FORMTTYPE = ds.Tables[0].Rows[i]["Formtype"].ToString() != null ? ds.Tables[0].Rows[i]["Formtype"].ToString() : "NA";
                        dwn.CREATEDDATE = ds.Tables[0].Rows[i]["CreatedDate"].ToString() != null ? ds.Tables[0].Rows[i]["CreatedDate"].ToString() : "NA";
                        dwn.SU_STATUS = ds.Tables[0].Rows[i]["SU_Status"].ToString() != null ? ds.Tables[0].Rows[i]["SU_Status"].ToString() : "NA";
                        dwn.SUBCHK = ds.Tables[0].Rows[i]["SubChk"].ToString() != null ? ds.Tables[0].Rows[i]["SubChk"].ToString() : "NA";
                        dwn.WITHDRAWAL_STATUS = ds.Tables[0].Rows[i]["withdrawal_Status"].ToString() != null ? ds.Tables[0].Rows[i]["withdrawal_Status"].ToString() : "NA";
                        dwn.AFF_FINALSUBMISSION = ds.Tables[0].Rows[i]["Aff_FinalSumbission"].ToString() != null ? ds.Tables[0].Rows[i]["Aff_FinalSumbission"].ToString() : "NA";
                        lstdwn.Add(dwn);
                    }
                }
                else if (localbodytype == "2")
                {
                    //Sql = "select [CreatedDate],[ElectoralDivisionID],[LocalBodyID],[FirstName],[MiddleName],[LastName],[CandidateMob],[Address],[Code],[GroupID],[Pin],[Formtype] FROM  [NominationZP_Reg] " +
                    //     "WHERE  Symbols_ID NOT IN(0) and Subchk=1 and Withdrawal_status is null and [CreatedDate] BETWEEN '2017-01-27' and '2017-02-07'";
                    Sql = "select [CreatedDate],[ElectoralDivisionID],[LocalBodyID],[FirstName],[MiddleName],[LastName],[CandidateMob],[Address],[Code],[GroupID],[Pin],[Formtype],[SU_Status],[SubChk],[withdrawal_Status],[Aff_FinalSumbission] FROM  [NominationZP_Reg] " +
                       "WHERE LocalBodyID='" + localbodyid + "'";
                    da = new SqlDataAdapter(Sql, seccon);
                    da.Fill(ds);

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

                        cmd = new SqlCommand("Select [LBName] from [LB_LocalBody] where [LBID]='" + Convert.ToString(ds.Tables[0].Rows[i]["LocalBodyID"]) + "'", seccon);
                        seccon.Open();
                        string LocalBodyName = Convert.ToString(cmd.ExecuteScalar());
                        seccon.Close();

                        Downloadreg dwn = new Downloadreg();
                        dwn.LOCALBODYID = ds.Tables[0].Rows[i]["LocalBodyID"].ToString();
                        dwn.LOCALBODYNAME = LocalBodyName.ToString();
                        dwn.ELECTROLDIVISIONID = ds.Tables[0].Rows[i]["ElectoralDivisionID"].ToString();
                        dwn.FIRSTNAME = ds.Tables[0].Rows[i]["FirstName"].ToString();
                        dwn.MIDDLENAME = ds.Tables[0].Rows[i]["MiddleName"].ToString();
                        dwn.LASTNAME = ds.Tables[0].Rows[i]["LastName"].ToString();
                        dwn.CANDIDATEMOB = ds.Tables[0].Rows[i]["CandidateMob"].ToString();
                        // dwn.ADDRESS = ds.Tables[0].Rows[i]["Address"].ToString();
                        dwn.NOMINATIONID = ds.Tables[0].Rows[i]["Code"].ToString();
                        dwn.DISTRICTID = districtId.ToString();
                        dwn.DISTRICTNAME = Districtname.ToString();
                        dwn.GROUPID = ds.Tables[0].Rows[i]["GroupID"].ToString();
                        dwn.PIN = ds.Tables[0].Rows[i]["Pin"].ToString();
                        dwn.FORMTTYPE = ds.Tables[0].Rows[i]["Formtype"].ToString();
                        dwn.CREATEDDATE = ds.Tables[0].Rows[i]["CreatedDate"].ToString();
                        dwn.SU_STATUS = ds.Tables[0].Rows[i]["SU_Status"].ToString() != null ? ds.Tables[0].Rows[i]["SU_Status"].ToString() : "NA";
                        dwn.SUBCHK = ds.Tables[0].Rows[i]["SubChk"].ToString() != null ? ds.Tables[0].Rows[i]["SubChk"].ToString() : "NA";
                        dwn.WITHDRAWAL_STATUS = ds.Tables[0].Rows[i]["withdrawal_Status"].ToString() != null ? ds.Tables[0].Rows[i]["withdrawal_Status"].ToString() : "NA";
                        dwn.AFF_FINALSUBMISSION = ds.Tables[0].Rows[i]["Aff_FinalSumbission"].ToString() != null ? ds.Tables[0].Rows[i]["Aff_FinalSumbission"].ToString() : "NA";
                        lstdwn.Add(dwn);
                    }
                }
                else if (localbodytype == "3")
                {
                    //Sql = "select [CreatedDate],[ElectoralDivisionID],[CollegeID],[LocalBodyID],[FirstName],[MiddleName],[LastName],[CandidateMob],[Address],[Code],[GroupID],[Pin],[Formtype] FROM  [Nomination_Reg] " +
                    //     "WHERE Symbols_ID NOT IN(0) and Subchk=1 and Withdrawal_status is null and [CreatedDate] BETWEEN '2017-01-27' and '2017-02-07'";
                    Sql = "select [CreatedDate],[ElectoralDivisionID],[CollegeID],[LocalBodyID],[FirstName],[MiddleName],[LastName],[CandidateMob],[Address],[Code],[GroupID],[Pin],[Formtype],[SU_Status],[SubChk],[withdrawal_Status],[Aff_FinalSumbission] FROM  [Nomination_Reg] " +
                      "WHERE LocalBodyID='" + localbodyid + "'";
                    da = new SqlDataAdapter(Sql, seccon);
                    da.Fill(ds);

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

                        cmd = new SqlCommand("Select [LBName] from [LB_LocalBody] where [LBID]='" + Convert.ToString(ds.Tables[0].Rows[i]["LocalBodyID"]) + "'", seccon);
                        seccon.Open();
                        string LocalBodyName = Convert.ToString(cmd.ExecuteScalar());
                        seccon.Close();

                        Downloadreg dwn = new Downloadreg();
                        dwn.LOCALBODYNAME = LocalBodyName.ToString();
                        dwn.LOCALBODYID = ds.Tables[0].Rows[i]["LocalBodyID"].ToString();
                        dwn.ELECTROLDIVISIONID = ds.Tables[0].Rows[i]["ElectoralDivisionID"].ToString();
                        dwn.ELECTROLCLGID = ds.Tables[0].Rows[i]["CollegeID"].ToString();
                        dwn.FIRSTNAME = ds.Tables[0].Rows[i]["FirstName"].ToString();
                        dwn.MIDDLENAME = ds.Tables[0].Rows[i]["MiddleName"].ToString();
                        dwn.LASTNAME = ds.Tables[0].Rows[i]["LastName"].ToString();
                        dwn.CANDIDATEMOB = ds.Tables[0].Rows[i]["CandidateMob"].ToString();
                        //   dwn.ADDRESS = ds.Tables[0].Rows[i]["Address"].ToString();
                        dwn.NOMINATIONID = ds.Tables[0].Rows[i]["Code"].ToString();
                        dwn.DISTRICTID = districtId.ToString();
                        dwn.DISTRICTNAME = Districtname.ToString();
                        dwn.GROUPID = ds.Tables[0].Rows[i]["GroupID"].ToString();
                        dwn.PIN = ds.Tables[0].Rows[i]["Pin"].ToString();
                        dwn.FORMTTYPE = ds.Tables[0].Rows[i]["Formtype"].ToString();
                        dwn.CREATEDDATE = ds.Tables[0].Rows[i]["CreatedDate"].ToString();
                        dwn.SU_STATUS = ds.Tables[0].Rows[i]["SU_Status"].ToString() != null ? ds.Tables[0].Rows[i]["SU_Status"].ToString() : "NA";
                        dwn.SUBCHK = ds.Tables[0].Rows[i]["SubChk"].ToString() != null ? ds.Tables[0].Rows[i]["SubChk"].ToString() : "NA";
                        dwn.WITHDRAWAL_STATUS = ds.Tables[0].Rows[i]["withdrawal_Status"].ToString() != null ? ds.Tables[0].Rows[i]["withdrawal_Status"].ToString() : "NA";
                        dwn.AFF_FINALSUBMISSION = ds.Tables[0].Rows[i]["Aff_FinalSumbission"].ToString() != null ? ds.Tables[0].Rows[i]["Aff_FinalSumbission"].ToString() : "NA";
                        lstdwn.Add(dwn);
                    }
                }
            }
            catch
            {
                //jsonDataStrngReturn = "0";
            }
            return lstdwn.ToList();//jsonDataStrngReturn;
        }

        public List<Downloadreg> DownloadRegDataMPDetails(string localbodyid, string localbodytype, string dateform, string dateto)
        {
            DataTable dt = new DataTable();
            List<Downloadreg> lstdwn = new List<Downloadreg>();
            try
            {
                if (localbodytype == "5")
                {
                    //Sql = "select [CreatedDate],[WardID],[LocalBodyID],[FirstName],[MiddleName],[LastName],[CandidateMob],[Address],[NominationID],[GroupID],[Pin],[Formtype],[SU_Status],[SubChk],[withdrawal_Status],[Aff_FinalSumbission] FROM  [NominationMP_Reg] " +
                    //     "WHERE  Symbols_ID NOT IN (0) and Subchk=1 and Withdrawal_status is null and [CreatedDate] BETWEEN '2017-01-27' and '2017-02-07'";

                    Sql = "select [CreatedDate],[WardID],[LocalBodyID],[FirstName],[MiddleName],[LastName],[CandidateMob],[NominationID],[GroupID],[Pin],[Formtype],[SU_Status],[SubChk],[withdrawal_Status],[Aff_FinalSumbission] FROM  [NominationMP_Reg] " +
                     "WHERE LocalBodyID='" + localbodyid + "' and [CreatedDate] BETWEEN '" + dateform + "' AND '" + dateto + "'";   //,[Address]
                    da = new SqlDataAdapter(Sql, seccon);
                    da.Fill(ds);

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

                        cmd = new SqlCommand("Select [LBName] from [LB_LocalBody] where [LBID]='" + Convert.ToString(ds.Tables[0].Rows[i]["LocalBodyID"]) + "'", seccon);
                        seccon.Open();
                        string LocalBodyName = Convert.ToString(cmd.ExecuteScalar());
                        seccon.Close();

                        Sql = "Select [WardID] from [WardSeatDetails] where [Ward_SeatID]='" + ds.Tables[0].Rows[i]["WardID"].ToString() + "'";
                        cmd = new SqlCommand(Sql, seccon);
                        seccon.Open();
                        string wardid = Convert.ToString(cmd.ExecuteScalar());
                        seccon.Close();

                        Downloadreg dwn = new Downloadreg();
                        dwn.LOCALBODYNAME = Convert.ToString(LocalBodyName);
                        dwn.LOCALBODYID = ds.Tables[0].Rows[i]["LocalBodyID"].ToString() != null ? ds.Tables[0].Rows[i]["LocalBodyID"].ToString() : "NA";
                        dwn.FIRSTNAME = ds.Tables[0].Rows[i]["FirstName"].ToString() != null ? ds.Tables[0].Rows[i]["FirstName"].ToString() : "NA";
                        dwn.MIDDLENAME = ds.Tables[0].Rows[i]["MiddleName"].ToString() != null ? ds.Tables[0].Rows[i]["MiddleName"].ToString() : "NA";
                        dwn.LASTNAME = ds.Tables[0].Rows[i]["LastName"].ToString() != null ? ds.Tables[0].Rows[i]["LastName"].ToString() : "NA";
                        dwn.CANDIDATEMOB = ds.Tables[0].Rows[i]["CandidateMob"].ToString() != null ? ds.Tables[0].Rows[i]["CandidateMob"].ToString() : "NA";
                        // dwn.ADDRESS = ds.Tables[0].Rows[i]["Address"].ToString() != null ? ds.Tables[0].Rows[i]["Address"].ToString() : "NA";
                        dwn.NOMINATIONID = ds.Tables[0].Rows[i]["NominationID"].ToString() != null ? ds.Tables[0].Rows[i]["NominationID"].ToString() : "NA";
                        dwn.DISTRICTID = Convert.ToString(districtId);
                        dwn.DISTRICTNAME = Convert.ToString(Districtname);
                        dwn.WARDID = Convert.ToString(wardid);
                        // dwn.TALUKAID = talukaId.ToString();
                        // dwn.TALUKANAME = talukaName.ToString();
                        dwn.GROUPID = ds.Tables[0].Rows[i]["GroupID"].ToString() != null ? ds.Tables[0].Rows[i]["GroupID"].ToString() : "NA";
                        dwn.PIN = ds.Tables[0].Rows[i]["Pin"].ToString() != null ? ds.Tables[0].Rows[i]["Pin"].ToString() : "NA";
                        dwn.FORMTTYPE = ds.Tables[0].Rows[i]["Formtype"].ToString() != null ? ds.Tables[0].Rows[i]["Formtype"].ToString() : "NA";
                        dwn.CREATEDDATE = ds.Tables[0].Rows[i]["CreatedDate"].ToString() != null ? ds.Tables[0].Rows[i]["CreatedDate"].ToString() : "NA";
                        dwn.SU_STATUS = ds.Tables[0].Rows[i]["SU_Status"].ToString() != null ? ds.Tables[0].Rows[i]["SU_Status"].ToString() : "NA";
                        dwn.SUBCHK = ds.Tables[0].Rows[i]["SubChk"].ToString() != null ? ds.Tables[0].Rows[i]["SubChk"].ToString() : "NA";
                        dwn.WITHDRAWAL_STATUS = ds.Tables[0].Rows[i]["withdrawal_Status"].ToString() != null ? ds.Tables[0].Rows[i]["withdrawal_Status"].ToString() : "NA";
                        dwn.AFF_FINALSUBMISSION = ds.Tables[0].Rows[i]["Aff_FinalSumbission"].ToString() != null ? ds.Tables[0].Rows[i]["Aff_FinalSumbission"].ToString() : "NA";

                        lstdwn.Add(dwn);
                    }
                }
                else if (localbodytype == "2")
                {
                    //Sql = "select [CreatedDate],[ElectoralDivisionID],[LocalBodyID],[FirstName],[MiddleName],[LastName],[CandidateMob],[Address],[Code],[GroupID],[Pin],[Formtype] FROM  [NominationZP_Reg] " +
                    //     "WHERE  Symbols_ID NOT IN(0) and Subchk=1 and Withdrawal_status is null and [CreatedDate] BETWEEN '2017-01-27' and '2017-02-07'";
                    Sql = "select [CreatedDate],[ElectoralDivisionID],[LocalBodyID],[FirstName],[MiddleName],[LastName],[CandidateMob],[Address],[Code],[GroupID],[Pin],[Formtype],[SU_Status],[SubChk],[withdrawal_Status],[Aff_FinalSumbission] FROM  [NominationZP_Reg] " +
                       "WHERE LocalBodyID='" + localbodyid + "'";
                    da = new SqlDataAdapter(Sql, seccon);
                    da.Fill(ds);

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

                        cmd = new SqlCommand("Select [LBName] from [LB_LocalBody] where [LBID]='" + Convert.ToString(ds.Tables[0].Rows[i]["LocalBodyID"]) + "'", seccon);
                        seccon.Open();
                        string LocalBodyName = Convert.ToString(cmd.ExecuteScalar());
                        seccon.Close();

                        Downloadreg dwn = new Downloadreg();
                        dwn.LOCALBODYID = ds.Tables[0].Rows[i]["LocalBodyID"].ToString();
                        dwn.LOCALBODYNAME = LocalBodyName.ToString();
                        dwn.ELECTROLDIVISIONID = ds.Tables[0].Rows[i]["ElectoralDivisionID"].ToString();
                        dwn.FIRSTNAME = ds.Tables[0].Rows[i]["FirstName"].ToString();
                        dwn.MIDDLENAME = ds.Tables[0].Rows[i]["MiddleName"].ToString();
                        dwn.LASTNAME = ds.Tables[0].Rows[i]["LastName"].ToString();
                        dwn.CANDIDATEMOB = ds.Tables[0].Rows[i]["CandidateMob"].ToString();
                        // dwn.ADDRESS = ds.Tables[0].Rows[i]["Address"].ToString();
                        dwn.NOMINATIONID = ds.Tables[0].Rows[i]["Code"].ToString();
                        dwn.DISTRICTID = districtId.ToString();
                        dwn.DISTRICTNAME = Districtname.ToString();
                        dwn.GROUPID = ds.Tables[0].Rows[i]["GroupID"].ToString();
                        dwn.PIN = ds.Tables[0].Rows[i]["Pin"].ToString();
                        dwn.FORMTTYPE = ds.Tables[0].Rows[i]["Formtype"].ToString();
                        dwn.CREATEDDATE = ds.Tables[0].Rows[i]["CreatedDate"].ToString();
                        dwn.SU_STATUS = ds.Tables[0].Rows[i]["SU_Status"].ToString() != null ? ds.Tables[0].Rows[i]["SU_Status"].ToString() : "NA";
                        dwn.SUBCHK = ds.Tables[0].Rows[i]["SubChk"].ToString() != null ? ds.Tables[0].Rows[i]["SubChk"].ToString() : "NA";
                        dwn.WITHDRAWAL_STATUS = ds.Tables[0].Rows[i]["withdrawal_Status"].ToString() != null ? ds.Tables[0].Rows[i]["withdrawal_Status"].ToString() : "NA";
                        dwn.AFF_FINALSUBMISSION = ds.Tables[0].Rows[i]["Aff_FinalSumbission"].ToString() != null ? ds.Tables[0].Rows[i]["Aff_FinalSumbission"].ToString() : "NA";
                        lstdwn.Add(dwn);
                    }
                }
                else if (localbodytype == "3")
                {
                    //Sql = "select [CreatedDate],[ElectoralDivisionID],[CollegeID],[LocalBodyID],[FirstName],[MiddleName],[LastName],[CandidateMob],[Address],[Code],[GroupID],[Pin],[Formtype] FROM  [Nomination_Reg] " +
                    //     "WHERE Symbols_ID NOT IN(0) and Subchk=1 and Withdrawal_status is null and [CreatedDate] BETWEEN '2017-01-27' and '2017-02-07'";
                    Sql = "select [CreatedDate],[ElectoralDivisionID],[CollegeID],[LocalBodyID],[FirstName],[MiddleName],[LastName],[CandidateMob],[Address],[Code],[GroupID],[Pin],[Formtype],[SU_Status],[SubChk],[withdrawal_Status],[Aff_FinalSumbission] FROM  [Nomination_Reg] " +
                      "WHERE LocalBodyID='" + localbodyid + "'";
                    da = new SqlDataAdapter(Sql, seccon);
                    da.Fill(ds);

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

                        cmd = new SqlCommand("Select [LBName] from [LB_LocalBody] where [LBID]='" + Convert.ToString(ds.Tables[0].Rows[i]["LocalBodyID"]) + "'", seccon);
                        seccon.Open();
                        string LocalBodyName = Convert.ToString(cmd.ExecuteScalar());
                        seccon.Close();

                        Downloadreg dwn = new Downloadreg();
                        dwn.LOCALBODYNAME = LocalBodyName.ToString();
                        dwn.LOCALBODYID = ds.Tables[0].Rows[i]["LocalBodyID"].ToString();
                        dwn.ELECTROLDIVISIONID = ds.Tables[0].Rows[i]["ElectoralDivisionID"].ToString();
                        dwn.ELECTROLCLGID = ds.Tables[0].Rows[i]["CollegeID"].ToString();
                        dwn.FIRSTNAME = ds.Tables[0].Rows[i]["FirstName"].ToString();
                        dwn.MIDDLENAME = ds.Tables[0].Rows[i]["MiddleName"].ToString();
                        dwn.LASTNAME = ds.Tables[0].Rows[i]["LastName"].ToString();
                        dwn.CANDIDATEMOB = ds.Tables[0].Rows[i]["CandidateMob"].ToString();
                        //   dwn.ADDRESS = ds.Tables[0].Rows[i]["Address"].ToString();
                        dwn.NOMINATIONID = ds.Tables[0].Rows[i]["Code"].ToString();
                        dwn.DISTRICTID = districtId.ToString();
                        dwn.DISTRICTNAME = Districtname.ToString();
                        dwn.GROUPID = ds.Tables[0].Rows[i]["GroupID"].ToString();
                        dwn.PIN = ds.Tables[0].Rows[i]["Pin"].ToString();
                        dwn.FORMTTYPE = ds.Tables[0].Rows[i]["Formtype"].ToString();
                        dwn.CREATEDDATE = ds.Tables[0].Rows[i]["CreatedDate"].ToString();
                        dwn.SU_STATUS = ds.Tables[0].Rows[i]["SU_Status"].ToString() != null ? ds.Tables[0].Rows[i]["SU_Status"].ToString() : "NA";
                        dwn.SUBCHK = ds.Tables[0].Rows[i]["SubChk"].ToString() != null ? ds.Tables[0].Rows[i]["SubChk"].ToString() : "NA";
                        dwn.WITHDRAWAL_STATUS = ds.Tables[0].Rows[i]["withdrawal_Status"].ToString() != null ? ds.Tables[0].Rows[i]["withdrawal_Status"].ToString() : "NA";
                        dwn.AFF_FINALSUBMISSION = ds.Tables[0].Rows[i]["Aff_FinalSumbission"].ToString() != null ? ds.Tables[0].Rows[i]["Aff_FinalSumbission"].ToString() : "NA";
                        lstdwn.Add(dwn);
                    }
                }
            }
            catch
            {
                //jsonDataStrngReturn = "0";
            }
            return lstdwn.ToList();//jsonDataStrngReturn;
        }

        public List<Downloadreg> DownloadRegDataZP(string localbodyid)
        {
            DataTable dt = new DataTable();
            List<Downloadreg> lstdwn = new List<Downloadreg>();
            try
            {
                //Sql = "select [CreatedDate],[ElectoralDivisionID],[LocalBodyID],[FirstName],[MiddleName],[LastName],[CandidateMob],[Address],[Code],[GroupID],[Pin],[Formtype] FROM  [NominationZP_Reg] " +
                //     "WHERE  Symbols_ID NOT IN(0) and Subchk=1 and Withdrawal_status is null and [CreatedDate] BETWEEN '2017-01-27' and '2017-02-07'";
                Sql = "select [CreatedDate],[ElectoralDivisionID],[LocalBodyID],[FirstName],[MiddleName],[LastName],[CandidateMob],[Address],[Code],[GroupID],[Pin],[Formtype],[SU_Status],[SubChk],[withdrawal_Status],[Aff_FinalSumbission] FROM  [NominationZP_Reg] " +
                   "WHERE LocalBodyID='" + localbodyid + "'";
                da = new SqlDataAdapter(Sql, seccon);
                da.Fill(ds);

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

                    cmd = new SqlCommand("Select [LBName] from [LB_LocalBody] where [LBID]='" + Convert.ToString(ds.Tables[0].Rows[i]["LocalBodyID"]) + "'", seccon);
                    seccon.Open();
                    string LocalBodyName = Convert.ToString(cmd.ExecuteScalar());
                    seccon.Close();

                    Downloadreg dwn = new Downloadreg();
                    dwn.LOCALBODYID = ds.Tables[0].Rows[i]["LocalBodyID"].ToString();
                    dwn.LOCALBODYNAME = LocalBodyName.ToString();
                    dwn.ELECTROLDIVISIONID = ds.Tables[0].Rows[i]["ElectoralDivisionID"].ToString();
                    dwn.FIRSTNAME = ds.Tables[0].Rows[i]["FirstName"].ToString();
                    dwn.MIDDLENAME = ds.Tables[0].Rows[i]["MiddleName"].ToString();
                    dwn.LASTNAME = ds.Tables[0].Rows[i]["LastName"].ToString();
                    dwn.CANDIDATEMOB = ds.Tables[0].Rows[i]["CandidateMob"].ToString();
                    dwn.ADDRESS = ds.Tables[0].Rows[i]["Address"].ToString();
                    dwn.NOMINATIONID = ds.Tables[0].Rows[i]["Code"].ToString();
                    dwn.DISTRICTID = districtId.ToString();
                    dwn.DISTRICTNAME = Districtname.ToString();
                    dwn.GROUPID = ds.Tables[0].Rows[i]["GroupID"].ToString();
                    dwn.PIN = ds.Tables[0].Rows[i]["Pin"].ToString();
                    dwn.FORMTTYPE = ds.Tables[0].Rows[i]["Formtype"].ToString();
                    dwn.CREATEDDATE = ds.Tables[0].Rows[i]["CreatedDate"].ToString();
                    lstdwn.Add(dwn);
                }

                //jsonDataStrngReturn = JsonConvert.SerializeObject(lstdwn, theJsonSerializerSettings); ;
            }
            catch
            {
                // jsonDataStrngReturn = "0";
            }
            return lstdwn.ToList();//jsonDataStrngReturn;
        }

        public List<Downloadreg> DownloadRegDataPS(string localbodyid)
        {
            DataTable dt = new DataTable();
            List<Downloadreg> lstdwn = new List<Downloadreg>();
            try
            {
                //Sql = "select [CreatedDate],[ElectoralDivisionID],[CollegeID],[LocalBodyID],[FirstName],[MiddleName],[LastName],[CandidateMob],[Address],[Code],[GroupID],[Pin],[Formtype] FROM  [Nomination_Reg] " +
                //     "WHERE Symbols_ID NOT IN(0) and Subchk=1 and Withdrawal_status is null and [CreatedDate] BETWEEN '2017-01-27' and '2017-02-07'";
                Sql = "select [CreatedDate],[ElectoralDivisionID],[CollegeID],[LocalBodyID],[FirstName],[MiddleName],[LastName],[CandidateMob],[Address],[Code],[GroupID],[Pin],[Formtype],[SU_Status],[SubChk],[withdrawal_Status],[Aff_FinalSumbission] FROM  [Nomination_Reg] " +
                  "WHERE LocalBodyID='" + localbodyid + "'";
                da = new SqlDataAdapter(Sql, seccon);
                da.Fill(ds);

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

                    cmd = new SqlCommand("Select [LBName] from [LB_LocalBody] where [LBID]='" + Convert.ToString(ds.Tables[0].Rows[i]["LocalBodyID"]) + "'", seccon);
                    seccon.Open();
                    string LocalBodyName = Convert.ToString(cmd.ExecuteScalar());
                    seccon.Close();

                    Downloadreg dwn = new Downloadreg();
                    dwn.LOCALBODYNAME = LocalBodyName.ToString();
                    dwn.LOCALBODYID = ds.Tables[0].Rows[i]["LocalBodyID"].ToString();
                    dwn.ELECTROLDIVISIONID = ds.Tables[0].Rows[i]["ElectoralDivisionID"].ToString();
                    dwn.ELECTROLCLGID = ds.Tables[0].Rows[i]["CollegeID"].ToString();
                    dwn.FIRSTNAME = ds.Tables[0].Rows[i]["FirstName"].ToString();
                    dwn.MIDDLENAME = ds.Tables[0].Rows[i]["MiddleName"].ToString();
                    dwn.LASTNAME = ds.Tables[0].Rows[i]["LastName"].ToString();
                    dwn.CANDIDATEMOB = ds.Tables[0].Rows[i]["CandidateMob"].ToString();
                    dwn.ADDRESS = ds.Tables[0].Rows[i]["Address"].ToString();
                    dwn.NOMINATIONID = ds.Tables[0].Rows[i]["Code"].ToString();
                    dwn.DISTRICTID = districtId.ToString();
                    dwn.DISTRICTNAME = Districtname.ToString();
                    dwn.GROUPID = ds.Tables[0].Rows[i]["GroupID"].ToString();
                    dwn.PIN = ds.Tables[0].Rows[i]["Pin"].ToString();
                    dwn.FORMTTYPE = ds.Tables[0].Rows[i]["Formtype"].ToString();
                    dwn.CREATEDDATE = ds.Tables[0].Rows[i]["CreatedDate"].ToString();
                    lstdwn.Add(dwn);
                }

                //jsonDataStrngReturn = JsonConvert.SerializeObject(lstdwn, theJsonSerializerSettings); ;
            }
            catch
            {
                // jsonDataStrngReturn = "0";
            }
            return lstdwn.ToList(); //jsonDataStrngReturn;
        }

        public string InsertRegRecordMP(string MaxId)
        {
            string file = string.Empty;
            SqlCommand cmd = new SqlCommand();
            DataSet ds1 = new DataSet();
            try
            {
                Sql = "select [LocalBodyID],[Id],[FirstName],[MiddleName],[LastName],[CandidateMob],[Address],[NominationID],[GroupID],[Pin],[Formtype] FROM  [NominationMP_Reg] " +
                       "WHERE NominationID > '" + MaxId + "'";
                da = new SqlDataAdapter(Sql, seccon);
                da.Fill(ds);

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string sql = string.Empty;

                    Sql = "Select [LBDistrictID] from [LB_District] where [LBID]='" + Convert.ToString(ds.Tables[0].Rows[i]["LocalBodyID"]) + "'";
                    cmd = new SqlCommand(Sql, seccon);
                    seccon.Open();
                    string districtId = Convert.ToString(cmd.ExecuteScalar());
                    seccon.Close();

                    cmd = new SqlCommand("Select [Districtname] from [mstDistrict] where [Districtcode]='" + Convert.ToString(districtId) + "'", seccon);
                    seccon.Open();
                    string Districtname = Convert.ToString(cmd.ExecuteScalar());
                    seccon.Close();

                    cmd = new SqlCommand("Select [LBName] from [LB_LocalBody] where [LBID]='" + Convert.ToString(ds.Tables[0].Rows[i]["LocalBodyID"]) + "'", seccon);
                    seccon.Open();
                    string LocalBodyName = Convert.ToString(cmd.ExecuteScalar());
                    seccon.Close();

                    Sql = "Select [LBTalukaID] from [LB_Taluka] where [LBID]='" + Convert.ToString(ds.Tables[0].Rows[i]["LocalBodyID"]) + "'";
                    cmd = new SqlCommand(Sql, seccon);
                    seccon.Open();
                    string talukaId = Convert.ToString(cmd.ExecuteScalar());
                    seccon.Close();

                    sql = "Select [SubDistrictname] from [mstTaluka] where [SubDistrictcode]='" + Convert.ToString(talukaId) + "'";
                    seccon.Open();
                    string talukaName = Convert.ToString(cmd.ExecuteScalar());
                    seccon.Close();

                    sql = "Insert into [SEC_TV].[dbo].[tblRegistrationSECNEW]([NominationID],[FirstName],[MiddleName],[LastName],[RegMobileNo],[DistrictId],[DistrictName],[TalukaId],[TalukaName],[Address],[LocalBodyName],[GroupID],[pin],[formtype]) " +
                         " values(N'" + ds.Tables[0].Rows[i]["NominationID"].ToString() + "',N'" + ds.Tables[0].Rows[i]["FirstName"].ToString() + "' " +
                         ",N'" + ds.Tables[0].Rows[i]["MiddleName"].ToString() + "',N'" + ds.Tables[0].Rows[i]["LastName"].ToString() + "',N'" + ds.Tables[0].Rows[i]["CandidateMob"].ToString() + "'" +
                         ",N'" + districtId + "',N'" + Districtname + "',N'" + talukaId + "',N'" + talukaName + "'" +
                        ",N'" + ds.Tables[0].Rows[i]["Address"].ToString().Replace("'", "") + "',N'" + LocalBodyName + "',N'" + Convert.ToString(ds.Tables[0].Rows[i]["GroupID"]) + "',N'" + Convert.ToString(ds.Tables[0].Rows[i]["Pin"]) + "',N'" + Convert.ToString(ds.Tables[0].Rows[i]["Formtype"]) + "')";
                    cmd = new SqlCommand(sql, seccon);
                    seccon.Open();
                    cmd.ExecuteNonQuery();
                    seccon.Close();

                    // CommanCode cc = new CommanCode();
                    //string Msg = "SEC Welcomes " + Convert.ToString(ds.Tables[0].Rows[i]["FirstName"]) + " " + "" + Convert.ToString(ds.Tables[0].Rows[i]["MiddleName"]) + " " + "" + Convert.ToString(ds.Tables[0].Rows[i]["LastName"]) + ".  Congratulations for filing online nomination form successfully! In this election daily election expenses are to be filled online through True Voter Mobile Application. Download True Voter app on your android mobile from the link or Play Store and register it using Mobile Number used in Nomination form. goo.gl/YN7qn1";

                    // cc.SMS(Convert.ToString(ds.Tables[0].Rows[i]["CandidateMob"]), Msg);

                }
                jsonDataStrngReturn += "1";
            }
            catch (Exception ex)
            {
                jsonDataStrngReturn += "0";
                file = string.Empty;

                using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("Error.txt")))
                {
                    file = reader.ReadToEnd();
                    //writer.WriteLine("Line"); 
                }
                using (StreamWriter writer = new StreamWriter(HttpContext.Current.Server.MapPath("Error.txt")))
                {
                    writer.Write(file + "\n" + ex.StackTrace.ToString() + "\n");
                    //writer.WriteLine("Line"); 
                }

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
                Sql = "select [LocalBodyID],[FirstName],[MiddleName],[LastName],[CandidateMob],[Address],[Code],[GroupID],[Pin],[ElectoralDivision],[Formtype] FROM  [NominationZP_Reg] " +
                       "WHERE Code > '" + MaxId + "'";
                da = new SqlDataAdapter(Sql, seccon);
                da.Fill(ds);

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string sql = string.Empty;

                    Sql = "Select [LBDistrictID] from [LB_District] where [LBID]='" + Convert.ToString(ds.Tables[0].Rows[i]["LocalBodyID"]) + "'";
                    cmd = new SqlCommand(Sql, seccon);
                    seccon.Open();
                    string districtId = Convert.ToString(cmd.ExecuteScalar());
                    seccon.Close();

                    cmd = new SqlCommand("Select [Districtname] from [mstDistrict] where [Districtcode]='" + Convert.ToString(districtId) + "'", seccon);
                    seccon.Open();
                    string Districtname = Convert.ToString(cmd.ExecuteScalar());
                    seccon.Close();

                    cmd = new SqlCommand("Select [LBName] from [LB_LocalBody] where [LBID]='" + Convert.ToString(ds.Tables[0].Rows[i]["LocalBodyID"]) + "'", seccon);
                    seccon.Open();
                    string LocalBodyName = Convert.ToString(cmd.ExecuteScalar());
                    seccon.Close();

                    Sql = "Select [LBTalukaID] from [LB_Taluka] where [LBID]='" + Convert.ToString(ds.Tables[0].Rows[i]["LocalBodyID"]) + "'";
                    cmd = new SqlCommand(Sql, seccon);
                    seccon.Open();
                    string talukaId = Convert.ToString(cmd.ExecuteScalar());
                    seccon.Close();

                    Sql = "Select [SubDistrictname] from [mstTaluka] where [SubDistrictcode]='" + Convert.ToString(talukaId) + "'";
                    cmd = new SqlCommand(Sql, seccon);
                    seccon.Open();
                    string talukaName = Convert.ToString(cmd.ExecuteScalar());
                    seccon.Close();


                    sql = "Insert into [SEC_TV].[dbo].[tblRegistrationSECNEW]([Code],[FirstName],[MiddleName],[LastName],[RegMobileNo],[DistrictId],[DistrictName],[TalukaId],[TalukaName],[Address],[LocalBodyName],[GroupID],[pin],[electrolDivision],[formtype]) " +
                        " values(N'" + ds.Tables[0].Rows[i]["Code"].ToString() + "',N'" + ds.Tables[0].Rows[i]["FirstName"].ToString() + "' " +
                        ",N'" + ds.Tables[0].Rows[i]["MiddleName"].ToString() + "',N'" + ds.Tables[0].Rows[i]["LastName"].ToString() + "',N'" + ds.Tables[0].Rows[i]["CandidateMob"].ToString() + "'" +
                        ",N'" + districtId + "',N'" + Districtname + "',N'" + talukaId + "',N'" + talukaName + "'" +
                       ",N'" + ds.Tables[0].Rows[i]["Address"].ToString().Replace("'", "") + "',N'" + LocalBodyName + "',N'" + Convert.ToString(ds.Tables[0].Rows[i]["GroupID"]) + "',N'" + Convert.ToString(ds.Tables[0].Rows[i]["Pin"]) + "' " +
                       ",N'" + Convert.ToString(ds.Tables[0].Rows[i]["ElectoralDivision"]) + "',N'" + Convert.ToString(ds.Tables[0].Rows[i]["Formtype"]) + "')";
                    cmd = new SqlCommand(sql, seccon);
                    seccon.Open();
                    cmd.ExecuteNonQuery();
                    seccon.Close();

                    // CommanCode cc = new CommanCode();
                    // string Msg = "SEC Welcomes " + Convert.ToString(ds.Tables[0].Rows[i]["FirstName"]) + " " + "" + Convert.ToString(ds.Tables[0].Rows[i]["MiddleName"]) + " " + "" + Convert.ToString(ds.Tables[0].Rows[i]["LastName"]) + ".  Congratulations for filing online nomination form successfully! In this election daily election expenses are to be filled online through True Voter Mobile Application. Download True Voter app on your android mobile from the link or Play Store and register it using Mobile Number used in Nomination form. goo.gl/YN7qn1";

                    //  cc.SMS(Convert.ToString(ds.Tables[0].Rows[i]["CandidateMob"]), Msg);

                }
                jsonDataStrngReturn += "1";
            }
            catch (Exception ex)
            {
                jsonDataStrngReturn += "0";
                using (StreamWriter writer = new StreamWriter(HttpContext.Current.Server.MapPath("Error.txt")))
                {
                    writer.Write(ex.Message.ToString() + "\n");
                    //writer.WriteLine("Line"); 
                }
            }
            finally
            {
                ds.Clear();
            }
            return jsonDataStrngReturn;
        }

        public string InsertRegRecordPS(string MaxId)
        {
            string file = string.Empty;
            SqlCommand cmd = new SqlCommand();
            DataSet ds1 = new DataSet();
            try
            {
                Sql = "select [LocalBodyID],[Id],[FirstName],[MiddleName],[LastName],[CandidateMob],[Address],[Code],[GroupID],[Pin],[ElectoralDivision],[Formtype] FROM  [Nomination_Reg] " +
                       "WHERE Code > '" + MaxId + "'";
                da = new SqlDataAdapter(Sql, seccon);
                da.Fill(ds);

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string sql = string.Empty;

                    Sql = "Select [LBDistrictID] from [LB_District] where [LBID]='" + Convert.ToString(ds.Tables[0].Rows[i]["LocalBodyID"]) + "'";
                    cmd = new SqlCommand(Sql, seccon);
                    seccon.Open();
                    string districtId = Convert.ToString(cmd.ExecuteScalar());
                    seccon.Close();

                    cmd = new SqlCommand("Select [Districtname] from [mstDistrict] where [Districtcode]='" + Convert.ToString(districtId) + "'", seccon);
                    seccon.Open();
                    string Districtname = Convert.ToString(cmd.ExecuteScalar());
                    seccon.Close();

                    cmd = new SqlCommand("Select [LBName] from [LB_LocalBody] where [LBID]='" + Convert.ToString(ds.Tables[0].Rows[i]["LocalBodyID"]) + "'", seccon);
                    seccon.Open();
                    string LocalBodyName = Convert.ToString(cmd.ExecuteScalar());
                    seccon.Close();

                    Sql = "Select [LBTalukaID] from [LB_Taluka] where [LBID]='" + Convert.ToString(ds.Tables[0].Rows[i]["LocalBodyID"]) + "'";
                    cmd = new SqlCommand(Sql, seccon);
                    seccon.Open();
                    string talukaId = Convert.ToString(cmd.ExecuteScalar());
                    seccon.Close();

                    sql = "Select [SubDistrictname] from [mstTaluka] where [SubDistrictcode]='" + Convert.ToString(talukaId) + "'";
                    seccon.Open();
                    string talukaName = Convert.ToString(cmd.ExecuteScalar());
                    seccon.Close();

                    sql = "Insert into [SEC_TV].[dbo].[tblRegistrationSECNEW]([CodePs],[FirstName],[MiddleName],[LastName],[RegMobileNo],[DistrictId],[DistrictName],[TalukaId],[TalukaName],[Address],[LocalBodyName],[GroupID],[pin],[electrolDivision],[formtype]) " +
                         " values(N'" + ds.Tables[0].Rows[i]["Code"] + "',N'" + ds.Tables[0].Rows[i]["FirstName"].ToString() + "' " +
                         ",N'" + ds.Tables[0].Rows[i]["MiddleName"].ToString() + "',N'" + ds.Tables[0].Rows[i]["LastName"].ToString() + "',N'" + ds.Tables[0].Rows[i]["CandidateMob"].ToString() + "'" +
                         ",N'" + districtId + "',N'" + Districtname + "',N'" + talukaId + "',N'" + talukaName + "'" +
                        ",N'" + ds.Tables[0].Rows[i]["Address"].ToString().Replace("'", "") + "',N'" + LocalBodyName + "',N'" + Convert.ToString(ds.Tables[0].Rows[i]["GroupID"]) + "',N'" + Convert.ToString(ds.Tables[0].Rows[i]["Pin"]) + "' " +
                        ",N'" + ds.Tables[0].Rows[i]["ElectoralDivision"] + "',N'" + ds.Tables[0].Rows[i]["Formtype"] + "')";
                    cmd = new SqlCommand(sql, seccon);
                    seccon.Open();
                    cmd.ExecuteNonQuery();
                    seccon.Close();

                    // CommanCode cc = new CommanCode();
                    //string Msg = "SEC Welcomes " + Convert.ToString(ds.Tables[0].Rows[i]["FirstName"]) + " " + "" + Convert.ToString(ds.Tables[0].Rows[i]["MiddleName"]) + " " + "" + Convert.ToString(ds.Tables[0].Rows[i]["LastName"]) + ".  Congratulations for filing online nomination form successfully! In this election daily election expenses are to be filled online through True Voter Mobile Application. Download True Voter app on your android mobile from the link or Play Store and register it using Mobile Number used in Nomination form. goo.gl/YN7qn1";

                    // cc.SMS(Convert.ToString(ds.Tables[0].Rows[i]["CandidateMob"]), Msg);

                }
                jsonDataStrngReturn += "1";
            }
            catch (Exception ex)
            {
                jsonDataStrngReturn += "0";
                file = string.Empty;

                using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("Error.txt")))
                {
                    file = reader.ReadToEnd();
                    //writer.WriteLine("Line"); 
                }
                using (StreamWriter writer = new StreamWriter(HttpContext.Current.Server.MapPath("Error.txt")))
                {
                    writer.Write(file + "\n" + ex.StackTrace.ToString() + "\n");
                    //writer.WriteLine("Line"); 
                }

            }
            finally
            {
                ds.Clear();
            }
            return jsonDataStrngReturn;
        }

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

                    Sql = "Select [LBDistrictID] from [LB_District] where [LBID]='" + Convert.ToString(ds.Tables[0].Rows[i]["LocalBodyID"]) + "'";
                    cmd = new SqlCommand(Sql, seccon);
                    seccon.Open();
                    string districtId = Convert.ToString(cmd.ExecuteScalar());
                    seccon.Close();

                    cmd = new SqlCommand("Select [Districtname] from [mstDistrict] where [Districtcode]='" + Convert.ToString(districtId) + "'", seccon);
                    seccon.Open();
                    string Districtname = Convert.ToString(cmd.ExecuteScalar());
                    seccon.Close();

                    Sql = "Select [LBTalukaID] from [LB_Taluka] where [LBID]='" + Convert.ToString(ds.Tables[0].Rows[i]["LocalBodyID"]) + "'";
                    cmd = new SqlCommand(Sql, seccon);
                    seccon.Open();
                    string talukaId = Convert.ToString(cmd.ExecuteScalar());
                    seccon.Close();


                    cmd = new SqlCommand("Select [LBName] from [LB_LocalBody] where [LBID]='" + Convert.ToString(ds.Tables[0].Rows[i]["LocalBodyID"]) + "'", seccon);
                    seccon.Open();
                    string LocalBodyName = Convert.ToString(cmd.ExecuteScalar());
                    seccon.Close();

                    Sql = "Select [SubDistrictname] from [mstTaluka] where [SubDistrictcode]='" + Convert.ToString(talukaId) + "'";
                    cmd = new SqlCommand(Sql, seccon);
                    seccon.Open();
                    string talukaName = Convert.ToString(cmd.ExecuteScalar());
                    seccon.Close();

                    sql = "Insert into [tblRegistrationSEC_Dtls]([RegIDSEC],[FirstName],[MiddleName],[LastName],[RegMobileNo],[DistrictId],[DistrictName],[TalukaId],[TalukaName],[ElectrolId] " +
                         ",[ElectrolName],[ElectrolClgId],[ElectrolClgName],[PartyName],[SymbolName],[ElectionName],[LocalBodyName]) values(N'" + Convert.ToString(ds.Tables[0].Rows[i]["Id"]) + "',N'" + Convert.ToString(ds.Tables[0].Rows[i]["FirstName"]) + "' " +
                         ",N'" + Convert.ToString(ds.Tables[0].Rows[i]["MiddleName"]) + "',N'" + Convert.ToString(ds.Tables[0].Rows[i]["LastName"]) + "',N'" + Convert.ToString(ds.Tables[0].Rows[i]["MobNo"]) + "' " +
                         ",N'" + Convert.ToString(districtId) + "','" + Convert.ToString(Districtname) + "',N'" + Convert.ToString(talukaId) + "',N'" + Convert.ToString(talukaName) + "' " +
                        ",N'" + Convert.ToString(ds.Tables[0].Rows[i]["EDivisionID"]) + "',N'" + Convert.ToString(ds.Tables[0].Rows[i]["EDivisionName"]) + "',N'" + Convert.ToString(ds.Tables[0].Rows[i]["ECollegeID"]) + "',N'" + Convert.ToString(ds.Tables[0].Rows[i]["ECollegeName"]) + "' " +
                      ",N'" + Convert.ToString(ds.Tables[0].Rows[i]["UserName"]) + "','" + Convert.ToString(LocalBodyName) + "')";
                    cmd = new SqlCommand(sql, sectempcon);
                    sectempcon.Open();
                    cmd.ExecuteNonQuery();
                    sectempcon.Close();

                    //  CommanCode cc = new CommanCode();
                    //  string Msg = "SEC Welcomes " + Convert.ToString(ds.Tables[0].Rows[i]["FirstName"]) + " " + "" + Convert.ToString(ds.Tables[0].Rows[i]["MiddleName"]) + " " + "" + Convert.ToString(ds.Tables[0].Rows[i]["LastName"]) + ".  Congratulations for filing online nomination form successfully! In this election daily election expenses are to be filled online through True Voter Mobile Application. Download True Voter app on your android mobile from the link or Play Store and register it using Mobile Number used in Nomination form. goo.gl/YN7qn1";

                    // cc.SMS(Convert.ToString(ds.Tables[0].Rows[i]["MobNo"]), Msg);
                }
                jsonDataStrngReturn += "1";
            }
            catch (Exception ex)
            {
                jsonDataStrngReturn += "0";

                using (StreamWriter writer = new StreamWriter(HttpContext.Current.Server.MapPath("Error.txt")))
                {
                    writer.Write(ex.Message.ToString() + "\n");
                    //writer.WriteLine("Line"); 
                }
            }
            finally
            {
                ds.Clear();
            }
            return jsonDataStrngReturn;
        }

        public List<KYCData> DownloadWardWiseCandidateData(string localbodyid, string wardid, string localbodytype, string electroldivisionNo, string electrolclgdivNo)
        {
            List<KYCData> lstkyc = new List<KYCData>();
            KYCData kyccls = new KYCData();
            DataSet DS = new DataSet();

            string sql = string.Empty;

            string file = string.Empty;
            try
            {
                if (localbodytype == "5")                //MP
                {
                    Sql = "select [Election_Data_ID] from [SEC].[dbo].[tbl_Election_Data] where EXISTS (Select Distinct([ElectionID]) From [SEC].[dbo].[tbl_Election_Data]) and [LBID]='" + localbodyid + "'";
                    cmd = new SqlCommand(Sql, seccon);
                    seccon.Open();
                    string electionDataid = Convert.ToString(cmd.ExecuteScalar());
                    seccon.Close();

                    Sql = "select [Ward_SeatID] FROM [SEC].[dbo].[WardSeatDetails] where [WardID]='" + wardid + "' and Election_Data_ID='" + electionDataid + "'";
                    cmd = new SqlCommand(Sql, seccon);
                    seccon.Open();
                    string wardID = Convert.ToString(cmd.ExecuteScalar());
                    seccon.Close();

                    sql = "Select  N.[RegistrationNo],N.[FirstName],N.[MiddleName],N.[LastName],N.[CandidateMob],N.[Address],S.[SymbolID],S.[SymbolName],S.[NameOfParty],N.[SeatID] from [SEC].[dbo].[NominationMP_Reg] AS N LEFT JOIN [SEC].[dbo].[Symbol_MasterNew] AS S ON N.[Symbols_ID]=S.[PPID]  where  [WardID]='" + wardID + "' and LocalBodyID='" + localbodyid + "' and Symbols_ID NOT IN(0) and [Aff_FinalSumbissionBy] IS NOT NULL and [Aff_FinalSumbissionCk] IS NOT NULL and [Aff_FinalSumbissionIP] IS NOT NULL";
                    da = new SqlDataAdapter(sql, seccon);
                    da.Fill(DS);

                    for (int i = 0; i < DS.Tables[0].Rows.Count; i++)
                    {
                        Sql = "Select [SeatName] FROM [SEC].[dbo].[SeatDetails] where SeatID='" + Convert.ToString(DS.Tables[0].Rows[i]["SeatID"]) + "'";
                        cmd = new SqlCommand(Sql, seccon);
                        seccon.Open();
                        string seatName = Convert.ToString(cmd.ExecuteScalar());
                        seccon.Close();

                        kyccls = new KYCData();
                        kyccls.nominationid = Convert.ToString(DS.Tables[0].Rows[i]["RegistrationNo"]);
                        kyccls.firstName = Convert.ToString(DS.Tables[0].Rows[i]["FirstName"]);
                        kyccls.middleName = Convert.ToString(DS.Tables[0].Rows[i]["MiddleName"]);
                        kyccls.lastName = Convert.ToString(DS.Tables[0].Rows[i]["LastName"]);
                        kyccls.candidayemobno = Convert.ToString(DS.Tables[0].Rows[i]["CandidateMob"]);
                        kyccls.address = Convert.ToString(DS.Tables[0].Rows[i]["Address"]);
                        kyccls.symbolID = Convert.ToString(DS.Tables[0].Rows[i]["SymbolID"]);
                        kyccls.symbolName = Convert.ToString(DS.Tables[0].Rows[i]["SymbolName"]);
                        kyccls.nameOfParty = Convert.ToString(DS.Tables[0].Rows[i]["NameOfParty"]);
                        kyccls.seatName = Convert.ToString(seatName);
                        lstkyc.Add(kyccls);
                    }

                }
                else if (localbodytype == "2")          //ZP
                {
                    Sql = "select [EDID] FROM [SEC].[dbo].[ZP_PSElectoralDivision] where ElectoralDivisionNumber='" + electroldivisionNo + "' and LBID='" + localbodyid + "'";
                    cmd = new SqlCommand(Sql, seccon);
                    seccon.Open();
                    string edid = Convert.ToString(cmd.ExecuteScalar());
                    seccon.Close();

                    sql = "Select  N.[RegistrationNo],N.[FirstName],N.[MiddleName],N.[LastName],N.[CandidateMob],N.[Address],S.[SymbolID],S.[SymbolName],S.[NameOfParty] from [SEC].[dbo].[NominationZP_Reg] AS N LEFT JOIN [SEC].[dbo].[Symbol_MasterNew] AS S ON N.[Symbols_ID]=S.[PPID]  where  LocalBodyID='" + localbodyid + "' and [ElectoralDivisionID]='" + edid + "' and Symbols_ID NOT IN(0)";
                    da = new SqlDataAdapter(sql, seccon);
                    da.Fill(DS);

                    for (int i = 0; i < DS.Tables[0].Rows.Count; i++)
                    {
                        kyccls = new KYCData();
                        kyccls.nominationid = Convert.ToString(DS.Tables[0].Rows[i]["RegistrationNo"]);
                        kyccls.firstName = Convert.ToString(DS.Tables[0].Rows[i]["FirstName"]);
                        kyccls.middleName = Convert.ToString(DS.Tables[0].Rows[i]["MiddleName"]);
                        kyccls.lastName = Convert.ToString(DS.Tables[0].Rows[i]["LastName"]);
                        kyccls.candidayemobno = Convert.ToString(DS.Tables[0].Rows[i]["CandidateMob"]);
                        kyccls.address = Convert.ToString(DS.Tables[0].Rows[i]["Address"]);
                        kyccls.symbolID = Convert.ToString(DS.Tables[0].Rows[i]["SymbolID"]);
                        kyccls.symbolName = Convert.ToString(DS.Tables[0].Rows[i]["SymbolName"]);
                        kyccls.nameOfParty = Convert.ToString(DS.Tables[0].Rows[i]["NameOfParty"]);

                        lstkyc.Add(kyccls);
                    }
                }
                else if (localbodytype == "3")                    //PS
                {
                    Sql = "select [EDID] FROM [SEC].[dbo].[ZP_PSElectoralDivision] where ElectoralDivisionNumber='" + electroldivisionNo + "' and LBID='" + localbodyid + "'";
                    cmd = new SqlCommand(Sql, seccon);
                    seccon.Open();
                    string edid = Convert.ToString(cmd.ExecuteScalar());
                    seccon.Close();

                    Sql = "select [CDID] FROM [SEC].[dbo].[ZP_PSCollegeMaster] where ElectoralDivisionNumber='" + edid + "' and LBID='" + localbodyid + "' and [ElectrolCollegeNumber]='" + electrolclgdivNo + "'";
                    cmd = new SqlCommand(Sql, seccon);
                    seccon.Open();
                    string cdid = Convert.ToString(cmd.ExecuteScalar());
                    seccon.Close();

                    sql = "Select  N.[RegistrationNo],N.[FirstName],N.[MiddleName],N.[LastName],N.[CandidateMob],N.[Address],S.[SymbolID],S.[SymbolName],S.[NameOfParty] from [SEC].[dbo].[Nomination_Reg] AS N LEFT JOIN [SEC].[dbo].[Symbol_MasterNew] AS S ON N.[Symbols_ID]=S.[PPID]  where LocalBodyID='" + localbodyid + "' and CollegeID='" + cdid + "' and Symbols_ID NOT IN(0)";
                    da = new SqlDataAdapter(sql, seccon);
                    da.Fill(DS);

                    for (int i = 0; i < DS.Tables[0].Rows.Count; i++)
                    {
                        kyccls = new KYCData();
                        kyccls.nominationid = Convert.ToString(DS.Tables[0].Rows[i]["RegistrationNo"]);
                        kyccls.firstName = Convert.ToString(DS.Tables[0].Rows[i]["FirstName"]);
                        kyccls.middleName = Convert.ToString(DS.Tables[0].Rows[i]["MiddleName"]);
                        kyccls.lastName = Convert.ToString(DS.Tables[0].Rows[i]["LastName"]);
                        kyccls.candidayemobno = Convert.ToString(DS.Tables[0].Rows[i]["CandidateMob"]);
                        kyccls.address = Convert.ToString(DS.Tables[0].Rows[i]["Address"]);
                        kyccls.symbolID = Convert.ToString(DS.Tables[0].Rows[i]["SymbolID"]);
                        kyccls.symbolName = Convert.ToString(DS.Tables[0].Rows[i]["SymbolName"]);
                        kyccls.nameOfParty = Convert.ToString(DS.Tables[0].Rows[i]["NameOfParty"]);

                        lstkyc.Add(kyccls);
                    }
                }
            }
            catch
            {
            }
            return lstkyc.ToList();
        }

        public List<KYCNominationData> KYCDetails(string NId, string localbodytype)
        {
            List<KYCNominationData> lstNmtionDtls = new List<KYCNominationData>();
            KYCNominationData nmDtls = new KYCNominationData();

            DataSet DS = new DataSet();
            DataSet DS1 = new DataSet();
            DataSet DS2 = new DataSet();
            DataSet DS3 = new DataSet();
            string SqlQry = string.Empty;
            Int32 AffID = 0;
            string Sql = string.Empty;
            string affid = string.Empty;
            try
            {
                {
                    string sql3 = "SELECT [AffID] FROM [SEC].[dbo].[aff_Summery_bk03022017] WHERE [RegisterationNo] ='" + NId + "'";
                    cmd = new SqlCommand(sql3, seccon);
                    da = new SqlDataAdapter(cmd);
                    da.Fill(DS3);
                    if (DS3.Tables[0].Rows.Count > 0)
                    {
                        AffID = Convert.ToInt32(DS3.Tables[0].Rows[0]["AffID"]);
                    }

                    if (localbodytype == "5")
                    {
                        string sql = "SELECT NMCR.[CandidateEmail],NMCR.[CandidateMob],NMCR.[Occuption],OM.[OccupationName]" +
                               "FROM [SEC].[dbo].[NominationMP_Reg] AS NMCR INNER JOIN [SEC].[dbo].[mstOccupation] AS OM ON NMCR.[Occuption]=OM.[OccupationCode] " +
                               " WHERE NMCR.[RegistrationNo] ='" + NId + "' AND OM.[LangID] = 1";
                        cmd = new SqlCommand(sql, seccon);
                        da = new SqlDataAdapter(cmd);
                        da.Fill(DS);
                    }
                    else if (localbodytype == "2")
                    {
                        string sql = "SELECT NMCR.[CandidateEmail],NMCR.[CandidateMob],NMCR.[Occuption],OM.[OccupationName]" +
                                "FROM [SEC].[dbo].[NominationZP_Reg] AS NMCR INNER JOIN [SEC].[dbo].[mstOccupation] AS OM ON NMCR.[Occuption]=OM.[OccupationCode] " +
                                " WHERE NMCR.RegistrationNo ='" + NId + "' AND OM.[LangID] = 1";
                        cmd = new SqlCommand(sql, seccon);
                        da = new SqlDataAdapter(cmd);
                        da.Fill(DS);
                    }
                    else if (localbodytype == "3")
                    {
                        string sql = "SELECT NMCR.[CandidateEmail],NMCR.[CandidateMob],NMCR.[Occuption],OM.[OccupationName]" +
                                     "FROM [SEC].[dbo].[Nomination_Reg] AS NMCR INNER JOIN [SEC].[dbo].[mstOccupation] AS OM ON NMCR.[Occuption]=OM.[OccupationCode] " +
                                     " WHERE NMCR.RegistrationNo ='" + NId + "' AND OM.[LangID] = 1";
                        cmd = new SqlCommand(sql, seccon);
                        da = new SqlDataAdapter(cmd);
                        da.Fill(DS);
                    }

                    string sql11 = "select Self,[SP],[Dep1],[Dep2],[Dep3] from [SEC].[dbo].[aff_Summery_bk03022017] where AffID='" + AffID + "' and Rank=2"; //movable
                    da = new SqlDataAdapter(sql11, seccon);
                    DataSet mvblds = new DataSet();
                    da.Fill(mvblds);

                    string sql12 = "select Self,[SP],[Dep1],[Dep2],[Dep3] from [SEC].[dbo].[aff_Summery_bk03022017] where AffID='" + AffID + "' and Rank=3";  //immvoble
                    da = new SqlDataAdapter(sql12, seccon);
                    DataSet immvblds = new DataSet();
                    da.Fill(immvblds);

                    string sql114 = "select Self,[SP],[Dep1],[Dep2],[Dep3] from [SEC].[dbo].[aff_Summery_bk03022017] where AffID='" + AffID + "' and Rank=4";  //total assests
                    da = new SqlDataAdapter(sql114, seccon);
                    DataSet tlsastsds = new DataSet();
                    da.Fill(tlsastsds);

                    string sql13 = "select Self,[SP],[Dep1],[Dep2],[Dep3] from [SEC].[dbo].[aff_Summery_bk03022017] where AffID='" + AffID + "' and Rank=5";    //liabilities
                    da = new SqlDataAdapter(sql13, seccon);
                    DataSet lblitesds = new DataSet();
                    da.Fill(lblitesds);

                    string sql4 = "select [Qualification] from [SEC].[dbo].[Aff_EducationalDetails_Submit] where AffID='" + AffID + "' ";
                    cmd = new SqlCommand(sql4, seccon);
                    seccon.Open();
                    string eductn = Convert.ToString(cmd.ExecuteScalar());
                    seccon.Close();

                    string sql5 = "select Sum(CAST(CaseNumber as int)) as NoOfCogiznce from [SEC].[dbo].[Aff_CognizanceDtls_Submit] where AffID='" + AffID + "'";
                    cmd = new SqlCommand(sql5, seccon);
                    seccon.Open();
                    Int32 cogznc = Convert.ToInt32(cmd.ExecuteScalar());
                    seccon.Close();
                    string sql6 = "select Sum(CAST(CaseNumber as int)) as NoOfConvict from [SEC].[dbo].[Aff_ConvictedDtls_Submit] where AffID='" + AffID + "'";
                    cmd = new SqlCommand(sql6, seccon);
                    seccon.Open();
                    Int32 convct = Convert.ToInt32(cmd.ExecuteScalar());
                    seccon.Close();

                    string sql14 = "select IncomeAmount from [SEC].[dbo].[Aff_Income_Submit] where AffID='" + AffID + "'";
                    da = new SqlDataAdapter(sql14, seccon);
                    DataSet incmds = new DataSet();
                    da.Fill(incmds);


                    nmDtls = new KYCNominationData();
                    nmDtls.candidatemobno = Convert.ToString(DS.Tables[0].Rows[0]["CandidateMob"]);
                    nmDtls.mailid = Convert.ToString(DS.Tables[0].Rows[0]["CandidateEmail"]);
                    nmDtls.occuptions = Convert.ToString(DS.Tables[0].Rows[0]["OccupationName"]);
                    nmDtls.movableproperty = Convert.ToString(mvblds.Tables[0].Rows[0]["Self"]);
                    nmDtls.spmovableproperty = Convert.ToString(mvblds.Tables[0].Rows[0]["SP"]);
                    nmDtls.dep1movableproperty = Convert.ToString(mvblds.Tables[0].Rows[0]["Dep1"]);
                    nmDtls.dep2movableproperty = Convert.ToString(mvblds.Tables[0].Rows[0]["Dep2"]);
                    nmDtls.dep3movableproperty = Convert.ToString(mvblds.Tables[0].Rows[0]["Dep3"]);
                    nmDtls.immovableproperty = Convert.ToString(immvblds.Tables[0].Rows[0]["Self"]);
                    nmDtls.immovableproperty = Convert.ToString(immvblds.Tables[0].Rows[0]["SP"]);
                    nmDtls.immovableproperty = Convert.ToString(immvblds.Tables[0].Rows[0]["Dep1"]);
                    nmDtls.immovableproperty = Convert.ToString(immvblds.Tables[0].Rows[0]["Dep2"]);
                    nmDtls.immovableproperty = Convert.ToString(immvblds.Tables[0].Rows[0]["Dep3"]);
                    nmDtls.liabilities = Convert.ToString(lblitesds.Tables[0].Rows[0]["Self"]);
                    nmDtls.spliabilities = Convert.ToString(lblitesds.Tables[0].Rows[0]["SP"]);
                    nmDtls.dep1liabilities = Convert.ToString(lblitesds.Tables[0].Rows[0]["Dep1"]);
                    nmDtls.dep2liabilities = Convert.ToString(lblitesds.Tables[0].Rows[0]["Dep2"]);
                    nmDtls.dep3liabilities = Convert.ToString(lblitesds.Tables[0].Rows[0]["Dep3"]);
                    nmDtls.income = Convert.ToString(incmds.Tables[0].Rows[0]["IncomeAmount"]);
                    nmDtls.totalassets = Convert.ToString(tlsastsds.Tables[0].Rows[0]["Self"]);
                    nmDtls.sptotalassets = Convert.ToString(tlsastsds.Tables[0].Rows[0]["SP"]);
                    nmDtls.dep1totalassets = Convert.ToString(tlsastsds.Tables[0].Rows[0]["Dep1"]);
                    nmDtls.dep2totalassets = Convert.ToString(tlsastsds.Tables[0].Rows[0]["Dep2"]);
                    nmDtls.dep3totalassets = Convert.ToString(tlsastsds.Tables[0].Rows[0]["Dep3"]);
                    nmDtls.education = Convert.ToString(eductn);
                    nmDtls.cognizances = Convert.ToString(cogznc);
                    nmDtls.convicted = Convert.ToString(convct);
                    nmDtls.totalcases = Convert.ToInt32(0);
                    lstNmtionDtls.Add(nmDtls);
                }
            }
            catch
            {

            }

            return lstNmtionDtls.ToList(); //xmlData;
        }

        public List<KYCNominationDataNEW> KYCDetailsNEW(string NId, string localbodytype)
        {
            List<KYCNominationDataNEW> lstNmtionDtls = new List<KYCNominationDataNEW>();
            KYCNominationDataNEW nmDtls = new KYCNominationDataNEW();

            DataSet DS = new DataSet();
            DataSet DS1 = new DataSet();
            DataSet DS2 = new DataSet();
            DataSet DS3 = new DataSet();
            string SqlQry = string.Empty;
            string Sql = string.Empty;
            string affid = string.Empty;
            string file = string.Empty;
            try
            {
                {
                    string sql3 = "SELECT [AffID] FROM [SEC].[dbo].[Aff_CandidateDetails_Submit] WHERE [RegisterationNum1] ='" + NId + "'";
                    cmd = new SqlCommand(sql3, seccon);
                    seccon.Open();
                    affid = Convert.ToString(cmd.ExecuteScalar());
                    seccon.Close();

                    if (affid == "")
                    {
                        SqlQry = "select AffID  FROM [SEC].[dbo].[Aff_Summery_Submit] where [RegisterationNo]='" + NId + "'";
                        cmd = new SqlCommand(SqlQry, seccon);
                        seccon.Open();
                        affid = Convert.ToString(cmd.ExecuteScalar());
                        seccon.Close();
                    }

                    if (localbodytype == "5")
                    {
                        string sql = "SELECT NMCR.[CandidateEmail],NMCR.[CandidateMob],NMCR.[Occuption],OM.[OccupationName]" +
                               "FROM [SEC].[dbo].[NominationMP_Reg] AS NMCR INNER JOIN [SEC].[dbo].[mstOccupation] AS OM ON NMCR.[Occuption]=OM.[OccupationCode] " +
                               " WHERE NMCR.[RegistrationNo] ='" + NId + "' AND OM.[LangID] = 1";
                        cmd = new SqlCommand(sql, seccon);
                        da = new SqlDataAdapter(cmd);
                        da.Fill(DS);
                    }
                    else if (localbodytype == "2")
                    {
                        string sql = "SELECT NMCR.[CandidateEmail],NMCR.[CandidateMob],NMCR.[Occuption],OM.[OccupationName]" +
                                "FROM [SEC].[dbo].[NominationZP_Reg] AS NMCR INNER JOIN [SEC].[dbo].[mstOccupation] AS OM ON NMCR.[Occuption]=OM.[OccupationCode] " +
                                " WHERE NMCR.RegistrationNo ='" + NId + "' AND OM.[LangID] = 1";
                        cmd = new SqlCommand(sql, seccon);
                        da = new SqlDataAdapter(cmd);
                        da.Fill(DS);
                    }
                    else if (localbodytype == "3")
                    {
                        string sql = "SELECT NMCR.[CandidateEmail],NMCR.[CandidateMob],NMCR.[Occuption],OM.[OccupationName]" +
                                     "FROM [SEC].[dbo].[Nomination_Reg] AS NMCR INNER JOIN [SEC].[dbo].[mstOccupation] AS OM ON NMCR.[Occuption]=OM.[OccupationCode] " +
                                     " WHERE NMCR.RegistrationNo ='" + NId + "' AND OM.[LangID] = 1";
                        cmd = new SqlCommand(sql, seccon);
                        da = new SqlDataAdapter(cmd);
                        da.Fill(DS);
                    }
                    string sql77 = string.Empty;
                    if (affid == "")
                    {
                        sql77 = "SELECT [Education],[Cog],[Con],[Income],[Mov],[Immov],[TotalMovImmov],[Loan],[PreEleName],[PreEleYear] FROM [SEC].[dbo].[Aff_AllSummery_2] WHERE AffID NOT IN(0) and AffID=0 ";
                    }
                    else
                    {
                        sql77 = "SELECT [Education],[Cog],[Con],[Income],[Mov],[Immov],[TotalMovImmov],[Loan],[PreEleName],[PreEleYear] FROM [SEC].[dbo].[Aff_AllSummery_2] WHERE AffID='" + affid + "'";
                    }
                    da = new SqlDataAdapter(sql77, seccon);
                    DataSet smryds = new DataSet();
                    da.Fill(smryds);

                    nmDtls = new KYCNominationDataNEW();
                    nmDtls.candidatemobno = Convert.ToString(DS.Tables[0].Rows[0]["CandidateMob"]);
                    nmDtls.mailid = Convert.ToString(DS.Tables[0].Rows[0]["CandidateEmail"]);
                    nmDtls.occuptions = Convert.ToString(DS.Tables[0].Rows[0]["OccupationName"]);
                    nmDtls.movableproperty = Convert.ToString(smryds.Tables[0].Rows[0]["Mov"]);
                    nmDtls.immovableproperty = Convert.ToString(smryds.Tables[0].Rows[0]["Immov"]);
                    nmDtls.previouselectionstatus = Convert.ToString(smryds.Tables[0].Rows[0]["PreEleName"]);
                    nmDtls.previouselectionyear = Convert.ToString(smryds.Tables[0].Rows[0]["PreEleYear"]);
                    nmDtls.liabilities = Convert.ToString(smryds.Tables[0].Rows[0]["Loan"]);
                    nmDtls.income = Convert.ToString(smryds.Tables[0].Rows[0]["Income"]);
                    nmDtls.totalassets = Convert.ToString(smryds.Tables[0].Rows[0]["TotalMovImmov"]);
                    nmDtls.education = Convert.ToString(smryds.Tables[0].Rows[0]["Education"]);
                    nmDtls.cognizances = Convert.ToString(smryds.Tables[0].Rows[0]["Cog"]);
                    nmDtls.convicted = Convert.ToString(smryds.Tables[0].Rows[0]["Con"]);

                    nmDtls.totalcases = Convert.ToInt32(0);
                    lstNmtionDtls.Add(nmDtls);
                }
            }
            catch
            {

            }
            return lstNmtionDtls.ToList();
        }

        public string KYCDetailsNEWString(string NId, string localbodytype)
        {
            List<KYCNominationDataNEW> lstNmtionDtls = new List<KYCNominationDataNEW>();
            KYCNominationDataNEW nmDtls = new KYCNominationDataNEW();

            DataSet DS = new DataSet();
            DataSet DS1 = new DataSet();
            DataSet DS2 = new DataSet();
            DataSet DS3 = new DataSet();
            string SqlQry = string.Empty;
            Int32 AffID = 0;
            string Sql = string.Empty;
            string affid = string.Empty;
            string file = string.Empty;
            try
            {
                {
                    string sql3 = "SELECT [AffID] FROM [SEC].[dbo].[aff_Summery_bk03022017] WHERE [RegisterationNo] ='" + NId + "'";
                    cmd = new SqlCommand(sql3, seccon);
                    seccon.Open();
                    affid = Convert.ToString(cmd.ExecuteScalar());
                    seccon.Close();

                    if (localbodytype == "5")
                    {
                        string sql = "SELECT NMCR.[CandidateEmail],NMCR.[CandidateMob],NMCR.[Occuption],OM.[OccupationName]" +
                               "FROM [SEC].[dbo].[NominationMP_Reg] AS NMCR INNER JOIN [SEC].[dbo].[mstOccupation] AS OM ON NMCR.[Occuption]=OM.[OccupationCode] " +
                               " WHERE NMCR.[RegistrationNo] ='" + NId + "' AND OM.[LangID] = 1";
                        cmd = new SqlCommand(sql, seccon);
                        da = new SqlDataAdapter(cmd);
                        da.Fill(DS);
                    }
                    else if (localbodytype == "2")
                    {
                        string sql = "SELECT NMCR.[CandidateEmail],NMCR.[CandidateMob],NMCR.[Occuption],OM.[OccupationName]" +
                                "FROM [SEC].[dbo].[NominationZP_Reg] AS NMCR INNER JOIN [SEC].[dbo].[mstOccupation] AS OM ON NMCR.[Occuption]=OM.[OccupationCode] " +
                                " WHERE NMCR.RegistrationNo ='" + NId + "' AND OM.[LangID] = 1";
                        cmd = new SqlCommand(sql, seccon);
                        da = new SqlDataAdapter(cmd);
                        da.Fill(DS);
                    }
                    else if (localbodytype == "3")
                    {
                        string sql = "SELECT NMCR.[CandidateEmail],NMCR.[CandidateMob],NMCR.[Occuption],OM.[OccupationName]" +
                                     "FROM [SEC].[dbo].[Nomination_Reg] AS NMCR INNER JOIN [SEC].[dbo].[mstOccupation] AS OM ON NMCR.[Occuption]=OM.[OccupationCode] " +
                                     " WHERE NMCR.RegistrationNo ='" + NId + "' AND OM.[LangID] = 1";
                        cmd = new SqlCommand(sql, seccon);
                        da = new SqlDataAdapter(cmd);
                        da.Fill(DS);
                    }
                    string sql11 = "select Sum((CAST(self as float)) + (CAST(SP as float)) + (CAST(Dep1 as float)) + (CAST(Dep2 as float)) + (CAST(Dep3 as float))) as movableproperty FROM [SEC].[dbo].[aff_Summery_bk03022017] where AffID='" + affid + "' and Rank=2"; //movable
                    cmd = new SqlCommand(sql11, seccon);
                    seccon.Open();
                    string mvble = Convert.ToString(cmd.ExecuteScalar());
                    seccon.Close();

                    string sql12 = "select Sum((CAST(self as float)) + (CAST(SP as float)) + (CAST(Dep1 as float)) + (CAST(Dep2 as float)) + (CAST(Dep3 as float))) as immovableproperty FROM [SEC].[dbo].[aff_Summery_bk03022017] where AffID='" + affid + "' and Rank=3";  //immvoble
                    cmd = new SqlCommand(sql12, seccon);
                    seccon.Open();
                    string immvble = Convert.ToString(cmd.ExecuteScalar());
                    seccon.Close();

                    string sql114 = "select Sum((CAST(self as float)) + (CAST(SP as float)) + (CAST(Dep1 as float)) + (CAST(Dep2 as float)) + (CAST(Dep3 as float))) as totalassets FROM [SEC].[dbo].[aff_Summery_bk03022017] where AffID='" + affid + "' and Rank=4";  //total assests
                    cmd = new SqlCommand(sql114, seccon);
                    seccon.Open();
                    string totalassts = Convert.ToString(cmd.ExecuteScalar());
                    seccon.Close();

                    string sql13 = "select Sum((CAST(self as float)) + (CAST(SP as float)) + (CAST(Dep1 as float)) + (CAST(Dep2 as float)) + (CAST(Dep3 as float))) as liabilities FROM [SEC].[dbo].[aff_Summery_bk03022017] where AffID='" + affid + "' and Rank=5";    //liabilities
                    cmd = new SqlCommand(sql13, seccon);
                    seccon.Open();
                    string liabilities = Convert.ToString(cmd.ExecuteScalar());
                    seccon.Close();

                    string sql4 = "select [Qualification] from [SEC].[dbo].[Aff_EducationalDetails_Submit] where AffID='" + AffID + "'"; //Education
                    cmd = new SqlCommand(sql4, seccon);
                    seccon.Open();
                    string eductn = Convert.ToString(cmd.ExecuteScalar());
                    seccon.Close();

                    string sql5 = "select Congnizance as congniznce  FROM [SEC].[dbo].[Aff_CandidateDetails_Submit] where AffID='" + affid + "'"; //Congnizance
                    cmd = new SqlCommand(sql5, seccon);
                    seccon.Open();
                    string cogznc = Convert.ToString(cmd.ExecuteScalar());
                    seccon.Close();

                    string sql6 = "select Convicted  as Convict  FROM [SEC].[dbo].[Aff_CandidateDetails_Submit] where AffID='" + affid + "'"; //Convicted
                    cmd = new SqlCommand(sql6, seccon);
                    seccon.Open();
                    string convct = Convert.ToString(cmd.ExecuteScalar());
                    seccon.Close();

                    string sql14 = "select Sum((CAST(self as float)) + (CAST(SP as float)) + (CAST(Dep1 as float)) + (CAST(Dep2 as float)) + (CAST(Dep3 as float)))  as Income FROM [SEC].[dbo].[aff_Summery_bk03022017] where AffID='" + affid + "' and Rank=1";  //FamilyIncome
                    cmd = new SqlCommand(sql6, seccon);
                    seccon.Open();
                    string incm = Convert.ToString(cmd.ExecuteScalar());
                    seccon.Close();

                    string sql111 = "select [NameOfElection] as PreviousElectionCount FROM [SEC].[dbo].[Aff_PreviousElections_Submit] where AffID='" + affid + "'"; //previousElectionName
                    cmd = new SqlCommand(sql111, seccon);
                    seccon.Open();
                    string prvsname = Convert.ToString(cmd.ExecuteScalar());
                    seccon.Close();

                    string sql1111 = "select [YearOfElection] as previousElectionYear FROM [SEC].[dbo].[Aff_PreviousElections_Submit] where AffID='" + affid + "'"; //PreviousElectionYear
                    cmd = new SqlCommand(sql1111, seccon);
                    seccon.Open();
                    string prvsyear = Convert.ToString(cmd.ExecuteScalar());
                    seccon.Close();

                    nmDtls = new KYCNominationDataNEW();
                    nmDtls.candidatemobno = Convert.ToString(DS.Tables[0].Rows[0]["CandidateMob"]);
                    nmDtls.mailid = Convert.ToString(DS.Tables[0].Rows[0]["CandidateEmail"]);
                    nmDtls.occuptions = Convert.ToString(DS.Tables[0].Rows[0]["OccupationName"]);
                    nmDtls.movableproperty = Convert.ToString(mvble);
                    nmDtls.immovableproperty = Convert.ToString(immvble);
                    nmDtls.previouselectionstatus = Convert.ToString(prvsname);
                    nmDtls.previouselectionyear = Convert.ToString(prvsyear);
                    nmDtls.liabilities = Convert.ToString(liabilities);
                    nmDtls.income = Convert.ToString(incm);
                    nmDtls.totalassets = Convert.ToString(totalassts);
                    nmDtls.education = Convert.ToString(eductn);
                    nmDtls.cognizances = Convert.ToString(cogznc);
                    nmDtls.convicted = Convert.ToString(convct);
                    nmDtls.totalcases = Convert.ToInt32(0);
                    lstNmtionDtls.Add(nmDtls);

                    jsonDataStrngReturn = JsonConvert.SerializeObject(lstNmtionDtls, theJsonSerializerSettings);
                }
            }
            catch
            {
                jsonDataStrngReturn += "0";
            }
            return jsonDataStrngReturn.ToString();
        }

        public string KYCDetailsString(string NId, string localbodytype)
        {
            List<KYCNominationData> lstNmtionDtls = new List<KYCNominationData>();
            KYCNominationData nmDtls = new KYCNominationData();

            DataSet DS = new DataSet();
            DataSet DS1 = new DataSet();
            DataSet DS2 = new DataSet();
            DataSet DS3 = new DataSet();
            string SqlQry = string.Empty;
            Int32 AffID = 0;
            string incomeAmount = string.Empty;
            string spouseOccupationName = string.Empty;
            string qualification = string.Empty;
            string dep1mvble = string.Empty;
            string dep2mvble = string.Empty;
            string dep3mvble = string.Empty;
            string spmvble = string.Empty;
            string Address = string.Empty;
            string file = string.Empty;
            string Sql = string.Empty;

            try
            {
                {
                    string sql3 = "SELECT [AffID] FROM [SEC].[dbo].[aff_Summery_bk03022017] WHERE [RegisterationNo] = '" + NId + "'";
                    cmd = new SqlCommand(sql3, seccon);
                    da = new SqlDataAdapter(cmd);
                    da.Fill(DS3);
                    if (DS3.Tables[0].Rows.Count > 0)
                    {
                        AffID = Convert.ToInt32(DS3.Tables[0].Rows[0]["AffID"]);
                    }

                    if (localbodytype == "5")
                    {
                        string sql = "SELECT NMCR.[CandidateEmail],NMCR.[CandidateMob],NMCR.[Occuption],OM.[OccupationName]" +
                               "FROM [SEC].[dbo].[NominationMP_Reg] AS NMCR INNER JOIN [SEC].[dbo].[mstOccupation] AS OM ON NMCR.[Occuption]=OM.[OccupationCode] " +
                               " WHERE NMCR.RegistrationNo ='" + NId + "' AND OM.[LangID] = 1";
                        cmd = new SqlCommand(sql, seccon);
                        da = new SqlDataAdapter(cmd);
                        da.Fill(DS);

                    }
                    else if (localbodytype == "2")
                    {
                        string sql = "SELECT NMCR.[CandidateEmail],NMCR.[CandidateMob],NMCR.[Occuption],OM.[OccupationName]" +
                                "FROM [SEC].[dbo].[NominationZP_Reg] AS NMCR INNER JOIN [SEC].[dbo].[mstOccupation] AS OM ON NMCR.[Occuption]=OM.[OccupationCode] " +
                                " WHERE NMCR.RegistrationNo ='" + NId + "' AND OM.[LangID] = 1";
                        cmd = new SqlCommand(sql, seccon);
                        da = new SqlDataAdapter(cmd);
                        da.Fill(DS);

                    }
                    else if (localbodytype == "3")
                    {
                        string sql = "SELECT NMCR.[CandidateEmail],NMCR.[CandidateMob],NMCR.[Occuption],OM.[OccupationName]" +
                                     "FROM [SEC].[dbo].[Nomination_Reg] AS NMCR INNER JOIN [SEC].[dbo].[mstOccupation] AS OM ON NMCR.[Occuption]=OM.[OccupationCode] " +
                                     " WHERE NMCR.RegistrationNo ='" + NId + "' AND OM.[LangID] = 1";
                        cmd = new SqlCommand(sql, seccon);
                        da = new SqlDataAdapter(cmd);
                        da.Fill(DS);
                    }

                    string sql11 = "select Self,[SP],[Dep1],[Dep2],[Dep3] from [SEC].[dbo].[aff_Summery_bk03022017] where AffID='" + AffID + "' and Rank=2"; //movable
                    da = new SqlDataAdapter(sql11, seccon);
                    DataSet mvblds = new DataSet();
                    da.Fill(mvblds);

                    string sql12 = "select Self,[SP],[Dep1],[Dep2],[Dep3] from [SEC].[dbo].[aff_Summery_bk03022017] where AffID='" + AffID + "' and Rank=3";  //immvoble
                    da = new SqlDataAdapter(sql12, seccon);
                    DataSet immvblds = new DataSet();
                    da.Fill(immvblds);

                    string sql114 = "select Self,[SP],[Dep1],[Dep2],[Dep3] from [SEC].[dbo].[aff_Summery_bk03022017] where AffID='" + AffID + "' and Rank=4";  //total assests
                    da = new SqlDataAdapter(sql114, seccon);
                    DataSet tlsastsds = new DataSet();
                    da.Fill(tlsastsds);

                    string sql13 = "select Self,[SP],[Dep1],[Dep2],[Dep3] from [SEC].[dbo].[aff_Summery_bk03022017] where AffID='" + AffID + "' and Rank=5";    //liabilities
                    da = new SqlDataAdapter(sql13, seccon);
                    DataSet lblitesds = new DataSet();
                    da.Fill(lblitesds);

                    string sql4 = "select [Qualification] from [SEC].[dbo].[Aff_EducationalDetails_Submit] where AffID='" + AffID + "' ";
                    cmd = new SqlCommand(sql4, seccon);
                    seccon.Open();
                    string eductn = Convert.ToString(cmd.ExecuteScalar());
                    seccon.Close();
                    string sql5 = "select Sum(CAST(CaseNumber as int)) as NoOfCogiznce from [SEC].[dbo].[Aff_CognizanceDtls_Submit] where AffID='" + AffID + "'";
                    cmd = new SqlCommand(sql5, seccon);
                    seccon.Open();
                    Int32 cogznc = Convert.ToInt32(cmd.ExecuteScalar());
                    seccon.Close();
                    string sql6 = "select Sum(CAST(CaseNumber as int)) as NoOfConvict from [SEC].[dbo].[Aff_ConvictedDtls_Submit] where AffID='" + AffID + "'";
                    cmd = new SqlCommand(sql6, seccon);
                    seccon.Open();
                    Int32 convct = Convert.ToInt32(cmd.ExecuteScalar());
                    seccon.Close();


                    string sql14 = "select IncomeAmount from [SEC].[dbo].[Aff_Income_Submit] where AffID='" + AffID + "'";
                    da = new SqlDataAdapter(sql14, seccon);
                    DataSet incmds = new DataSet();
                    da.Fill(incmds);

                    nmDtls = new KYCNominationData();
                    nmDtls.candidatemobno = Convert.ToString(DS.Tables[0].Rows[0]["CandidateMob"]);
                    nmDtls.mailid = Convert.ToString(DS.Tables[0].Rows[0]["CandidateEmail"]);
                    nmDtls.occuptions = Convert.ToString(DS.Tables[0].Rows[0]["OccupationName"]);
                    nmDtls.movableproperty = Convert.ToString(mvblds.Tables[0].Rows[0]["Self"]);
                    nmDtls.spmovableproperty = Convert.ToString(mvblds.Tables[0].Rows[0]["SP"]);
                    nmDtls.dep1movableproperty = Convert.ToString(mvblds.Tables[0].Rows[0]["Dep1"]);
                    nmDtls.dep2movableproperty = Convert.ToString(mvblds.Tables[0].Rows[0]["Dep2"]);
                    nmDtls.dep3movableproperty = Convert.ToString(mvblds.Tables[0].Rows[0]["Dep3"]);
                    nmDtls.immovableproperty = Convert.ToString(immvblds.Tables[0].Rows[0]["Self"]);
                    nmDtls.immovableproperty = Convert.ToString(immvblds.Tables[0].Rows[0]["SP"]);
                    nmDtls.immovableproperty = Convert.ToString(immvblds.Tables[0].Rows[0]["Dep1"]);
                    nmDtls.immovableproperty = Convert.ToString(immvblds.Tables[0].Rows[0]["Dep2"]);
                    nmDtls.immovableproperty = Convert.ToString(immvblds.Tables[0].Rows[0]["Dep3"]);
                    nmDtls.liabilities = Convert.ToString(lblitesds.Tables[0].Rows[0]["Self"]);
                    nmDtls.spliabilities = Convert.ToString(lblitesds.Tables[0].Rows[0]["SP"]);
                    nmDtls.dep1liabilities = Convert.ToString(lblitesds.Tables[0].Rows[0]["Dep1"]);
                    nmDtls.dep2liabilities = Convert.ToString(lblitesds.Tables[0].Rows[0]["Dep2"]);
                    nmDtls.dep3liabilities = Convert.ToString(lblitesds.Tables[0].Rows[0]["Dep3"]);
                    nmDtls.income = Convert.ToString(incmds.Tables[0].Rows[0]["IncomeAmount"]);
                    nmDtls.totalassets = Convert.ToString(tlsastsds.Tables[0].Rows[0]["Self"]);
                    nmDtls.sptotalassets = Convert.ToString(tlsastsds.Tables[0].Rows[0]["SP"]);
                    nmDtls.dep1totalassets = Convert.ToString(tlsastsds.Tables[0].Rows[0]["Dep1"]);
                    nmDtls.dep2totalassets = Convert.ToString(tlsastsds.Tables[0].Rows[0]["Dep2"]);
                    nmDtls.dep3totalassets = Convert.ToString(tlsastsds.Tables[0].Rows[0]["Dep3"]);
                    nmDtls.education = Convert.ToString(eductn);
                    nmDtls.cognizances = Convert.ToString(cogznc);
                    nmDtls.convicted = Convert.ToString(convct);
                    nmDtls.totalcases = Convert.ToInt32(0);
                    lstNmtionDtls.Add(nmDtls);

                    jsonDataStrngReturn = JsonConvert.SerializeObject(lstNmtionDtls, theJsonSerializerSettings);
                }
            }
            catch
            {
                jsonDataStrngReturn += "0";
            }

            return jsonDataStrngReturn.ToString();
        }

        public List<CandidateResult> DownloadCandidateResult(string LBID, string wardno, string localbodytype, string electrolDivNo, string electrolClgNo)
        {
            string cuntrslt = string.Empty; string electionstr = string.Empty;
            string wardstr = string.Empty; string cuntstr = string.Empty; string seatstr = string.Empty;
            string cuntdaystr = string.Empty; string seatName = string.Empty;
            List<CandidateResult> lstcndrslt = new List<CandidateResult>();
            CandidateResult cndobj = new CandidateResult();
            try
            {
                if (localbodytype == "5")
                {
                    Sql = "select [Election_Data_ID] from [SEC].[dbo].[tbl_Election_Data] where [LBID]='" + LBID + "'";
                    da = new SqlDataAdapter(Sql, seccon);
                    DataSet ds1 = new DataSet();
                    da.Fill(ds1);

                    Sql = "select [Ward_SeatID] FROM [SEC].[dbo].[WardSeatDetails] where [WardID]='" + wardno + "' and [Election_Data_ID]='" + ds1.Tables[0].Rows[0]["Election_Data_ID"].ToString() + "'";// IN(" + electionstr + ")";
                    da = new SqlDataAdapter(Sql, seccon);
                    DataSet ds2 = new DataSet();
                    da.Fill(ds2);

                    Sql = "select [CountDayID],[SeatID] FROM [SEC].[dbo].[CountingDayProcess] WHERE [LBID]='" + LBID + "' AND [WardID] ='" + ds2.Tables[0].Rows[0]["Ward_SeatID"].ToString() + "'";        //IN(" + wardstr + ")";
                    da = new SqlDataAdapter(Sql, seccon);
                    DataSet ds3 = new DataSet();
                    da.Fill(ds3);
                    for (int c = 0; c < ds3.Tables[0].Rows.Count; c++)
                    {
                        Sql = "select [SeatName] FROM [SEC].[dbo].[SeatDetails] WHERE [SeatID]='" + Convert.ToString(ds3.Tables[0].Rows[c]["SeatID"]) + "'";
                        cmd = new SqlCommand(Sql, seccon);
                        seccon.Open();
                        seatName = Convert.ToString(cmd.ExecuteScalar());
                        seccon.Close();

                        Sql = "select [CountDayID] FROM [SEC].[dbo].[CountingDayProcess] WHERE [CountDayID] ='" + Convert.ToString(ds3.Tables[0].Rows[c]["CountDayID"]) + "'  AND [SeatID]='" + Convert.ToString(ds3.Tables[0].Rows[c]["SeatID"]) + "' ";    //IN(" + cuntstr + ") IN (" + seatstr + ")
                        da = new SqlDataAdapter(Sql, seccon);
                        DataSet ds4 = new DataSet();
                        da.Fill(ds4);

                        Sql = "select [LnameCandidate],[FnameCandidate],[MnameCandidate],[Partyname],[TotVotesSecured],[IsWinner] FROM [SEC].[dbo].[CountingDayProcessRpt] WHERE [CountDayID] ='" + Convert.ToString(ds4.Tables[0].Rows[0]["CountDayID"]) + "'";    //IN (" + cuntdaystr + ")";
                        da = new SqlDataAdapter(Sql, seccon);
                        DataSet DS = new DataSet();
                        da.Fill(DS);

                        for (int j = 0; j < DS.Tables[0].Rows.Count; j++)
                        {
                            cndobj = new CandidateResult();
                            cndobj.candidateLName = DS.Tables[0].Rows[j]["LnameCandidate"].ToString() != null ? DS.Tables[0].Rows[j]["LnameCandidate"].ToString() : "NA";
                            cndobj.candidateFName = DS.Tables[0].Rows[j]["FnameCandidate"].ToString() != null ? DS.Tables[0].Rows[j]["FnameCandidate"].ToString() : "NA";
                            cndobj.candidateMName = DS.Tables[0].Rows[j]["MnameCandidate"].ToString() != null ? DS.Tables[0].Rows[j]["MnameCandidate"].ToString() : "NA";
                            cndobj.partyname = DS.Tables[0].Rows[j]["Partyname"].ToString() != null ? DS.Tables[0].Rows[j]["Partyname"].ToString() : "NA";
                            cndobj.totalvote = DS.Tables[0].Rows[j]["TotVotesSecured"].ToString() != null ? DS.Tables[0].Rows[j]["TotVotesSecured"].ToString() : "NA";
                            cndobj.iswinner = Convert.ToString(DS.Tables[0].Rows[j]["IsWinner"] != null ? DS.Tables[0].Rows[j]["IsWinner"] : "NA");
                            cndobj.wardno = Convert.ToString(wardno);
                            cndobj.section = Convert.ToString(seatName);
                            cndobj.localbodyid = Convert.ToString(LBID);
                            lstcndrslt.Add(cndobj);
                        }
                    }
                }
                else if (localbodytype == "3")
                {
                    string sql4 = "select CDID FROM [SEC].[dbo].[ZP_PSCollegeMaster] where [ElectrolCollegeNumber]='" + electrolClgNo + "' and LBID='" + LBID + "'";
                    cmd = new SqlCommand(sql4, seccon);
                    seccon.Open();
                    string cdid = Convert.ToString(cmd.ExecuteScalar());
                    seccon.Close();

                    Sql = "select [CountDayID] FROM [SEC].[dbo].[CountingDayProcess] WHERE [LBID]='" + LBID + "' AND [CollegeID]='" + cdid + "'";
                    da = new SqlDataAdapter(Sql, seccon);
                    DataSet ds3 = new DataSet();
                    da.Fill(ds3);
                    for (int c = 0; c < ds3.Tables[0].Rows.Count; c++)
                    {
                        cuntstr = cuntstr + "," + Convert.ToString(ds3.Tables[0].Rows[c]["CountDayID"]);
                    }
                    cuntstr = cuntstr.Substring(1);

                    for (int i = 0; i < ds3.Tables[0].Rows.Count; i++)
                    {
                        Sql = "select [LnameCandidate],[FnameCandidate],[MnameCandidate],[Partyname],[TotVotesSecured],[IsWinner] FROM [SEC].[dbo].[CountingDayProcessRpt] WHERE [CountDayID] IN (" + cuntstr + ")";
                        da = new SqlDataAdapter(Sql, seccon);
                        DataSet DS = new DataSet();
                        da.Fill(DS);

                        for (int j = 0; j < DS.Tables[0].Rows.Count; j++)
                        {
                            cndobj = new CandidateResult();
                            cndobj.candidateLName = DS.Tables[0].Rows[j]["LnameCandidate"].ToString() != null ? DS.Tables[0].Rows[j]["LnameCandidate"].ToString() : "NA";
                            cndobj.candidateFName = DS.Tables[0].Rows[j]["FnameCandidate"].ToString() != null ? DS.Tables[0].Rows[j]["FnameCandidate"].ToString() : "NA";
                            cndobj.candidateMName = DS.Tables[0].Rows[j]["MnameCandidate"].ToString() != null ? DS.Tables[0].Rows[j]["MnameCandidate"].ToString() : "NA";
                            cndobj.partyname = DS.Tables[0].Rows[j]["Partyname"].ToString() != null ? DS.Tables[0].Rows[j]["Partyname"].ToString() : "NA";
                            cndobj.totalvote = DS.Tables[0].Rows[j]["TotVotesSecured"].ToString() != null ? DS.Tables[0].Rows[j]["TotVotesSecured"].ToString() : "NA";
                            cndobj.iswinner = Convert.ToString(DS.Tables[0].Rows[0]["IsWinner"] != null ? DS.Tables[0].Rows[j]["IsWinner"] : "NA");
                            cndobj.wardno = Convert.ToString(wardno);
                            cndobj.section = "NA";
                            cndobj.localbodyid = Convert.ToString(LBID);
                            lstcndrslt.Add(cndobj);
                        }
                    }
                }
                else if (localbodytype == "2")
                {
                    string sql4 = "select EDID FROM [SEC].[dbo].[ZP_PSElectoralDivision] where ElectoralDivisionNumber='" + electrolDivNo + "' and LBID='" + LBID + "'";
                    cmd = new SqlCommand(sql4, seccon);
                    seccon.Open();
                    string edid = Convert.ToString(cmd.ExecuteScalar());
                    seccon.Close();


                    Sql = "select [CountDayID] FROM [SEC].[dbo].[CountingDayProcess] WHERE [LBID]='" + LBID + "' AND [ElectoralDivisionID]='" + edid + "'";
                    da = new SqlDataAdapter(Sql, seccon);
                    DataSet ds3 = new DataSet();
                    da.Fill(ds3);
                    for (int c = 0; c < ds3.Tables[0].Rows.Count; c++)
                    {
                        cuntstr = cuntstr + "," + Convert.ToString(ds3.Tables[0].Rows[c]["CountDayID"]);
                    }
                    cuntstr = cuntstr.Substring(1);

                    for (int i = 0; i < ds3.Tables[0].Rows.Count; i++)
                    {
                        Sql = "select [LnameCandidate],[FnameCandidate],[MnameCandidate],[Partyname],[TotVotesSecured],[IsWinner] FROM [SEC].[dbo].[CountingDayProcessRpt] WHERE [CountDayID] IN (" + cuntstr + ")";
                        da = new SqlDataAdapter(Sql, seccon);
                        DataSet DS = new DataSet();
                        da.Fill(DS);

                        for (int j = 0; j < DS.Tables[0].Rows.Count; j++)
                        {
                            cndobj = new CandidateResult();
                            cndobj.candidateLName = DS.Tables[0].Rows[j]["LnameCandidate"].ToString() != null ? DS.Tables[0].Rows[j]["LnameCandidate"].ToString() : "NA";
                            cndobj.candidateFName = DS.Tables[0].Rows[j]["FnameCandidate"].ToString() != null ? DS.Tables[0].Rows[j]["FnameCandidate"].ToString() : "NA";
                            cndobj.candidateMName = DS.Tables[0].Rows[j]["MnameCandidate"].ToString() != null ? DS.Tables[0].Rows[j]["MnameCandidate"].ToString() : "NA";
                            cndobj.partyname = DS.Tables[0].Rows[j]["Partyname"].ToString() != null ? DS.Tables[0].Rows[j]["Partyname"].ToString() : "NA";
                            cndobj.totalvote = DS.Tables[0].Rows[j]["TotVotesSecured"].ToString() != null ? DS.Tables[0].Rows[j]["TotVotesSecured"].ToString() : "NA";
                            cndobj.iswinner = Convert.ToString(DS.Tables[0].Rows[0]["IsWinner"] != null ? DS.Tables[0].Rows[j]["IsWinner"] : "NA");
                            cndobj.wardno = Convert.ToString(wardno);
                            cndobj.section = "NA";
                            cndobj.localbodyid = Convert.ToString(LBID);
                            lstcndrslt.Add(cndobj);
                        }
                    }
                }
            }
            catch
            {

            }
            return lstcndrslt.ToList();
        }

        public List<AgeWiseDistrictData> AgeWiseReoprt(string districtid, string localbodytype)
        {
            Int32 agecunt1to20; string lbid_str = string.Empty;
            Int32 agecunt21to30; Int32 agecunt31to40; Int32 agecunt41to50;
            Int32 agecunt51to60; Int32 agecunt61to70; Int32 agecunt71to80;
            Int32 agecunt81to90; Int32 totalcount;
            List<AgeWiseDistrictData> lstagewise = new List<AgeWiseDistrictData>();
            AgeWiseDistrictData agewise = new AgeWiseDistrictData();
            try
            {
                Sql = "select [LBID] from [SEC].[dbo].[LB_District] where [LBDistrictID]='" + districtid + "'";
                da = new SqlDataAdapter(Sql, seccon);
                DataSet ds1 = new DataSet();
                da.Fill(ds1);
                for (int a = 0; a < ds1.Tables[0].Rows.Count; a++)
                {
                    lbid_str = lbid_str + "," + Convert.ToString(ds1.Tables[0].Rows[a]["LBID"]);
                }
                lbid_str = lbid_str.Substring(1);

                if (localbodytype == "3")
                {
                    Sql = "select Count(*) as TotalCount from [SEC].[dbo].[Nomination_Reg] where [Symbols_ID] Not in(0) and [Subchk]=1 and [Withdrawal_status] is null and LocalBodyId In(" + lbid_str + ")";
                    cmd = new SqlCommand(Sql, seccon);
                    seccon.Open();
                    totalcount = Convert.ToInt32(cmd.ExecuteScalar());
                    seccon.Close();

                    Sql = "select Count(*) as AgeCount1To20 from [SEC].[dbo].[Nomination_Reg] where (Age Between 1 and 20) and [Symbols_ID] Not in(0) and [Subchk]=1 and [Withdrawal_status] is null and LocalBodyId In(" + lbid_str + ")";
                    cmd = new SqlCommand(Sql, seccon);
                    seccon.Open();
                    agecunt1to20 = Convert.ToInt32(cmd.ExecuteScalar());
                    seccon.Close();

                    Sql = "select Count(*) as AgeCount21To30 from [SEC].[dbo].[Nomination_Reg] where (Age Between 21 and 30) and [Symbols_ID] Not in(0) and [Subchk]=1 and [Withdrawal_status] is null and LocalBodyId In(" + lbid_str + ")";
                    cmd = new SqlCommand(Sql, seccon);
                    seccon.Open();
                    agecunt21to30 = Convert.ToInt32(cmd.ExecuteScalar());
                    seccon.Close();

                    Sql = "select Count(*) as AgeCount31To40 from [SEC].[dbo].[Nomination_Reg] where (Age Between 31 and 40) and [Symbols_ID] Not in(0) and [Subchk]=1 and [Withdrawal_status] is null and LocalBodyId In(" + lbid_str + ")";
                    cmd = new SqlCommand(Sql, seccon);
                    seccon.Open();
                    agecunt31to40 = Convert.ToInt32(cmd.ExecuteScalar());
                    seccon.Close();

                    float perage31to40 = (agecunt31to40 / totalcount) * 100;

                    Sql = "select Count(*) as AgeCount41To50 from [SEC].[dbo].[Nomination_Reg] where (Age Between 41 and 50) and [Symbols_ID] Not in(0) and [Subchk]=1 and [Withdrawal_status] is null and LocalBodyId In(" + lbid_str + ")";
                    cmd = new SqlCommand(Sql, seccon);
                    seccon.Open();
                    agecunt41to50 = Convert.ToInt32(cmd.ExecuteScalar());
                    seccon.Close();

                    Sql = "select Count(*) as AgeCount51To60 from [SEC].[dbo].[Nomination_Reg] where (Age Between 51 and 60) and [Symbols_ID] Not in(0) and [Subchk]=1 and [Withdrawal_status] is null and LocalBodyId In(" + lbid_str + ")";
                    cmd = new SqlCommand(Sql, seccon);
                    seccon.Open();
                    agecunt51to60 = Convert.ToInt32(cmd.ExecuteScalar());
                    seccon.Close();

                    Sql = "select Count(*) as AgeCount61To70 from [SEC].[dbo].[Nomination_Reg] where (Age Between 61 and 70) and [Symbols_ID] Not in(0) and [Subchk]=1 and [Withdrawal_status] is null and LocalBodyId In(" + lbid_str + ")";
                    cmd = new SqlCommand(Sql, seccon);
                    seccon.Open();
                    agecunt61to70 = Convert.ToInt32(cmd.ExecuteScalar());
                    seccon.Close();

                    Sql = "select Count(*) as AgeCount71To80 from [SEC].[dbo].[Nomination_Reg] where (Age Between 71 and 80) and [Symbols_ID] Not in(0) and [Subchk]=1 and [Withdrawal_status] is null and LocalBodyId In(" + lbid_str + ")";
                    cmd = new SqlCommand(Sql, seccon);
                    seccon.Open();
                    agecunt71to80 = Convert.ToInt32(cmd.ExecuteScalar());
                    seccon.Close();

                    Sql = "select Count(*) as AgeCount81To90 from [SEC].[dbo].[Nomination_Reg] where (Age Between 81 and 90) and [Symbols_ID] Not in(0) and [Subchk]=1 and [Withdrawal_status] is null and LocalBodyId In(" + lbid_str + ")";
                    cmd = new SqlCommand(Sql, seccon);
                    seccon.Open();
                    agecunt81to90 = Convert.ToInt32(cmd.ExecuteScalar());
                    seccon.Close();

                    agewise = new AgeWiseDistrictData();
                    agewise.AGECUNT1TO20 = Convert.ToInt32(agecunt1to20);
                    agewise.AGECUNT21TO30 = Convert.ToInt32(agecunt21to30);
                    agewise.AGECUNT31TO40 = Convert.ToInt32(agecunt31to40);
                    agewise.AGECUNT41TO50 = Convert.ToInt32(agecunt41to50);
                    agewise.AGECUNT51TO60 = Convert.ToInt32(agecunt51to60);
                    agewise.AGECUNT61TO70 = Convert.ToInt32(agecunt61to70);
                    agewise.AGECUNT71TO80 = Convert.ToInt32(agecunt71to80);
                    agewise.AGECUNT81TO90 = Convert.ToInt32(agecunt81to90);
                    lstagewise.Add(agewise);
                }
                else if (localbodytype == "5")
                {
                    Sql = "select Count(*) as TotalCount from [SEC].[dbo].[NominationMP_Reg] where [Symbols_ID] Not in(0) and [Subchk]=1 and [Withdrawal_status] is null and LocalBodyId In(" + lbid_str + ")";
                    cmd = new SqlCommand(Sql, seccon);
                    seccon.Open();
                    totalcount = Convert.ToInt32(cmd.ExecuteScalar());
                    seccon.Close();

                    Sql = "select Count(*) as AgeCount1To20 from [SEC].[dbo].[NominationMP_Reg] where (Age Between 1 and 20) and [Symbols_ID] Not in(0) and [Subchk]=1 and [Withdrawal_status] is null and LocalBodyId In(" + lbid_str + ")";
                    cmd = new SqlCommand(Sql, seccon);
                    seccon.Open();
                    agecunt1to20 = Convert.ToInt32(cmd.ExecuteScalar());
                    seccon.Close();

                    float perage1to20 = (agecunt1to20 / totalcount) * 100;

                    Sql = "select Count(*) as AgeCount21To30 from [SEC].[dbo].[NominationMP_Reg] where (Age Between 21 and 30) and [Symbols_ID] Not in(0) and [Subchk]=1 and [Withdrawal_status] is null and LocalBodyId In(" + lbid_str + ")";
                    cmd = new SqlCommand(Sql, seccon);
                    seccon.Open();
                    agecunt21to30 = Convert.ToInt32(cmd.ExecuteScalar());
                    seccon.Close();
                    float perage21to30 = (agecunt1to20 / totalcount) * 100;

                    Sql = "select Count(*) as AgeCount31To40 from [SEC].[dbo].[NominationMP_Reg] where (Age Between 21 and 30) and [Symbols_ID] Not in(0) and [Subchk]=1 and [Withdrawal_status] is null and LocalBodyId In(" + lbid_str + ")";
                    cmd = new SqlCommand(Sql, seccon);
                    seccon.Open();
                    agecunt31to40 = Convert.ToInt32(cmd.ExecuteScalar());
                    seccon.Close();
                    float perage31to40 = (agecunt1to20 / totalcount) * 100;

                    Sql = "select Count(*) as AgeCount41To50 from [SEC].[dbo].[NominationMP_Reg] where (Age Between 21 and 30) and [Symbols_ID] Not in(0) and [Subchk]=1 and [Withdrawal_status] is null and LocalBodyId In(" + lbid_str + ")";
                    cmd = new SqlCommand(Sql, seccon);
                    seccon.Open();
                    agecunt41to50 = Convert.ToInt32(cmd.ExecuteScalar());
                    seccon.Close();
                    float perage41to50 = (agecunt1to20 / totalcount) * 100;

                    Sql = "select Count(*) as AgeCount51To60 from [SEC].[dbo].[NominationMP_Reg] where (Age Between 21 and 30) and [Symbols_ID] Not in(0) and [Subchk]=1 and [Withdrawal_status] is null and LocalBodyId In(" + lbid_str + ")";
                    cmd = new SqlCommand(Sql, seccon);
                    seccon.Open();
                    agecunt51to60 = Convert.ToInt32(cmd.ExecuteScalar());
                    seccon.Close();
                    float perage51to60 = (agecunt1to20 / totalcount) * 100;

                    Sql = "select Count(*) as AgeCount61To70 from [SEC].[dbo].[NominationMP_Reg] where (Age Between 21 and 30) and [Symbols_ID] Not in(0) and [Subchk]=1 and [Withdrawal_status] is null and LocalBodyId In(" + lbid_str + ")";
                    cmd = new SqlCommand(Sql, seccon);
                    seccon.Open();
                    agecunt61to70 = Convert.ToInt32(cmd.ExecuteScalar());
                    seccon.Close();
                    float perage61to70 = (agecunt1to20 / totalcount) * 100;

                    Sql = "select Count(*) as AgeCount71To80 from [SEC].[dbo].[NominationMP_Reg] where (Age Between 21 and 30) and [Symbols_ID] Not in(0) and [Subchk]=1 and [Withdrawal_status] is null and LocalBodyId In(" + lbid_str + ")";
                    cmd = new SqlCommand(Sql, seccon);
                    seccon.Open();
                    agecunt71to80 = Convert.ToInt32(cmd.ExecuteScalar());
                    seccon.Close();
                    float perage71to80 = (agecunt1to20 / totalcount) * 100;


                    Sql = "select Count(*) as AgeCount81To90 from [SEC].[dbo].[NominationMP_Reg] where (Age Between 21 and 30) and [Symbols_ID] Not in(0) and [Subchk]=1 and [Withdrawal_status] is null and LocalBodyId In(" + lbid_str + ")";
                    cmd = new SqlCommand(Sql, seccon);
                    seccon.Open();
                    agecunt81to90 = Convert.ToInt32(cmd.ExecuteScalar());
                    seccon.Close();
                    float perage81to90 = (agecunt1to20 / totalcount) * 100;

                    agewise = new AgeWiseDistrictData();
                    agewise.AGECUNT1TO20 = Convert.ToInt32(agecunt1to20);
                    agewise.AGECUNT21TO30 = Convert.ToInt32(agecunt21to30);
                    agewise.AGECUNT31TO40 = Convert.ToInt32(agecunt31to40);
                    agewise.AGECUNT41TO50 = Convert.ToInt32(agecunt41to50);
                    agewise.AGECUNT51TO60 = Convert.ToInt32(agecunt51to60);
                    agewise.AGECUNT61TO70 = Convert.ToInt32(agecunt61to70);
                    agewise.AGECUNT71TO80 = Convert.ToInt32(agecunt71to80);
                    agewise.AGECUNT81TO90 = Convert.ToInt32(agecunt81to90);
                    lstagewise.Add(agewise);
                }
                else
                {
                    Sql = "select Count(*) as TotalCount from [SEC].[dbo].[NominationMP_Reg] where [Symbols_ID] Not in(0) and [Subchk]=1 and [Withdrawal_status] is null and LocalBodyId In(" + lbid_str + ")";
                    cmd = new SqlCommand(Sql, seccon);
                    seccon.Open();
                    totalcount = Convert.ToInt32(cmd.ExecuteScalar());
                    seccon.Close();

                    Sql = "select Count(*) as AgeCount1To20 from [SEC].[dbo].[NominationZP_Reg] where (Age Between 1 and 20) and [Symbols_ID] Not in(0) and [Subchk]=1 and [Withdrawal_status] is null and LocalBodyId In(" + lbid_str + ")";
                    cmd = new SqlCommand(Sql, seccon);
                    seccon.Open();
                    agecunt1to20 = Convert.ToInt32(cmd.ExecuteScalar());
                    seccon.Close();
                    float perage1to20 = (agecunt1to20 / totalcount) * 100;

                    Sql = "select Count(*) as AgeCount21To30 from [SEC].[dbo].[NominationZP_Reg] where (Age Between 21 and 30) and [Symbols_ID] Not in(0) and [Subchk]=1 and [Withdrawal_status] is null and LocalBodyId In(" + lbid_str + ")";
                    cmd = new SqlCommand(Sql, seccon);
                    seccon.Open();
                    agecunt21to30 = Convert.ToInt32(cmd.ExecuteScalar());
                    seccon.Close();
                    float perage21to30 = (agecunt21to30 / totalcount) * 100;

                    Sql = "select Count(*) as AgeCount31To40 from [SEC].[dbo].[NominationZP_Reg] where (Age Between 21 and 30) and [Symbols_ID] Not in(0) and [Subchk]=1 and [Withdrawal_status] is null and LocalBodyId In(" + lbid_str + ")";
                    cmd = new SqlCommand(Sql, seccon);
                    seccon.Open();
                    agecunt31to40 = Convert.ToInt32(cmd.ExecuteScalar());
                    seccon.Close();
                    float perage31to40 = (agecunt31to40 / totalcount) * 100;

                    Sql = "select Count(*) as AgeCount41To50 from [SEC].[dbo].[NominationZP_Reg] where (Age Between 21 and 30) and [Symbols_ID] Not in(0) and [Subchk]=1 and [Withdrawal_status] is null and LocalBodyId In(" + lbid_str + ")";
                    cmd = new SqlCommand(Sql, seccon);
                    seccon.Open();
                    agecunt41to50 = Convert.ToInt32(cmd.ExecuteScalar());
                    seccon.Close();
                    float perage41to50 = (agecunt41to50 / totalcount) * 100;

                    Sql = "select Count(*) as AgeCount51To60 from [SEC].[dbo].[NominationZP_Reg] where (Age Between 21 and 30) and [Symbols_ID] Not in(0) and [Subchk]=1 and [Withdrawal_status] is null and LocalBodyId In(" + lbid_str + ")";
                    cmd = new SqlCommand(Sql, seccon);
                    seccon.Open();
                    agecunt51to60 = Convert.ToInt32(cmd.ExecuteScalar());
                    seccon.Close();
                    float perage51to60 = (agecunt51to60 / totalcount) * 100;

                    Sql = "select Count(*) as AgeCount61To70 from [SEC].[dbo].[NominationZP_Reg] where (Age Between 21 and 30) and [Symbols_ID] Not in(0) and [Subchk]=1 and [Withdrawal_status] is null and LocalBodyId In(" + lbid_str + ")";
                    cmd = new SqlCommand(Sql, seccon);
                    seccon.Open();
                    agecunt61to70 = Convert.ToInt32(cmd.ExecuteScalar());
                    seccon.Close();
                    float perage61to70 = (agecunt61to70 / totalcount) * 100;

                    Sql = "select Count(*) as AgeCount71To80 from [SEC].[dbo].[NominationZP_Reg] where (Age Between 21 and 30) and [Symbols_ID] Not in(0) and [Subchk]=1 and [Withdrawal_status] is null and LocalBodyId In(" + lbid_str + ")";
                    cmd = new SqlCommand(Sql, seccon);
                    seccon.Open();
                    agecunt71to80 = Convert.ToInt32(cmd.ExecuteScalar());
                    seccon.Close();
                    float perage71to80 = (agecunt71to80 / totalcount) * 100;


                    Sql = "select Count(*) as AgeCount81To90 from [SEC].[dbo].[NominationZP_Reg] where (Age Between 21 and 30) and [Symbols_ID] Not in(0) and [Subchk]=1 and [Withdrawal_status] is null and LocalBodyId In(" + lbid_str + ")";
                    cmd = new SqlCommand(Sql, seccon);
                    seccon.Open();
                    agecunt81to90 = Convert.ToInt32(cmd.ExecuteScalar());
                    seccon.Close();
                    float perage81to90 = (agecunt81to90 / totalcount) * 100;

                    agewise = new AgeWiseDistrictData();
                    agewise.AGECUNT1TO20 = Convert.ToInt32(agecunt1to20);
                    agewise.AGECUNT21TO30 = Convert.ToInt32(agecunt21to30);
                    agewise.AGECUNT31TO40 = Convert.ToInt32(agecunt31to40);
                    agewise.AGECUNT41TO50 = Convert.ToInt32(agecunt41to50);
                    agewise.AGECUNT51TO60 = Convert.ToInt32(agecunt51to60);
                    agewise.AGECUNT61TO70 = Convert.ToInt32(agecunt61to70);
                    agewise.AGECUNT71TO80 = Convert.ToInt32(agecunt71to80);
                    agewise.AGECUNT81TO90 = Convert.ToInt32(agecunt81to90);
                    lstagewise.Add(agewise);
                }
            }
            catch
            {

            }
            return lstagewise.ToList();
        }

        public List<Assests> HighAssetsReoprt(string districtid, string localbodytype)
        {
            string Nomid = string.Empty; string lbid_str = string.Empty;
            List<Assests> lstAssts = new List<Assests>();
            Assests assts = new Assests();
            try
            {
                Sql = "select [LBID] from [SEC].[dbo].[LB_District] where [LBDistrictID]='" + districtid + "'";
                da = new SqlDataAdapter(Sql, seccon);
                DataSet ds1 = new DataSet();
                da.Fill(ds1);
                for (int a = 0; a < ds1.Tables[0].Rows.Count; a++)
                {
                    lbid_str = lbid_str + "," + Convert.ToString(ds1.Tables[0].Rows[a]["LBID"]);
                }
                lbid_str = lbid_str.Substring(1);

                if (localbodytype == "5")
                {
                    Sql = "select top 10 Name,[Mov],[Immov],[TotalMovImmov],[AffID] FROM [SEC].[dbo].[Aff_AllSummery_2] where LBID in(" + lbid_str + ") and LBType='MP' order by CONVERT(money,[TotalMovImmov]) Desc";
                    da = new SqlDataAdapter(Sql, seccon);
                    da.Fill(ds);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        string sql = "  select [RegisterationNum1]   FROM [SEC].[dbo].[Aff_CandidateDetails_Submit] where AffID='" + Convert.ToString(ds.Tables[0].Rows[i]["AffID"]) + "'";
                        cmd = new SqlCommand(sql, seccon);
                        seccon.Open();
                        string regid = Convert.ToString(cmd.ExecuteScalar());
                        seccon.Close();

                        string sql11 = "  select [NameofPartyID] FROM [SEC].[dbo].[NominationMP_Reg] where [RegistrationNo]='" + Convert.ToString(regid) + "'";
                        cmd = new SqlCommand(sql11, seccon);
                        seccon.Open();
                        string partyid = Convert.ToString(cmd.ExecuteScalar());
                        seccon.Close();

                        string sql17 = "  select [NameOfParty] FROM [SEC].[dbo].[Symbol_MasterNew] where [PPID]='" + Convert.ToString(partyid) + "'";
                        cmd = new SqlCommand(sql17, seccon);
                        seccon.Open();
                        string NameofParty = Convert.ToString(cmd.ExecuteScalar());
                        seccon.Close();


                        assts = new Assests();
                        assts.Name1 = Convert.ToString(ds.Tables[0].Rows[i]["Name"]);
                        assts.partyName = Convert.ToString(NameofParty);
                        assts.movableAssts1 = Convert.ToString(ds.Tables[0].Rows[i]["Mov"]);
                        assts.immovableAssts1 = Convert.ToString(ds.Tables[0].Rows[i]["Immov"]);
                        assts.totalassts1 = Convert.ToString(ds.Tables[0].Rows[i]["TotalMovImmov"]);
                        lstAssts.Add(assts);
                    }
                }
                else if (localbodytype == "3")
                {
                    Sql = "select top 10 Name,[Mov],[Immov],[TotalMovImmov],[AffID] FROM [SEC].[dbo].[Aff_AllSummery_2] where LBID in(" + lbid_str + ") and LBType='PS' order by CONVERT(money,[TotalMovImmov]) Desc";
                    da = new SqlDataAdapter(Sql, seccon);
                    da.Fill(ds);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        string sql = "  select [RegisterationNum1]   FROM [SEC].[dbo].[Aff_CandidateDetails_Submit] where AffID='" + Convert.ToString(ds.Tables[0].Rows[i]["AffID"]) + "'";
                        cmd = new SqlCommand(sql, seccon);
                        seccon.Open();
                        string regid = Convert.ToString(cmd.ExecuteScalar());
                        seccon.Close();

                        string sql11 = "  select [NameofPartyID] FROM [SEC].[dbo].[Nomination_Reg] where [RegistrationNo]='" + Convert.ToString(regid) + "'";
                        cmd = new SqlCommand(sql11, seccon);
                        seccon.Open();
                        string partyid = Convert.ToString(cmd.ExecuteScalar());
                        seccon.Close();

                        string sql17 = "  select [NameOfParty] FROM [SEC].[dbo].[Symbol_MasterNew] where [PPID]='" + Convert.ToString(partyid) + "'";
                        cmd = new SqlCommand(sql17, seccon);
                        seccon.Open();
                        string NameofParty = Convert.ToString(cmd.ExecuteScalar());
                        seccon.Close();


                        assts = new Assests();
                        assts.Name1 = Convert.ToString(ds.Tables[0].Rows[i]["Name"]);
                        assts.partyName = Convert.ToString(NameofParty);
                        assts.movableAssts1 = Convert.ToString(ds.Tables[0].Rows[i]["Mov"]);
                        assts.immovableAssts1 = Convert.ToString(ds.Tables[0].Rows[i]["Immov"]);
                        assts.totalassts1 = Convert.ToString(ds.Tables[0].Rows[i]["TotalMovImmov"]);
                        lstAssts.Add(assts);
                    }
                }
                else if (localbodytype == "2")
                {
                    Sql = "select top 10 Name,[Mov],[Immov],[TotalMovImmov],[AffID] FROM [SEC].[dbo].[Aff_AllSummery_2] where LBID in(" + lbid_str + ") and LBType='ZP' order by CONVERT(money,[TotalMovImmov]) Desc";
                    da = new SqlDataAdapter(Sql, seccon);
                    da.Fill(ds);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        string sql = "  select [RegisterationNum1] FROM [SEC].[dbo].[Aff_CandidateDetails_Submit] where AffID='" + Convert.ToString(ds.Tables[0].Rows[i]["AffID"]) + "'";
                        cmd = new SqlCommand(sql, seccon);
                        seccon.Open();
                        string regid = Convert.ToString(cmd.ExecuteScalar());
                        seccon.Close();

                        string sql11 = "  select [NameofPartyID] FROM [SEC].[dbo].[NominationZP_Reg] where [RegistrationNo]='" + Convert.ToString(regid) + "'";
                        cmd = new SqlCommand(sql11, seccon);
                        seccon.Open();
                        string partyid = Convert.ToString(cmd.ExecuteScalar());
                        seccon.Close();

                        string sql17 = "  select [NameOfParty] FROM [SEC].[dbo].[Symbol_MasterNew] where [PPID]='" + Convert.ToString(partyid) + "'";
                        cmd = new SqlCommand(sql17, seccon);
                        seccon.Open();
                        string NameofParty = Convert.ToString(cmd.ExecuteScalar());
                        seccon.Close();


                        assts = new Assests();
                        assts.Name1 = Convert.ToString(ds.Tables[0].Rows[i]["Name"]);
                        assts.partyName = Convert.ToString(NameofParty);
                        assts.movableAssts1 = Convert.ToString(ds.Tables[0].Rows[i]["Mov"]);
                        assts.immovableAssts1 = Convert.ToString(ds.Tables[0].Rows[i]["Immov"]);
                        assts.totalassts1 = Convert.ToString(ds.Tables[0].Rows[i]["TotalMovImmov"]);
                        lstAssts.Add(assts);
                    }
                }


            }
            catch
            {

            }
            return lstAssts.ToList();
        }

        public List<Gender> GenderWiseReoprt(string districtid, string localbodytype)
        {
            string lbid_str = string.Empty;
            List<Gender> lstGender = new List<Gender>();
            Gender gender = new Gender();
            try
            {
                Sql = "select [LBID] from [SEC].[dbo].[LB_District] where [LBDistrictID]='" + districtid + "'";
                da = new SqlDataAdapter(Sql, seccon);
                DataSet ds1 = new DataSet();
                da.Fill(ds1);
                for (int a = 0; a < ds1.Tables[0].Rows.Count; a++)
                {
                    lbid_str = lbid_str + "," + Convert.ToString(ds1.Tables[0].Rows[a]["LBID"]);
                }
                lbid_str = lbid_str.Substring(1);

                if (localbodytype == "5")
                {
                    Sql = " select COUNT(*) as GTotalCount  FROM [SEC].[dbo].[NominationMP_Reg]  where Symbols_ID not in(0)  and Withdrawal_status is null and Subchk=1 and LocalBodyID in (" + lbid_str + ")";
                    cmd = new SqlCommand(Sql, seccon);
                    seccon.Open();
                    double gtotalcunt = Convert.ToInt32(cmd.ExecuteScalar());
                    seccon.Close();

                    string sql = "select COUNT(*) MtotalCount  FROM [SEC].[dbo].[NominationMP_Reg]  where [Gender]=1000 and Symbols_ID not in(0)  and Withdrawal_status is null and Subchk=1 and LocalBodyID in (" + lbid_str + ")";
                    cmd = new SqlCommand(sql, seccon);
                    seccon.Open();
                    double mtotalcunt = Convert.ToInt32(cmd.ExecuteScalar());
                    seccon.Close();

                    double mpercntage = ((double)mtotalcunt / (double)gtotalcunt);

                    double mpercunt = Math.Round((double)mpercntage * 100);

                    string sql11 = "select COUNT(*) FtotalCount  FROM [SEC].[dbo].[NominationMP_Reg]  where [Gender]=1001 and Symbols_ID not in(0)  and Withdrawal_status is null and Subchk=1 and LocalBodyID in (" + lbid_str + ")";
                    cmd = new SqlCommand(sql11, seccon);
                    seccon.Open();
                    double ftotalcunt = Convert.ToInt32(cmd.ExecuteScalar());
                    seccon.Close();

                    double fpercntage = ((double)ftotalcunt / (double)gtotalcunt);

                    double fpercunt = Math.Round((double)fpercntage * 100);


                    gender = new Gender();
                    gender.gTotalCount = Convert.ToString(gtotalcunt);
                    gender.mTotalCount = Convert.ToString(mtotalcunt);
                    gender.fTotalCount = Convert.ToString(ftotalcunt);
                    gender.mpercentage = Convert.ToString(mpercunt);
                    gender.fpercentage = Convert.ToString(fpercunt);
                    gender.districtid = Convert.ToString(districtid);
                    gender.localbodytype = Convert.ToString(localbodytype);
                    lstGender.Add(gender);
                }
                else if (localbodytype == "3")
                {
                    Sql = " select COUNT(*) as GTotalCount  FROM [SEC].[dbo].[Nomination_Reg]  where Symbols_ID not in(0)  and Withdrawal_status is null and Subchk=1 and LocalBodyID in (" + lbid_str + ")";
                    cmd = new SqlCommand(Sql, seccon);
                    seccon.Open();
                    int gtotalcunt = Convert.ToInt32(cmd.ExecuteScalar());
                    seccon.Close();

                    string sql = "select COUNT(*) MtotalCount  FROM [SEC].[dbo].[Nomination_Reg]  where [Gender]=1000 and Symbols_ID not in(0)  and Withdrawal_status is null and Subchk=1 and LocalBodyID in (" + lbid_str + ")";
                    cmd = new SqlCommand(sql, seccon);
                    seccon.Open();
                    int mtotalcunt = Convert.ToInt32(cmd.ExecuteScalar());
                    seccon.Close();

                    double mpercntage = ((double)mtotalcunt / (double)gtotalcunt);

                    double mpercunt = Math.Round((double)mpercntage * 100);

                    string sql11 = "select COUNT(*) FtotalCount  FROM [SEC].[dbo].[Nomination_Reg]  where [Gender]=1001 and Symbols_ID not in(0)  and Withdrawal_status is null and Subchk=1 and LocalBodyID in (" + lbid_str + ")";
                    cmd = new SqlCommand(sql11, seccon);
                    seccon.Open();
                    int ftotalcunt = Convert.ToInt32(cmd.ExecuteScalar());
                    seccon.Close();

                    double fpercntage = ((double)ftotalcunt / (double)gtotalcunt);

                    double fpercunt = Math.Round((double)fpercntage * 100);


                    gender = new Gender();
                    gender.gTotalCount = Convert.ToString(gtotalcunt);
                    gender.mTotalCount = Convert.ToString(mtotalcunt);
                    gender.fTotalCount = Convert.ToString(ftotalcunt);
                    gender.mpercentage = Convert.ToString(mpercunt);
                    gender.fpercentage = Convert.ToString(fpercunt);
                    gender.districtid = Convert.ToString(districtid);
                    gender.localbodytype = Convert.ToString(localbodytype);
                    lstGender.Add(gender);
                }
                else if (localbodytype == "2")
                {
                    Sql = " select COUNT(*) as GTotalCount  FROM [SEC].[dbo].[NominationZP_Reg]  where Symbols_ID not in(0)  and Withdrawal_status is null and Subchk=1 and LocalBodyID in (" + lbid_str + ")";
                    cmd = new SqlCommand(Sql, seccon);
                    seccon.Open();
                    int gtotalcunt = Convert.ToInt32(cmd.ExecuteScalar());
                    seccon.Close();

                    string sql = "select COUNT(*) MtotalCount  FROM [SEC].[dbo].[NominationZP_Reg]  where [Gender]=1000 and Symbols_ID not in(0)  and Withdrawal_status is null and Subchk=1 and LocalBodyID in (" + lbid_str + ")";
                    cmd = new SqlCommand(sql, seccon);
                    seccon.Open();
                    int mtotalcunt = Convert.ToInt32(cmd.ExecuteScalar());
                    seccon.Close();

                    double mpercntage = ((double)mtotalcunt / (double)gtotalcunt);

                    double mpercunt = Math.Round((double)mpercntage * 100);

                    string sql11 = "select COUNT(*) FtotalCount  FROM [SEC].[dbo].[NominationZP_Reg]  where [Gender]=1001 and Symbols_ID not in(0)  and Withdrawal_status is null and Subchk=1 and LocalBodyID in (" + lbid_str + ")";
                    cmd = new SqlCommand(sql11, seccon);
                    seccon.Open();
                    int ftotalcunt = Convert.ToInt32(cmd.ExecuteScalar());
                    seccon.Close();

                    double fpercntage = ((double)ftotalcunt / (double)gtotalcunt);

                    double fpercunt = Math.Round((double)fpercntage * 100);

                    gender = new Gender();
                    gender.gTotalCount = Convert.ToString(gtotalcunt);
                    gender.mTotalCount = Convert.ToString(mtotalcunt);
                    gender.fTotalCount = Convert.ToString(ftotalcunt);
                    gender.mpercentage = Convert.ToString(mpercunt);
                    gender.fpercentage = Convert.ToString(fpercunt);
                    gender.districtid = Convert.ToString(districtid);
                    gender.localbodytype = Convert.ToString(localbodytype);
                    lstGender.Add(gender);
                }
            }
            catch
            {

            }
            return lstGender.ToList();
        }

        //public List<Gender> PartyWiseCountReports(string districtid, string localbodytype, string nameofpartyid)
        //{
        //    string lbid_str = string.Empty; string seatid = string.Empty;
        //    List<Gender> lstGender = new List<Gender>();
        //    Gender gender = new Gender();
        //    try
        //    {
        //        Sql = "select [LBID] from [SEC].[dbo].[LB_District] where [LBDistrictID]='" + districtid + "'";
        //        da = new SqlDataAdapter(Sql, seccon);
        //        DataSet ds1 = new DataSet();
        //        da.Fill(ds1);
        //        for (int a = 0; a < ds1.Tables[0].Rows.Count; a++)
        //        {
        //            lbid_str = lbid_str + "," + Convert.ToString(ds1.Tables[0].Rows[a]["LBID"]);
        //        }
        //        lbid_str = lbid_str.Substring(1);

        //        if (localbodytype == "5")
        //        {
        //            Sql = " select [SeatID] FROM [SEC].[dbo].[NominationMP_Reg] where Symbols_ID not in(0) and Withdrawal_status is null and Subchk=1 and LocalBodyID in (" + lbid_str + ") and NameofPartyID='" + nameofpartyid + "'";
        //            da = new SqlDataAdapter(Sql, seccon);
        //            DataSet ds11 = new DataSet();
        //            da.Fill(ds11);
        //            double totalCount = Convert.ToInt32(ds11.Tables[0].Rows.Count);
        //            for (int b = 0; b < ds11.Tables[0].Rows.Count; b++)
        //            {
        //                seatid = seatid + "," + Convert.ToString(ds11.Tables[0].Rows[b]["SeatID"]);
        //            }
        //            seatid = seatid.Substring(1);

        //            string sql = "select  COUNT(*) as totCount  FROM [SEC].[dbo].[Aff_AllSummery_2] where [LBID] IN("+ lbid_str +") and CONVERT(money,[TotalMovImmov]) > '100000000' and [SeatID] IN("+ seatid +")";
        //            cmd = new SqlCommand(sql, seccon);
        //            seccon.Open();
        //            double mtotalcunt = Convert.ToInt32(cmd.ExecuteScalar());
        //            seccon.Close();

        //            double mpercntage = ((double)mtotalcunt / (double)totalCount);

        //            double mpercunt = Math.Round((double)mpercntage * 100);

        //            gender = new Gender();
        //            gender.gTotalCount = Convert.ToString(totalCount);
        //            gender.mTotalCount = Convert.ToString(mtotalcunt);
        //            gender.mpercentage = Convert.ToString(mpercunt);
        //            gender.districtid = Convert.ToString(districtid);
        //            gender.localbodytype = Convert.ToString(localbodytype);
        //            lstGender.Add(gender);
        //        }
        //        else if (localbodytype == "3")  //ps
        //        {
        //            Sql = " select COUNT(*) as GTotalCount  FROM [SEC].[dbo].[Nomination_Reg]  where Symbols_ID not in(0)  and Withdrawal_status is null and Subchk=1 and LocalBodyID in (" + lbid_str + ")";
        //            cmd = new SqlCommand(Sql, seccon);
        //            seccon.Open();
        //            int gtotalcunt = Convert.ToInt32(cmd.ExecuteScalar());
        //            seccon.Close();

        //            string sql = "select COUNT(*) MtotalCount  FROM [SEC].[dbo].[Nomination_Reg]  where [Gender]=1000 and Symbols_ID not in(0)  and Withdrawal_status is null and Subchk=1 and LocalBodyID in (" + lbid_str + ")";
        //            cmd = new SqlCommand(sql, seccon);
        //            seccon.Open();
        //            int mtotalcunt = Convert.ToInt32(cmd.ExecuteScalar());
        //            seccon.Close();

        //            double mpercntage = ((double)mtotalcunt / (double)gtotalcunt);

        //            double mpercunt = Math.Round((double)mpercntage * 100);

        //            string sql11 = "select COUNT(*) FtotalCount  FROM [SEC].[dbo].[Nomination_Reg]  where [Gender]=1001 and Symbols_ID not in(0)  and Withdrawal_status is null and Subchk=1 and LocalBodyID in (" + lbid_str + ")";
        //            cmd = new SqlCommand(sql11, seccon);
        //            seccon.Open();
        //            int ftotalcunt = Convert.ToInt32(cmd.ExecuteScalar());
        //            seccon.Close();

        //            double fpercntage = ((double)ftotalcunt / (double)gtotalcunt);

        //            double fpercunt = Math.Round((double)fpercntage * 100);


        //            gender = new Gender();
        //            gender.gTotalCount = Convert.ToString(gtotalcunt);
        //            gender.mTotalCount = Convert.ToString(mtotalcunt);
        //            gender.fTotalCount = Convert.ToString(ftotalcunt);
        //            gender.mpercentage = Convert.ToString(mpercunt);
        //            gender.fpercentage = Convert.ToString(fpercunt);
        //            gender.districtid = Convert.ToString(districtid);
        //            gender.localbodytype = Convert.ToString(localbodytype);
        //            lstGender.Add(gender);
        //        }
        //        else if (localbodytype == "2")
        //        {
        //            Sql = " select COUNT(*) as GTotalCount  FROM [SEC].[dbo].[NominationZP_Reg]  where Symbols_ID not in(0)  and Withdrawal_status is null and Subchk=1 and LocalBodyID in (" + lbid_str + ")";
        //            cmd = new SqlCommand(Sql, seccon);
        //            seccon.Open();
        //            int gtotalcunt = Convert.ToInt32(cmd.ExecuteScalar());
        //            seccon.Close();

        //            string sql = "select COUNT(*) MtotalCount  FROM [SEC].[dbo].[NominationZP_Reg]  where [Gender]=1000 and Symbols_ID not in(0)  and Withdrawal_status is null and Subchk=1 and LocalBodyID in (" + lbid_str + ")";
        //            cmd = new SqlCommand(sql, seccon);
        //            seccon.Open();
        //            int mtotalcunt = Convert.ToInt32(cmd.ExecuteScalar());
        //            seccon.Close();

        //            double mpercntage = ((double)mtotalcunt / (double)gtotalcunt);

        //            double mpercunt = Math.Round((double)mpercntage * 100);

        //            string sql11 = "select COUNT(*) FtotalCount  FROM [SEC].[dbo].[NominationZP_Reg]  where [Gender]=1001 and Symbols_ID not in(0)  and Withdrawal_status is null and Subchk=1 and LocalBodyID in (" + lbid_str + ")";
        //            cmd = new SqlCommand(sql11, seccon);
        //            seccon.Open();
        //            int ftotalcunt = Convert.ToInt32(cmd.ExecuteScalar());
        //            seccon.Close();

        //            double fpercntage = ((double)ftotalcunt / (double)gtotalcunt);

        //            double fpercunt = Math.Round((double)fpercntage * 100);

        //            gender = new Gender();
        //            gender.gTotalCount = Convert.ToString(gtotalcunt);
        //            gender.mTotalCount = Convert.ToString(mtotalcunt);
        //            gender.fTotalCount = Convert.ToString(ftotalcunt);
        //            gender.mpercentage = Convert.ToString(mpercunt);
        //            gender.fpercentage = Convert.ToString(fpercunt);
        //            gender.districtid = Convert.ToString(districtid);
        //            gender.localbodytype = Convert.ToString(localbodytype);
        //            lstGender.Add(gender);
        //        }
        //    }
        //    catch
        //    {

        //    }
        //    return lstGender.ToList();
        //}

        //public List<Gender> NominationCount(string districtid, string localbodytype)
        //{
        //    string lbid_str = string.Empty;
        //    List<Gender> lstGender = new List<Gender>();
        //    Gender gender = new Gender();
        //    try
        //    {
        //        Sql = "select [LBID] from [SEC].[dbo].[LB_District] where [LBDistrictID]='" + districtid + "'";
        //        da = new SqlDataAdapter(Sql, seccon);
        //        DataSet ds1 = new DataSet();
        //        da.Fill(ds1);
        //        for (int a = 0; a < ds1.Tables[0].Rows.Count; a++)
        //        {
        //            lbid_str = lbid_str + "," + Convert.ToString(ds1.Tables[0].Rows[a]["LBID"]);
        //        }
        //        lbid_str = lbid_str.Substring(1);

        //        if (localbodytype == "5")
        //        {
        //            Sql = " select COUNT(*) as GTotalCount  FROM [SEC].[dbo].[NominationMP_Reg]  where Symbols_ID not in(0)  and Withdrawal_status is null and Subchk=1 and LocalBodyID in (" + lbid_str + ")";
        //            cmd = new SqlCommand(Sql, seccon);
        //            seccon.Open();
        //            double gtotalcunt = Convert.ToInt32(cmd.ExecuteScalar());
        //            seccon.Close();

        //            string sql = "select COUNT(*) MtotalCount  FROM [SEC].[dbo].[NominationMP_Reg]  where [Gender]=1000 and Symbols_ID not in(0)  and Withdrawal_status is null and Subchk=1 and LocalBodyID in (" + lbid_str + ")";
        //            cmd = new SqlCommand(sql, seccon);
        //            seccon.Open();
        //            double mtotalcunt = Convert.ToInt32(cmd.ExecuteScalar());
        //            seccon.Close();

        //            double mpercntage = ((double)mtotalcunt / (double)gtotalcunt);

        //            double mpercunt = Math.Round((double)mpercntage * 100);

        //            string sql11 = "select COUNT(*) FtotalCount  FROM [SEC].[dbo].[NominationMP_Reg]  where [Gender]=1001 and Symbols_ID not in(0)  and Withdrawal_status is null and Subchk=1 and LocalBodyID in (" + lbid_str + ")";
        //            cmd = new SqlCommand(sql11, seccon);
        //            seccon.Open();
        //            double ftotalcunt = Convert.ToInt32(cmd.ExecuteScalar());
        //            seccon.Close();

        //            double fpercntage = ((double)ftotalcunt / (double)gtotalcunt);

        //            double fpercunt = Math.Round((double)fpercntage * 100);


        //            gender = new Gender();
        //            gender.gTotalCount = Convert.ToString(gtotalcunt);
        //            gender.mTotalCount = Convert.ToString(mtotalcunt);
        //            gender.fTotalCount = Convert.ToString(ftotalcunt);
        //            gender.mpercentage = Convert.ToString(mpercunt);
        //            gender.fpercentage = Convert.ToString(fpercunt);
        //            gender.districtid = Convert.ToString(districtid);
        //            gender.localbodytype = Convert.ToString(localbodytype);
        //            lstGender.Add(gender);
        //        }
        //        else if (localbodytype == "3")
        //        {
        //            Sql = " select COUNT(*) as GTotalCount  FROM [SEC].[dbo].[Nomination_Reg]  where Symbols_ID not in(0)  and Withdrawal_status is null and Subchk=1 and LocalBodyID in (" + lbid_str + ")";
        //            cmd = new SqlCommand(Sql, seccon);
        //            seccon.Open();
        //            int gtotalcunt = Convert.ToInt32(cmd.ExecuteScalar());
        //            seccon.Close();

        //            string sql = "select COUNT(*) MtotalCount  FROM [SEC].[dbo].[Nomination_Reg]  where [Gender]=1000 and Symbols_ID not in(0)  and Withdrawal_status is null and Subchk=1 and LocalBodyID in (" + lbid_str + ")";
        //            cmd = new SqlCommand(sql, seccon);
        //            seccon.Open();
        //            int mtotalcunt = Convert.ToInt32(cmd.ExecuteScalar());
        //            seccon.Close();

        //            double mpercntage = ((double)mtotalcunt / (double)gtotalcunt);

        //            double mpercunt = Math.Round((double)mpercntage * 100);

        //            string sql11 = "select COUNT(*) FtotalCount  FROM [SEC].[dbo].[Nomination_Reg]  where [Gender]=1001 and Symbols_ID not in(0)  and Withdrawal_status is null and Subchk=1 and LocalBodyID in (" + lbid_str + ")";
        //            cmd = new SqlCommand(sql11, seccon);
        //            seccon.Open();
        //            int ftotalcunt = Convert.ToInt32(cmd.ExecuteScalar());
        //            seccon.Close();

        //            double fpercntage = ((double)ftotalcunt / (double)gtotalcunt);

        //            double fpercunt = Math.Round((double)fpercntage * 100);


        //            gender = new Gender();
        //            gender.gTotalCount = Convert.ToString(gtotalcunt);
        //            gender.mTotalCount = Convert.ToString(mtotalcunt);
        //            gender.fTotalCount = Convert.ToString(ftotalcunt);
        //            gender.mpercentage = Convert.ToString(mpercunt);
        //            gender.fpercentage = Convert.ToString(fpercunt);
        //            gender.districtid = Convert.ToString(districtid);
        //            gender.localbodytype = Convert.ToString(localbodytype);
        //            lstGender.Add(gender);
        //        }
        //        else if (localbodytype == "2")
        //        {
        //            Sql = " select COUNT(*) as GTotalCount  FROM [SEC].[dbo].[NominationZP_Reg]  where Symbols_ID not in(0)  and Withdrawal_status is null and Subchk=1 and LocalBodyID in (" + lbid_str + ")";
        //            cmd = new SqlCommand(Sql, seccon);
        //            seccon.Open();
        //            int gtotalcunt = Convert.ToInt32(cmd.ExecuteScalar());
        //            seccon.Close();

        //            string sql = "select COUNT(*) MtotalCount  FROM [SEC].[dbo].[NominationZP_Reg]  where [Gender]=1000 and Symbols_ID not in(0)  and Withdrawal_status is null and Subchk=1 and LocalBodyID in (" + lbid_str + ")";
        //            cmd = new SqlCommand(sql, seccon);
        //            seccon.Open();
        //            int mtotalcunt = Convert.ToInt32(cmd.ExecuteScalar());
        //            seccon.Close();

        //            double mpercntage = ((double)mtotalcunt / (double)gtotalcunt);

        //            double mpercunt = Math.Round((double)mpercntage * 100);

        //            string sql11 = "select COUNT(*) FtotalCount  FROM [SEC].[dbo].[NominationZP_Reg]  where [Gender]=1001 and Symbols_ID not in(0)  and Withdrawal_status is null and Subchk=1 and LocalBodyID in (" + lbid_str + ")";
        //            cmd = new SqlCommand(sql11, seccon);
        //            seccon.Open();
        //            int ftotalcunt = Convert.ToInt32(cmd.ExecuteScalar());
        //            seccon.Close();

        //            double fpercntage = ((double)ftotalcunt / (double)gtotalcunt);

        //            double fpercunt = Math.Round((double)fpercntage * 100);

        //            gender = new Gender();
        //            gender.gTotalCount = Convert.ToString(gtotalcunt);
        //            gender.mTotalCount = Convert.ToString(mtotalcunt);
        //            gender.fTotalCount = Convert.ToString(ftotalcunt);
        //            gender.mpercentage = Convert.ToString(mpercunt);
        //            gender.fpercentage = Convert.ToString(fpercunt);
        //            gender.districtid = Convert.ToString(districtid);
        //            gender.localbodytype = Convert.ToString(localbodytype);
        //            lstGender.Add(gender);
        //        }
        //    }
        //    catch
        //    {

        //    }
        //    return lstGender.ToList();
        //}

        public List<WardAndBooth> UpdateWardVoterAnalysis(string acno, string partno, string serialno)
        {
            List<WardAndBooth> lstwardandboth = new List<WardAndBooth>();
            WardAndBooth wardandboth = new WardAndBooth();
            try
            {
                cmd.CommandText = "VoterAnlysisByWardBooth";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@TableName", SqlDbType.NVarChar).Value = "Ac_" + acno + "_";
                cmd.Parameters.Add("@AC_No", SqlDbType.NVarChar).Value = acno;
                cmd.Parameters.Add("@PART_NO", SqlDbType.NVarChar).Value = partno;
                cmd.Parameters.Add("@SERIAL_NO", SqlDbType.NVarChar).Value = serialno;
                cmd.Connection = con;
                da.SelectCommand = cmd;
                da.Fill(ds);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        wardandboth = new WardAndBooth();
                        wardandboth.wardNo = Convert.ToString(ds.Tables[0].Rows[0]["WardNo"]);
                        wardandboth.boothNo = Convert.ToString(ds.Tables[0].Rows[0]["BoothNumber"]);
                        //wardandboth.boothName = Convert.ToString(ds.Tables[0].Rows[i]["boothname"]);
                        //wardandboth.boothAddress = Convert.ToString(ds.Tables[0].Rows[i]["BoothAddress"]);
                        lstwardandboth.Add(wardandboth);
                    }
                }
            }
            catch
            {

            }
            return lstwardandboth.ToList();
        }

        public List<Downloadreg> DownloadRegSchedularDataMP(string dateform, string dateto)
        {
            DataTable dt = new DataTable();
            List<Downloadreg> lstdwn = new List<Downloadreg>();
            try
            {
                Sql = "select [CreatedDate],[WardID],[LocalBodyID],[FirstName],[MiddleName],[LastName],[CandidateMob],[NominationID],[GroupID],[Pin],[Formtype],[SU_Status],[SubChk],[withdrawal_Status],[Aff_FinalSumbission],[NameofPartyID],[Symbols_ID] FROM  [NominationMP_Reg] " +
                   "WHERE  [CreatedDate] BETWEEN '" + dateform + "' AND '" + dateto + "'";

                da = new SqlDataAdapter(Sql, seccon);
                da.Fill(ds);

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

                    cmd = new SqlCommand("Select [LBName] from [LB_LocalBody] where [LBID]='" + Convert.ToString(ds.Tables[0].Rows[i]["LocalBodyID"]) + "'", seccon);
                    seccon.Open();
                    string LocalBodyName = Convert.ToString(cmd.ExecuteScalar());
                    seccon.Close();

                    Sql = "Select [WardID] from [WardSeatDetails] where [Ward_SeatID]='" + ds.Tables[0].Rows[i]["WardID"].ToString() + "'";
                    cmd = new SqlCommand(Sql, seccon);
                    seccon.Open();
                    string wardid = Convert.ToString(cmd.ExecuteScalar());
                    seccon.Close();

                    cmd = new SqlCommand("Select [NameOfParty] from [Symbol_MasterNew] where [PPID]='" + Convert.ToString(ds.Tables[0].Rows[i]["NameofPartyID"]) + "'", seccon);
                    seccon.Open();
                    string Party_Name = Convert.ToString(cmd.ExecuteScalar());
                    seccon.Close();

                    cmd = new SqlCommand("Select [SymbolName] from [Symbol_MasterNew] where [PPID]='" + Convert.ToString(ds.Tables[0].Rows[i]["Symbols_ID"]) + "'", seccon);
                    seccon.Open();
                    string Symbol_Name = Convert.ToString(cmd.ExecuteScalar());
                    seccon.Close();

                    Downloadreg dwn = new Downloadreg();
                    dwn.LOCALBODYNAME = Convert.ToString(LocalBodyName);
                    dwn.LOCALBODYID = ds.Tables[0].Rows[i]["LocalBodyID"].ToString() != null ? ds.Tables[0].Rows[i]["LocalBodyID"].ToString() : "NA";
                    dwn.FIRSTNAME = ds.Tables[0].Rows[i]["FirstName"].ToString() != null ? ds.Tables[0].Rows[i]["FirstName"].ToString() : "NA";
                    dwn.MIDDLENAME = ds.Tables[0].Rows[i]["MiddleName"].ToString() != null ? ds.Tables[0].Rows[i]["MiddleName"].ToString() : "NA";
                    dwn.LASTNAME = ds.Tables[0].Rows[i]["LastName"].ToString() != null ? ds.Tables[0].Rows[i]["LastName"].ToString() : "NA";
                    dwn.CANDIDATEMOB = ds.Tables[0].Rows[i]["CandidateMob"].ToString() != null ? ds.Tables[0].Rows[i]["CandidateMob"].ToString() : "NA";
                    // dwn.ADDRESS = ds.Tables[0].Rows[i]["Address"].ToString() != null ? ds.Tables[0].Rows[i]["Address"].ToString() : "NA";
                    dwn.NOMINATIONID = ds.Tables[0].Rows[i]["NominationID"].ToString() != null ? ds.Tables[0].Rows[i]["NominationID"].ToString() : "NA";
                    dwn.DISTRICTID = Convert.ToString(districtId);
                    dwn.DISTRICTNAME = Convert.ToString(Districtname);
                    dwn.WARDID = Convert.ToString(wardid);
                    // dwn.TALUKAID = talukaId.ToString();
                    // dwn.TALUKANAME = talukaName.ToString();
                    dwn.GROUPID = ds.Tables[0].Rows[i]["GroupID"].ToString() != null ? ds.Tables[0].Rows[i]["GroupID"].ToString() : "NA";
                    dwn.PIN = ds.Tables[0].Rows[i]["Pin"].ToString() != null ? ds.Tables[0].Rows[i]["Pin"].ToString() : "NA";
                    dwn.FORMTTYPE = ds.Tables[0].Rows[i]["Formtype"].ToString() != null ? ds.Tables[0].Rows[i]["Formtype"].ToString() : "NA";
                    dwn.CREATEDDATE = ds.Tables[0].Rows[i]["CreatedDate"].ToString() != null ? ds.Tables[0].Rows[i]["CreatedDate"].ToString() : "NA";
                    dwn.SU_STATUS = ds.Tables[0].Rows[i]["SU_Status"].ToString() != null ? ds.Tables[0].Rows[i]["SU_Status"].ToString() : "NA";
                    dwn.SUBCHK = ds.Tables[0].Rows[i]["SubChk"].ToString() != null ? ds.Tables[0].Rows[i]["SubChk"].ToString() : "NA";
                    dwn.WITHDRAWAL_STATUS = ds.Tables[0].Rows[i]["withdrawal_Status"].ToString() != null ? ds.Tables[0].Rows[i]["withdrawal_Status"].ToString() : "NA";
                    dwn.AFF_FINALSUBMISSION = ds.Tables[0].Rows[i]["Aff_FinalSumbission"].ToString() != null ? ds.Tables[0].Rows[i]["Aff_FinalSumbission"].ToString() : "NA";
                    dwn.PARTY_ID = ds.Tables[0].Rows[i]["NameofPartyID"].ToString() != null ? ds.Tables[0].Rows[i]["NameofPartyID"].ToString() : "NA";
                    dwn.PARTY_NAME = Convert.ToString(Party_Name) != null ? Party_Name.ToString() : "NA";
                    dwn.SYMBOL_ID = ds.Tables[0].Rows[i]["Symbols_ID"].ToString() != null ? ds.Tables[0].Rows[i]["Symbols_ID"].ToString() : "NA";
                    dwn.SYMBOL_NAME = Convert.ToString(Symbol_Name) != null ? Symbol_Name.ToString() : "NA";
                    lstdwn.Add(dwn);
                }
            }
            catch
            {
                //jsonDataStrngReturn = "0";
            }
            return lstdwn.ToList();
        }

        public List<clsVoterSearchWS> DownloadEPICIdWiseData(string EpicId)
        {
            DataTable dt = new DataTable();
            List<clsVoterSearchWS> lstdwn = new List<clsVoterSearchWS>();
            try
            {
                cmd.CommandText = "Get_EpikIDWise";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@TableName", SqlDbType.NVarChar).Value = "Ac_145_";
                cmd.Parameters.Add("@EPIKNOID", SqlDbType.NVarChar).Value = EpicId.ToString();
                cmd.Connection = con;
                da.SelectCommand = cmd;
                da.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        objcls = new clsVoterSearchWS();
                        objcls.WardNo = ds.Tables[0].Rows[i]["WardNo"] != null ? ds.Tables[0].Rows[i]["WardNo"].ToString() : "NA";
                        objcls.IDCARD_NO = ds.Tables[0].Rows[i]["IDCARD_NO"] != null ? ds.Tables[0].Rows[i]["IDCARD_NO"].ToString() : "NA";
                        objcls.FM_NAMEEN = ds.Tables[0].Rows[i]["FM_NAMEEN"] != null ? ds.Tables[0].Rows[i]["FM_NAMEEN"].ToString() : "NA";
                        objcls.LASTNAMEEN = ds.Tables[0].Rows[i]["LASTNAMEEN"] != null ? ds.Tables[0].Rows[i]["LASTNAMEEN"].ToString() : "NA";
                        objcls.BoothNumber = ds.Tables[0].Rows[i]["BoothNumber"] != null ? Convert.ToString(ds.Tables[0].Rows[i]["BoothNumber"]) : "NA";
                        objcls.boothname = ds.Tables[0].Rows[i]["boothname"] != null ? ds.Tables[0].Rows[i]["boothname"].ToString() : "NA";
                        objcls.BoothAddress = ds.Tables[0].Rows[i]["BoothAddress"] != null ? ds.Tables[0].Rows[i]["BoothAddress"].ToString() : "NA";
                        //if (assemblyid == "071" || assemblyid == "096" || assemblyid == "235" || assemblyid == "114" || assemblyid =="115" || assemblyid =="136" || assemblyid == "137" || assemblyid =="188")
                        //{
                        //  objcls.SerialNoInBooth = ds.Tables[0].Rows[i]["SerialNo_Booth"] != null ? ds.Tables[0].Rows[i]["SerialNo_Booth"].ToString() : "NA";
                        //}
                        //else
                        //{
                        //    objcls.SerialNoInBooth = "NA";
                        //}
                        cls.Add(objcls);
                    }
                }
                else
                {
                    DataSet Ds = new DataSet();
                    cmd.CommandText = "Get_EpikIDWise";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@TableName", SqlDbType.NVarChar).Value = "Ac_146_";
                    cmd.Parameters.Add("@EPIKNOID", SqlDbType.NVarChar).Value = EpicId.ToString();
                    cmd.Connection = con;
                    da.SelectCommand = cmd;
                    da.Fill(Ds);

                    for (int i = 0; i < Ds.Tables[0].Rows.Count; i++)
                    {
                        objcls = new clsVoterSearchWS();
                        objcls.WardNo = Ds.Tables[0].Rows[i]["WardNo"] != null ? Ds.Tables[0].Rows[i]["WardNo"].ToString() : "NA";
                        objcls.IDCARD_NO = Ds.Tables[0].Rows[i]["IDCARD_NO"] != null ? Ds.Tables[0].Rows[i]["IDCARD_NO"].ToString() : "NA";
                        objcls.FM_NAMEEN = Ds.Tables[0].Rows[i]["FM_NAMEEN"] != null ? Ds.Tables[0].Rows[i]["FM_NAMEEN"].ToString() : "NA";
                        objcls.LASTNAMEEN = Ds.Tables[0].Rows[i]["LASTNAMEEN"] != null ? Ds.Tables[0].Rows[i]["LASTNAMEEN"].ToString() : "NA";
                        objcls.BoothNumber = Ds.Tables[0].Rows[i]["BoothNumber"] != null ? Convert.ToString(Ds.Tables[0].Rows[i]["BoothNumber"]) : "NA";
                        objcls.boothname = Ds.Tables[0].Rows[i]["boothname"] != null ? Ds.Tables[0].Rows[i]["boothname"].ToString() : "NA";
                        objcls.BoothAddress = Ds.Tables[0].Rows[i]["BoothAddress"] != null ? Ds.Tables[0].Rows[i]["BoothAddress"].ToString() : "NA";
                        //if (assemblyid == "071" || assemblyid == "096" || assemblyid == "235" || assemblyid == "114" || assemblyid =="115" || assemblyid =="136" || assemblyid == "137" || assemblyid =="188")
                        //{
                        // objcls.SerialNoInBooth = Ds.Tables[0].Rows[i]["SerialNo_Booth"] != null ? Ds.Tables[0].Rows[i]["SerialNo_Booth"].ToString() : "NA";
                        //}
                        //else
                        //{
                        //    objcls.SerialNoInBooth = "NA";
                        //}
                        cls.Add(objcls);
                    }
                }

            }
            catch
            {

            }
            return cls.ToList();
        }

        public List<clsVoterSearchWS> DownloadEPICIdWise_GetName(string EpicId, string acNo)
        {
            DataTable dt = new DataTable();
            List<clsVoterSearchWS> lstdwn = new List<clsVoterSearchWS>();
            try
            {
                cmd.CommandText = "Get_EpikIDWise";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@TableName", SqlDbType.NVarChar).Value = "Ac_" + acNo + "_";
                cmd.Parameters.Add("@EPIKNOID", SqlDbType.NVarChar).Value = EpicId.ToString();
                cmd.Connection = con;
                da.SelectCommand = cmd;
                da.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        objcls = new clsVoterSearchWS();
                        objcls.WardNo = ds.Tables[0].Rows[i]["WardNo"] != null ? ds.Tables[0].Rows[i]["WardNo"].ToString() : "NA";
                        objcls.IDCARD_NO = ds.Tables[0].Rows[i]["IDCARD_NO"] != null ? ds.Tables[0].Rows[i]["IDCARD_NO"].ToString() : "NA";
                        objcls.FM_NAMEEN = ds.Tables[0].Rows[i]["FM_NAMEEN"] != null ? ds.Tables[0].Rows[i]["FM_NAMEEN"].ToString() : "NA";
                        objcls.LASTNAMEEN = ds.Tables[0].Rows[i]["LASTNAMEEN"] != null ? ds.Tables[0].Rows[i]["LASTNAMEEN"].ToString() : "NA";
                        objcls.BoothNumber = ds.Tables[0].Rows[i]["BoothNumber"] != null ? Convert.ToString(ds.Tables[0].Rows[i]["BoothNumber"]) : "NA";
                        objcls.boothname = ds.Tables[0].Rows[i]["boothname"] != null ? ds.Tables[0].Rows[i]["boothname"].ToString() : "NA";
                        objcls.BoothAddress = ds.Tables[0].Rows[i]["BoothAddress"] != null ? ds.Tables[0].Rows[i]["BoothAddress"].ToString() : "NA";
                        objcls.AC_NO = ds.Tables[0].Rows[i]["AC_NO"] != null ? ds.Tables[0].Rows[i]["AC_NO"].ToString() : "NA";
                        objcls.PART_NO = ds.Tables[0].Rows[i]["PART_NO"] != null ? ds.Tables[0].Rows[i]["PART_NO"].ToString() : "NA";
                        objcls.SLNOINPART = ds.Tables[0].Rows[i]["SLNOINPART"] != null ? ds.Tables[0].Rows[i]["SLNOINPART"].ToString() : "NA";
                        //if (assemblyid == "071" || assemblyid == "096" || assemblyid == "235" || assemblyid == "114" || assemblyid =="115" || assemblyid =="136" || assemblyid == "137" || assemblyid =="188")
                        //{
                        //  objcls.SerialNoInBooth = ds.Tables[0].Rows[i]["SerialNo_Booth"] != null ? ds.Tables[0].Rows[i]["SerialNo_Booth"].ToString() : "NA";
                        //}
                        //else
                        //{
                        //    objcls.SerialNoInBooth = "NA";
                        //}
                        cls.Add(objcls);
                    }
                }
            }
            catch
            {

            }
            return cls.ToList();
        }

        public List<PartyDetails> CandidateResult(string LBID, string lbtype)
        {
            List<PartyDetails> lstparty = new List<PartyDetails>();
            PartyDetails party = new PartyDetails();
            try
            {
                cmd.Connection = sectempcon;
                cmd.CommandText = "[GetPartyWinnerDetails]";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@lbId",Convert.ToInt32(LBID.ToString()));
                cmd.Parameters.Add("@lbtype",Convert.ToInt32(lbtype.ToString()));
                da.SelectCommand = cmd;
                da.Fill(ds);
                
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for(int i=0; i < ds.Tables[0].Rows.Count; i++)
                    {
                       
                        party = new PartyDetails();
                        party.partyname = Convert.ToString(ds.Tables[0].Rows[i]["Partyname"]);
                        party.partycount = Convert.ToString(ds.Tables[0].Rows[i]["winnercount"]);
                        party.type = Convert.ToString("1");
                        lstparty.Add(party);
                    }
                    for (int j = 0; j < ds.Tables[1].Rows.Count; j++)
                    {
                        party = new PartyDetails();
                        party.partyname = Convert.ToString(ds.Tables[1].Rows[j]["WardId"]);
                        party.partycount = Convert.ToString("NA");
                        party.type = Convert.ToString("2");
                        lstparty.Add(party);
                    }
                }
                else
                {
                    party = new PartyDetails();
                    party.nodata = "107";
                    lstparty.Add(party);
                }
            }
            catch
            {
                party = new PartyDetails();
                party.error = "106";
                lstparty.Add(party);
            }
            return lstparty.ToList();
        }

        public List<CandidateDetails> ResultDetails(string LBID, string partyname)
        {
            List<CandidateDetails> lstcandidate = new List<CandidateDetails>();
            CandidateDetails candidate = new CandidateDetails();
            try
            {
                cmd.Connection = sectempcon;
                cmd.CommandText = "[GetCandidateResult]";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@lbId", Convert.ToInt32(LBID.ToString()));
                cmd.Parameters.Add("@partyname", partyname.ToString());
                da.SelectCommand = cmd;
                da.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        candidate = new CandidateDetails();
                        candidate.candidatefname = Convert.ToString(ds.Tables[0].Rows[i]["LnameCandidate"]);
                        candidate.candidatelname = Convert.ToString(ds.Tables[0].Rows[i]["FnameCandidate"]);
                        candidate.candidatemname = Convert.ToString(ds.Tables[0].Rows[i]["MnameCandidate"]);
                        candidate.candidatevotes = Convert.ToString(ds.Tables[0].Rows[i]["TotVotesSecured"]);
                        lstcandidate.Add(candidate);
                    }
                }
                else
                {
                    candidate = new CandidateDetails();
                    candidate.nodata = "107";
                    lstcandidate.Add(candidate);
                }
            }
            catch
            {
                candidate = new CandidateDetails();
                candidate.error = "106";
                lstcandidate.Add(candidate);
            }
            return lstcandidate.ToList();
        }

        public List<AllCandidateDetailsInWard> AllCandidateDetailsInWard(string LBID, string wardno)
        {
            List<AllCandidateDetailsInWard> lstcandidate = new List<AllCandidateDetailsInWard>();
            AllCandidateDetailsInWard candidate = new AllCandidateDetailsInWard();
            try
            {
                cmd.Connection = sectempcon;
                cmd.CommandText = "[GetAllCandidateDetailsWardWise]";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@lbId", Convert.ToInt32(LBID.ToString()));
                cmd.Parameters.Add("@wardno", wardno.ToString());
                da.SelectCommand = cmd;
                da.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        candidate = new AllCandidateDetailsInWard();
                        candidate.candidatelname = Convert.ToString(ds.Tables[0].Rows[i]["LnameCandidate"]);
                        candidate.candidatefname = Convert.ToString(ds.Tables[0].Rows[i]["FnameCandidate"]);
                        candidate.candidatemname = Convert.ToString(ds.Tables[0].Rows[i]["MnameCandidate"]);
                        candidate.candidatevotes = Convert.ToString(ds.Tables[0].Rows[i]["TotVotesSecured"]);
                        candidate.sectionname = Convert.ToString(ds.Tables[0].Rows[i]["SeatName"]);
                        candidate.iswinner = Convert.ToString(ds.Tables[0].Rows[i]["IsWinner"]);
                        candidate.partyname = Convert.ToString(ds.Tables[0].Rows[i]["Partyname"]);
                        lstcandidate.Add(candidate);
                    }
                }
                else
                {
                    candidate = new AllCandidateDetailsInWard();
                    candidate.nodata = "107";
                    lstcandidate.Add(candidate);
                }
            }
            catch
            {
                candidate = new AllCandidateDetailsInWard();
                candidate.error = "106";
                lstcandidate.Add(candidate);
            }
            return lstcandidate.ToList();
        }
    }
}
