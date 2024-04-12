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

namespace Platformy_Projekt
{
    public partial class LogInControl : UserControl
    {
        public delegate void UserLoggeInHandler();
        public event UserLoggeInHandler UserLoggedIn;
        public LogInControl()
        {
            InitializeComponent();
        }

        private void loginBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string lgn = login.Text;
                string pswd = passwd.Password;
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
                    LoggedUser.SetInstance(id, dt.Rows[0]["name"].ToString(), dt.Rows[0]["surname"].ToString(), dt.Rows[0]["surname"].ToString(), perms);

                    UserLoggedIn.Invoke();

                    //login.Text = "";
                    //passwd.Password = "";

                    MessageBox.Show("Zalogowano poprawnie");
                }
                else
                {
                    MessageBox.Show("Błędne dane logowania");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błędne dane logowania");
            }
        }
    }
}
