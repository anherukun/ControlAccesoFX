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
##[ToBytes(GlobalSettings globalSettings)]()

Serializa de la clase `GlobalSettings` a un arreglo de `bytes`

``` csharp
public static byte[] ToBytes(GlobalSettings globalSettings)
{
    if (globalSettings == null)
        return null;

    BinaryFormatter formatter = new BinaryFormatter();
    MemoryStream stream = new MemoryStream();
    formatter.Serialize(stream, globalSettings);

    return stream.ToArray();
}
```

**Retorno**
``` csharp 
byte[]
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