# Definición
Clase con múltiples módulos que ejecutan diversas funciones que ayudan al funcionamiento del programa.

Proporcionando en su mayoría métodos estáticos para: 
- Iniciar el recolector de basura, 
- Verificar, leer y escribir archivos ubicados en la carpeta `%APPDATA%` del programa, 
- Crear y eliminar nuevos valores en las llaves de registro de Windows

[[_TOC_]]

# Metodos
----
##[RetriveFromSourcesFile()]()

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
##[ReadBinaryFileOnAppdata(string filename)]()

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

----
##[WriteBinaryFileOnAppdata(byte[] bytes, string filename)]()

Escribe un arreglo de bytes en un archivo de la ruta preestablecida: `C:\Users\%USERNAME%\AppData\Local\ControlAcceso`

``` csharp
static public bool WriteBinaryFileOnAppdata(byte[] bytes, string filename)
{
    if (!Directory.Exists($"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\\ControlAcceso\\{filename}"))
    {
        Directory.CreateDirectory($"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\\ControlAcceso");
    }

    try
    {
        using (FileStream stream = File.OpenWrite($"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\\ControlAcceso\\{filename}"))
        {
            stream.Write(bytes, 0, bytes.Length);
            return true;
        }
    }
    catch (Exception ex)
    {
        MessageBox.Show(ex.Message);
        return false;
    }
}
```

**Retorno**
``` csharp
true // Si fue escrito el archivo correctamente

false // Cuando ocurre un error inesperado
```

----
##[FileExistOnAppdata(string filename)]()

Verifica que exista un archivo de la ruta preestablecida:
`C:\Users\%USERNAME%\AppData\Local\ControlAcceso`

``` csharp
static public bool FileExistOnAppdata(string filename) => File.Exists($"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}/ControlAcceso/{filename}") ? true : false;
// Es lo mismo que escribir esto:
// { 
//     if (File.Exists($"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}/ControlAcceso/{filename}"))
//     {
//         return true;
//     }
//     return false;
// }
```

``` csharp
bool
```

----
##[WriteRegistryKey(string keyPath, string valueName, object value)]()

Escribe un valor en una llave de registro de Windows

``` csharp
static public bool WriteRegistryKey(string keyPath, string valueName, object value)
{
    try
    {
        RegistryKey registryKey = Registry.CurrentUser;
        RegistryKey path = registryKey.OpenSubKey(keyPath, true);
        path.SetValue(valueName, value);
        
        Console.WriteLine("Application: RegistryKey Value Created Successfully");
        return true;
    }
    catch (Exception ex)
    {
        MessageBox.Show(ex.Message);
        return false;
    }
}
```

**Retorno**
``` csharp
true // Si fue escrito el archivo correctamente

false // Cuando ocurre un error inesperado
```

----
##[DeleteRegistryKey(string keyPath, string valueName)]()

Elimina un valor un una llave de registro de 

``` csharp
static public bool DeleteRegistryKey(string keyPath, string valueName)
{
    try
    {
        RegistryKey registryKey = Registry.CurrentUser;
        RegistryKey path = registryKey.OpenSubKey(keyPath, true);
        
        path.DeleteValue(valueName);
        Console.WriteLine("Application: RegistryKey Value Deleted Successfully");
        return true;
    }
    catch (Exception ex)
    {
        MessageBox.Show(ex.Message);
        return false;
    }
}
```

**Retorno**
``` csharp
true // Si fue escrito el archivo correctamente

false // Cuando ocurre un error inesperado
```

----
##[InitGB()]()

Ejecuta el recolector de basura para liberar memoria que ya no se esta utilizando

``` csharp
static public void InitGB()
{
    GC.Collect();
    GC.WaitForPendingFinalizers();
    GC.Collect();
    GC.WaitForFullGCComplete();
}
```

----
##[ExceptionHandler(Exception e)]()

Recibe una excepcion e intenta documentarla en archivo de texto que es guardado en la ruta preestablecida:
`C:\Users\%USERNAME%\AppData\Local\ControlAcceso\Error-Log`

``` csharp
static public void ExceptionHandler(Exception e)
{
    try
    {
        dateTime = DateTime.Now;
        if (!Directory.Exists($"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\\ControlAcceso\\Error-Log\\{dateTime.Ticks}-Error.txt"))
        {
            Directory.CreateDirectory($"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\\ControlAcceso\\Error-Log");
        }

        MessageBox.Show($"Se ha detectado un error durante la ejecicion del programa, si persiste contacte a su departamento de TI:\n{e.Message}");
        string txt = $"Mensaje:\t{e.Message}\n" +
            $"Source: \t{e.Source}\n" +
            $"StackTrace:\n{e.StackTrace}\n" +
            $"TargetSite:\t{e.TargetSite}" +
            $"HelpLink:\t{e.HelpLink}\n" +
            $"HResult:\t{e.HResult}\n";

        if (e.Data.Count > 0)
            foreach (var key in e.Data)
            {
                txt += $"{key.ToString()}\t";
            }
            
            File.WriteAllText($"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\\ControlAcceso\\Error-Log\\{dateTime.Ticks}-Error.txt", txt);
    }
    catch (Exception ex)
    {
        MessageBox.Show($"Se detectado un error en la ejecucion y se intento registrar el evento, contacte a su departamto de TI\nRaiz: {e.Message} \nCatcher:{ex.Message}");
        throw;
    }
}
```
# Ver también
- [SharedCode](/SharedCode)
- [SharedCode.ApplicationManager.GlobalSettings](/SharedCode/SharedCode.ApplicationManager/Sharedcode.ApplicationManager.GlobalSettings)