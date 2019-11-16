using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Workshop4.Controller
{
    public class Products_suppliersDB
    {
        /*
         * Author: Ibraheem
         * Collaborator: DongMing Hu
         */

        // ----- READ -----
        public static List<Products_suppliers> GetProductsSuppliers()
        {
            List<Products_suppliers> prod_suppliers = new List<Products_suppliers>();
            Products_suppliers prod_sup;
            string selectQuery = "SELECT ProductSupplierId, ProductId, SupplierId " +
                                 "FROM Products_Suppliers " +
                                 "ORDER BY ProductSupplierId";

            using (SqlConnection con = TravelExpertsDB.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(selectQuery, con))
                {
                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    while (dr.Read())
                    {
                        prod_sup = new Products_suppliers();
                        prod_sup.ProductSupplierId = (int)dr["ProductSupplierId"];

                        // Get the ProductId column number
                        int ProductIdColIndex = dr.GetOrdinal("ProductId");
                        if (dr.IsDBNull(ProductIdColIndex))
                            prod_sup.ProductId = null;
                        else
                            prod_sup.ProductId = Convert.ToInt32(dr["ProductId"]);

                        int SupplierIdColIndex = dr.GetOrdinal("SupplierId");
                        if (dr.IsDBNull(SupplierIdColIndex))
                            prod_sup.SupplierId = null;
                        else
                            prod_sup.SupplierId = Convert.ToInt32(dr["SupplierId"]);

                        prod_suppliers.Add(prod_sup);
                    }
                }
            }

            return prod_suppliers;
        }

        /// <summary>
        /// Query all Product Suppliers with names instead of ids
        /// </summary>
        /// <returns>A list of ProductSupplierWithName object.</returns>
        public static List<ProductSupplierWithName> GetAllProductSupplierWithNames()
        {
            // setting
            var psList = new List<ProductSupplierWithName>();
            var sqlText = "SELECT ps.ProductSupplierId, p.ProdName, s.SupName " +
                          "FROM Products p, Products_Suppliers ps, Suppliers s " +
                          "WHERE p.ProductId = ps.ProductId " +
                          "AND s.SupplierId = ps.SupplierId";
            var connection = TravelExpertsDB.GetConnection();
            var cmd = new SqlCommand(sqlText, connection);
            // execute
            connection.Open();
            using (var dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
            {
                while (dr.Read())
                {
                    var ps = new ProductSupplierWithName
                    {
                        ProductSupplierId = Convert.ToInt32(dr[0]),
                        ProdName = dr[1].ToString(),
                        // SupName is a nullable column in DB
                        SupName = dr[2] == DBNull.Value ? null : dr[2].ToString()
                    };
                    psList.Add(ps);
                }
            }
            // reader is close when exit using statement, meanwhile connection is closed by CommandBehavior

            return psList;
        }


        // ----- CREATE -----
        public static int AddProductSupplier(Products_suppliers prod_sup)
        {
            int prodSupId = 0;

            SqlConnection con = TravelExpertsDB.GetConnection();
            string insertStatement = "INSERT INTO Products_Suppliers (ProductId, SupplierId) " +
                                       "VALUES(@ProductId, @SupplierId)";
            SqlCommand cmd = new SqlCommand(insertStatement, con);
            if (prod_sup.ProductId == null)
                cmd.Parameters.AddWithValue("@ProductId", DBNull.Value);
            else
                cmd.Parameters.AddWithValue("@ProductId", prod_sup.ProductId);

            if (prod_sup.SupplierId == null)
                cmd.Parameters.AddWithValue("@SupplierId", DBNull.Value);
            else
                cmd.Parameters.AddWithValue("@SupplierId", prod_sup.SupplierId);

            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
                string selectQuery = "SELECT IDENT_CURRENT('Products_Suppliers') FROM Products_Suppliers"; // Identity value
                SqlCommand selectCommand = new SqlCommand(selectQuery, con);
                prodSupId = Convert.ToInt32(selectCommand.ExecuteScalar()); // single value
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
            }
            return prodSupId;
        }


        // ----- UPDATE -----
        public static int UpdateProductSupplier(Products_suppliers oldProdSupplier, Products_suppliers newProdSupplier)
        {
            int count = 0;
            string updateStatement = "UPDATE Products_Suppliers SET " +
                                     "ProductId = @NewProductId, " +
                                     "SupplierId = @NewSupplierId " +
                                     "WHERE ProductSupplierId = @OldProductSupplierId " +
                                     "AND (ProductId = @OldProductId " +
                                     " OR ProductId IS NULL AND @OldProductId IS NULL)" +
                                     "AND (SupplierId = @OldSupplierId " +
                                     " OR SupplierId IS NULL AND @OldSupplierId IS NULL)";

            using (SqlConnection con = TravelExpertsDB.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(updateStatement, con))
                {
                    if (newProdSupplier.ProductId == null)
                        cmd.Parameters.AddWithValue("@NewProductId", DBNull.Value);
                    else
                        cmd.Parameters.AddWithValue("@NewProductId", newProdSupplier.ProductId);

                    if (newProdSupplier.SupplierId == null)
                        cmd.Parameters.AddWithValue("@NewSupplierId", DBNull.Value);
                    else
                        cmd.Parameters.AddWithValue("@NewSupplierId", newProdSupplier.SupplierId);

                    cmd.Parameters.AddWithValue("@OldProductSupplierId", oldProdSupplier.ProductSupplierId); // PK is not null

                    if (oldProdSupplier.ProductId == null)
                        cmd.Parameters.AddWithValue("@OldProductId", DBNull.Value);
                    else
                        cmd.Parameters.AddWithValue("@OldProductId", oldProdSupplier.ProductId);

                    if (oldProdSupplier.SupplierId == null)
                        cmd.Parameters.AddWithValue("@OldSupplierId", DBNull.Value);
                    else
                        cmd.Parameters.AddWithValue("@OldSupplierId", oldProdSupplier.SupplierId);

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
    }
}
