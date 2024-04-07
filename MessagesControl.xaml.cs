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

namespace Platformy_Projekt
{
    public partial class MessagesControl : UserControl
    {
        LoggedUser loggedUser;
        public MessagesControl()
        {
            InitializeComponent();
            FetchMessages();
        }

        private void FetchMessages()
        {
            loggedUser = LoggedUser.GetInstance();

            string query = $"SELECT * FROM messages WHERE senderId={loggedUser.Id} OR addresseeId={loggedUser.Id};";
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
                for (int i = 0; i < array.Length; i++)
                {
                    grid.ColumnDefinitions.Add(new ColumnDefinition());
                    TextBlock textBlock = new TextBlock();
                    textBlock.HorizontalAlignment = HorizontalAlignment.Center;
                    textBlock.Text = array[i]!.ToString();

                    Grid.SetColumn(textBlock, i);
                    grid.Children.Add(textBlock);
                }

                button.Height = 50;
                button.Content = grid;
                contentGrid.RowDefinitions.Add(new RowDefinition());
                Grid.SetRow(button, j);
                contentGrid.Children.Add(button);
            }
        }
    }
}
