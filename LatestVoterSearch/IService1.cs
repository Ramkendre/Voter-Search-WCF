using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace LatestVoterSearch
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IService1
    {
        //[OperationContract]
        //[WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, UriTemplate = "UploadDataFile/jsData")]
        //string UploadDataFile(Stream jsData);

        //[OperationContract]
        //[WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, UriTemplate = "VoterAnalysisuploadData/js")]
        //string VoterAnalysisuploadData(Stream jsData);

        [OperationContract]
        [WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, UriTemplate = "InsertRegRecord?MaxId={MaxId}")]
        string InsertRegRecord(string MaxId);

        [OperationContract]
        [WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, UriTemplate = "InsertRegRecordMP?MaxId={MaxId}")]
        string InsertRegRecordMP(string MaxId);

        [OperationContract]
        [WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, UriTemplate = "InsertRegRecordZP?MaxId={MaxId}")]
        string InsertRegRecordZP(string MaxId);

        [OperationContract]
        [WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, UriTemplate = "DownloadRegData?maxid={maxid}")]
        string DownloadRegData(string maxid);

        //[OperationContract]
        //[WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, UriTemplate = "HighAssetsReoprt?districtid={districtid}")]
        //List<Assests> HighAssetsReoprt(string districtid);//, string localbodytype);

        [OperationContract]
        [WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, UriTemplate = "HighAssetsReoprt?districtid={districtid}")]
        List<Liablity> HighliablityReoprt(string districtid);
    }


    [DataContract]
    public class Employee
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Address { get; set; }
        [DataMember]
        public string salary { get; set; }
        [DataMember]
        public string Dept { get; set; }
    }

    //[DataContract]
    //public class Assests
    //{
    //    [DataMember]
    //    public string Name { get; set; }
    //    //[DataMember]
    //    //public string partyName { get; set; }
    //    [DataMember]
    //    public string movableAssts { get; set; }
    //    [DataMember]
    //    public string immovableAssts { get; set; }
    //    [DataMember]
    //    public string totalassts { get; set; }
    //}

     [DataContract]
    public class Liablity
    {
        [DataMember]
        public string Name { get; set; }
        //[DataMember]
        //public string partyName { get; set; }
        [DataMember]
        public string liablities { get; set; }
        //[DataMember]
        //public string immovableAssts { get; set; }
        [DataMember]
        public string totalassts { get; set; }
    }
}
