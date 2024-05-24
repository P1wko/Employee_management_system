using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
using Mysqlx.Crud;
using Mysqlx.Datatypes;
using System.Xml.Linq;
using MySql.Data.MySqlClient;
using System.Data;
using Org.BouncyCastle.Bcpg;
using Org.BouncyCastle.Tls;
using Microsoft.VisualBasic;

namespace Platformy_Projekt
{
    public partial class AddUser : Window
    {
        private bool isEdited = false;
        private bool passChanged = false;
        private string userId;
        public AddUser()
        {
            InitializeComponent();
        }

        public void FetchUser(string id)
        {
            try
            {
                userId = id;
                string query = $"SELECT users.name, users.surname, users.permissions, users.login, users.password, users.salary, users.email FROM users WHERE users.id = {userId}";
                MySqlCommand command = new MySqlCommand(query, DatabaseConnection.Connection);
                command.ExecuteNonQuery();
                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(command);
                DataTable dt = new DataTable("user");
                dataAdapter.Fill(dt);

                Name.Text = dt.Rows[0][0].ToString();
                Surname.Text = dt.Rows[0][1].ToString();

                foreach (ComboBoxItem item in Permissions.Items)
                {
                    if (item.Tag != null && item.Tag.ToString() == dt.Rows[0][2].ToString())
                    {
                        Permissions.SelectedItem = item.ToString();
                        break;
                    }
                }

                Email.Text = dt.Rows[0][6].ToString();
                Permissions.Text = dt.Rows[0][2].ToString();
                Login.Text = dt.Rows[0][3].ToString();
                Password.Password = dt.Rows[0][4].ToString();
                passChanged = false;
                Salary.Text = dt.Rows[0][5].ToString();
            }
            catch(Exception e)
            {
                MessageBox.Show(e.ToString());
            }
            isEdited = true;
        }

        private void Confirm(object sender, RoutedEventArgs e)
        {

            ComboBoxItem comboBoxItem = new ComboBoxItem();
            comboBoxItem = Permissions.SelectedItem as ComboBoxItem;
            int perm = int.Parse(comboBoxItem.Tag.ToString());

            try
            {
                string query;
                if (!isEdited)
                {
                    query = $"INSERT INTO users (name, surname, permissions, login, password, salary, email) VALUES ('{Name.Text}', '{Surname.Text}', {perm}, '{Login.Text}', '{HashPassword.HashString(Password.Password)}', {float.Parse(Salary.Text)}, '{Email.Text}')";
                }
                else 
                { 
                    if (!passChanged)
                    {
                        query = $"UPDATE users SET name = '{Name.Text}', surname = '{Surname.Text}', permissions = {perm}, " +
                           $"login = '{Login.Text}', password = '{Password.Password}', " +
                           $"salary = {float.Parse(Salary.Text)}, email = '{Email.Text}' " +
                           $"WHERE id = {userId};";
                    }
                    else
                    {
                        query = $"UPDATE users SET name = '{Name.Text}', surname = '{Surname.Text}', permissions = {perm}, " +
                           $"login = '{Login.Text}', password = '{HashPassword.HashString(Password.Password)}', " +
                           $"salary = {float.Parse(Salary.Text)}, email = '{Email.Text}' " +
                           $"WHERE id = {userId};";
                    }
                }
                MySqlCommand command = new MySqlCommand(query, DatabaseConnection.Connection);
                command.ExecuteNonQuery();
                    
            }
            catch(Exception f)
            {
                MessageBox.Show(f.ToString());
            }
            Close();
        }

        private void PasswordChanger(object sender, RoutedEventArgs e)
        {
            passChanged = true;

        }

    }
}
