﻿using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Resources;
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
    public partial class TasksControll : UserControl
    {
        LoggedUser loggedUser;
        private int userId;
        private int idToDel;
        private DataTable dt;


        public delegate void TaskOpening();
        public event TaskOpening TaskToOpen;

        public delegate void OnTaskOpened(string title, string employee, string desc, string status);
        public event OnTaskOpened TaskOpened;
        public TasksControll()
        {
            InitializeComponent();
            loggedUser = LoggedUser.GetInstance();
            userId = loggedUser.Id;
            DrawTasks();
        }



        private void DrawTasks()
        {
            try
            {

                string query = $"SELECT id, title, body, progress FROM tasks WHERE asignee = {userId};";
                MySqlCommand command = new MySqlCommand(query, DatabaseConnection.Connection);
                command.ExecuteNonQuery();
                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(command);
                dt = new DataTable("tasks");
                dataAdapter.Fill(dt);

                int counter1 = 0;
                int counter2 = 0;
                int counter3 = 0;
                int counter4 = 0;

                foreach (DataRow row in dt.Rows)
                {
                    TextBox task = new TextBox
                    {
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        TextWrapping = TextWrapping.Wrap,
                        VerticalAlignment = VerticalAlignment.Stretch,
                        VerticalContentAlignment = VerticalAlignment.Center,
                        HorizontalContentAlignment = HorizontalAlignment.Center,
                        IsReadOnly = true,
                        Cursor = Cursors.Hand
                    };
                    task.PreviewMouseLeftButtonDown += DragTextbox;
                    task.MouseDoubleClick += OpenTask;

                    task.Text = row.Field<string>("title");
                    if (row.Field<string>("progress") == "todo")
                    {
                        toDoGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(40) });
                        Grid.SetRow(task, counter1);
                        toDoGrid.Children.Add(task);
                        counter1++;
                    }
                    else if(row.Field<string>("progress") == "inprogress")
                    {
                        inProgressGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(40) });
                        Grid.SetRow(task, counter2);
                        inProgressGrid.Children.Add(task);
                        counter2++;
                    }
                    else if (row.Field<string>("progress") == "waiting")
                    {
                        waitingGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(40) });
                        Grid.SetRow(task, counter3);
                        waitingGrid.Children.Add(task);
                        counter3++;
                    }
                    else if (row.Field<string>("progress") == "completed")
                    {
                        completedGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(40) });
                        Grid.SetRow(task, counter4);
                        completedGrid.Children.Add(task);
                        counter4++;
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }


        private void CompletedOnDrop(object sender, DragEventArgs e)
        {
            try
            {
                if (e.Data.GetData(typeof(TextBox)) is TextBox dragedTextBox)
                {
                    string query = $"UPDATE `tasks` SET `progress` = 'completed' WHERE `tasks`.`title` = '{dragedTextBox.Text}';";
                    MySqlCommand command = new MySqlCommand(query, DatabaseConnection.Connection);
                    command.ExecuteNonQuery();
                }
                refreshTasks();
            }
            catch (Exception f)
            {
                MessageBox.Show(f.Message);
            }
        }

        private void WaitingOnDrop(object sender, DragEventArgs e)
        {
            try
            {
                if (e.Data.GetData(typeof(TextBox)) is TextBox dragedTextBox)
                {
                    string query = $"UPDATE `tasks` SET `progress` = 'waiting' WHERE `tasks`.`title` = '{dragedTextBox.Text}';";
                    MySqlCommand command = new MySqlCommand(query, DatabaseConnection.Connection);
                    command.ExecuteNonQuery();
                }
                refreshTasks();
            }
            catch (Exception f)
            {
                MessageBox.Show(f.Message);
            }
        }

        private void InProgressOnDrop(object sender, DragEventArgs e)
        {
            try
            {
                if (e.Data.GetData(typeof(TextBox)) is TextBox dragedTextBox)
                {
                    string query = $"UPDATE `tasks` SET `progress` = 'inprogress' WHERE `tasks`.`title` = '{dragedTextBox.Text}';";
                    MySqlCommand command = new MySqlCommand(query, DatabaseConnection.Connection);
                    command.ExecuteNonQuery();
                }
                refreshTasks();
            }
            catch (Exception f)
            {
                MessageBox.Show(f.Message);
            }
        }

        private void TodoOnDrop(object sender, DragEventArgs e)
        {
            try
            {
                if (e.Data.GetData(typeof(TextBox)) is TextBox dragedTextBox)
                {
                    string query = $"UPDATE `tasks` SET `progress` = 'todo' WHERE `tasks`.`title` = '{dragedTextBox.Text}';";
                    MySqlCommand command = new MySqlCommand(query, DatabaseConnection.Connection);
                    command.ExecuteNonQuery();
                }
                refreshTasks();
            }
            catch (Exception f)
            {
                MessageBox.Show(f.Message);
            }
        }

        private void DragTextbox(object sender, MouseButtonEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                DataObject data = new DataObject(typeof(TextBox), textBox);
                DragDrop.DoDragDrop(textBox, data, DragDropEffects.Move);
            }
        }

        private void refreshTasks()
        {
            toDoGrid.Children.Clear();
            toDoGrid.RowDefinitions.Clear();

            inProgressGrid.Children.Clear();
            inProgressGrid.RowDefinitions.Clear();

            waitingGrid.Children.Clear();
            waitingGrid.RowDefinitions.Clear();

            completedGrid.Children.Clear();
            completedGrid.RowDefinitions.Clear();

            DrawTasks();
        }

        private void refreshBtnClick(object sender, RoutedEventArgs e)
        {
            refreshTasks();
        }

        private void OpenTask(object sender, RoutedEventArgs e)
        {
            idToDel = 0;

            foreach (DataRow row in dt.Rows)
            {
                if(sender is TextBox text && text.Text == row["title"].ToString())
                {
                    idToDel = (int)row["id"];
                    string title = row["title"].ToString();
                    string body = row["body"].ToString();
                    string progress = row["progress"].ToString();
                    TaskToOpen.Invoke();
                    TaskOpened.Invoke(title, $"{loggedUser.Name} {loggedUser.Surname}", body, progress);
                }
            }

        }
        public void DeleteTask(object sender, RoutedEventArgs e)
        {
            string query = $"DELETE FROM tasks WHERE id = {idToDel};";
            MySqlCommand command = new MySqlCommand(query, DatabaseConnection.Connection);
            command.ExecuteNonQuery();
            refreshTasks();
        }

        

        private void CreateTaskBtnClick(object sender, RoutedEventArgs e)
        {
            AddTask addTask = new AddTask();
            addTask.Show();

            addTask.Closed += ((o, args) => DrawTasks());
        }
    }
}
