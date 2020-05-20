using SharedCode;
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

namespace ControlAcceso.Layouts
{
    /// <summary>
    /// Lógica de interacción para DateInput.xaml
    /// </summary>
    public partial class DateInput : Window
    {
        private string message;
        private DateTime selectedDate;

        public DateInput(string message)
        {
            InitializeComponent();
            this.message = message;
        }

        private new void UpdateLayout()
        {
            txt_mensaje.Text = message;
        }

        public bool HasSelection() => datepicker.SelectedDate.HasValue;

        public DateTime RetriveSelection() => selectedDate;

        private void Window_Closed(object sender, EventArgs e)
        {
            if (selectedDate == null)
                ApplicationManager.InitGB();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateLayout();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (datepicker.SelectedDate.HasValue)
                selectedDate = datepicker.SelectedDate.Value;
            this.Close();
        }
    }
}
