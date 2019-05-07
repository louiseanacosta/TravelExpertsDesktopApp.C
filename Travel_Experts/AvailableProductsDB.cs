using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//--------------------------- LOUISE ACOSTA ------------------------------

namespace Travel_Experts
{
    public class AvailableProductsDB
    {

        // select products that are not yet included in packages
        public static List<AvailableProducts> GetAvailableProducts(string search = "")
        {
            List<AvailableProducts> products = new List<AvailableProducts>();
            AvailableProducts prod = null;
            // create connection
            SqlConnection connection = TravelExpertsDB.GetConnection();


            string selectQuery = "SELECT q.ProductSupplierId, p.ProdName, s.SupName " +
                                "FROM Products p, Products_Suppliers q, Suppliers s " +
                                "WHERE p.ProductId = q.ProductId and " +
                                "s.SupplierId = q.SupplierId and ProductSupplierId NOT IN " +
                                "(SELECT ProductSupplierId FROM Packages_Products_Suppliers) ";

            // check search string
            if(search != "")
            {
                // update query
                selectQuery += " AND (p.ProdName LIKE @search or s.SupName LIKE @search)";
            }

            // add order to query
            selectQuery += " ORDER BY ProdName";

            SqlCommand cmd = new SqlCommand(selectQuery, connection);

            // check search
            if (search != "")
            {
                // bind
                cmd.Parameters.AddWithValue("@search", "%" + search + "%");

            }

            // check
            try
            {
                // open the connection
                connection.Open();

                // execute the SELECT query
                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                while (dr.Read()) // we have a customer
                {
                    // create new object
                    prod = new AvailableProducts();

                    prod.ProductSupplierId = (int)dr["ProductSupplierId"];
                    prod.ProdName = (string)dr["ProdName"];
                    prod.SupName = (string)dr["SupName"];

                    products.Add(prod);

                }
                dr.Close();

                return products;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
    }
}
