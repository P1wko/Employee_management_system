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
    /// Logika interakcji dla klasy EmployeesControl.xaml
    /// </summary>
    public partial class EmployeesControl : UserControl
    {
        DatabaseData? databaseConn;
        string? connectionString;
        public EmployeesControl()
        {
            InitializeComponent();
            ShowEmployees();
        }

        private void ShowEmployees()
        {
            DataGrid usersGrid = new DataGrid();
            contentGrid.Children.Add(usersGrid);
            usersGrid.Margin = new Thickness(0, 0, 0, 0);
            usersGrid.GridLinesVisibility = DataGridGridLinesVisibility.None;
            //usersGrid.IsReadOnly = true;
            usersGrid.AutoGenerateColumns = true;

            try
            {
                string query = "SELECT * FROM users";
                MySqlCommand command = new MySqlCommand(query, DatabaseConnection.Connection);

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
            }
            catch (Exception ex)
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
    }
}
