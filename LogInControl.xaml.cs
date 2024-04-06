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
    /// <summary>
    /// Logika interakcji dla klasy LogInControl.xaml
    /// </summary>
    public partial class LogInControl : UserControl
    {
        public event EventHandler UserLoggedIn;
        DatabaseData? databaseConn;
        string? connectionString;
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
                string query = "SELECT * FROM users WHERE login='" + lgn + "';";
                MySqlCommand command = new MySqlCommand(query, DatabaseConnection.Connection);
                command.ExecuteNonQuery();
                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(command);
                DataTable dt = new DataTable("siema");
                dataAdapter.Fill(dt);

                if (pswd == dt.Rows[0]["password"].ToString())
                {
                    MessageBox.Show("Zalogowano poprawnie");
                    UserLoggedIn.Invoke(this, EventArgs.Empty);
                    login.Text = "";
                    passwd.Password = "";
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
