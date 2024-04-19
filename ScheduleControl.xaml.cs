using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MySql.Data.MySqlClient;

namespace Platformy_Projekt
{
    public partial class ScheduleControl : UserControl
    {
        private DateTime today;
        
        LoggedUser loggedUser;
        private int userId;
        private DataTable comboBoxUsersTable;
        private int setShift;
        public ScheduleControl()
        {
            InitializeComponent();

            loggedUser = LoggedUser.GetInstance();
            userId = loggedUser.Id;
            setShift = -1;

            today = DateTime.Today;
            DrawCalendar();
            PaintCalendar();
            SetComboBoxUsers();

            CheckPerms();
        }

        private void DrawCalendar()
        {
            DayOfWeek dayLabel = DayOfWeek.Monday;
            for(int i = 0; i < 7; i++)
            {
                TextBox label = new TextBox
                {
                    Text = dayLabel.ToString(),
                    IsReadOnly = true,
                    FontFamily = new FontFamily("Yu Gothic UI Light"),
                    FontSize = 16,
                    FontWeight = FontWeights.Bold,
                    Background = new SolidColorBrush(Color.FromRgb(49, 54, 63)),
                    Foreground = new SolidColorBrush(Color.FromRgb(221, 221, 221)),
                    BorderThickness = new Thickness(0),
                    TextAlignment = TextAlignment.Center,
                    VerticalContentAlignment = VerticalAlignment.Center,
                    Cursor = Cursors.Arrow
                };
                Grid.SetColumn(label, i);
                Grid.SetRow(label, 0);
                Calendar.Children.Add(label);
                dayLabel = dayLabel < DayOfWeek.Saturday ? dayLabel + 1 : DayOfWeek.Sunday;
            }

            Month.Text = ((Months)today.Month - 1) + " " + (today.Year);

            DateTime firstOfMonth = new DateTime(today.Year, today.Month, 1);
            int dayOfWeek = (int)firstOfMonth.DayOfWeek - 1;
            if (dayOfWeek < 0) dayOfWeek = 6;

            int days = DateTime.DaysInMonth(today.Year, today.Month);

            int counter = 1;
            for (int i = 0; i < days; i++)
            {
                Button date = new Button
                {
                    Content = $"{i + 1}",
                    Name = $"a{i + 1}",
                    Background = new SolidColorBrush(Color.FromRgb(185, 185, 185)),
                    FontFamily = new FontFamily("Yu Gothic UI Light"),
                    FontSize = 16,
                    BorderThickness = new Thickness(1),
                    FontWeight = FontWeights.Bold,
                    Cursor = Cursors.Arrow
                };
                date.Click += SetShift;
                RegisterName($"a{i + 1}", date);
                Grid.SetColumn(date, dayOfWeek);
                Grid.SetRow(date, counter);
                Calendar.Children.Add(date);
                dayOfWeek += 1;
                if (dayOfWeek % 7 == 0)
                {
                    counter++;
                    dayOfWeek = 0;
                }

            }
        }

        private void PaintCalendar()
        {
            try
            {

                string query = $"SELECT date, shift FROM schedule WHERE workerId = {userId} AND MONTH(date)={(int)today.Month} AND YEAR(date)={(int)today.Year};";
                MySqlCommand command = new MySqlCommand(query, DatabaseConnection.Connection);
                command.ExecuteNonQuery();
                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(command);
                DataTable dt = new DataTable("schedule");
                dataAdapter.Fill(dt);

                foreach (DataRow row in dt.Rows)
                {
                    int day = row.Field<DateTime>("date").Day;

                    string name = $"a{day}";

                    Button? textbox = (Button?)Calendar.FindName(name);

                    
                    int shift = (int)row["shift"];
                    switch (shift)
                    {
                        case 1:
                            textbox.Background = new SolidColorBrush(Color.FromRgb(235, 237, 130));
                            break;
                        case 2:
                            textbox.Background = new SolidColorBrush(Color.FromRgb(113, 134, 217));
                            break;
                        case 3:
                            textbox.Background = new SolidColorBrush(Color.FromRgb(217, 113, 208));
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
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

            UsersList.SelectedItem = $"{loggedUser.Name} {loggedUser.Surname}";
        }

        private void SelectedUserChanged(object sender, SelectionChangedEventArgs args)
        {
            userId = (int)comboBoxUsersTable.Rows[UsersList.SelectedIndex]["id"];
            refreshCalendar();
        }

        private void ButtonR_Click(object sender, RoutedEventArgs e)
        {
            today = today.AddMonths(1);
            refreshCalendar();
        }

        private void ButtonL_Click(object sender, RoutedEventArgs e)
        {
            today = today.AddMonths(-1);
            refreshCalendar();
        }

        private void refreshCalendar()
        {
            foreach (var child in Calendar.Children)
            {
                if (child is Button textBox && !string.IsNullOrEmpty(textBox.Name))
                {
                    UnregisterName(textBox.Name);
                }
            }
            Calendar.Children.Clear();
            DrawCalendar();
            PaintCalendar();
        }

        private void NoInfoBtn_Click(object sender, RoutedEventArgs e)
        {
            setShift = 0;
        }

        private void MorningShiftBtn_Click(object sender, RoutedEventArgs e)
        {
            setShift = 1;
        }

        private void EveningShiftBtn_Click(object sender, RoutedEventArgs e)
        {
            setShift = 2;
        }

        private void DayOffBtn_Click(object sender, RoutedEventArgs e)
        {
            setShift = 3;
        }

        private void SetShift(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            Brush buttonBackground = button.Background;
            

            if(setShift >= 0) {
                try
                {
                    if (buttonBackground.ToString() != "#FFB9B9B9")
                    {
                        string query = $"DELETE FROM `schedule` WHERE `workerId` = {userId} AND `date` = '{today.Year + "-" + today.Month + "-" + button.Content + " "}';";
                        MySqlCommand command = new MySqlCommand(query, DatabaseConnection.Connection);
                        command.ExecuteNonQuery();
                    }
                    else
                    {
                        string query = $"INSERT INTO `schedule` (`workerId`, `date`, `shift`) VALUES ('{userId}', '{today.Year + "-" + today.Month + "-" + button.Content + " "}', '{setShift}');";
                        MySqlCommand command = new MySqlCommand(query, DatabaseConnection.Connection);
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            refreshCalendar();
        }

        private void CheckPerms()
        {
            switch (loggedUser.Permissions)
            {
                case 1:
                    break;
                case 2:
                    break;
                case 3:
                    UsersList.Visibility = Visibility.Hidden;
                    NoInfoBtn.Click -= NoInfoBtn_Click;
                    DayOffBtn.Click -= DayOffBtn_Click;
                    EveShiftBtn.Click -= EveningShiftBtn_Click;
                    MornShiftBtn.Click -= MorningShiftBtn_Click;
                    break;
                default:
                    UsersList.Visibility = Visibility.Hidden;
                    NoInfoBtn.Click -= NoInfoBtn_Click;
                    DayOffBtn.Click -= DayOffBtn_Click;
                    EveShiftBtn.Click -= EveningShiftBtn_Click;
                    MornShiftBtn.Click -= MorningShiftBtn_Click;
                    break;
            }
        }
    }
}
