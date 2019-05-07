using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Travel_Experts
{
    /*
     * Author: DongMing Hu
     */
    public class TravelExpertsDB
    {
        public static SqlConnection GetConnection()
        {
            return new SqlConnection(@"Data Source=localhost\sqlexpress;Initial Catalog=TravelExperts;Integrated Security=True");
        }
    }
}
