using System;
using MySql.Data.MySqlClient;




namespace Westbot
{
    public class DBConnection
    {
        public DBConnection()
        {
        }
        public string DatabaseName { get; set; } = string.Empty;

        public string Password { get; set; }
        public MySqlConnection Connection { get; private set; } = null;
        public string DatabaseName1 { get => DatabaseName; set => DatabaseName = value; }

        private static DBConnection _instance = null;
        public static DBConnection Instance()
        {
            if (_instance == null)
                _instance = new DBConnection();
            return _instance;
        }

        public bool IsConnect()
        {
            if (Connection == null)
            {
                if (string.IsNullOrEmpty(DatabaseName))
                    return false;
                string connstring = string.Format("Server=localhost; database={0}; UID=root; password=GoodGames!", DatabaseName);
                Connection = new MySqlConnection(connstring);
                Connection.Open();
            }

            return true;
        }

        public void Close()
        {
            Connection.Close();
        }

        public void Connect()
        {
            var dbCon = DBConnection.Instance();
            dbCon.DatabaseName = "Test";
            if (dbCon.IsConnect())
            {
                string query = "SELECT main_capcom_id,discord_id FROM server_users";
                var cmd = new MySqlCommand(query, dbCon.Connection);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string someStringFromColumnZero = reader.GetString(0);
                    string someStringFromColumnOne = reader.GetString(1);
                    Console.WriteLine(someStringFromColumnZero + "," + someStringFromColumnOne);
                }
                dbCon.Close();
            }
        }
    }
}
