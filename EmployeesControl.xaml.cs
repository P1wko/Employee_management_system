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
using System.Xml.Linq;


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
            //DataGrid usersGrid = new DataGrid();
            //contentGrid.Children.Add(usersGrid);
            //usersGrid.Margin = new Thickness(0, 0, 0, 0);
            //usersGrid.GridLinesVisibility = DataGridGridLinesVisibility.None;
            //usersGrid.IsReadOnly = true;
            //usersGrid.AutoGenerateColumns = true;

            try
            {
                string query = "SELECT id, name, surname, login, salary, email FROM users";
                MySqlCommand command = new MySqlCommand(query, DatabaseConnection.Connection);

                command.ExecuteNonQuery();

                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(command);
                DataTable dt = new DataTable("users");
                dataAdapter.Fill(dt);

                int counter = 0;

                foreach (DataRow row in dt.Rows)
                {
                    contentGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(40) });

                    TextBox IDTextbox = new TextBox();
                    setStyle(IDTextbox);
                    IDTextbox.Text = row.Field<int>("ID").ToString();
                    Grid.SetColumn(IDTextbox, 1);
                    Grid.SetRow(IDTextbox, counter);
                    contentGrid.Children.Add(IDTextbox);

                    TextBox nameTextbox = new TextBox();
                    setStyle(nameTextbox);
                    nameTextbox.Text = row.Field<string>("name");
                    Grid.SetColumn(nameTextbox, 2);
                    Grid.SetRow(nameTextbox, counter);
                    contentGrid.Children.Add(nameTextbox);

                    TextBox surnameTextbox = new TextBox();
                    setStyle(surnameTextbox);
                    surnameTextbox.Text = row.Field<string>("surname");
                    Grid.SetColumn(surnameTextbox, 3);
                    Grid.SetRow(surnameTextbox, counter);
                    contentGrid.Children.Add(surnameTextbox);

                    TextBox loginTextbox = new TextBox();
                    setStyle(loginTextbox);
                    loginTextbox.Text = row.Field<string>("login");
                    Grid.SetColumn(loginTextbox, 4);
                    Grid.SetRow(loginTextbox, counter);
                    contentGrid.Children.Add(loginTextbox);

                    TextBox salaryTextbox = new TextBox();
                    setStyle(salaryTextbox);
                    salaryTextbox.Text = row.Field<float>("salary").ToString();
                    Grid.SetColumn(salaryTextbox, 5);
                    Grid.SetRow(salaryTextbox, counter);
                    contentGrid.Children.Add(salaryTextbox);

                    TextBox emailTextbox = new TextBox();
                    setStyle(emailTextbox);
                    emailTextbox.Text = row.Field<string>("email");
                    Grid.SetColumn(emailTextbox, 6);
                    Grid.SetRow(emailTextbox, counter);
                    contentGrid.Children.Add(emailTextbox);

                    ComboBox actions = new ComboBox();
                    actions.ItemsSource = getActionsColumnItems();
                    actions.VerticalAlignment = VerticalAlignment.Center;
                    actions.HorizontalAlignment = HorizontalAlignment.Center;
                    Grid.SetColumn(actions, 0);
                    Grid.SetRow(actions, counter);

                    contentGrid.Children.Add(actions);


                    counter++;
                }
                //dataAdapter.Update(dt);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private List<Button> getActionsColumnItems()
        {
            List<Button> items = new List<Button>();
            Button sendMsg = new Button
            {
                BorderThickness = new Thickness(0),
                Background = Brushes.Red,
                Width = 100,
                FontFamily = new FontFamily("Yu Gothic UI Light"),
                FontWeight = FontWeights.Bold,
            };
            sendMsg.Content = "Send Message";
            items.Add(sendMsg);

            Button edit = new Button
            {
                BorderThickness = new Thickness(0),
                Background = Brushes.Red,
                Width = 100,
                FontFamily = new FontFamily("Yu Gothic UI Light"),
                FontWeight = FontWeights.Bold,
            };
            edit.Content = "Edit";
            items.Add(edit);

            Button delete = new Button
            {
                BorderThickness = new Thickness(0),
                Background = Brushes.Red,
                Width = 100,
                FontFamily = new FontFamily("Yu Gothic UI Light"),
                FontWeight = FontWeights.Bold,
            };
            delete.Content = "Delete";
            items.Add(delete);

            return items;
        }

        private void setStyle(TextBox element)
        {
            element.VerticalAlignment = VerticalAlignment.Center;
            element.HorizontalAlignment = HorizontalAlignment.Center;
            element.Background = Brushes.Transparent;
            element.FontFamily = new FontFamily("Yu Gothic UI Light");
            element.FontSize = 16;
            element.IsReadOnly = true;
            element.BorderThickness = new Thickness(0);
            element.FontWeight = FontWeights.Bold;
            element.Cursor = Cursors.Arrow;
        }
    }
}
