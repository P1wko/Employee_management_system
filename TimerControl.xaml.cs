﻿using System;
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
using System.Windows.Threading;

namespace Platformy_Projekt
{
    /// <summary>
    /// Logika interakcji dla klasy TimerControl.xaml
    /// </summary>
    public partial class TimerControl : UserControl
    {
        private DispatcherTimer timer;
        private TimeSpan elapsedTime;
        private TimeSpan start;

        public TimerControl()
        {
            InitializeComponent();
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            start = TimeSpan.FromHours(8);
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            start -= elapsedTime.Add(TimeSpan.FromSeconds(1));
            Time.Text = start.ToString(@"hh\:mm\:ss");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            timer.Start();
        }
    }
}
