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
    /// Lógica de interacción para ConfigWindow.xaml
    /// </summary>
    public partial class ConfigWindow : Window
    {
        private List<Departamento> listaDepartamento = new List<Departamento>();
        private ApplicationManager.GlobalSettings globalSettings;
        private bool saved;
        public ConfigWindow(object list)
        {
            InitializeComponent();
            listaDepartamento = list as List<Departamento>;
            if (ApplicationManager.FileExistOnAppdata("Settings.data"))
                globalSettings = ApplicationManager.GlobalSettings.FromBytes(ApplicationManager.ReadBinaryFileOnAppdata("Settings.data"));
            else
                globalSettings = new ApplicationManager.GlobalSettings();
        }

        new private void UpdateLayout()
        {
            if (listaDepartamento != null)
                foreach (Departamento departamento in listaDepartamento)
                    cmb_departamento.Items.Add($"{departamento.Clave}\t|  {departamento.Nombre}");

            for (int i = 0; i < listaDepartamento.Count; i++)
                if (listaDepartamento[i].Clave == globalSettings.ClaveDepto)
                    cmb_departamento.SelectedIndex = i;

            check_bootStartup.IsChecked = globalSettings.BootOnStartup;
        }

        public bool SavedByUser() => saved;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateLayout();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            globalSettings.ClaveDepto = listaDepartamento[cmb_departamento.SelectedIndex].Clave;
            globalSettings.BootOnStartup = check_bootStartup.IsChecked.Value;

            ApplicationManager.WriteBinaryFileOnAppdata(ApplicationManager.GlobalSettings.ToBytes(globalSettings), "Settings.data");

            saved = true;

            this.Close();
        }
    }
}
