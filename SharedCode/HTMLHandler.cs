using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;

namespace SharedCode
{
    class HTMLHandler
    {
        /// <summary>Genera un HTML intercambiando valores con el <see cref="Dictionary{TKey, TValue}"/></summary>
        /// <param name="HTMLSource">Ruta del archivo HTML original</param>
        /// <param name="values"><see cref="Dictionary{TKey, TValue}"/> con los nuevos valores que remplazaran en el HTML</param>
        /// <returns>HTML generado con los nuevos valores del diccionario</returns>
        public static string GenerateHTML(string HTMLSource, Dictionary<string, string> values)
        {
            string result = File.ReadAllText(HTMLSource);

            foreach (var valuePair in values)
            {
                // Remplazara el nombre de la llave del par de valores con el nombre clave correspondiente en el documento html
                result = result.Replace(valuePair.Key, valuePair.Value);
            }

            return result;
        }

        // /// <summary>Le pregunta al usuario la ruta donde exportar el html y utiliza <see cref="ExportHTML(string, string)"/> para crear el archivo solicitado</summary>
        // /// <param name="HTMLData">Codigo HTML</param>
        // /// <param name="filename">Nombre del archivo</param>
        // /// <returns>Retorna true si el archivo fue creado correctamente. Retorna false si hubo un problema al crear el archivo</returns>
        // public static bool ExportHTML(string HTMLData, string filename)
        // {
        //     try
        //     {
        //         using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
        //         {
        //             System.Windows.Forms.DialogResult result = dialog.ShowDialog();
        //             if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(dialog.SelectedPath))
        //             {
        //                 if (HTMLHandler.ExportHTML(HTMLData, dialog.SelectedPath, filename))
        //                     MessageBox.Show("Fue exportado correctamente");
        //                 else
        //                     MessageBox.Show("Hubo un problema el exportar el archivo");
        //             }
        //             else
        //             {
        //                 MessageBox.Show("Operacion abortada o ruta seleccionada no es valida");
        //             }
        //         }
        // 
        //         return true;
        //     }
        //     catch (Exception)
        //     {
        // 
        //         return false;
        //     }
        // }
    }
}
