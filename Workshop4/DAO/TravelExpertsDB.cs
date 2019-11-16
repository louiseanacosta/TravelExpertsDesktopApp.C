using System.Data.SqlClient;

namespace Workshop4.Controller
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
