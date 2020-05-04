using SharedCode;
using SharedCode.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
            
            txt_registrosmax.Text = "15";

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
            txt_registrosmax.Text = globalSettings.LogLimit.ToString();
        }

        private bool BootStartupCheck() => check_bootStartup.IsChecked.Value ? 
            ApplicationManager.WriteRegistryKey(@"Software\Microsoft\Windows\CurrentVersion\Run", "ControlAcceso", System.Reflection.Assembly.GetEntryAssembly().Location) :
            ApplicationManager.DeleteRegistryKey(@"Software\Microsoft\Windows\CurrentVersion\Run", "ControlAcceso");

        public bool SavedByUser() => saved;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateLayout();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (globalSettings.BootOnStartup != check_bootStartup.IsChecked)
            {
                if (BootStartupCheck())
                {
                    globalSettings.ClaveDepto = listaDepartamento[cmb_departamento.SelectedIndex].Clave;
                    globalSettings.BootOnStartup = check_bootStartup.IsChecked.Value;
                    globalSettings.LogLimit = int.Parse(txt_registrosmax.Text);

                    ApplicationManager.WriteBinaryFileOnAppdata(ApplicationManager.GlobalSettings.ToBytes(globalSettings), "Settings.data");
                    saved = true;
                    this.Close();
                }
            }

            else if (globalSettings.BootOnStartup == check_bootStartup.IsChecked)
            {
                globalSettings.ClaveDepto = listaDepartamento[cmb_departamento.SelectedIndex].Clave;
                globalSettings.BootOnStartup = check_bootStartup.IsChecked.Value;
                globalSettings.LogLimit = int.Parse(txt_registrosmax.Text);

                ApplicationManager.WriteBinaryFileOnAppdata(ApplicationManager.GlobalSettings.ToBytes(globalSettings), "Settings.data");
                saved = true;
                this.Close();
            }
        }

        private void NumberValidation(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
