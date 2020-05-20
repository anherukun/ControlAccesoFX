using SharedCode;
using SharedCode.Metadata;
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
    /// Lógica de interacción para ComboInput.xaml
    /// </summary>
    public partial class ComboInput : Window
    {
        private List<Personal> listaPersonal;
        private List<Departamento> listaDepartamento;
        private int selectedIndex = -1;
        private string message;

        public ComboInput(object list, string message)
        {
            InitializeComponent();
            if (list.GetType() == typeof(List<Personal>))
                this.listaPersonal = list as List<Personal>;
            else if (list.GetType() == typeof(List<Departamento>))
                this.listaDepartamento = list as List<Departamento>;

            this.message = message;
        }

        new private async void UpdateLayout()
        {
            txt_mensaje.Text = message;

            await Task.Run(() =>
            {
                if (listaPersonal != null)
                    Application.Current.Dispatcher.Invoke(new Action(() => { cmb_lista.ItemsSource = listaPersonal; }));
                

                else if (listaDepartamento != null)
                    foreach (Departamento departamento in listaDepartamento)
                        Application.Current.Dispatcher.Invoke(new Action(() => { cmb_lista.Items.Add($"{departamento.Clave}\t|  {departamento.Nombre}"); }));
            });
        }

        public int RetriveSelection() => selectedIndex > -1 ? selectedIndex : -1;
        // {
        //     if (selectedIndex > -1)
        //         return lista[selectedIndex];
        //     return null;
        // }

        public bool HasSelection() => selectedIndex > -1;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateLayout();
        }
        
        private void Window_Closed(object sender, EventArgs e)
        {
            if (selectedIndex == -1)
                ApplicationManager.InitGB();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // ACEPTAR
            if (cmb_lista.SelectedIndex > -1)
            {
                selectedIndex = cmb_lista.SelectedIndex;
                this.Close();
            }
            else
                MessageBox.Show("Debes seleccionar un elemento de la lista.");
        }
    }
}
