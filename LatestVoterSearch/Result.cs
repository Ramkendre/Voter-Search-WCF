using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace LatestVoterSearch
{
    [DataContract]
    public class Result
    {
        public Result(string flag,List<PartyDetails> lstparty, List<WardDetails> wdlist)
        {
            this.status = flag;
            this.partylist = lstparty;
            this.wardlist = wdlist;
        }

        [DataMember]
        public string status { get; set; }

        [DataMember]
        public List<PartyDetails> partylist { get; set; }

        [DataMember]
        public List<WardDetails> wardlist { get; set; }
    }

    [DataContract]
    public class PartyDetails
    {
        [DataMember]
        public string partyname { get; set; }

        [DataMember]
        public string partycount { get; set; }

        [DataMember]
        public string type { get; set; }
        [DataMember]
        public string nodata { get; set; }
        [DataMember]
        public string error { get; set; }
    }

    [DataContract]
    public class WardDetails
    {
        //[DataMember]
        //public string wardname { get; set; }

        [DataMember]
        public string wardno { get; set; }

        [DataMember]
        public List<PartyDetails> prtydtls = new List<PartyDetails>();

        [DataMember]
        public List<WardSections> wrdlst = new List<WardSections>();

        [DataMember]
        public List<CandidateDetails> candetails = new List<CandidateDetails>();

        [DataMember]
        public List<PartyDetails> secpartylst = new List<PartyDetails>();
    }

    [DataContract]
    public class WardSections
    {
        [DataMember]
        public string sectionname { get; set; }

        [DataMember]
        public string sectionno { get; set; }
    }

    [DataContract]
    public class CandidateDetails
    {
        [DataMember]
        public string  candidatefname { get; set; }

        [DataMember]
        public string candidatelname { get; set; }

        [DataMember]
        public string candidatemname { get; set; }

        [DataMember]
        public string candidatevotes { get; set; }

        [DataMember]
        public string nodata { get; set; }

        [DataMember]
        public string error { get; set; }
    }

    [DataContract]
    public class AllCandidateDetailsInWard
    {
        [DataMember]
        public string candidatefname { get; set; }

        [DataMember]
        public string candidatelname { get; set; }

        [DataMember]
        public string candidatemname { get; set; }

        [DataMember]
        public string candidatevotes { get; set; }
        [DataMember]
        public string sectionname { get; set; }
        [DataMember]
        public string iswinner { get; set; }
        [DataMember]
        public string partyname { get; set; }
        [DataMember]
        public string nodata { get; set; }

        [DataMember]
        public string error { get; set; }
    }
}