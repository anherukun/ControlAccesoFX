# Definición
Clase con múltiples módulos que ejecutan diversas funciones que ayudan al funcionamiento del programa.

Proporcionando en su mayoría métodos estáticos para: 
- Iniciar el recolector de basura, 
- Verificar, leer y escribir archivos ubicados en la carpeta `%APPDATA%` del programa, 
- Crear y eliminar nuevos valores en las llaves de registro de Windows

[[_TOC_]]

# Metodos


|Retorno|Método|Descripción|
|--|--|--|
| `Dictionary<string, string>` | `RetriveFromSourcesFile()` | Lee el archivo sources.data de la carpeta donde se encuentre el ejecutable del programa, devolviendo un diccionario de cadenas con los valores que contenga el archivo |
|  |  |  |
|  |  |  |
|  |  |  |



----
[RetriveFromSourcesFile()]()

Lee el archivo sources.data de la carpeta donde se encuentre el ejecutable del programa, devolviendo un diccionario de cadenas con los valores que contenga el archivo


``` csharp
using SharedCode;
using SharedCode.Metadata;
```

# Ver tambien
- [SharedCode](/SharedCode)