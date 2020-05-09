using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Text;
using System.Windows;

namespace SharedCode
{
    class DatabaseManager
    {
        private string ConnectionString = "";
        public DatabaseManager()
        {
            Dictionary<string, string> values = ApplicationManager.RetriveFromSourcesFile();

            OleDbConnectionStringBuilder builder = new OleDbConnectionStringBuilder();
            builder.Provider = $"{values["DATABASE_PROVIDER"]}";
#if DEBUG
            builder.DataSource = $"{values["DATABASE_PATH_DEBUG"]}";
#else
            builder.DataSource = $"{values["DATABASE_PATH"]}";
#endif
            ConnectionString = builder.ConnectionString;
        }

        /// <summary>Realiza un comando SQL para insertar una fila</summary>
        /// <param name="sql">Instriccion de insercion en SQL</param>
        public bool InsertData(string sql)
        {
            using (OleDbConnection connection = new OleDbConnection(ConnectionString))
            {
                OleDbCommand command = new OleDbCommand(sql, connection);

                try
                {
                    connection.Open();

                    command.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
                catch (Exception ex)
                {
                    connection.Close();

                    ApplicationManager.ExceptionHandler(ex);
                    return false;
                }
            }
        }

        /// <summary>Crea un Diccionario&#60;calve, valor&#62; de objetos con los datos obtenidos de una Base de Datos</summary>
        /// <param name="sql">Instruccion de lectura SQL</param>
        /// <example>
        /// Una instruccion SQL donde el resultado puede ser flexible
        /// <para>Pueden ser de: X nuemaro de filas y Z numero de columnas (Matriz N x M)</para>
        ///     <code>ReadSingleDataToDictionary("SELECT * FROM TABLA WHERE CONDITION LIKE TRUE")</code>
        /// </example>
        /// <returns>Diccionario&#60;calve, valor&#62; de elementos</returns>
        public List<Dictionary<string, object>> FromDatabaseToDictionary(string sql)
        {
            List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
            using (OleDbConnection connection = new OleDbConnection(ConnectionString))
            {
                OleDbCommand command = new OleDbCommand(sql, connection);
                OleDbDataReader reader;
                try
                {
                    connection.Open();
                    reader = command.ExecuteReader();
                    int index = reader.FieldCount;
                    while (reader.Read())
                    {
                        Dictionary<string, object> o = new Dictionary<string, object>();
                        for (int i = 0; i < index; i++)
                        {
                            o.Add(reader.GetName(i), reader.GetValue(i));
                        }
                        data.Add(o);
                    }

                    reader.Close();
                    connection.Close();
                    return data;
                }
                catch (Exception ex)
                {
                    connection.Close();

                    ApplicationManager.ExceptionHandler(ex);
                    return null;
                }
            }
        }

        /// <summary>Crea un Diccionario&#60;calve, valor&#62; de objetos con los datos obtenidos de una Base de Datos, Usar solo para consultas de un solo registro
        /// y no una matriz extensa</summary>
        /// <example>
        /// Una instruccion SQL donde no devuelva mas de una fila, sin limitar la cantidad de columnas necesarias
        ///     <code>ReadSingleDataToDictionary("SELECT * FROM TABLA WHERE CONDITION LIKE TRUE")</code>
        /// </example>
        /// <param name="sql">Instruccion de lectura SQL</param>
        /// <returns>Diccionario&#60;calve, valor&#62; de elementos
        /// <para>Para acceder o extraer un valor del Diccionario, se hace igual a una lectura de matriz, solo que envez de
        /// referrenciar al indice, se referencia al nombre que tenia la COLUMNA de la base de datos</para>
        /// <code>Dictionary d = new Dictionary&#60;string, object&#62;();
        /// ...
        /// MessageBox.Show((string)d["mensaje"]);
        /// ...</code></returns>
        public Dictionary<string, object> FromDatabaseToSingleDictionary(string sql)
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            using (OleDbConnection connection = new OleDbConnection(ConnectionString))
            {
                OleDbCommand command = new OleDbCommand(sql, connection);
                OleDbDataReader reader;
                try
                {
                    connection.Open();
                    reader = command.ExecuteReader();
                    int index = reader.FieldCount;
                    while (reader.Read())
                    {
                        Dictionary<string, object> o = new Dictionary<string, object>();
                        for (int i = 0; i < index; i++)
                        {
                            o.Add(reader.GetName(i), reader.GetValue(i));
                        }
                        data = o;
                    }

                    reader.Close();
                    connection.Close();
                    return data;
                }
                catch (Exception ex)
                {
                    connection.Close();

                    ApplicationManager.ExceptionHandler(ex);
                    return null;
                }
            }
        }
    }
}
