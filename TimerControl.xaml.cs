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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using MySql.Data.MySqlClient;

namespace Platformy_Projekt
{
    /// <summary>
    /// Logika interakcji dla klasy TimerControl.xaml
    /// </summary>
    public partial class TimerControl : UserControl
    {
        private DispatcherTimer timer;
        private TimeSpan current;
        private LoggedUser loggedUser;
        private int shift = 0;

        public TimerControl()
        {
            InitializeComponent();

            loggedUser = LoggedUser.GetInstance();

            FetchShift();
            SetTimer();
            
        }

        private void FetchShift()
        {
            try
            {
                string date = DateTime.Today.ToString("yyyy-MM-dd");
                string query = $"SELECT shift FROM schedule WHERE workerId={loggedUser.Id} AND date='{date}';";
                MySqlCommand command = new MySqlCommand(query, DatabaseConnection.Connection);
                command.ExecuteNonQuery();
                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(command);
                DataTable dt = new DataTable("shift");
                dataAdapter.Fill(dt);

                if (dt.Rows.Count != 0)
                {
                    if ((int)dt.Rows[0][0] == 1) shift = 1;
                    else if ((int)dt.Rows[0][0] == 2) shift = 2;
                }

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (current.TotalMinutes < 0)
            {
                Time.Text = "It looks like you're free for today :)!";
                Time.FontSize = 61;
            }
            else
            {
                Time.Text = current.ToString(@"hh\:mm");
            }
        }

        private void SetTimer()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;

            switch (shift)
            {
                case 1:
                    current = DateTime.Today.AddHours(17) - DateTime.Now;
                    timer.Start();
                    break;
                case 2:
                    current = DateTime.Today.AddHours(22) - DateTime.Now;
                    timer.Start();
                    break;
                default:
                    Time.Text = "It looks like you're free for today :)";
                    Time.FontSize = 61;
                    break;
            }
        }
    }
}
