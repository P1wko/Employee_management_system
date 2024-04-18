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
                        grid.ColumnDefinitions.Add(new ColumnDefinition());
                        TextBlock textBlock = new TextBlock();
                        textBlock.HorizontalAlignment = HorizontalAlignment.Center;

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

                    button.Height = 50;
                    button.Content = grid;
                    button.Click += OpenMessage;
                    contentGrid.RowDefinitions.Add(new RowDefinition());
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
                string messageId = messageIdBlock.Text;

                MessageOpen?.Invoke(messageId);
            }



        }

        private void WriteMessage(object sender, RoutedEventArgs e)
        {
            SendMessage sendMessage;
            sendMessage = new SendMessage();
            sendMessage.Show();
        }
    }
}
