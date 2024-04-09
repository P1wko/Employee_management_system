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

namespace Platformy_Projekt
{
    /// <summary>
    /// Logika interakcji dla klasy ScheduleControl.xaml
    /// </summary>
    public partial class ScheduleControl : UserControl
    {
        

        public ScheduleControl()
        {
            InitializeComponent();

            int dayOfWeek;
            DateTime today;
            int days;
            today = DateTime.Today;
            DateTime firstOfMonth = new DateTime(today.Year, today.Month, 1);
            dayOfWeek = (int)firstOfMonth.DayOfWeek-1;
            days=DateTime.DaysInMonth(today.Year, today.Month);

            int counter = 1;
            for (int i=0; i<days; i++)
            {
                TextBox date = new TextBox();
                date.Text = $"{i + 1}";
                date.Name = $"a{i + 1}";
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
    }
}
