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
using System.Reflection;

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
            //usersGrid.IsReadOnly = true;
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
                //dt.Columns[2].ColumnName = "Department";
                //dt.Columns[3].ColumnName = "E-mail";
                dt.Columns[3].ColumnName = "Salary";
                //dt.Columns[3].ColumnName = "Actions";

                usersGrid.ItemsSource = dt.DefaultView;

                DataGridComboBoxColumn actionsColumn = new DataGridComboBoxColumn();
                actionsColumn.Header = "Actions";
                actionsColumn.ItemsSource = getActionsColumnItems();

                //actionsColumn.SelectedValueBinding = new Binding("ID");

                usersGrid.Columns.Add(actionsColumn);
                
                dataAdapter.Update(dt);
                connection.Close();
            } catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private List<Button> getActionsColumnItems()
        {
            List<Button> items = new List<Button>();
            Button sendMsg = new Button();
            sendMsg.Content = "Send Message";
            items.Add(sendMsg);

            Button edit = new Button();
            edit.Content = "Edit";
            items.Add(edit);

            Button delete = new Button();
            delete.Content = "Delete";
            items.Add(delete);

            return items;
        }

        private void loginBtn_Click(object sender, RoutedEventArgs e)
        {
            MySqlConnection connection = new MySqlConnection(connectionString);
            try
            {
                connection.Open();
                string lgn = login.Text;
                string pswd = passwd.Text;
                string query = "SELECT * FROM users WHERE login='" + lgn + "';";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.ExecuteNonQuery();
                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(command);
                DataTable dt = new DataTable("siema");
                dataAdapter.Fill(dt);

                //DataGrid loginy = new DataGrid();
                //contentGrid.Children.Add(loginy);
                //loginy.AutoGenerateColumns = true;
                //loginy.ItemsSource = dt.DefaultView;

                if (pswd == dt.Rows[0]["password"].ToString())
                {
                    employeesBtn.IsEnabled = true;
                    tasksBtn.IsEnabled = true;
                    messagesBtn.IsEnabled = true;
                    logoutBtn.IsEnabled = true;
                    timerBtn.IsEnabled = true;
                    MessageBox.Show("dobrze");
                }


            } catch (Exception ex)
            {
                MessageBox.Show("Błędne dane logowania");
            }
        }
    }
}
