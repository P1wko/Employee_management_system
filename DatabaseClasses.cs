using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System.IO;
using System.Security.Cryptography;

namespace Platformy_Projekt
{
    public class Credentials
    {
        public int Remembered {  get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
    }
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

        private LoggedUser(int id, string? name, string? surname, string? userName, int permissions)
        {
            Id = id;
            UserName = userName;
            Name = name;
            Surname = surname;
            Permissions = permissions;
        }

        public static void SetInstance(int id,string? name, string? surname, string? userName, int permissions)
        {
            instance = new LoggedUser(id, name, surname, userName, permissions);
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

    public class HashPassword
    {
        public static string HashString(string text)
        {
            using SHA256 sha = SHA256.Create();
            byte[] bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(text));

            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }

            return builder.ToString();
        } 
    }
}
