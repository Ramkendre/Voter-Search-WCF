using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace LatestVoterSearch
{
   
    public class Report
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["VoterSearchConStr"].ConnectionString);
        SqlConnection seccon = new SqlConnection(ConfigurationManager.ConnectionStrings["SecConStr"].ConnectionString);
        SqlConnection sectempcon = new SqlConnection(ConfigurationManager.ConnectionStrings["SecTempConStr"].ConnectionString);

    
    }
}