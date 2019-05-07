using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Travel_Experts
{
    /*
     * Author: Ibraheem
     * Collaborator: DongMing
     */
    public static class ProductsDB
    {
        public static List<Products> GetProducts()
        {
            List<Products> products = new List<Products>();
            Products prod;
            string selectQuery = "SELECT ProductId, ProdName " +
                                 "FROM Products " +
                                 "ORDER BY ProductId";

            using (SqlConnection con = TravelExpertsDB.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(selectQuery, con))
                {
                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    while (dr.Read())
                    {
                        prod = new Products();
                        prod.ProductId = (int)dr["ProductId"];
                        prod.ProdName = (string)dr["ProdName"];
                       
                        products.Add(prod);
                    }
                }
            }

            return products;
        }

        public static int AddProduct(Products prod)
        {
            int prodId = 0;

            SqlConnection con = TravelExpertsDB.GetConnection();
            string insertStatement = "INSERT INTO Products (ProdName) " +
                                       "VALUES(@ProdName)";
            SqlCommand cmd = new SqlCommand(insertStatement, con);
            cmd.Parameters.AddWithValue("@ProdName", prod.ProdName);
            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
                string selectQuery = "SELECT IDENT_CURRENT('Products') FROM Products"; // Identity value
                SqlCommand selectCommand = new SqlCommand(selectQuery, con);
                prodId = Convert.ToInt32(selectCommand.ExecuteScalar()); // single value
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
            }
            return prodId;
        }

        // ----- UPDATE -----
        public static int UpdateProduct(Products oldProduct, Products newProduct)
        {
            int count = 0;
            string updateStatement = "UPDATE Products SET " +
                                     "ProdName = @NewProdName " +
                                     "WHERE ProductId = @OldProductId " +
                                     "AND ProdName = @OldProdName ";

            using (SqlConnection con = TravelExpertsDB.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(updateStatement, con))
                {
                    cmd.Parameters.AddWithValue("@NewProdName", newProduct.ProdName);

                    cmd.Parameters.AddWithValue("@OldProductId", oldProduct.ProductId); // PK is not null
                    cmd.Parameters.AddWithValue("@OldProdName", oldProduct.ProdName);

                    try
                    {
                        con.Open();
                        count = cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        con.Close();
                    }
                }
            }
            return count;
        }

//--------------------------- LOUISE ACOSTA ------------------------------

        // select products included in the package
        public static List<Products> GetProductsFromPackage(int packageID)
        {
            List<Products> products = new List<Products>();
            Products prod=null;
            // create connection
            SqlConnection connection = TravelExpertsDB.GetConnection();


            string selectQuery = "SELECT p.ProductId, ProdName " +
                                 "FROM Products p, Products_Suppliers s, Packages_Products_Suppliers r " +
                                 "WHERE p.ProductId=s.ProductId and s.ProductSupplierId=r.ProductSupplierId and PackageId = @PackageId";

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
                    prod = new Products();
                    prod.ProductId = (int)dr["ProductId"];
                    prod.ProdName = (string)dr["ProdName"];

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
