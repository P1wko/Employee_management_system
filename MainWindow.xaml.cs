using System;
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
        Boolean isAuthed = false;
        LogInControl logInControl = new LogInControl();
        
        public MainWindow()
        {
            InitializeComponent();

            contentGrid.Content = logInControl;
            logInControl.UserLoggedIn += userLoggedIn;
            DatabaseConnection.GetConnection();
        }

        private void userLoggedIn(object sender, EventArgs e)
        {
            employeesBtn.IsEnabled = true;
            messagesBtn.IsEnabled = true;
            tasksBtn.IsEnabled = true;
            timerBtn.IsEnabled = true;
            scheduleBtn.IsEnabled = true;
            logoutBtn.IsEnabled = true;

        }

        private void employeesBtn_Click(object sender, RoutedEventArgs e)
        {
            contentGrid.Content = new EmployeesControl();
        }


        private void MainWindow_OnClosing_(object? sender, CancelEventArgs e)
        {
            DatabaseConnection.ConnectionClose();
        }
    }

    
}
