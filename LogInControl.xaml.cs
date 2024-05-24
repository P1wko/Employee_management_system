using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MySql.Data.MySqlClient;
using System.Data;
using Newtonsoft.Json;
using System.IO;
using System.Net;


namespace Platformy_Projekt
{
    public partial class LogInControl : UserControl
    {
        public delegate void UserLoggeInHandler();
        public event UserLoggeInHandler UserLoggedIn;
        private string RemLogin;
        private string RemPassword;
        private bool isRemembered;
        public LogInControl()
        {
            InitializeComponent();
            rememberMe();
        }

        private void rememberMe()
        {
            string jsonFile = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "rememberMe.json");
            try
            {
                string jsonContent = File.ReadAllText(jsonFile);
                Credentials credentials = JsonConvert.DeserializeObject<Credentials>(jsonContent);

                if(credentials.Remembered == 0)
                {
                    isRemembered = false;
                }
                else { isRemembered= true; }

                if (isRemembered)
                {
                    RemLogin = credentials.Login;
                    RemPassword = credentials.Password;
                    login.Text = RemLogin;
                    passwd.Password = "12345";
                    rememberChck.IsChecked= true;
                }

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        private void loginBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string lgn;
                string pswd;
                if (isRemembered)
                {
                    lgn = RemLogin;
                    pswd = RemPassword;
                }
                else
                {
                    lgn = login.Text;
                    pswd = HashPassword.HashString(passwd.Password);
                }
                string query = $"SELECT * FROM users WHERE login='{lgn}';";
                MySqlCommand command = new MySqlCommand(query, DatabaseConnection.Connection);
                command.ExecuteNonQuery();
                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(command);
                DataTable dt = new DataTable("siema");
                dataAdapter.Fill(dt);

                if (pswd == dt.Rows[0]["password"].ToString())
                {
                    int id = Convert.ToInt16(dt.Rows[0]["id"].ToString());
                    int perms = Convert.ToInt16(dt.Rows[0]["permissions"].ToString());
                    LoggedUser.SetInstance(id, dt.Rows[0]["name"].ToString(), dt.Rows[0]["surname"].ToString(), dt.Rows[0]["login"].ToString(), perms);

                    UserLoggedIn.Invoke();

                    //login.Text = "";
                    //passwd.Password = "";

                    MessageBox.Show("Zalogowano poprawnie");
                    saveCreds(lgn, pswd);
                }
                else
                {
                    MessageBox.Show("Błędne dane logowania");
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }



        private void saveCreds(string login, string passwd)
        {
            string jsonFile = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "rememberMe.json");
            try
            {
                string jsonContent = File.ReadAllText(jsonFile);
                Credentials credentials = JsonConvert.DeserializeObject<Credentials>(jsonContent);

                if (rememberChck.IsChecked==true)
                {
                    credentials.Remembered = 1;
                    credentials.Login = login;
                    credentials.Password = passwd;
                }
                else
                {
                    credentials.Remembered = 0;
                    credentials.Login = "";
                    credentials.Password = "";
                }

                string updatedJsonContent = JsonConvert.SerializeObject(credentials);
                File.WriteAllText(jsonFile, updatedJsonContent);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
    }
}
