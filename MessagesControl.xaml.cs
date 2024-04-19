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
using Mysqlx.Resultset;
using System.Reflection.Metadata.Ecma335;
using System.Security.Permissions;

namespace Platformy_Projekt
{
    public partial class MessagesControl : UserControl
    {
        
        LoggedUser loggedUser;
        public delegate void MessageButttonClicked();
        public event MessageButttonClicked ButtonClicked;

        public delegate void OnMessageOpen(string sendId);
        public event OnMessageOpen MessageOpen;

        public delegate void OnMessageSendButtonClick();
        public event OnMessageSendButtonClick MessageSendButtonClick;

        private string messageId;

        public MessagesControl()
        {
            InitializeComponent();
            FetchMessages();
        }

        private void FetchMessages()
        {
            loggedUser = LoggedUser.GetInstance();

            try
            {
                string query = $"SELECT messages.id, messages.title, messages.message, users.name, users.surname FROM messages INNER JOIN users ON messages.senderid = users.id WHERE addresseeId={loggedUser.Id};";
                MySqlCommand command = new MySqlCommand(query, DatabaseConnection.Connection);
                command.ExecuteNonQuery();
                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(command);
                DataTable dt = new DataTable("messagesTable");
                dataAdapter.Fill(dt);

                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    Button button = new Button();
                    button.HorizontalContentAlignment = HorizontalAlignment.Stretch;
                    Grid grid = new Grid();
                    var array = dt.Rows[j].ItemArray;
                    for (int i = 0; i < (array.Length - 1); i++)
                    {
                        if (i == 0)
                        {
                            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(0) });
                        }
                        else
                        {
                            grid.ColumnDefinitions.Add(new ColumnDefinition());
                        }
                        
                        TextBlock textBlock = new TextBlock();
                        if(i == 2)
                        {
                            textBlock.HorizontalAlignment = HorizontalAlignment.Left;
                        }
                        else
                        {
                            textBlock.HorizontalAlignment = HorizontalAlignment.Center;
                            textBlock.Margin = new Thickness(20, 0, 0, 0);
                        }
                        textBlock.VerticalAlignment = VerticalAlignment.Center;

                        if (i != (array.Length - 2))
                        {
                            textBlock.Text = array[i]!.ToString();
                        }
                        else
                        {
                            textBlock.Text = array[i] + " " + array[i + 1];
                        }

                        Grid.SetColumn(textBlock, i);
                        grid.Children.Add(textBlock);
                    }

                    button.Content = grid;
                    button.Click += OpenMessage;
                    button.Background = MessageBackground();
                    button.BorderThickness = new Thickness(0);
                    button.Cursor = Cursors.Hand;
                    button.FontFamily = new FontFamily("Yu Gothic UI Light");
                    contentGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(40) });
                    Grid.SetRow(button, j);
                    contentGrid.Children.Add(button);
                }
            }
            catch
            {
                MessageBox.Show("Wystąpił błąd podczas pobierania wiadomości");
            }
        }
        private void OpenMessage(object sender, RoutedEventArgs e)
        {

            ButtonClicked?.Invoke();

            Button button = sender as Button;

            Grid grid = new Grid();
            grid = button.Content as Grid;

            if (grid.Children[0] is TextBlock messageIdBlock)
            {
                messageId = messageIdBlock.Text;

                MessageOpen?.Invoke(messageId);
            }


        }
            
        public void DeleteMessage()
        {
            try
            {
                string query = $"DELETE FROM messages WHERE id = {messageId};";
                MySqlCommand command = new MySqlCommand(query, DatabaseConnection.Connection);
                command.ExecuteNonQuery();
            }
            catch
            {
                MessageBox.Show("Wystąpił błąd podczas usuwania wiadomości");
            }
            refreshMessages();
        }

        private void WriteMessage(object sender, RoutedEventArgs e)
        {
            SendMessage sendMessage;
            sendMessage = new SendMessage();
            sendMessage.Show();
        }

        private void refreshMessages()
        {
            contentGrid.RowDefinitions.Clear();
            contentGrid.Children.Clear();
            FetchMessages();
        }

        private void refreshBtnClick(object sender, RoutedEventArgs e)
        {
            refreshMessages();
        }

        private LinearGradientBrush MessageBackground()
        {
            GradientStop gradientStop1 = new GradientStop();
            gradientStop1.Color = Color.FromRgb(200, 200, 200); // #222831

            GradientStop gradientStop2 = new GradientStop();
            gradientStop2.Color = Color.FromRgb(238, 238, 238); // #31363F
            gradientStop2.Offset = 1;

            LinearGradientBrush linearGradientBrush = new LinearGradientBrush();
            linearGradientBrush.StartPoint = new Point(0.5, 0);
            linearGradientBrush.EndPoint = new Point(0.5, 1);
            linearGradientBrush.GradientStops.Add(gradientStop1);
            linearGradientBrush.GradientStops.Add(gradientStop2);

            return linearGradientBrush;
        }
    }
}
