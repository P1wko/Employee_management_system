using System;
using System.Collections.Generic;
using System.Data;
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
using System.Windows.Shapes;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Bcpg.OpenPgp;

namespace Platformy_Projekt
{
    /// <summary>
    /// Interaction logic for SendMessage.xaml
    /// </summary>
    public partial class SendMessage : Window
    {

        private DataTable UsersDataTable;
        private LoggedUser loggedUser;

        public SendMessage()
        {   
            InitializeComponent();
            loggedUser = LoggedUser.GetInstance();
            FetchComboboxUsersList();
        }

        private void FetchComboboxUsersList()
        {
            try
            {
                string query = $"SELECT id, name, surname FROM users";
                MySqlCommand command = new MySqlCommand(query, DatabaseConnection.Connection);
                command.ExecuteNonQuery();
                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(command);
                UsersDataTable = new DataTable("ComboBoxUsersList");
                dataAdapter.Fill(UsersDataTable);
            }
            catch
            {
                MessageBox.Show("Wystąpił błąd podczas pobierania listy użytkowników");
            }

            foreach (DataRow user in UsersDataTable.Rows) {

                string userNameAndSurname = user[1] + " " + user[2];
                UsersList.Items.Add(userNameAndSurname);
            }
        }

        private void OnSendMessageClick(object sender, RoutedEventArgs e)
        {
            int selectedUserId = 0;
            foreach (DataRow user in UsersDataTable.Rows) {
                if(UsersList.SelectedValue.ToString() == (user[1] + " " + user[2]))
                {
                    selectedUserId = (int)user[0];
                }
            }

            if (selectedUserId != 0)
            {
                try
                {

                    string query = $"INSERT INTO messages (senderId, title, message, addresseeId) VALUES ({loggedUser.Id}, '{Title.Text}', '{MessageContent.Text}', {selectedUserId})";
                    MySqlCommand command = new MySqlCommand(query, DatabaseConnection.Connection);
                    command.ExecuteNonQuery();

                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

            Close();

        }

        public void OnReplyClick(string nameAndSurname)
        {
            UsersList.SelectedValue = nameAndSurname;
        }

    }
}
