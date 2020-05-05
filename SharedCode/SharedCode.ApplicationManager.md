# Definición
Clase con múltiples módulos que ejecutan diversas funciones que ayudan al funcionamiento del programa.

Proporcionando en su mayoría métodos estáticos para: 
- Iniciar el recolector de basura, 
- Verificar, leer y escribir archivos ubicados en la carpeta `%APPDATA%` del programa, 
- Crear y eliminar nuevos valores en las llaves de registro de Windows

[[_TOC_]]

# Metodos
----
[RetriveFromSourcesFile()]()

Lee el archivo sources.data de la carpeta donde se encuentre el ejecutable del programa, devolviendo un diccionario de cadenas con los valores que contenga el archivo


``` csharp
public static Dictionary<string, string> RetriveFromSourcesFile()
{
    Dictionary<string, string> result = new Dictionary<string, string>();
    try
    {
        string[] lines = System.IO.File.ReadAllLines($"{Environment.CurrentDirectory}/sources.data");
        foreach (string s in lines)
        {
            result.Add(s.Split(',')[0].Trim(), s.Split(',')[1].Trim());
        }
        return result;
    }
    catch (Exception ex)
    {
        MessageBox.Show(ex.Message);
        return null;
    }
}
```
El contenido del archivo ``sources.data`` son lineas con el nombre de un indice y el valor que toma ese indice, separado entre comas y cada linea en el archivo es un valor. Ejemplo:

```
DATABASE_PATH, F:\angel\Escritorio\HomeOffice\Proyecto ControlAccesos Checador\BaseControlAcceso_bkp.accdb
DATABASE_PATH_DEBUG, F:\angel\Escritorio\HomeOffice\Proyecto ControlAccesos Checador\BaseControlAcceso.accdb
```
**Retorno**
``` csharp 
Dictionary<string, string>
```
----
[ReadBinaryFileOnAppdata(string filename)]()

Lee un archivo binario alojado en la carpeta %APPDATA% del programa, retornando un arreglo de bytes

``` csharp 
static public byte[] ReadBinaryFileOnAppdata(string filename)
{
    try
    {
        return File.ReadAllBytes($"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\\ControlAcceso\\{filename}");
    }
    catch (Exception ex)
    {
        MessageBox.Show(ex.Message);
        return null;
    }
}
```

El archivo que se esta leyendo con este metodo es una clase serializada y exportada en la carpeta `%APPDATA%` del programa

**Retorno**
``` csharp
byte[]
```

# Ver tambien
- [SharedCode](/SharedCode)