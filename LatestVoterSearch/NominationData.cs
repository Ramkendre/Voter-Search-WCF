using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace LatestVoterSearch
{
   [DataContract]
    public class NominationData
    {
       [DataMember]
       public string candidatemobno { get; set; }
       [DataMember]
       public string firstname { get; set; }
       [DataMember]
       public string middlename { get; set; }
       [DataMember]
       public string lastname { get; set; }
       [DataMember]
       public string education { get; set; }
       [DataMember]
       public string movableproperty { get; set; }
       [DataMember]
       public string immovableproperty { get; set; }
       [DataMember]
       public string income { get; set; }
       [DataMember]
       public string occuption { get; set; }
       [DataMember]
       public string totalassets { get; set; }
       [DataMember]
       public string convicated { get; set; }
       [DataMember]
       public string congnizance { get; set; }
       [DataMember]
       public string totalcases { get; set; }
       [DataMember]
       public string spouse { get; set; }

    }
}