using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace LatestVoterSearch
{
    [DataContract]
    public class VoterAnalysis_Class
    {
        [DataMember]
        public int APPID { get; set; }
        [DataMember]
        public string ACNO { set; get; }
        [DataMember]
        public string PART_NO { set; get; }
        [DataMember]
        public string SRNO { set; get; }
        [DataMember]
        public string HOUSE_NO { set; get; }
        [DataMember]
        public string SECTION_NO { set; get; }

        [DataMember]
        public string FNAME_M { set; get; }
        [DataMember]
        public string LNAME_M { set; get; }
        [DataMember]
        public string RELATION_TYPE { set; get; }
        [DataMember]
        public string RELATION_FNAME_M { set; get; }
        [DataMember]
        public string RELATION_LNAME_M { set; get; }



        [DataMember]
        public string IDCARD_ID { set; get; }
        [DataMember]
        public string STATUS_TYPE { set; get; }
        [DataMember]
        public string SEX { set; get; }
        [DataMember]
        public string AGE { set; get; }
        [DataMember]
        public string FNAME_E { set; get; }
        [DataMember]
        public string LNAME_E { set; get; }
        [DataMember]
        public string RELATION_FNAME_E { set; get; }
        [DataMember]
        public string RELATION_LNAME_E { set; get; }
        [DataMember]
        public string FULLNAME_E { set; get; }
        [DataMember]
        public string EB_NO { set; get; }


        [DataMember]
        public string ALLOCATED_WARD { set; get; }
        [DataMember]
        public string SERIALNO_INWARD { set; get; }
        [DataMember]
        public string BOOTH_NO { set; get; }
        [DataMember]
        public string SERIALNO_FOR_FINAL_LIST { set; get; }
        [DataMember]
        public string OLD_SERIALIN_WARD { set; get; }
        [DataMember]
        public string PHOTO { set; get; }
        [DataMember]
        public string VUI_CODE { set; get; }
        [DataMember]
        public string Election_ID { set; get; }
        [DataMember]
        public string ST_CODE { set; get; }
        [DataMember]
        public string FVTM_TYPE { set; get; }
        [DataMember]
        public string SEGMENT_NO { set; get; }



        [DataMember]
        public string FVTM_NO { set; get; }
        [DataMember]
        public string SECN_CATY { set; get; }
        [DataMember]
        public string STARTSL_NO { set; get; }
        [DataMember]
        public string HADBAST_NO { set; get; }
        [DataMember]
        public string PINCODE { set; get; }
        [DataMember]
        public string Dist_no { set; get; }
        [DataMember]
        public string section_name_v1 { set; get; }
        [DataMember]
        public string section_name_en { set; get; }
        [DataMember]
        public string section_id { set; get; }
        [DataMember]
        public string AC_Name_V1 { set; get; }
        [DataMember]
        public string AC_Name_En { set; get; }


        [DataMember]
        public string Part_Name_V1 { set; get; }
        [DataMember]
        public string Part_Name_En { set; get; }
        [DataMember]
        public string District_ID { set; get; }
        [DataMember]
        public string District_Name_En { set; get; }
        [DataMember]
        public string District_Name_V1 { set; get; }
        [DataMember]
        public string DistId { set; get; }
        [DataMember]
        public string LocalBodyType { set; get; }
        [DataMember]
        public string LocalBodyId { set; get; }


        [DataMember]
        public string COLOR { set; get; }
        [DataMember]
        public string CASTE { set; get; }
        [DataMember]
        public string MOB_NUM { set; get; }
        [DataMember]
        public string CATEGORY { set; get; }
        [DataMember]
        public string Qualification { set; get; }
        [DataMember]
        public string Occupation { set; get; }
        [DataMember]
        public string CreatedBy { set; get; }
        [DataMember]
        public string CreatedDate { set; get; }
        [DataMember]
        public string ModifyBy { set; get; }
        [DataMember]
        public string ModifyDate { set; get; }
    }
}