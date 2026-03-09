using CampusMind.Data1.Database;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace CampusMind.Data1.DataAccess
{
    public static class MessageDataAccess
    {

        static int Create(int conversationId, int role, string content)
        {
            int messageId = -1;

            SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString);

            string query = @"INSERT INTO Messages (MessageRole, Content, ConversationID)
                             VALUES (@Role, @Content, @ConversationID);
                             SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@Role", role);
            command.Parameters.AddWithValue("@Content", content);
            command.Parameters.AddWithValue("@ConversationID", conversationId);

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                messageId = Convert.ToInt32(result);
            }
            catch (Exception ex)
            {
                throw new Exception("SQL error: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }

            return messageId;
        }


        static bool GetMessageById(int messageId, ref int role, ref string content, ref int conversationId)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString);

            string query = @"SELECT * FROM Messages WHERE MessageID = @MessageID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@MessageID", messageId);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;

                    role = (int)reader["MessageRole"];
                    content = (string)reader["Content"];
                    conversationId = (int)reader["ConversationID"];
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


        static List<int> GetMessagesByConversationId(int conversationId)
        {
            List<int> messageIds = new List<int>();

            SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString);

            string query = @"SELECT MessageID
                             FROM Messages
                             WHERE ConversationID = @ConversationID
                             ORDER BY CreatedAt ASC";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ConversationID", conversationId);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    messageIds.Add((int)reader["MessageID"]);
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

            return messageIds;
        }


        static bool GetLastMessage(int conversationId, ref int messageId, ref int role, ref string content)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString);

            string query = @"SELECT TOP 1 *
                             FROM Messages
                             WHERE ConversationID = @ConversationID
                             ORDER BY CreatedAt DESC";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ConversationID", conversationId);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;

                    messageId = (int)reader["MessageID"];
                    role = (int)reader["MessageRole"];
                    content = (string)reader["Content"];
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


        static bool DeleteByConversationId(int conversationId)
        {
            int rowsAffected = 0;

            SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionString);

            string query = @"DELETE FROM Messages 
                             WHERE ConversationID = @ConversationID";

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
    }
}