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
     */
    public class SuppliersDB
    {
        public static List<Suppliers> GetSuppliers()
        {
            List<Suppliers> suppliers = new List<Suppliers>();
            Suppliers sup;
            string selectQuery = "SELECT SupplierId, SupName " +
                                 "FROM Suppliers " +
                                 "ORDER BY SupplierId";

            using (SqlConnection con = TravelExpertsDB.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(selectQuery, con))
                {
                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    while (dr.Read())
                    {
                        sup = new Suppliers();
                        sup.SupplierId = (int)dr["SupplierId"];

                        // Get the SupName column number
                        int supNameColIndex = dr.GetOrdinal("SupName");
                        if (dr.IsDBNull(supNameColIndex))
                            sup.SupName = null;
                        else
                            sup.SupName = dr["SupName"].ToString();

                        suppliers.Add(sup);
                    }
                }
            }

            return suppliers;
        }

        public static int AddSupplier(Suppliers sup)
        {
            int supId = 0;

            SqlConnection con = TravelExpertsDB.GetConnection();
            string insertStatement = "INSERT INTO Suppliers(SupplierId,SupName) VALUES(@SupId,@SupName)";
            SqlCommand cmd = new SqlCommand(insertStatement, con);
            // bind parameters
            cmd.Parameters.AddWithValue("@SupId", sup.SupplierId);

            if (sup.SupName == null)
                cmd.Parameters.AddWithValue("@SupName", DBNull.Value);
            else
                cmd.Parameters.AddWithValue("@SupName", sup.SupName);

            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
                string selectQuery = "SELECT * FROM Suppliers WHERE SupplierId=@SupId"; // Identity value
                SqlCommand selectCommand = new SqlCommand(selectQuery, con);
                selectCommand.Parameters.AddWithValue("@SupId", sup.SupplierId);
                supId = Convert.ToInt32(selectCommand.ExecuteScalar()); // single value
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
            }
            return supId;
        }

        public static int UpdateSupplier(Suppliers oldSupplier, Suppliers newSupplier)
        {
            int count = 0;
            string updateStatement = "UPDATE Suppliers SET " +
                                     "SupName = @NewSupName " +
                                     "WHERE SupplierId = @OldSupplierId " +
                                     "AND (SupName = @OldSupName " +
                                     " OR SupName IS NULL AND @OldSupName IS NULL)";

            using (SqlConnection con = TravelExpertsDB.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(updateStatement, con))
                {
                    if (newSupplier.SupName == null)
                        cmd.Parameters.AddWithValue("@NewSupName", DBNull.Value);
                    else
                        cmd.Parameters.AddWithValue("@NewSupName", newSupplier.SupName);

                    cmd.Parameters.AddWithValue("@OldSupplierId", oldSupplier.SupplierId); // PK is not null
                    
                    if (oldSupplier.SupName == null)
                        cmd.Parameters.AddWithValue("@OldSupName", DBNull.Value);
                    else
                        cmd.Parameters.AddWithValue("@OldSupName", oldSupplier.SupName);

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
