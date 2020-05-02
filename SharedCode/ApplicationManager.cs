using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Windows;

namespace SharedCode
{
    class ApplicationManager
    {
        [Serializable()]
        public class GlobalSettings
        {
            public int ClaveDepto { get; set; }
            public bool BootOnStartup { get; set; }

            public static byte[] ToBytes(GlobalSettings globalSettings)
            {
                if (globalSettings == null)
                    return null;

                BinaryFormatter formatter = new BinaryFormatter();
                MemoryStream stream = new MemoryStream();
                formatter.Serialize(stream, globalSettings);

                return stream.ToArray();
            }
            public static GlobalSettings FromBytes(byte[] bytes)
            {
                MemoryStream stream = new MemoryStream();
                BinaryFormatter formatter = new BinaryFormatter();
                stream.Write(bytes, 0, bytes.Length);
                stream.Seek(0, SeekOrigin.Begin);
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

        /// <summary>Verifica que exista el archivo de configuracion en la ruta preestablecida: <c>C:\Users\%USERNAME%\AppData\Local\ControlAcceso</c></summary>
        /// <example><code>
        /// if (Configuracion.ConfigFileExist)
        /// {
        ///     /// Do something...
        /// }
        /// ...</code></example>
        static public bool FileExistOnAppdata(string filename)
        {
            if (File.Exists($"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}/ControlAcceso/{filename}"))
            {
                return true;
            }
            return false;
        }

        static public bool WriteBinaryFileOnAppdata(byte[] bytes, string filename)
        {
            if (!Directory.Exists($"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\\ControlAcceso\\{filename}"))
            {
                Directory.CreateDirectory($"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\\ControlAcceso");
            }


            try
            {
                using (FileStream stream = File.OpenWrite($"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\\ControlAcceso\\{filename}"))
                {
                    stream.Write(bytes, 0, bytes.Length);
                    return true;
                }
                //MessageBox.Show($"Configuraciones Guardadas");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        static public byte[] ReadBinaryFileOnAppdata(string filename)
        {
            try
            {
                return File.ReadAllBytes($"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\\ControlAcceso\\{filename}");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Extrae valores como rutas de acceso del requeridas por la aplicacion
        /// </summary>
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
}
