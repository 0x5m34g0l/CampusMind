using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Data;
using System.Data.SqlClient;

namespace CampusMind.Data1.Database
{
    // No object can be created from this class.
    // This class holds data settings only.
    public static class DataAccessSettings
    {
        private static string _connectionString;

        public static string ConnectionString
        {
            get
            {
                if (_connectionString == null)
                    _connectionString = LoadFromJson();

                return _connectionString;
            }
        }

        private static string LoadFromJson()
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "dbsettings.json");

            if (!File.Exists(path))
                throw new Exception($"dbsettings.json not found at: {path}");

            string json = File.ReadAllText(path);

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true // makes "connectionString" work too
            };

            DbSettings? settings = JsonSerializer.Deserialize<DbSettings>(json, options);

            if (settings == null)
                throw new Exception("dbsettings.json is invalid: JSON deserialized to null.");

            if (string.IsNullOrWhiteSpace(settings.ConnectionString))
                throw new Exception("dbsettings.json is missing 'ConnectionString' value.");

            return settings.ConnectionString;
        }

    }
}
