using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace LatestVoterSearch
{
    public class CommanCode
    {
        private string aid = "639250"; //NEWLY ADDED BY Ram Kendre
        private string pin = "M@h123";
        private WebProxy objProxy1 = null;
        public string SMS(string Mobile_Number, string Message)
        {
            Mobile_Number = "91" + Mobile_Number;
            //System.Object stringpost = "aid=" + aid + "&pin=" + pin + "&mnumber=" + Mobile_Number + "&message=" + Message + "&signature=MAHSEC";
            System.Object stringpost = "aid=" + aid + "&pin=" + pin + "&mnumber=" + Mobile_Number + "&message=" + Message + "&signature=MAHSEC";

            HttpWebRequest objWebRequest = null;
            HttpWebResponse objWebResponse = null;
            StreamWriter objStreamWriter = null;
            StreamReader objStreamReader = null;

            try
            {
                string stringResult = null;
                //objWebRequest = (HttpWebRequest)WebRequest.Create(" http://otp.zone:7501/failsafe/HttpLink?aid=639128&pin=M@h123&mnumber=91XXXXXXXXXX&message=test&signature=MAHSEC");
                //objWebRequest = (HttpWebRequest)WebRequest.Create("  http://otp.zone:7501/failsafe/HttpLink?aid=639250&pin=M@h123&mnumber=91XXXXXXXXXX&message=test&signature=MGAGEX");

                objWebRequest = (HttpWebRequest)WebRequest.Create("http://otp.zone:7501/failsafe/HttpLink?");
                objWebRequest.Method = "POST";

                if ((objProxy1 != null))
                {
                    objWebRequest.Proxy = objProxy1;
                }

                objWebRequest.ContentType = "application/x-www-form-urlencoded";
                objStreamWriter = new StreamWriter(objWebRequest.GetRequestStream());
                objStreamWriter.Write(stringpost);
                objStreamWriter.Flush();
                objStreamWriter.Close();
                objWebResponse = (HttpWebResponse)objWebRequest.GetResponse();
                objStreamReader = new StreamReader(objWebResponse.GetResponseStream());
                stringResult = objStreamReader.ReadToEnd();
                objStreamReader.Close();
                return (stringResult);
            }
            catch (Exception ex)
            {
                return (ex.Message);
            }
            finally
            {

                if ((objStreamWriter != null))
                {
                    objStreamWriter.Close();
                }
                if ((objStreamReader != null))
                {
                    objStreamReader.Close();
                }
                objWebRequest = null;
                objWebResponse = null;
                objProxy1 = null;
            }
        }
    }
}