# Definición
Clase con módulos para la creación de documentos HTML con base a plantillas programa con la base de datos.

Proporcionando métodos para: 
- Generar el documento final con la plantilla determinada
- Exportar y guardar el documento en el ordenador

[[_TOC_]]

# Metodos
----
##[GenerateHTML(string HTMLSource, Dictionary<string, string> values)]()

Con una plantilla de un documento HTML, intercambia las claves de los elementos con los valores del diccionario

``` csharp
public static void GenerateHTML(string HTMLSource, Dictionary<string, string> values)
{
    try
    {
        string result = File.ReadAllText(HTMLSource);

        foreach (var valuePair in values)
        {
            result = result.Replace(valuePair.Key, valuePair.Value);
        }

        ExportHTMLFile(result);
    }
    catch (Exception ex)
    {
        ApplicationManager.ExceptionHandler(ex);
    }
}
```

----
##[ExportHTMLFile(string HTMLSource)]()

Escribe en disco un documento HTML

``` csharp
public static bool ExportHTMLFile(string HTMLSource)
{    
    try
    {
        SaveFileDialog saveFileDialog = new SaveFileDialog();
        saveFileDialog.Filter = "Documento HTML (*.html)|*.html";
        saveFileDialog.DefaultExt = "*.html";
        if (saveFileDialog.ShowDialog() == true)
        {
            File.WriteAllText(saveFileDialog.FileName, HTMLSource);
            return true;
        }
    }
    catch (Exception ex)
    {
        ApplicationManager.ExceptionHandler(ex);
    }
    return false;
}
```

**Retorno**
``` csharp 
bool
```

---
# Ver también
- [Clase SharedCode](/SharedCode)
- [Clase ApplicationManager](/SharedCode/SharedCode.ApplicationManager)