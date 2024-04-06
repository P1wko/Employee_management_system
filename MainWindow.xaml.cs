﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using Newtonsoft.Json;
using System.IO;

namespace Platformy_Projekt
{
    public partial class MainWindow : Window
    {
        LogInControl logInControl = new LogInControl();
        private LoggedUser loggedUser;
        
        public MainWindow()
        {
            InitializeComponent();

            contentGrid.Content = logInControl;
            logInControl.UserLoggedIn += userLoggedIn;
            DatabaseConnection.GetConnection();
        }

        private void userLoggedIn(int id, string? name, string? surname, string? login, int perms)
        {
            employeesBtn.IsEnabled = true;
            messagesBtn.IsEnabled = true;
            tasksBtn.IsEnabled = true;
            timerBtn.IsEnabled = true;
            scheduleBtn.IsEnabled = true;
            logoutBtn.IsEnabled = true;

            loggedUser = new LoggedUser(id, login, name, surname, perms);
            contentGrid.Content = null;
        }

        private void employeesBtn_Click(object sender, RoutedEventArgs e)
        {
            contentGrid.Content = new EmployeesControl();
        }


        private void MainWindow_OnClosing_(object? sender, CancelEventArgs e)
        {
            DatabaseConnection.ConnectionClose();
        }

        private void logoutBtn_Click(object sender, RoutedEventArgs e)
        {
            employeesBtn.IsEnabled = false;
            messagesBtn.IsEnabled = false;
            tasksBtn.IsEnabled = false;
            timerBtn.IsEnabled = false;
            scheduleBtn.IsEnabled = false;
            logoutBtn.IsEnabled = false;

            contentGrid.Content = logInControl;
        }
    }

    
}
