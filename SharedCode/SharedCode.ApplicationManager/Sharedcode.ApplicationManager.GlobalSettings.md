# Definición
Clase con propiedades y modulos para el manejo de las configuraciones del programa.

Proporcionando métodos estáticos para: 
- Serializar la clase en un arreglo de bytes
- Dezerializar un arreglo de bytes en la clase

[[_TOC_]]

#Propiedades
----

| Acceso   | Tipo de dato | Nombre de la variable &emsp;&emsp;&emsp;&emsp;&emsp; | Default &emsp;&emsp;&emsp;&emsp; |
|----------|--------------|------------------------------------------------------|----------------------------------|
| `public` | `int`        | ClaveDepto                                           | 0                                |
| `public` | `int`        | LogLimit                                             | 15                               |
| `public` | `bool`       | BootOnStartup                                        | false                            |

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
byte[]
```

# Ver también
- [SharedCode](/SharedCode)
- [SharedCode.ApplicationManager](/SharedCode/SharedCode.ApplicationManager)