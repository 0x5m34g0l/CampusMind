using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace CampusMind.Data.Database
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
                throw new Exception("dbsettings.json not found");

            string json = File.ReadAllText(path);

            DbSettings settings = JsonSerializer.Deserialize<DbSettings>(json);

            return settings.ConnectionString;
        }
    }
}
