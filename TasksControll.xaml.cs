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
    public partial class TasksControll : UserControl
    {
        public TasksControll()
        {
            InitializeComponent();
        }

        private void InProgressOnDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetData(typeof(TextBox)) is TextBox dragedTextBox)
            {
                Grid.SetColumn(dragedTextBox, 1);
            }
        }

        private void TodoOnDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetData(typeof(TextBox)) is TextBox dragedTextBox)
            {
                Grid.SetColumn(dragedTextBox, 0);
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
    }
}
