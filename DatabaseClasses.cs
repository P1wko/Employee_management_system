using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System.IO;
using Org.BouncyCastle.Bcpg.Sig;

namespace Platformy_Projekt
{
    public class DatabaseData
    {
        public string Address { get; set; }
        public string Name { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
    }

    public class DatabaseConnection
    {
        public static MySqlConnection Connection { get; set; }

        public static void GetConnection()
        {
            DatabaseData databaseConn = GetDatabaseData();
            string connectionString = $"SERVER={databaseConn.Address};DATABASE={databaseConn.Name};UID={databaseConn.Login};PASSWORD={databaseConn.Password};";
            Connection = new MySqlConnection(connectionString);
            Connection.Open();
        }

        public static void ConnectionClose()
        {
            Connection.Close();
        }

        private static DatabaseData GetDatabaseData()
        {
            string jsonFile = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.json");
            DatabaseData? databaseData = new DatabaseData();
            try
            {
                string jsonContent = File.ReadAllText(jsonFile);
                databaseData = JsonConvert.DeserializeObject<DatabaseData>(jsonContent);

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            return databaseData;
        }
    }

    public class LoggedUser
    {
        private static LoggedUser instance;
        public int Id { get; set; }
        public string? UserName { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public int Permissions { get; set; }

        private LoggedUser(int id, string? userName, string? name, string? surname, int permissions)
        {
            Id = id;
            UserName = userName;
            Name = name;
            Surname = surname;
            Permissions = permissions;
        }

        public static void SetInstance(int id, string? userName, string? name, string? surname, int permissions)
        {
            instance = new LoggedUser(id, userName, name, surname, permissions);
        }

        public static LoggedUser GetInstance()
        {
                return instance;
        }
    }

    enum  Months
    {
        January,
        February,
        March,
        April,
        May,
        June,
        July,
        August,
        September,
        October,
        November,
        December,
    }
}
