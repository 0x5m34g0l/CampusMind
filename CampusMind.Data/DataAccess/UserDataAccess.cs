using CampusMind.Data.Database;
using System;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Net;
using System.Numerics;
using System.Security.Policy;

namespace CampusMind.Data.DataAccess
{
    public class UserDataAccess
    {


        // this method will be called in the Login() method in logic layer.
        public static bool GetUserByEmail(string email, ref int id, ref string passwordHash,ref string name)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString);

            string query = @"SELECT * FROM Users WHERE Email = @Email";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@Email", email);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    // The record was found
                    isFound = true;

                    id = (int)reader["UserID"];
                    passwordHash = (string)reader["PasswordHash"];
                    name = (string)reader["Name"];

                }

                reader.Close();


            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }


            return isFound;
        }
        public static int AddNewUser(string email, string passwordHash, string name)
        {
            // this value will be returned if the user is not added
            int userID = -1;

            SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString);

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
