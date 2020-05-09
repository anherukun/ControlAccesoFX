using Microsoft.Win32;
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
        public static void GenerateHTML(string HTMLSource, Dictionary<string, string> values)
        {
            string result = File.ReadAllText(HTMLSource);

            foreach (var valuePair in values)
            {
                // Remplazara el nombre de la llave del par de valores con el nombre clave correspondiente en el documento html
                result = result.Replace(valuePair.Key, valuePair.Value);
            }

            ExportHTMLFile(result);
        }

        public static bool ExportHTMLFile(string HTMLSource)
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Documento HTML (*.html)|*.html";
                saveFileDialog.DefaultExt = "*.html";
                if (saveFileDialog.ShowDialog() == true)
                {
                    File.WriteAllText(saveFileDialog.FileName, HTMLSource);
                    return true;
                }
            }
            catch (Exception ex)
            {
                ApplicationManager.ExceptionHandler(ex);
            }
            return false;
        }
    }
}
