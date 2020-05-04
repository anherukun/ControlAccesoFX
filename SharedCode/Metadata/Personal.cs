using System;
using System.Collections.Generic;
using System.Text;

namespace SharedCode.Metadata
{
    class Personal
    {
        public int Ficha { get; set; }
        public string Nombre { get; set; }

        /// <summary>
        /// Transforma una <see cref="List{T}"/> de elementos de un <see cref="Dictionary{TKey, TValue}"/> en una <see cref="List{T}"/> de objetos de la clase <see cref="Personal"/>
        /// </summary>
        /// <param name="keyValues"><see cref="List{T}"/> de elementos <see cref="Dictionary{TKey, TValue}"/> resultante a una busqueda de la base de datos</param>
        /// <returns><see cref="List{T}"/> de objetos de la case <see cref="Personal"/></returns>
        public static List<Personal> FromDictionaryListToList(List<Dictionary<string, object>> keyValues)
        {
            if (keyValues.Count > 0)
            {
                List<Personal> ls = new List<Personal>();
                foreach (Dictionary<string, object> item in keyValues)
                    ls.Add(new Personal()
                    {
                        Ficha = (int)item["FICHA"],
                        Nombre = (string)item["NOMBRE"]
                    });

                return ls;
            }
            else return null;
        }

        /// <summary>
        /// Transforma un <see cref="Dictionary{TKey, TValue}"/> a un objeto de clase <see cref="Personal"/>
        /// </summary>
        /// <param name="keyValues"><see cref="Dictionary{TKey, TValue}"/> resultante a una busqueda de la base de datos</param>
        /// <returns>Objeto de la clase <see cref="Personal"/></returns>
        public static Personal FromDictionarySingle(Dictionary<string, object> keyValues)
        {
            if (keyValues.Count > 0)
            {
                Personal p = new Personal()
                {
                    Ficha = (int)keyValues["FICHA"],
                    Nombre = (string)keyValues["NOMBRE"]
                };

                return p;
            }
            else return null;
        }
    }
}
