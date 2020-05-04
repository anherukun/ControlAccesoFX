using System;
using System.Collections.Generic;
using System.Text;

namespace SharedCode.Metadata
{
    class CARegistro
    {
        public int UID { get; set; }
        public int Ficha { get; set; }
        public int ClaveDepto { get; set; }
        public DateTime Fecha { get; set; }
        public string HEntrada { get; set; }
        public string HSalida { get; set; }

        /// <summary>Crea una instruccion SQL para ejecutar en una Base de Datos</summary>
        /// <param name="registro">Un objeto de tipo <see cref="CARegistro"/></param>
        /// <returns>Instruccion SQL</returns>
        public static string WriteSQL(CARegistro registro)
        {
            string sql = "";
            sql = $"INSERT INTO REGISTRO (FICHA, CLAVEDEPTO, FECHA, HENTRADA, HSALIDA) " +
                $"VALUES ({registro.Ficha}, {registro.ClaveDepto}, \"{registro.Fecha.Date}\", {registro.HEntrada}, {registro.HSalida})";
            return sql;
        }

        /// <summary>Crea una instruccion SQL para ejecutar en una Base de Datos</summary>
        /// <param name="registro">Un objeto de tipo <see cref="CARegistro"/></param>
        /// <returns>Instruccion SQL</returns>
        public static string UpdateSQL(CARegistro registro)
        {
            string sql = "";
            sql = $"UPDATE REGISTRO SET " +
                $"FICHA = {registro.Ficha}, " +
                $"CLAVEDEPTO = {registro.ClaveDepto}, " +
                $"FECHA = \"{registro.Fecha}\", " +
                $"HENTRADA = {registro.HEntrada}, " +
                $"HSALIDA = {registro.HSalida} " +
                $"WHERE UID LIKE {registro.UID}";
            return sql;
        }

        /// <summary>
        /// Transforma una <see cref="List{T}"/> de elementos de un <see cref="Dictionary{TKey, TValue}"/> en una <see cref="List{T}"/> de objetos de la clase <see cref="Personal"/>
        /// </summary>
        /// <param name="keyValues"><see cref="List{T}"/> de elementos <see cref="Dictionary{TKey, TValue}"/> resultante a una busqueda de la base de datos</param>
        /// <returns><see cref="List{T}"/> de objetos de la case <see cref="Personal"/></returns>
        public static List<CARegistro> FromDictionaryListToList(List<Dictionary<string, object>> keyValues)
        {
            if (keyValues.Count > 0)
            {
                List<CARegistro> ls = new List<CARegistro>();
                foreach (Dictionary<string, object> item in keyValues)
                    ls.Add(new CARegistro()
                    {
                        UID = (int)item["UID"],
                        Ficha = (int)item["FICHA"],
                        ClaveDepto = (int)item["CLAVEDEPTO"],
                        Fecha = (DateTime)item["FECHA"],
                        HEntrada = (string)item["HENTRADA"],
                        HSalida = (string)item["HSALIDA"]
                    });

                return ls;
            }
            else return null;
        }
    }
}
