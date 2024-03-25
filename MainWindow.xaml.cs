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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //string connectionString = "SERVER=sql11.freemysqlhosting.net;DATABASE=sql11692625;UID=sql11692625;PASSWORD=RA3KJ7Ehva;";
        string connectionString = "SERVER=127.0.0.1;DATABASE=ems;UID=root;PASSWORD=admin;";
        public MainWindow()
        {
            InitializeComponent();
        }

        private void employeesBtn_Click(object sender, RoutedEventArgs e)
        {
            DataGrid usersGrid = new DataGrid();
            contentGrid.Children.Add(usersGrid);
            usersGrid.Margin = new Thickness(0, 0, 0, 0);
            usersGrid.GridLinesVisibility = DataGridGridLinesVisibility.None;
            usersGrid.IsReadOnly = true;
            usersGrid.AutoGenerateColumns = true;

            MySqlConnection connection = new MySqlConnection(connectionString);
            try
            {
                connection.Open();

                string query = "SELECT * FROM users";
                MySqlCommand command = new MySqlCommand(query, connection);

                command.ExecuteNonQuery();

                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(command);
                DataTable dt = new DataTable("users");

                dataAdapter.Fill(dt);

                dt.Columns[0].ColumnName = "ID";
                dt.Columns[1].ColumnName = "Name";
                dt.Columns[2].ColumnName = "Surname";
                dt.Columns[3].ColumnName = "Salary";

                usersGrid.ItemsSource = dt.DefaultView;
                
                dataAdapter.Update(dt);
                connection.Close();
            } catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
