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
using Mysqlx.Crud;

namespace Platformy_Projekt
{
    public partial class MainWindow : Window
    {
        private LogInControl logInControl = new LogInControl();
        private LoggedUser loggedUser;

        private TimerControl timeControl;
        private MessagesControl messagesControl;
        private ScheduleControl scheduleControl;
        private EmployeesControl employeesControl;
        private TasksControll tasksControll;
        private Message messWind;

        public MainWindow()
        {
            InitializeComponent();

            contentGrid.Content = logInControl;
            logInControl.UserLoggedIn += userLoggedIn;

            DatabaseConnection.GetConnection();
        }

        private void userLoggedIn()
        {
            employeesBtn.IsEnabled = true;
            messagesBtn.IsEnabled = true;
            tasksBtn.IsEnabled = true;
            timerBtn.IsEnabled = true;
            scheduleBtn.IsEnabled = true;
            logoutBtn.IsEnabled = true;

            loggedUser = LoggedUser.GetInstance();

            timeControl = new TimerControl();
            messagesControl = new MessagesControl();
            messagesControl.ButtonClicked += ChangeMessageControlWindow;
            scheduleControl = new ScheduleControl();
            employeesControl = new EmployeesControl();
            tasksControll = new TasksControll();

            contentGrid.Content = null;
        }

        private void employeesBtn_Click(object sender, RoutedEventArgs e)
        {
            contentGrid.Content = employeesControl;
            bolding((Button)sender);
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

            bolding((Button)null);
        }

        private void messagesBtn_Click(object sender, RoutedEventArgs e)
        {
            contentGrid.Content = messagesControl;

            bolding((Button)sender);
        }

        private void timerBtn_Click(object sender, RoutedEventArgs e)
        {
            contentGrid.Content = timeControl;

            bolding((Button)sender);
        }

        private void scheduleBtn_Click(object sender, RoutedEventArgs e)
        {
            contentGrid.Content = scheduleControl;

            bolding((Button)sender);
        }
        private void tasksBtn_Click(object sender, RoutedEventArgs e)
        {
            contentGrid.Content = tasksControll;
            bolding((Button)sender);
        }

        private void bolding(Button btn)
        {
            employeesBtn.FontFamily = new FontFamily("Yu Gothic UI Light");
            messagesBtn.FontFamily = new FontFamily("Yu Gothic UI Light");
            tasksBtn.FontFamily = new FontFamily("Yu Gothic UI Light");
            timerBtn.FontFamily = new FontFamily("Yu Gothic UI Light");
            scheduleBtn.FontFamily = new FontFamily("Yu Gothic UI Light");
            logoutBtn.FontFamily = new FontFamily("Yu Gothic UI Light");
            if(btn != null)
            {
                btn.FontFamily = new FontFamily("Yu Gothic UI");
            }
        }

        private void ChangeMessageControlWindow()
        {
            messWind = new Message();
            messWind.BackButtonClicked += GetBackToMessages;
            messagesControl.MessageOpen += messWind.Fetchmessage;
            messWind.MessageDeleteChangeWindow += GetBackToMessages;
            messWind.MessageDelete += messagesControl.DeleteMessage;
            contentGrid.Content = messWind;
        }

        private void GetBackToMessages()
        {
            contentGrid.Content = messagesControl;
            messagesControl.MessageOpen -= messWind.Fetchmessage;
        }

    }

    
}
