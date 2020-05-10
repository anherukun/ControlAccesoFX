# Definición
Clase con propiedades y modulos para la interaccion del programa con la base de datos.

Proporcionando métodos para: 
- Insertar y obtener registros en la base de datos

[[_TOC_]]

# Constructor
----

Inicializa un objeto de la clase creando una cadena de conexion, ya sea para el modo depuracion (DEBUG) o produccion (RELEASE)

``` csharp
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
```

Los parametros son valores que obtiene del archivo `sources.data` ubicado en la carpeta de instalación del programa

# Propiedades
----

| Acceso    | Tipo de dato | Nombre de la variable &emsp;&emsp;&emsp;&emsp;&emsp; | Default &emsp;&emsp;&emsp;&emsp; |
|-----------|--------------|------------------------------------------------------|----------------------------------|
| `private` | `string`     | ConnectionString                                     | ""                               |

# Metodos
----
##[InsertData(string sql)]()

Ejecuta una instrucción de inserción SQL en la base de datos.

``` csharp
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
```

**Retorno**
``` csharp 
bool
```

----
##[FromDatabaseToDictionary(string sql)]()

Obtiene una lista con múltiples registros representados en elementos <Clave, Valor> como respuesta a la transacción SQL enviada a la base de datos

``` csharp
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
```

**Retorno**
``` csharp 
List<Dictionary<string, string>>
```

# Ver también
- [Clase SharedCode](/SharedCode)
- [Clase ApplicationManager](/SharedCode/SharedCode.ApplicationManager)