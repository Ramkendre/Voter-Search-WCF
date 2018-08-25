using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Xml;

namespace LatestVoterSearch
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IWCFVoterSearchWS" in both code and config file together.
    [ServiceContract]
    public interface IWCFVoterSearchWS
    {
        //[OperationContract]
        //[WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, UriTemplate = "GetVoterDetailslstSearch?Name={Name}&LName={LName}&MaxId={MaxId}&acno={acno}")]
        //List<clsVoterSearchWS> GetVoterDetailslstSearch(string Name, string LName, string MaxId, string acno);

        [OperationContract]
        [WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, UriTemplate = "GetVoterDetailsSearch?Name={Name}&LName={LName}&MaxId={MaxId}&acno={acno}")]
        List<clsVoterSearchWS> GetVoterDetailsSearch(string Name, string LName, string MaxId, string acno);

        [OperationContract]
        [WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, UriTemplate = "GetWardWiseDetails?assemblyid={assemblyid}&wardno={wardno}&maxserverid={maxserverid}&localBodyId={localBodyId}&localBodyType={localBodyType}")]
        List<clsVoterSearchWS> GetWardWiseDetails(string assemblyid, string wardno, string maxserverid, string localBodyId, string localBodyType);

        [OperationContract]
        [WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, UriTemplate = "GetPartWiseDetails?assemblyid={assemblyid}&partno={partno}&maxserverid={maxserverid}&localBodyId={localBodyId}&localBodyType={localBodyType}")]
        List<clsVoterSearchWS> GetPartWiseDetails(string assemblyid, string partno, string maxserverid, string localBodyId, string localBodyType);

        [OperationContract]
        [WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, UriTemplate = "GetEpikNoWiseDetails?assemblyid={assemblyid}&maxserverid={maxserverid}&epiknoId={epiknoId}")]
        List<clsVoterSearchWS> GetEpikNoWiseDetails(string assemblyid, string maxserverid, string epiknoId);

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, UriTemplate = "InsertRegRecord?MaxId={MaxId}")]
        string InsertRegRecord(string MaxId);

        [OperationContract]
        [WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, UriTemplate = "InsertRegRecordMP?MaxId={MaxId}")]
        string InsertRegRecordMP(string MaxId);

        [OperationContract]
        [WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, UriTemplate = "InsertRegRecordZP?MaxId={MaxId}")]
        string InsertRegRecordZP(string MaxId);

        [OperationContract]
        [WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, UriTemplate = "InsertRegRecordPS?MaxId={MaxId}")]
        string InsertRegRecordPS(string MaxId);

        [OperationContract]
        [WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, UriTemplate = "DownloadRegDataMP?localbodyid={localbodyid}&localbodytype={localbodytype}&dateform={dateform}&dateto={dateto}&reportStatus={reportStatus}")]
        List<Downloadreg> DownloadRegDataMP(string localbodyid, string localbodytype, string dateform, string dateto, string reportStatus);

        [OperationContract]
        [WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, UriTemplate = "DownloadRegDataMPDetails?localbodyid={localbodyid}&localbodytype={localbodytype}&dateform={dateform}&dateto={dateto}")]
        List<Downloadreg> DownloadRegDataMPDetails(string localbodyid, string localbodytype, string dateform, string dateto);

        [OperationContract]
        [WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, UriTemplate = "DownloadRegDataZP?localbodyid={localbodyid}")]
        List<Downloadreg> DownloadRegDataZP(string localbodyid);

        [OperationContract]
        [WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, UriTemplate = "DownloadRegDataPS?localbodyid={localbodyid}")]
        List<Downloadreg> DownloadRegDataPS(string localbodyid);

        [OperationContract]
        [WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, UriTemplate = "GetStringVoterDetails?Name={Name}&LName={LName}&MaxId={MaxId}&acno={acno}")]
        string GetStringVoterDetails(string Name, string LName, string MaxId, string acno);

        [OperationContract]
        [WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, UriTemplate = "DownloadWardWiseCandidateData?localbodyid={localbodyid}&wardid={wardid}&localbodytype={localbodytype}&electroldivisionNo={electroldivisionNo}&electrolclgdivNo={electrolclgdivNo}")]
        List<KYCData> DownloadWardWiseCandidateData(string localbodyid, string wardid, string localbodytype, string electroldivisionNo, string electrolclgdivNo);

        [OperationContract]
        [WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, UriTemplate = "KYCDetails?NId={NId}&localbodytype={localbodytype}")]
        List<KYCNominationData> KYCDetails(string NId, string localbodytype);

        [OperationContract]
        [WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, UriTemplate = "KYCDetailsNEW?NId={NId}&localbodytype={localbodytype}")]
        List<KYCNominationDataNEW> KYCDetailsNEW(string NId, string localbodytype);

        [OperationContract]
        [WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, UriTemplate = "KYCDetailsNEWString?NId={NId}&localbodytype={localbodytype}")]
        string KYCDetailsNEWString(string NId, string localbodytype);

        [OperationContract]
        [WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, UriTemplate = "KYCDetailsString?NId={NId}&localbodytype={localbodytype}")]
        string KYCDetailsString(string NId, string localbodytype);

        [OperationContract]
        [WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, UriTemplate = "DownloadCandidateResult?LBID={LBID}&wardno={wardno}&localbodytype={localbodytype}&electrolDivNo={electrolDivNo}&electrolClgNo={electrolClgNo}")]
        List<CandidateResult> DownloadCandidateResult(string LBID, string wardno, string localbodytype, string electrolDivNo, string electrolClgNo);

        [OperationContract]
        [WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, UriTemplate = "AgeWiseReoprt?districtid={districtid}&localbodytype={localbodytype}")]
        List<AgeWiseDistrictData> AgeWiseReoprt(string districtid, string localbodytype);

        [OperationContract]
        [WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, UriTemplate = "HighAssetsReoprt?districtid={districtid}&localbodytype={localbodytype}")]
        List<Assests> HighAssetsReoprt(string districtid, string localbodytype);

        [OperationContract]
        [WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, UriTemplate = "GenderWiseReoprt?districtid={districtid}&localbodytype={localbodytype}")]
        List<Gender> GenderWiseReoprt(string districtid, string localbodytype);

        [OperationContract]
        [WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, UriTemplate = "UpdateWardVoterAnalysis?acno={acno}&partno={partno}&serialno={serialno}")]
        List<WardAndBooth> UpdateWardVoterAnalysis(string acno, string partno, string serialno);

        [OperationContract]
        [WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, UriTemplate = "DownloadRegSchedularDataMP?dateform={dateform}&dateto={dateto}")]
        List<Downloadreg> DownloadRegSchedularDataMP(string dateform, string dateto);

        [OperationContract]
        [WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, UriTemplate = "DownloadEPICIdWiseData?EpicId={EpicId}")]
        List<clsVoterSearchWS> DownloadEPICIdWiseData(string EpicId);

        [OperationContract]
        [WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, UriTemplate = "DownloadEPICIdWise_GetName?EpicId={EpicId}&acNo={acNo}")]
        List<clsVoterSearchWS> DownloadEPICIdWise_GetName(string EpicId,string acNo);

        [OperationContract]
        [WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, UriTemplate = "CandidateResult?LBID={LBID}&lbtype={lbtype}")]
        List<PartyDetails> CandidateResult(string LBID, string lbtype);  

        [OperationContract]
        [WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, UriTemplate = "ResultDetails?LBID={LBID}&partyname={partyname}")]
        List<CandidateDetails> ResultDetails(string LBID, string partyname);

        [OperationContract]
        [WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, UriTemplate = "AllCandidateDetailsInWard?LBID={LBID}&wardno={wardno}")]
        List<AllCandidateDetailsInWard> AllCandidateDetailsInWard(string LBID, string wardno);
    }

    [DataContract]
    public class clsVoterSearchWS
    {
        [DataMember]
        public string Id { get; set; }
        [DataMember]
        public string Sr { get; set; }
        [DataMember]
        public string WardNo { get; set; }
        [DataMember]
        public string IDCARD_NO { get; set; }
        [DataMember]
        public string fm_name_v1 { get; set; }
        [DataMember]
        public string Lastname_v1 { get; set; }
        [DataMember]
        public string RLN_FM_NM_v1 { get; set; }
        [DataMember]
        public string RLN_L_NM_v1 { get; set; }
        [DataMember]
        public string FM_NAMEEN { get; set; }
        [DataMember]
        public string LASTNAMEEN { get; set; }
        [DataMember]
        public string RLN_FM_NMEN { get; set; }
        [DataMember]
        public string SEX { get; set; }
        [DataMember]
        public string AGE { get; set; }
        [DataMember]
        public string AC_NO { get; set; }
        [DataMember]
        public string PART_NO { get; set; }
        [DataMember]
        public string SLNOINPART { get; set; }
        [DataMember]
        public string BoothNumber { get; set; }
        [DataMember]
        public string boothname { get; set; }
        [DataMember]
        public string BoothAddress { get; set; }
        [DataMember]
        public string SerialNoInBooth { get; set; }
        [DataMember]
        public string dob { get; set; }
        [DataMember]
        public string aadharno { get; set; }
        [DataMember]
        public string house_no { get; set; }
        [DataMember]
        public string Section_No { get; set; }
        [DataMember]
        public string section_name_v1 { get; set; }
        [DataMember]
        public string section_name_en { get; set; }
    }

    [DataContract]
    public class Downloadreg
    {
        [DataMember]
        public string LOCALBODYID { set; get; }
        [DataMember]
        public string ELECTROLDIVISIONID { set; get; }
        [DataMember]
        public string ELECTROLCLGID { set; get; }
        [DataMember]
        public string LOCALBODYNAME { set; get; }
        [DataMember]
        public string ID { set; get; }
        [DataMember]
        public string FIRSTNAME { set; get; }
        [DataMember]
        public string MIDDLENAME { set; get; }
        [DataMember]
        public string LASTNAME { set; get; }
        [DataMember]
        public string CANDIDATEMOB { set; get; }
        [DataMember]
        public string ADDRESS { set; get; }
        [DataMember]
        public string NOMINATIONID { set; get; }
        [DataMember]
        public string DISTRICTID { set; get; }
        [DataMember]
        public string DISTRICTNAME { set; get; }
        [DataMember]
        public string TALUKAID { set; get; }
        [DataMember]
        public string TALUKANAME { set; get; }
        [DataMember]
        public string GROUPID { set; get; }
        [DataMember]
        public string PIN { set; get; }
        [DataMember]
        public string FORMTTYPE { set; get; }
        [DataMember]
        public string WARDID { set; get; }
        [DataMember]
        public string CREATEDDATE { set; get; }
        [DataMember]
        public string SU_STATUS { set; get; }
        [DataMember]
        public string SUBCHK { set; get; }
        [DataMember]
        public string WITHDRAWAL_STATUS { set; get; }
        [DataMember]
        public string AFF_FINALSUBMISSION { set; get; }
        [DataMember]
        public string PARTY_ID { set; get; }
        [DataMember]
        public string PARTY_NAME { set; get; }
        [DataMember]
        public string SYMBOL_ID { set; get; }
        [DataMember]
        public string SYMBOL_NAME { set; get; }
    }

    [DataContract]
    public class KYCData
    {
        [DataMember]
        public string nominationid { get; set; }
        [DataMember]
        public string firstName { get; set; }
        [DataMember]
        public string middleName { get; set; }
        [DataMember]
        public string lastName { get; set; }
        [DataMember]
        public string symbolID { get; set; }
        [DataMember]
        public string symbolName { get; set; }
        [DataMember]
        public string nameOfParty { get; set; }
        [DataMember]
        public string candidayemobno { get; set; }
        [DataMember]
        public string address { get; set; }
        [DataMember]
        public string seatName { get; set; }
    }

    [DataContract]
    public class KYCNominationData
    {
        [DataMember]
        public string candidatemobno { get; set; }
        [DataMember]
        public string mailid { get; set; }
        [DataMember]
        public string education { get; set; }
        [DataMember]
        public string totalassets { get; set; }
        [DataMember]
        public string sptotalassets { get; set; }
        [DataMember]
        public string dep1totalassets { get; set; }
        [DataMember]
        public string dep2totalassets { get; set; }
        [DataMember]
        public string dep3totalassets { get; set; }
        [DataMember]
        public string movableproperty { get; set; }
        [DataMember]
        public string spmovableproperty { get; set; }
        [DataMember]
        public string dep1movableproperty { get; set; }
        [DataMember]
        public string dep2movableproperty { get; set; }
        [DataMember]
        public string dep3movableproperty { get; set; }
        [DataMember]
        public string immovableproperty { get; set; }
        [DataMember]
        public string spimmovableproperty { get; set; }
        [DataMember]
        public string dep1immovableproperty { get; set; }
        [DataMember]
        public string dep2immovableproperty { get; set; }
        [DataMember]
        public string dep3immovableproperty { get; set; }
        [DataMember]
        public string income { get; set; }
        [DataMember]
        public string liabilities { get; set; }
        [DataMember]
        public string spliabilities { get; set; }
        [DataMember]
        public string dep1liabilities { get; set; }
        [DataMember]
        public string dep2liabilities { get; set; }
        [DataMember]
        public string dep3liabilities { get; set; }
        [DataMember]
        public string occuptions { get; set; }
        [DataMember]
        public int totalcases { get; set; }
        [DataMember]
        public string convicted { get; set; }
        [DataMember]
        public string cognizances { get; set; }
        [DataMember]
        public string previouselectionstatus { get; set; }
        [DataMember]
        public string previouselectionyear { get; set; }
    }

    [DataContract]
    public class KYCNominationDataNEW
    {
        [DataMember]
        public string candidatemobno { get; set; }
        [DataMember]
        public string mailid { get; set; }
        [DataMember]
        public string education { get; set; }
        [DataMember]
        public string totalassets { get; set; }
        [DataMember]
        public string movableproperty { get; set; }
        [DataMember]
        public string immovableproperty { get; set; }
        [DataMember]
        public string income { get; set; }
        [DataMember]
        public string liabilities { get; set; }
        [DataMember]
        public string occuptions { get; set; }
        [DataMember]
        public int totalcases { get; set; }
        [DataMember]
        public string convicted { get; set; }
        [DataMember]
        public string cognizances { get; set; }
        [DataMember]
        public string previouselectionstatus { get; set; }
        [DataMember]
        public string previouselectionyear { get; set; }
    }

    [DataContract]
    public class CandidateResult
    {
        [DataMember]
        public string candidateFName { get; set; }
        [DataMember]
        public string candidateMName { get; set; }
        [DataMember]
        public string candidateLName { get; set; }
        [DataMember]
        public string wardno { get; set; }
        [DataMember]
        public string section { get; set; }
        [DataMember]
        public string totalvote { get; set; }
        //[DataMember]
        //public string electedvote { get; set; }
        [DataMember]
        public string partyname { get; set; }
        [DataMember]
        public string iswinner { get; set; }
        [DataMember]
        public string localbodyid { get; set; }
    }

    [DataContract]
    public class AgeWiseDistrictData
    {
        [DataMember]
        public int AGECUNT1TO20 { get; set; }
        [DataMember]
        public int AGECUNT21TO30 { get; set; }
        [DataMember]
        public int AGECUNT31TO40 { get; set; }
        [DataMember]
        public int AGECUNT41TO50 { get; set; }
        [DataMember]
        public int AGECUNT51TO60 { get; set; }
        [DataMember]
        public int AGECUNT61TO70 { get; set; }
        [DataMember]
        public int AGECUNT71TO80 { get; set; }
        [DataMember]
        public int AGECUNT81TO90 { get; set; }
    }

    [DataContract]
    public class Assests
    {
        [DataMember]
        public string Name1 { get; set; }
        [DataMember]
        public string partyName { get; set; }
        [DataMember]
        public string movableAssts1 { get; set; }
        [DataMember]
        public string immovableAssts1 { get; set; }
        [DataMember]
        public string totalassts1 { get; set; }
    }

    [DataContract]
    public class Gender
    {
        [DataMember]
        public string gTotalCount { get; set; }
        [DataMember]
        public string mTotalCount { get; set; }
        [DataMember]
        public string fTotalCount { get; set; }
        [DataMember]
        public string fpercentage { get; set; }
        [DataMember]
        public string mpercentage { get; set; }
        [DataMember]
        public string districtid { get; set; }
        [DataMember]
        public string localbodytype { get; set; }
    }

    [DataContract]
    public class NominationCount
    {
        [DataMember]
        public string totalcount { get; set; }
        [DataMember]
        public string submittedcount { get; set; }
        [DataMember]
        public string finalcount { get; set; }
        [DataMember]
        public string withdrawalstatuscount { get; set; }
        [DataMember]
        public string rejectedcount { get; set; }
        [DataMember]
        public string districtid { get; set; }
        [DataMember]
        public string localbodytype { get; set; }
    }

    [DataContract]
    public class WardAndBooth
    {
        [DataMember]
        public string wardNo { set; get; }
        [DataMember]
        public string boothNo { set; get; }
        //[DataMember]
        //public string boothName { set; get; }
        //[DataMember]
        //public string boothAddress { set; get; }
        [DataMember]
        public string AadharNo { set; get; }
        [DataMember]
        public string DateOfBirth { set; get; }
    }

}
