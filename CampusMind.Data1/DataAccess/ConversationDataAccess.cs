using CampusMind.Data1.Database;
using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Net;
using System.Numerics;
using System.Security.Policy;
using static System.Net.WebRequestMethods;


namespace CampusMind.Data1.DataAccess
{
    public static class ConversationDataAccess
    {

        public static int Create(int toolType, string title, int userId)
        {
            int conversationID = -1;

            SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString);

            string query = @"INSERT INTO Conversations (ToolType,Title,UserID)
                             VALUES (@ToolType, @Title, @UserID);
                             SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ToolType", toolType);
            command.Parameters.AddWithValue("@Title", title);
            command.Parameters.AddWithValue("@UserID", userId);

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                conversationID = Convert.ToInt32(result);

            }

            catch (Exception ex)
            {
                throw new Exception("SQL error: " + ex.Message);

            }

            finally
            {
                connection.Close();
            }


            return conversationID;


        }

        public static bool GetConversationById(int conversationId, ref int toolType, ref string title, ref int userId)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString);

            string query = @"SELECT * FROM Conversations WHERE ConversationID = @ConversationID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ConversationID", conversationId);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    // The record was found
                    isFound = true;

                    toolType = (int)reader["ToolType"];
                    title = (string)reader["Title"];
                    userId = (int)reader["UserID"];

                }

                reader.Close();


            }
            catch (Exception ex)
            {
                throw new Exception("SQL error: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }


            return isFound;
        }

        public static  bool UpdateTitle(int conversationId, string newTitle)
        {
            int rowsAffected = -1;
            SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString);

            string query = @"UPDATE Conversations 
                             SET Title=@NewTitle 
                             WHERE ConversationID=@ConversationID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@NewTitle", newTitle);
            command.Parameters.AddWithValue("@ConversationID", conversationId);

            try
            {
                connection.Open();
                rowsAffected = command.ExecuteNonQuery();

            }

            catch (Exception ex)
            {
                throw new Exception("SQL error: " + ex.Message);

            }

            finally
            {
                connection.Close();
            }


            return (rowsAffected > 0);
        }

        public static bool DeleteConversation(int conversationId)
        {
            int rowsAffected = 0;

            SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString);

            string query = @"Delete FROM Conversations 
                                where ConversationID = @ConversationID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ConversationID", conversationId);

            try
            {
                connection.Open();

                rowsAffected = command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                throw new Exception("SQL error: " + ex.Message);
            }
            finally
            {

                connection.Close();

            }

            return (rowsAffected > 0);
        }

        public static List<int> GetConversationsByUserId(int userId)
        {
            List<int> conversationIds = new List<int>();

            SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString);

            string query = @"SELECT ConversationID 
                     FROM Conversations 
                     WHERE UserID = @UserID
                     ORDER BY CreatedAt DESC";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@UserID", userId);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    conversationIds.Add((int)reader["ConversationID"]);
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("SQL error: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }

            return conversationIds;
        }


    }
    }

