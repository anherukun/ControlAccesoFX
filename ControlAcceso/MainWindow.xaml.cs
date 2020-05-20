using ControlAcceso.Layouts;
using SharedCode;
using SharedCode.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;

namespace ControlAcceso
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        class BindingRegister
        {
            public int UID { get; set; }
            public string Ficha { get; set; }
            public string NombreCompleto { get; set; }
            public string FechaCorta { get; set; }
            public string HEntradaCompleta { get; set; }
            public string HSalidaCompleta { get; set; }
            public string Horas { get; set; }
        }

        private List<CARegistro> registros;
        private ApplicationManager.GlobalSettings globalSettings;
        private Departamento departamento;

        private CancellationToken cancellationToken = new CancellationToken();
        private bool isLogRefreshing;

        public MainWindow()
        {
            InitializeComponent();
            
            //MessageBox.Show(System.Reflection.Assembly.GetEntryAssembly().Location);

            if (ApplicationManager.FileExistOnAppdata("Settings.data"))
            {
                RefreshGlobalSettings();
                RefreshRegLog();
            }
            else
                MessageBox.Show("Para utilizar el sistema, ve a configuraciones y selecciona el departamento");
        }
        private void UpdateClock(object sender, EventArgs e)
        {
            DateTime dateTime = DateTime.Now;
            string clockstring = "";
            string month = "", hour = $"{dateTime.Hour}", minute = $"{dateTime.Minute}", second = $"{dateTime.Second}";

            switch (DateTime.Now.Month)
            {
                case 1:
                    month = "ENE";
                    break;
                case 2:
                    month = "FEB";
                    break;
                case 3:
                    month = "MAR";
                    break;
                case 4:
                    month = "ABR";
                    break;
                case 5:
                    month = "MAY";
                    break;
                case 6:
                    month = "JUN";
                    break;
                case 7:
                    month = "JUL";
                    break;
                case 8:
                    month = "AGO";
                    break;
                case 9:
                    month = "SEP";
                    break;
                case 10:
                    month = "OCT";
                    break;
                case 11:
                    month = "NOV";
                    break;
                case 12:
                    month = "DIC";
                    break;
                default:
                    break;
            }

            if (hour.Length == 1)
                hour = $"0{dateTime.Hour}";
            if (minute.Length == 1)
                minute = $"0{dateTime.Minute}";
            if (second.Length == 1)
                second = $"0{dateTime.Second}";


            // <GridViewColumn DisplayMemberBinding = "{Binding Horas}" Header = "Horas"/>
               clockstring = $"{dateTime.Day} {month} [{hour}:{minute}:{second}]";
            //Application.Current.Dispatcher.Invoke(new Action(() => { txt_reloj.Text = clockstring; }));
            txt_reloj.Text = clockstring;
        }

        new private void UpdateLayout()
        {
            if (globalSettings == null)
                btn_entrada.IsEnabled = false;
            else
            {
                btn_entrada.IsEnabled = true;
                txt_depto.Text = departamento.Nombre.Trim().ToUpper();
            }
        }

        private void RefreshGlobalSettings()
        {
            globalSettings = ApplicationManager.GlobalSettings.FromBytes(ApplicationManager.ReadBinaryFileOnAppdata("Settings.data"));
            departamento = Departamento.FromDictionarySingle(new DatabaseManager().FromDatabaseToSingleDictionary($"SELECT * FROM DEPARTAMENTOS WHERE DEPARTAMENTOS.[CLAVE] LIKE {globalSettings.ClaveDepto}"));
        }

        private List<CARegistro> RefreshCARegistro()
        {
            Console.WriteLine("Application: RefreshCARegistro Started");
            return CARegistro.FromDictionaryListToList(new DatabaseManager().FromDatabaseToDictionary($"SELECT TOP {globalSettings.LogLimit} UID, FICHA, CLAVEDEPTO, FECHA, HENTRADA, HSALIDA FROM REGISTRO WHERE REGISTRO.[CLAVEDEPTO] LIKE {globalSettings.ClaveDepto} ORDER BY REGISTRO.[UID] DESC"));
        }

        private async void RefreshRegLog()
        {
            Console.WriteLine($"{DateTime.Now.ToLongTimeString()}\tApplication: RefreshCARegistro Requested");
            isLogRefreshing = true;

            List<BindingRegister> bindings = new List<BindingRegister>();

            progressbar.Visibility = Visibility.Visible;

            if (registros != null)
                registros.Clear();

            registros = await Task.Run(() =>
            {
                //throw new TaskCanceledException();
                Console.WriteLine("Application: RefreshCARegistro Requested");
                return RefreshCARegistro();
            });

            Console.WriteLine($"Application: RefreshLog Started");
            progressbar.IsIndeterminate = false;

            if (registros != null && registros.Count > 0)
            {
                await Task.Run(() =>
                {
                    foreach (var item in registros)
                    {
                        BindingRegister br = new BindingRegister
                        {
                            Ficha = $"{item.Ficha}",
                            FechaCorta = item.FechaCorta,
                            HEntradaCompleta = item.HEntradaCompleta,
                            HSalidaCompleta = item.HSalidaCompleta
                        };

                        bindings.Add(br);
                    }

                    Application.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        lst_registro.ItemsSource = bindings;
                    }));
                });

                try
                {
                    Task<int> t = Task.Run(() =>
                    {
                        Application.Current.Dispatcher.Invoke(new Action(() =>
                        {
                            progressbar.Visibility = Visibility.Visible;
                            progressbar.Maximum = registros.Count;
                        }));
                        for (int i = 0; i < bindings.Count; i++)
                        {
                            if (cancellationToken.IsCancellationRequested)
                                throw new TaskCanceledException("Application: Operation Aborted");

                            Application.Current.Dispatcher.Invoke(new Action(() =>
                            {
                                progressbar.Value += 1;
                            }));

                            bindings[i].NombreCompleto = CARegistro.ObtenerNombreCompleto(int.Parse(bindings[i].Ficha));


                            Application.Current.Dispatcher.Invoke(new Action(() =>
                            {
                                lst_registro.Items.Refresh();
                            }));
                        }

                        Application.Current.Dispatcher.Invoke(new Action(() =>
                        {
                            progressbar.Visibility = Visibility.Hidden;
                            progressbar.Maximum = registros.Count;
                        }));


                        isLogRefreshing = false;
                        return 0;
                    });
                }
                catch (TaskCanceledException ex)
                {
                    isLogRefreshing = false;
                    Console.WriteLine(ex);
                }

            }

            progressbar.Value = 0;
            Console.WriteLine($"{DateTime.Now.ToLongTimeString()}\tApplication: RefreshLog Finished");
        }

        private void SendNewEntry(Personal p)
        {
            DateTime dateTime = DateTime.Now;

            // MessageBox.Show($"{dateTime.Ticks.ToString()}");
            CARegistro reg = new CARegistro()
            {
                ClaveDepto = globalSettings.ClaveDepto,
                Ficha = p.Ficha,
                Fecha = dateTime,
                HEntrada = dateTime.Ticks.ToString(),
                HSalida = "0"
            };

            new DatabaseManager().InsertData(CARegistro.WriteSQL(reg));

            RefreshRegLog();
        }

        private void SendUpdatedEntry(CARegistro registro)
        {
            registro.HSalida = DateTime.Now.Ticks.ToString();

            new DatabaseManager().InsertData(CARegistro.UpdateHSalidaSQL(registro));

            RefreshRegLog();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += UpdateClock;
            timer.Start();

            UpdateLayout();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            //NUEVA ENTRADA
            if (isLogRefreshing == true)
                cancellationToken = new CancellationToken(true);

            Thread.Sleep(350);

            lst_registro.SelectedIndex = -1;
            List<Personal> ls = await Task.Run(() =>
            {
                return Personal.FromDictionaryListToList(new DatabaseManager().FromDatabaseToDictionary("SELECT * FROM PERSONAL ORDER BY PERSONAL.[FICHA] ASC;"));
            });

            ComboInput input = new ComboInput(ls, "Ingresa tu ficha y pulsa Aceptar");
            input.Owner = this;
            input.ShowDialog();

            if (input.HasSelection())
                SendNewEntry(ls[input.RetriveSelection()]);
            else
            {
                MessageBox.Show("Se ha cancelado la operacion");
                RefreshRegLog();
            }

            cancellationToken = new CancellationToken(false);

            ApplicationManager.InitGB();
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            // ABRIR OPCIONESs
            cancellationToken = new CancellationToken(true);
            List<Departamento> ls = await Task.Run(() =>
            {
                return Departamento.FromDictionaryListToList(new DatabaseManager().FromDatabaseToDictionary("SELECT * FROM DEPARTAMENTOS ORDER BY DEPARTAMENTOS.[CLAVE] ASC"));
            });

            ConfigWindow config = new ConfigWindow(ls);
            config.Owner = this;
            config.ShowDialog();

            if (config.SavedByUser())
            {
                RefreshGlobalSettings();
                UpdateLayout();
                RefreshRegLog();
            }

            cancellationToken = new CancellationToken(false);

            ApplicationManager.InitGB();
        }

        private async void Button_Click_2(object sender, RoutedEventArgs e)
        {
            // GENERAR INFORME
            cancellationToken = new CancellationToken(true);
            DateInput input = new DateInput("Selecciona la fecha para generar el informe");
            input.Owner = this;
            input.ShowDialog();

            if (input.HasSelection())
            {
                btn_informe.IsEnabled = false;
                List<CARegistro> r = new List<CARegistro>();
                await Task.Run(() =>
                {
                    r = CARegistro.FromDictionaryListToList(new DatabaseManager().FromDatabaseToDictionary($"SELECT * FROM REGISTRO WHERE REGISTRO.[FECHA] LIKE \"{input.RetriveSelection().ToShortDateString()}\" AND REGISTRO.[CLAVEDEPTO] LIKE {this.departamento.Clave} ORDER BY REGISTRO.[UID] DESC"));
                });

                CARegistro.PrepareDataToTemplete(this.departamento, input.RetriveSelection(), r);

                btn_informe.IsEnabled = true;
            }
            else
                MessageBox.Show("Se ha cancelado la operacion");

            cancellationToken = new CancellationToken(false);
        }

        private void btn_salida_Click(object sender, RoutedEventArgs e)
        {
            // REGISTRAR SALIDA
            cancellationToken = new CancellationToken(true);
            MessageBoxResult result = MessageBox.Show($"Deseas registrar la salida de {Personal.FromDictionarySingle(new DatabaseManager().FromDatabaseToSingleDictionary($"SELECT * FROM PERSONAL WHERE PERSONAL.[FICHA] LIKE {registros[lst_registro.SelectedIndex].Ficha}")).Nombre}", "", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
                SendUpdatedEntry(registros[lst_registro.SelectedIndex]);

            lst_registro.SelectedIndex = -1;
            btn_salida.IsEnabled = false;

            cancellationToken = new CancellationToken(false);
        }

        private void lst_registro_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lst_registro.SelectedIndex > -1 && isLogRefreshing == false)
                if (long.Parse(registros[lst_registro.SelectedIndex].HSalida) > 0)
                    btn_salida.IsEnabled = false;
                else
                    btn_salida.IsEnabled = true;
            else
                btn_salida.IsEnabled = false;
        }
    }
}
