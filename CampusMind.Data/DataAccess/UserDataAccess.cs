using CampusMind.Data.Database;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Security.Policy;

namespace CampusMind.Data.DataAccess
{
    public class UserDataAccess
    {
        
        public static int AddNewUser(string email, string passwordHash, string name)
        {
            // this value will be returned if the user is not found
            int userID = -1;

            SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString);

            // my question here will the date be automaitaclly updated?
            string query = @"INSERT INTO Users (Email,PasswordHash,Name)
                             VALUES (@Email, @PasswordHash, @Name);
                             SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@Email", email);
            command.Parameters.AddWithValue("@PasswordHash", passwordHash);
            command.Parameters.AddWithValue("@Name", name);

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                userID = Convert.ToInt32(result);

            }

            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);

            }

            finally
            {
                connection.Close();
            }


            return userID;


        }
    }
}
