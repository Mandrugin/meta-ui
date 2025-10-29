using System.IO;
using PlasticGui;
using UnityEditor;

public abstract class BoilerPlateGenerator
{
    private static string entityName = "Test";
    private static string entityNameLowerCase = "test";
    
    [MenuItem("Tools/BoilerPlateGenerator")]
    private static void Generate()
    {
        CreateView();
    }

    private static void CreateView()
    {
        var viewFileName = entityName + "View.cs";
        var viewText = File.ReadAllText("Assets/Scripts/Editor/Views/PlaceHolderView.cs");
        viewText = viewText.Replace("PlaceHolder", entityName);
        File.WriteAllText("Assets/Scripts/Meta/Views/" + viewFileName, viewText);
    }
}