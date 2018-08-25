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
    public class WardWiseSearch
    {
        [DataMember]
        public string distId { get; set; }
        [DataMember]
        public string localBodyType { get; set; }
        [DataMember]
        public string localBodyId { get; set; }
        [DataMember]
        public string wardNo { get; set; }

        [DataMember]
        public string divId { get; set; }
        [DataMember]
        public string eCollId { get; set; }
        [DataMember]
        public string assemblyId { get; set; }
        [DataMember]
        public string maxServerId { get; set; }
    }
}