using System;
using System.Collections.Generic;
using System.Text;

namespace SharedCode.Metadata
{
    class Departamento
    {
        public int Clave { get; set; }
        public string Nombre { get; set; }

        /// <summary>
        /// Transforma una <see cref="List{T}"/> de elementos de un <see cref="Dictionary{TKey, TValue}"/> en una <see cref="List{T}"/> de objetos de la clase <see cref="Departamento"/>
        /// </summary>
        /// <param name="keyValues"><see cref="List{T}"/> de elementos <see cref="Dictionary{TKey, TValue}"/> resultante a una busqueda de la base de datos</param>
        /// <returns><see cref="List{T}"/> de objetos de la case <see cref="Departamento"/></returns>
        public static List<Departamento> FromDictionaryListToList(List<Dictionary<string, object>> keyValues)
        {
            if (keyValues.Count > 0)
            {
                List<Departamento> ls = new List<Departamento>();
                foreach (Dictionary<string, object> item in keyValues)
                    ls.Add(new Departamento()
                    {
                        Clave = (int)item["CLAVE"],
                        Nombre = (string)item["NOMBRE"]
                    });

                return ls;
            }
            else return null;
        }

        /// <summary>
        /// Transforma un <see cref="Dictionary{TKey, TValue}"/> a un objeto de clase <see cref="Departamento"/>
        /// </summary>
        /// <param name="keyValues"><see cref="Dictionary{TKey, TValue}"/> resultante a una busqueda de la base de datos</param>
        /// <returns>objeto de la clase <see cref="Departamento"/></returns>
        public static Departamento FromDictionarySingle(Dictionary<string, object> keyValues)
        {
            if (keyValues.Count > 0)
            {
                Departamento d = new Departamento()
                {
                    Clave = (int)keyValues["CLAVE"],
                    Nombre = (string)keyValues["NOMBRE"]
                };

                return d;
            }
            else return null;
        }
    }
}
