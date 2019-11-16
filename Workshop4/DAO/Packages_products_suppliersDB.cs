using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Workshop4.Controller
{
    /*
     * Author: Ibraheem, Louise
     * Collaborator: DongMing
     */
    public class Packages_products_suppliersDB
    {
        public static List<Packages_products_suppliers> GetPackagesProductsSuppliers()
        {
            List<Packages_products_suppliers> packages_products_suppliers = new List<Packages_products_suppliers>();
            Packages_products_suppliers pck_prod_sup;
            string selectQuery = "SELECT PackageId, ProductSupplierId " +
                                 "FROM Packages_Products_Suppliers " +
                                 "ORDER BY PackageId";

            using (SqlConnection con = TravelExpertsDB.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(selectQuery, con))
                {
                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    while (dr.Read())
                    {
                        pck_prod_sup = new Packages_products_suppliers();
                        pck_prod_sup.PackageId = (int)dr["PackageId"];
                        pck_prod_sup.ProductSupplierId = (int)dr["ProductSupplierId"];

                        packages_products_suppliers.Add(pck_prod_sup);
                    }
                }
            }

            return packages_products_suppliers;
        }

        public static int AddProductPackage(Packages_products_suppliers pck_prod_sup)
        {
            int prodPckId = 0;

            SqlConnection con = TravelExpertsDB.GetConnection();
            string insertStatement = "INSERT INTO Packages_Products_Suppliers (ProductSupplierId) " +
                                       "VALUES(@ProductSupplierId)";
            SqlCommand cmd = new SqlCommand(insertStatement, con);
            cmd.Parameters.AddWithValue("@ProductSupplierId", pck_prod_sup.ProductSupplierId);
            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
                string selectQuery = "SELECT IDENT_CURRENT('Packages_Products_Suppliers') FROM Packages_Products_Suppliers"; // Identity value
                SqlCommand selectCommand = new SqlCommand(selectQuery, con);
                prodPckId = Convert.ToInt32(selectCommand.ExecuteScalar()); // single value
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
            }
            return prodPckId;
        }

        public static int UpdateProductPackage(Packages_products_suppliers oldProductPackage, Packages_products_suppliers newProductPackage)
        {
            int count = 0;
            string updateStatement = "UPDATE Packages_Products_Suppliers SET " +
                                     "ProductSupplierId = @NewProductSupplierId " +
                                     "WHERE PackageId = @OldPackageId " +
                                     "AND ProductSupplierId = @OldProductSupplierId ";

            using (SqlConnection con = TravelExpertsDB.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(updateStatement, con))
                {
                    cmd.Parameters.AddWithValue("@NewProductSupplierId", newProductPackage.ProductSupplierId);

                    cmd.Parameters.AddWithValue("@PackageId", oldProductPackage.PackageId); // PK is not null
                    cmd.Parameters.AddWithValue("@OldProductSupplierId", oldProductPackage.ProductSupplierId);

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
        // Delete products from existing packages
        public static int Delete(int packageId)
        {
            int count = 0;
            string deleteStatement = "DELETE FROM Packages_Products_Suppliers WHERE PackageId=@PackageId";

            using (SqlConnection con = TravelExpertsDB.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(deleteStatement, con))
                {
                    cmd.Parameters.AddWithValue("@PackageId", packageId);

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

        // add products to existing packages
        public static void Add(int packageId, int productSupplierId)
        {
            // create connection
            SqlConnection connection = TravelExpertsDB.GetConnection();

            //create command object
            string sqlInsert = "INSERT INTO Packages_Products_Suppliers (PackageId, ProductSupplierId) " +
                                    "VALUES(@PackageId, @ProductSupplierId)";

            SqlCommand cmd = new SqlCommand(sqlInsert, connection);

            cmd.Parameters.AddWithValue("@PackageId", packageId);
            cmd.Parameters.AddWithValue("@ProductSupplierId", productSupplierId);

            // check
            try
            {
                // open connection
                connection.Open();

                // execute
                cmd.ExecuteScalar();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                connection.Close();
            }

        }

        // add product-supplier to package product supplier
        public static void AddProductsToNewPackage(int packageId, int productSupplierId)
        {
            packageId = 0;
            // create connection
            SqlConnection connection = TravelExpertsDB.GetConnection();

            //create command object
            string sqlInsert = "INSERT INTO Packages_Products_Suppliers (PackageId, ProductSupplierId) " +
                                    "VALUES(@PackageId, @ProductSupplierId)";

            SqlCommand cmd = new SqlCommand(sqlInsert, connection);

            cmd.Parameters.AddWithValue("@PackageId", packageId);
            cmd.Parameters.AddWithValue("@ProductSupplierId", productSupplierId);

            // check
            // check
            try
            {
                // open connection
                connection.Open();

                // execute
                cmd.ExecuteScalar();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                connection.Close();
            }

        }
    }
}
