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
using MySql.Data.MySqlClient;

namespace Platformy_Projekt
{
    /// <summary>
    /// Interaction logic for Message.xaml
    /// </summary>
    public partial class Message : UserControl
    {

        public delegate void GobackButoonClicked();
        public event GobackButoonClicked BackButtonClicked;

        public delegate void OnReply(string userNameAndSurname);
        public event OnReply MessageReply;

        public delegate void OnMessageDelete();
        public event OnMessageDelete MessageDelete;

        public delegate void OnMessageDeleteChangeWindow();
        public event OnMessageDeleteChangeWindow MessageDeleteChangeWindow;


        public Message()
        {
            InitializeComponent();
        }

        private void GoBackToMessages(object sender, RoutedEventArgs e)
        {
            BackButtonClicked?.Invoke();
        }

        public void Fetchmessage(string senderId)
        {
            try
            {
                string query = $"SELECT messages.id, messages.title, messages.message, users.name, users.surname, users.email FROM messages INNER JOIN users ON messages.senderid = users.id WHERE messages.id = {senderId}";
                MySqlCommand command = new MySqlCommand(query, DatabaseConnection.Connection);
                command.ExecuteNonQuery();
                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(command);
                DataTable dt = new DataTable("message");
                dataAdapter.Fill(dt);

                Title.Text = dt.Rows[0][1].ToString();
                MessageContent.Text = dt.Rows[0][2].ToString();
                NameAndSurname.Text = dt.Rows[0][3] + " " + dt.Rows[0][4];
                Email.Text = dt.Rows[0][5].ToString();

            }
            catch
            {
                MessageBox.Show("Wystąpił błąd podczas pobierania wiadomości");
            }
        }

        private void Reply(object sender, RoutedEventArgs e)
        {
            SendMessage sendMessage;
            sendMessage = new SendMessage();
            sendMessage.Show();

            MessageReply += sendMessage.OnReplyClick;
            MessageReply?.Invoke(NameAndSurname.Text);
        }

        public void Delete(object sender, RoutedEventArgs e)
        {

            MessageDelete?.Invoke();
            MessageDeleteChangeWindow?.Invoke();

        }

    }
}
