using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Windows;

namespace SharedCode
{
    /// <summary>
    /// Clase con multiples metodos que ejecutan diversas tareas que ayudan al funcionamiento del sistema
    /// </summary>
    class ApplicationManager
    {
        [Serializable()]
        public class GlobalSettings
        {
            public int ClaveDepto { get; set; }
            public int LogLimit { get; set; } = 15;
            public bool BootOnStartup { get; set; }

            /// <summary>Serializa un objeto de clase <see cref="GlobalSettings"/> a un arreglo de bytes</summary>
            /// <param name="globalSettings">Objeto de clase <see cref="GlobalSettings"/></param>
            /// <returns>Secuencia de un arreglo de bytes</returns>
            public static byte[] ToBytes(GlobalSettings globalSettings)
            {
                // Verifica que globalSettings no sea un valor nulo, y en caso de que se haya pasado como un objeto nulo
                // se terminara el proceso retornando un null
                if (globalSettings == null)
                    return null;

                // Se inicializan un objeto de tipo BinaryFormatter
                // Se serializa el objeto GlobalSettings
                // Retornandolo a un arreglo de bytes
                BinaryFormatter formatter = new BinaryFormatter();
                MemoryStream stream = new MemoryStream();
                formatter.Serialize(stream, globalSettings);

                return stream.ToArray();
            }
            /// <summary>Deserializa un arreglo de bytes de un objeto de clase <see cref="GlobalSettings"/></summary>
            /// <param name="bytes">Objeto <see cref="GlobalSettings"/> serializado en un arreglo de bytes</param>
            /// <returns>Objeto <see cref="GlobalSettings"/></returns>
            public static GlobalSettings FromBytes(byte[] bytes)
            {
                // Se inicializan un objeto de tipo BinaryFormatter
                MemoryStream stream = new MemoryStream();
                BinaryFormatter formatter = new BinaryFormatter();
                // Mantiene los bytes en un buffer de memoria
                stream.Write(bytes, 0, bytes.Length);
                // Orgaliza la secuencia de bytes
                stream.Seek(0, SeekOrigin.Begin);
                // Deserealiza los el buffer como un objeto explicito de GlobalSettings
                GlobalSettings globalSettings = (GlobalSettings)formatter.Deserialize(stream);

                return globalSettings;
            }
        }

        /// <summary>Ejecuta el recolector de basura para liberar memoria que ya no se usa</summary>
        static public void InitGB()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            GC.WaitForFullGCComplete();
        }

        /// <summary>Verifica de que exista el archivo de configuracion en la ruta preestablecida: <c>C:\Users\%USERNAME%\AppData\Local\ControlAcceso</c></summary>
        /// <param name="filename">Nombre del archivo con extension</param>
        /// <example><code>
        /// if (ApplicationManager.FileExistOnAppdata("Settings.data"))
        /// {
        ///     /// Do something...
        /// }
        /// ...</code></example>
        static public bool FileExistOnAppdata(string filename) => File.Exists($"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}/ControlAcceso/{filename}") ? true : false;
        // Es lo mismo que escribir esto:
        // { 
        //     if (File.Exists($"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}/ControlAcceso/{filename}"))
        //     {
        //         return true;
        //     }
        //     return false;
        // }

        /// <summary>Escribe un arreglo de bytes en la ruta preestablecida: <c>C:\Users\%USERNAME%\AppData\Local\ControlAcceso</c></summary>
        /// <param name="bytes">Objeto serializado en un arreglo de bytes</param>
        /// <param name="filename">Nombre del archivo con extension</param>
        /// <returns><see cref="true"/> Si el archivo pudo escribirse correctamente. <see cref="false"/> Si ocurrio alguna excepcion, mostrara un mensaje en pantalla con el error</returns>
        static public bool WriteBinaryFileOnAppdata(byte[] bytes, string filename)
        {
            // Se comprueba de que exista la rita donde se escribira el archivo, cuando no la encuentre, se encargara de crearla
            if (!Directory.Exists($"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\\ControlAcceso\\{filename}"))
            {
                Directory.CreateDirectory($"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\\ControlAcceso");
            }

            try
            {
                // Haciendo el uso de FileStream creara el archivo con el arreglo de bytes en la ruta de LocalApplicationData
                // Y cuando concluya el proceso ratornara un True
                using (FileStream stream = File.OpenWrite($"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\\ControlAcceso\\{filename}"))
                {
                    stream.Write(bytes, 0, bytes.Length);
                    return true;
                }
                //MessageBox.Show($"Configuraciones Guardadas");
            }
            catch (Exception ex)
            {
                // Cuando ocurra algo inesperado, mandara el error en pantalla y retornara un False
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        /// <summary>Lee un los bytes de un archivo binario ubicado en la ruta preestablecida: <c>C:\Users\%USERNAME%\AppData\Local\ControlAcceso</c></summary>
        /// <param name="filename">Nombre del archivo con extension</param>
        /// <returns><see cref="object"/> Serializado en un arreglo de bytes</returns>
        static public byte[] ReadBinaryFileOnAppdata(string filename)
        {
            try
            {
                // Retorna un arreglo de bytes leidos del archivo
                return File.ReadAllBytes($"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\\ControlAcceso\\{filename}");
            }
            catch (Exception ex)
            {
                // Cuando ocurra algo inesperado, mandara el error en pantalla y retornara un False
                MessageBox.Show(ex.Message);
                return null;
            }
        }

        static public void ExceptionHandler(Exception e)
        {
            try
            {
                DateTime dateTime = DateTime.Now;

                // Se comprueba de que exista la rita donde se escribira el archivo, cuando no la encuentre, se encargara de crearla
                if (!Directory.Exists($"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\\ControlAcceso\\Error-Log\\{dateTime.Ticks}-Error.txt"))
                {
                    Directory.CreateDirectory($"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\\ControlAcceso\\Error-Log");
                }

                
                MessageBox.Show($"Se ha detectado un error durante la ejecicion del programa, si persiste contacte a su departamento de TI:\n{e.Message}");

                string txt = $"Mensaje:\t{e.Message}\n" +
                    $"Source: \t{e.Source}\n" +
                    $"StackTrace:\n{e.StackTrace}\n" +
                    $"TargetSite:\t{e.TargetSite}" +
                    $"HelpLink:\t{e.HelpLink}\n" +
                    $"HResult:\t{e.HResult}\n";

                if (e.Data.Count > 0)
                    foreach (var key in e.Data)
                    {
                        txt += $"{key.ToString()}\t";
                    }
                    
                File.WriteAllText($"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\\ControlAcceso\\Error-Log\\{dateTime.Ticks}-Error.txt", txt);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Se detectado un error en la ejecucion y se intento registrar el evento, contacte a su departamto de TI\nRaiz: {e.Message} \nCatcher:{ex.Message}");
                throw;
            }
        }

        /// <summary>Escribe un valor en una llave del registro de Windows</summary>
        /// <param name="keyPath">Direccion de la llave de registro</param>
        /// <param name="valueName">Nombre del valor</param>
        /// <param name="value">Objeto que se guardara en el valor de registro</param>
        /// <returns><see cref="true"/>: Si el valor fue escrito correctamente. <see cref="false"/>: Si ocurrio alguna excepcion, mostrara un mensaje en pantalla con el error</returns>
        static public bool WriteRegistryKey(string keyPath, string valueName, object value)
        {
            try
            {
                // Se posiciona en modo escritura en la llave de registro
                RegistryKey registryKey = Registry.CurrentUser;
                RegistryKey path = registryKey.OpenSubKey(keyPath, true);
                
                // Escribe el nombre del nuevo valor y el valor en la llave del registro
                // Retorna True para finalizar el proceso
                path.SetValue(valueName, value);

                Console.WriteLine("Application: RegistryKey Value Created Successfully");
                return true;
            }
            catch (Exception ex)
            {
                // Cuando ocurra algo inesperado, mandara el error en pantalla y retornara un False
                MessageBox.Show(ex.Message);
                return false;
            }
        }
        /// <summary>Elimina algun valor de una llave del registro de Windows</summary>
        /// <param name="keyPath">Ruta de la llave del regisro, esta ruta estara anidada en la raiz: HKEY_CURRENT_USER\</param>
        /// <param name="valueName">Nombre del valor</param>
        /// <returns><see cref="true"/>: Si el valor fue eliminado correctamente. <see cref="false"/>: Si ocurrio alguna excepcion, mostrara un mensaje en pantalla con el error</returns>
        static public bool DeleteRegistryKey(string keyPath, string valueName)
        {
            try
            {
                // Se posiciona en modo escritura en la llave de registro
                RegistryKey registryKey = Registry.CurrentUser;
                RegistryKey path = registryKey.OpenSubKey(keyPath, true);

                // Elimina el valor en la llave de registro
                // Retorna True para finalizar el proceso
                path.DeleteValue(valueName);

                Console.WriteLine("Application: RegistryKey Value Deleted Successfully");
                return true;
            }
            catch (Exception ex)
            {
                // Cuando ocurra algo inesperado, mandara el error en pantalla y retornara un False
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        /// <summary>Lee el archivo Sources.data ubicado en la carpeta del ejecutable de la aplicacion</summary>
        /// <returns>Diccionario con los los valores extraidos del archivo sources.data</returns>
        public static Dictionary<string, string> RetriveFromSourcesFile()
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            // MessageBox.Show($@"{Environment.CurrentDirectory}\db.data");
            try
            {
                // Lee el archivo y asigna cada linea de texto en un espacio de un arreglo
                // Cada linea es separada por el caracter (,) el primer valor obtenido es el nombre de la clave para el Dictionary y el 
                // segundo el valor que le corresponde a esa clave
                string[] lines = System.IO.File.ReadAllLines($"{Environment.CurrentDirectory}/sources.data");
                foreach (string s in lines)
                {
                    result.Add(s.Split(',')[0].Trim(), s.Split(',')[1].Trim());
                }
                return result;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }
    }

    // Iniciar la aplicacion al arrancar windows
    // https://www.codeproject.com/Questions/201370/starting-the-wpf-application-when-system-starts
    // https://docs.microsoft.com/en-us/windows/win32/setupapi/run-and-runonce-registry-keys?redirectedfrom=MSDN

    // Iconos
    // https://www.flaticon.com/packs/graphic-design-152
}
