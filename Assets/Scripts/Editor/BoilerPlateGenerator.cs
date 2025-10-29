using System.IO;
using UnityEditor;

public abstract class BoilerPlateGenerator
{
    private static string entityName = "Test";
    private static string entityNameLowerCase = "test";
    
    [MenuItem("Tools/BoilerPlateGenerator")]
    private static void Generate()
    {
        CreateView();
        CreatePresenter();
    }

    private static void CreateView()
    {
        var viewFileName = entityName + "View.cs";
        var viewText = File.ReadAllText("Assets/Scripts/Editor/Views/PlaceHolderView.cs");
        viewText = ReplacePlaceHolder(viewText);
        File.WriteAllText("Assets/Scripts/Meta/Views/" + viewFileName, viewText);
    }

    private static void CreatePresenter()
    {
        var presenterFileName = entityName + "Presenter.cs";
        var iViewFileName = "I" + entityName + "View.cs";
        
        var iViewText = File.ReadAllText("Assets/Scripts/Editor/Presenters/IPlaceHolderView.cs");
        iViewText = ReplacePlaceHolder(iViewText);
        File.WriteAllText("Assets/Scripts/Meta/Presenters/" + iViewFileName, iViewText);
        
        var presenterText = File.ReadAllText("Assets/Scripts/Editor/Presenters/PlaceHolderPresenter.cs");
        presenterText = ReplacePlaceHolder(presenterText);
        File.WriteAllText("Assets/Scripts/Meta/Presenters/" + presenterFileName, presenterText);
    }

    private static string ReplacePlaceHolder(string toReplace)
    {
        toReplace = toReplace.Replace("PlaceHolder", entityName);
        toReplace = toReplace.Replace("placeHolder", entityNameLowerCase);
        return toReplace;
    }
}