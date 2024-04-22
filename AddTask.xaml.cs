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

namespace Platformy_Projekt
{
    public partial class AddTask : Window
    {
        private DataTable comboBoxUsersTable;
        public AddTask()
        {
            InitializeComponent();
            SetComboBoxUsers();
        }

        private void SubmitBtnClick(object sender, EventArgs e)
        {
            int selectedUserId = 0;
            foreach (DataRow user in comboBoxUsersTable.Rows)
            {
                if (UsersList.SelectedValue != null && UsersList.SelectedValue.ToString() == (user[1] + " " + user[2]))
                {
                    selectedUserId = (int)user[0];
                }
            }

            if (selectedUserId != 0)
            {
                try
                {
                    ComboBoxItem user = Progress.SelectedItem as ComboBoxItem;
                    string query = $"INSERT INTO tasks (asignee, title, body, progress) VALUES ({selectedUserId}, '{TaskTitle.Text}', '{TaskBody.Text}', '{user.Tag}');";
                    MySqlCommand command = new MySqlCommand(query, DatabaseConnection.Connection);
                    command.ExecuteNonQuery();

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                Close();
            }
            else
            {
                MessageBox.Show("Assignee has not been chosen");
            }
        }

        private void SetComboBoxUsers()
        {
            string query = $"SELECT id, name, surname FROM users;";
            MySqlCommand command = new MySqlCommand(query, DatabaseConnection.Connection);
            command.ExecuteNonQuery();
            MySqlDataAdapter dataAdapter = new MySqlDataAdapter(command);
            comboBoxUsersTable = new DataTable("schedule");
            dataAdapter.Fill(comboBoxUsersTable);
            foreach (DataRow row in comboBoxUsersTable.Rows)
            {
                string user = $"{row["name"]} {row["surname"]}";
                UsersList.Items.Add(user);
            }
        }
    }
}
