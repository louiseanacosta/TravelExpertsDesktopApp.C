/**
 * Author: : Louise Acosta
 * Date: March 27, 2019
 **/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Travel_Experts
{
    public static class PackageDB
    {
        /// <summary>
        /// Get Package List from Travel Experts database
        /// </summary>
        /// <returns>A list of all packages</returns>
        public static List<Package> GetPackages(int packageId = 0, string search="")
        {
            List<Package> packages = new List<Package>();

            SqlConnection connection = TravelExpertsDB.GetConnection();

            var sqlSelect = "SELECT PackageId, PkgName, PkgStartDate, PkgEndDate, PkgDesc, "+
                            "PkgBasePrice, PkgAgencyCommission " + 
                            "FROM Packages";


            // check package id
            if(packageId != 0)
            {
                // update sql
                sqlSelect += " WHERE PackageId=@PackageId";

            }

            // check search string
            if (search != "")
            {
                // update query
                sqlSelect += " WHERE (PkgName LIKE @search or PkgDesc LIKE @search)";
            }

            SqlCommand selectCmd = new SqlCommand(sqlSelect, connection);

            // check package id
            if (packageId != 0)
            {
                // bind
                selectCmd.Parameters.AddWithValue("@PackageId", packageId);

            }

            // check search
            if(search != "")
            {
                selectCmd.Parameters.AddWithValue("@search", "%" + search + "%");
            }

            // execute
            try
            {
                // connect
                connection.Open();

                SqlDataReader dr = selectCmd.ExecuteReader(CommandBehavior.CloseConnection);
                while (dr.Read())
                {
                    var package = new Package();
                    package.PackageId = (int)dr["PackageId"];
                    package.PkgName = (string)dr["PkgName"];

                    if (dr["PkgStartDate"] == DBNull.Value)
                        package.PkgStartDate = null;
                    else
                        package.PkgStartDate = (DateTime)dr["PkgStartDate"];

                    if (dr["PkgEndDate"] == DBNull.Value)
                        package.PkgEndDate = null;
                    else
                        package.PkgEndDate = (DateTime)dr["PkgEndDate"];

                    package.PkgDesc = (string)dr["PkgDesc"];


                    package.PkgBasePrice = (decimal)dr["PkgBasePrice"];


                    if (dr["PkgAgencyCommission"] == DBNull.Value)
                        package.PkgAgencyCommission = null;
                    else
                        package.PkgAgencyCommission = (decimal)dr["PkgAgencyCommission"];

                    packages.Add(package);

                }
                dr.Close();

                return packages;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        } // end get package method

        /// <summary>
        /// Adds new package to the database (Packages table)
        /// </summary>
        /// <param name="package">object that contains data of the new package</param>
        /// <returns>auto-generated PackageID</returns>

        public static int AddPackage(Package package)
        {
            int packageID = 0;
            // create connection
            SqlConnection connection = TravelExpertsDB.GetConnection();

            //create command object
            string sqlInsert = "INSERT INTO Packages (PkgName, PkgStartDate, " +
                                    "PkgEndDate, PkgDesc, PkgBasePrice, PkgAgencyCommission) " +
                                    "OUTPUT inserted.[PackageId] " +
                                    "VALUES(@PkgName, @PkgStartDate, @PkgEndDate, @PkgDesc, " +
                                    "@PkgBasePrice, @PkgAgencyCommission)";
            SqlCommand cmd = new SqlCommand(sqlInsert, connection);

            cmd.Parameters.AddWithValue("@PkgName", package.PkgName);
            cmd.Parameters.AddWithValue("@PkgStartDate", package.PkgStartDate);
            cmd.Parameters.AddWithValue("@PkgEndDate", package.PkgEndDate);
            cmd.Parameters.AddWithValue("@PkgDesc", package.PkgDesc);
            cmd.Parameters.AddWithValue("@PkgBasePrice", package.PkgBasePrice);
            cmd.Parameters.AddWithValue("@PkgAgencyCommission", package.PkgAgencyCommission);

            // check
            try
            {
                // open connection
                connection.Open();

                // execute
                packageID = (int)cmd.ExecuteScalar(); 

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                connection.Close();
            }
            return packageID;
        } // end add package

        /// <summary>
        /// Updates current record in the Packages table
        /// </summary>
        /// <param name="oldPackage">data before update</param>
        /// <param name="newPackage">new data for the update</param>
        /// <returns>indicator of success</returns>
        public static int UpdatePackage(Package oldPackage, Package newPackage)
        {
            // connect
            int count = 0;
            SqlConnection connection = TravelExpertsDB.GetConnection();

            string sqlUpdate = "UPDATE Packages SET " +
                                    "PkgName = @NewPkgName, " +
                                    "PkgStartDate = @NewPkgStartDate, " +
                                    "PkgEndDate = @NewPkgEndDate, " +
                                    "PkgDesc = @NewPkgDesc, " +
                                    "PkgBasePrice = @NewPkgBasePrice, " +
                                    "PkgAgencyCommission = @NewPkgAgencyCommission " +

                                    "WHERE PackageID = @OldPackageID " +
                                    "AND PkgStartDate = @OldPkgStartDate " +
                                    "AND PkgEndDate = @OldPkgEndDate " +
                                    "AND PkgDesc = @OldPkgDesc " +
                                    "AND PkgBasePrice = @OldPkgBasePrice " +
                                    "AND PkgAgencyCommission = @OldPkgAgencyCommission";

            SqlCommand cmd = new SqlCommand(sqlUpdate, connection);
            cmd.Parameters.AddWithValue("@NewPkgName", newPackage.PkgName);
            cmd.Parameters.AddWithValue("@NewPkgStartDate", newPackage.PkgStartDate);
            cmd.Parameters.AddWithValue("@NewPkgEndDate", newPackage.PkgEndDate);
            cmd.Parameters.AddWithValue("@NewPkgDesc", newPackage.PkgDesc);
            cmd.Parameters.AddWithValue("@NewPkgBasePrice", newPackage.PkgBasePrice);
            cmd.Parameters.AddWithValue("@NewPkgAgencyCommission", newPackage.PkgAgencyCommission);

            cmd.Parameters.AddWithValue("@OldPackageId", oldPackage.PackageId);
            cmd.Parameters.AddWithValue("@OldPkgName", oldPackage.PkgName);
            cmd.Parameters.AddWithValue("@OldPkgStartDate", oldPackage.PkgStartDate);
            cmd.Parameters.AddWithValue("@OldPkgEndDate", oldPackage.PkgEndDate);
            cmd.Parameters.AddWithValue("@OldPkgDesc", oldPackage.PkgDesc);
            cmd.Parameters.AddWithValue("@OldPkgBasePrice", oldPackage.PkgBasePrice);
            cmd.Parameters.AddWithValue("@OldPkgAgencyCommission", oldPackage.PkgAgencyCommission);

            // check
            try
            {
                // open connection
                connection.Open();
                count = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                connection.Close();
            }
            return count;
        } // end update package

        /// <summary>
        /// Delete package
        /// </summary>
        /// <param name="package">package to delete</param>
        /// <returns>indicator of success</returns>
        public static bool DeletePackage(Package package)
        {
            bool success = true;
            // connect
            SqlConnection connection = TravelExpertsDB.GetConnection();
            string sqlDelete = "DELETE FROM Packages " +
                                    "WHERE PackageId = @PackageId " + // to identify record
                                    "AND PkgName = @PkgName " + // remaining: for optimistic concurrency
                                    "AND PkgStartDate = @PkgStartDate " +
                                    "AND PkgEndDate = @PkgEndDate " +
                                    "AND PkgDesc = @PkgDesc " +
                                    "AND PkgBasePrice = @PkgBasePrice " +
                                    "AND PkgAgencyCommission = @PkgAgencyCommission";

            SqlCommand cmd = new SqlCommand(sqlDelete, connection);
            cmd.Parameters.AddWithValue("@PackageId", package.PackageId);
            cmd.Parameters.AddWithValue("@PkgName", package.PkgName);
            cmd.Parameters.AddWithValue("@PkgStartDate", package.PkgStartDate);
            cmd.Parameters.AddWithValue("@PkgEndDate", package.PkgEndDate);
            cmd.Parameters.AddWithValue("@PkgDesc", package.PkgDesc);
            cmd.Parameters.AddWithValue("@PkgBasePrice", package.PkgBasePrice);
            cmd.Parameters.AddWithValue("@PkgAgencyCommission", package.PkgAgencyCommission);


            // check
            try
            {
                // open connection
                connection.Open();
                int count = cmd.ExecuteNonQuery();
                // check if 0
                if (count == 0) 
                    success = false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                connection.Close();
            }
            
            return success;
        }

    } //end class
}// end name space
