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
    /// <summary>
    /// Logika interakcji dla klasy ScheduleControl.xaml
    /// </summary>
    public partial class ScheduleControl : UserControl
    {
        private DateTime today;
        
        LoggedUser loggedUser;
        public ScheduleControl()
        {
            InitializeComponent();
            loggedUser = LoggedUser.GetInstance();
            today = DateTime.Today;
            DrawCalendar();
            PaintCalendar();
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
                    Background = new SolidColorBrush(Color.FromRgb(118, 171, 174)),
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
                TextBox date = new TextBox
                {
                    Text = $"{i + 1}",
                    Name = $"a{i + 1}",
                    Background = new SolidColorBrush(Color.FromRgb(185, 185, 185)),
                    IsReadOnly = true,
                    FontFamily = new FontFamily("Yu Gothic UI Light"),
                    FontSize = 16,
                    BorderThickness = new Thickness(2),
                    FontWeight = FontWeights.Bold,
                    Cursor = Cursors.Arrow
                };
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

                string query = $"SELECT date, shift FROM schedule WHERE workerId = {loggedUser.Id} AND MONTH(date)={(int)today.Month} AND YEAR(date)={(int)today.Year};";
                MySqlCommand command = new MySqlCommand(query, DatabaseConnection.Connection);
                command.ExecuteNonQuery();
                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(command);
                DataTable dt = new DataTable("schedule");
                dataAdapter.Fill(dt);

                foreach (DataRow row in dt.Rows)
                {
                    int day = row.Field<DateTime>("date").Day;

                    string name = $"a{day}";

                    TextBox? textbox = (TextBox?)Calendar.FindName(name);

                    
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

        private void ButtonR_Click(object sender, RoutedEventArgs e)
        {
            foreach (var child in Calendar.Children)
            {
                if (child is TextBox textBox && !string.IsNullOrEmpty(textBox.Name))
                {
                    UnregisterName(textBox.Name);
                }
            }
            Calendar.Children.Clear();

            today = today.AddMonths(1);
            DrawCalendar();
            PaintCalendar();
        }

        private void ButtonL_Click(object sender, RoutedEventArgs e)
        {
            foreach (var child in Calendar.Children)
            {
                if (child is TextBox textBox && !string.IsNullOrEmpty(textBox.Name))
                {
                    UnregisterName(textBox.Name);
                }
            }
            Calendar.Children.Clear();

            today = today.AddMonths(-1);
            DrawCalendar();
            PaintCalendar();
        }
    }
}
