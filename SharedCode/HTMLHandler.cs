using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

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
    }
}
