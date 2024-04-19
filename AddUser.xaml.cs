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

namespace Platformy_Projekt
{
    /// <summary>
    /// Interaction logic for AddUser.xaml
    /// </summary>
    public partial class AddUser : Window
    {
        public AddUser()
        {
            InitializeComponent();
        }
        private void Confirm(object sender, RoutedEventArgs e)
        {

            ComboBoxItem comboBoxItem = new ComboBoxItem();
            comboBoxItem = Permissions.SelectedItem as ComboBoxItem;
            int perm = int.Parse(comboBoxItem.Tag.ToString());

            try
            {
                string query = $"INSERT INTO users (name, surname, permissions, login, password, salary, email) VALUES ('{Name.Text}', '{Surname.Text}', {perm}, '{Login.Text}', '{Password.Password}', {float.Parse(Salary.Text)}, '{Email.Text}')";
                MySqlCommand command = new MySqlCommand(query, DatabaseConnection.Connection);
                command.ExecuteNonQuery();
            }
            catch
            {
                MessageBox.Show("Nie wypełniono wszystkich pól");
            }
            Close();
        }

    }
}
