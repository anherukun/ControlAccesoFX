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
##[FromBytes(byte[] bytes)]()

Deserializa un arreglo de `bytes` a un objeto de clase `GlobalSetings`

``` csharp
public static GlobalSettings FromBytes(byte[] bytes)
{
    MemoryStream stream = new MemoryStream();
    BinaryFormatter formatter = new BinaryFormatter();
    stream.Write(bytes, 0, bytes.Length);
    stream.Seek(0, SeekOrigin.Begin);
    GlobalSettings globalSettings = (GlobalSettings)formatter.Deserialize(stream);

    return globalSettings;
}
```

**Retorno**
``` csharp 
GlobalSettings
```

# Ver también
- [Clase SharedCode](/SharedCode)
- [Clase ApplicationManager](/SharedCode/SharedCode.ApplicationManager)