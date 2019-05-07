//--------------------------- LOUISE ACOSTA ------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Travel_Experts
{
    public class ProductsInPackageDB
    {
        // select products that are included in the package
        public static List<ProductsInPackage> GetProductsFromPackage(int packageID)
        {
            List<ProductsInPackage> products = new List<ProductsInPackage>();
            ProductsInPackage prod = null;
            // create connection
            SqlConnection connection = TravelExpertsDB.GetConnection();


            string selectQuery = "SELECT q.ProductSupplierId, p.ProdName, s.SupName " +
                                 "FROM Products p, Products_Suppliers q, Suppliers s, Packages t, " +
                                 "Packages_Products_Suppliers u " +
                                 "WHERE p.ProductId = q.ProductId and s.SupplierId = q.SupplierId " +
                                 "and q.ProductSupplierId = u.ProductSupplierId and t.PackageId = u.PackageId " +
                                 "and t.PackageId = @PackageId " +
                                 "ORDER BY ProdName";

            SqlCommand cmd = new SqlCommand(selectQuery, connection);
            cmd.Parameters.AddWithValue("@PackageID", packageID);


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
                    prod = new ProductsInPackage();

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

        public static List<ProductsInPackage> GetProducts()
        {
            List<ProductsInPackage> products = new List<ProductsInPackage>();
            return products;
        }
    }
}
