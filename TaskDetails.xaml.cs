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
using System.Windows.Shapes;

namespace Platformy_Projekt
{
    /// <summary>
    /// Logika interakcji dla klasy TaskDetails.xaml
    /// </summary>
    public partial class TaskDetails : UserControl
    {
        public TaskDetails()
        {
            InitializeComponent();
        }

        public void DisplayTask(string title, string employee, string desc, string status)
        {
            TitleTxtBlock.Text = title;
            PersonTxtBlock.Text = employee;
            TaskDescription.Text = desc;
            switch (status)
            {
                case "todo":
                    StatusTxtBlock.Text = " To do";
                    break;
                case "inprogress":
                    StatusTxtBlock.Text = "In Progress";
                    break;
                case "waiting":
                    StatusTxtBlock.Text = "Waiting";
                    break;
                case "completed":
                    StatusTxtBlock.Text = "Completed";
                    break;
            }
            
        }
    }
}
