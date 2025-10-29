using System.IO;
using UnityEditor;
using UnityEngine;

public abstract class BoilerPlateGenerator
{
    private static string entityName = "Authenticator";
    private static string entityNameLowerCase = "authenticator";
    
    [MenuItem("Tools/BoilerPlateGenerator")]
    private static void Generate()
    {
        CreateView();
        CreatePresenter();
        CreateUseCase();
        CreateFactory();
        
        Debug.Log("Generation is finished");
    }

    private static void CreateView()
    {
        Directory.CreateDirectory("Assets/Scripts/Meta/Views/" + entityName);
        
        var viewFileName = entityName + "View.cs";
        var viewText = File.ReadAllText("Assets/Scripts/Editor/Views/PlaceHolderView.cs");
        viewText = ReplacePlaceHolder(viewText);
        File.WriteAllText("Assets/Scripts/Meta/Views/" + entityName + "/" + viewFileName, viewText);
    }

    private static void CreatePresenter()
    {
        Directory.CreateDirectory("Assets/Scripts/Meta/Presenters/" + entityName);
        
        var presenterFileName = entityName + "Presenter.cs";
        var iViewFileName = "I" + entityName + "View.cs";
        
        var iViewText = File.ReadAllText("Assets/Scripts/Editor/Presenters/IPlaceHolderView.cs");
        iViewText = ReplacePlaceHolder(iViewText);
        File.WriteAllText("Assets/Scripts/Meta/Presenters/" + entityName + "/" + iViewFileName, iViewText);
        
        var presenterText = File.ReadAllText("Assets/Scripts/Editor/Presenters/PlaceHolderPresenter.cs");
        presenterText = ReplacePlaceHolder(presenterText);
        File.WriteAllText("Assets/Scripts/Meta/Presenters/" + entityName + "/" + presenterFileName, presenterText);
    }

    private static void CreateUseCase()
    {
        Directory.CreateDirectory("Assets/Scripts/Meta/UseCases/" + entityName);
        
        var useCaseFileName = entityName + "UseCase.cs";
        var iPresenterFileName = "I" + entityName + "Presenter.cs";
        var iFactoryFileName = "I" + entityName + "Factory.cs";
        
        var useCaseText = File.ReadAllText("Assets/Scripts/Editor/UseCases/PlaceHolderUseCase.cs");
        useCaseText = ReplacePlaceHolder(useCaseText);
        File.WriteAllText("Assets/Scripts/Meta/UseCases/" + entityName + "/" + useCaseFileName, useCaseText);
        
        var iPresenterText = File.ReadAllText("Assets/Scripts/Editor/UseCases/IPlaceHolderPresenter.cs");
        iPresenterText = ReplacePlaceHolder(iPresenterText);
        File.WriteAllText("Assets/Scripts/Meta/UseCases/" + entityName + "/" + iPresenterFileName, iPresenterText);
        
        var iFactoryText = File.ReadAllText("Assets/Scripts/Editor/UseCases/IPlaceHolderFactory.cs");
        iFactoryText = ReplacePlaceHolder(iFactoryText);
        File.WriteAllText("Assets/Scripts/Meta/UseCases/" + entityName + "/" + iFactoryFileName, iFactoryText);
    }

    private static void CreateFactory()
    {
        var factoryFileName = entityName + "Factory.cs";
        
        var factoryText = File.ReadAllText("Assets/Scripts/Editor/Factories/PlaceHolderFactory.cs");
        factoryText = ReplacePlaceHolder(factoryText);
        File.WriteAllText("Assets/Scripts/Meta/Factories/" + factoryFileName, factoryText);
    }

    private static string ReplacePlaceHolder(string toReplace)
    {
        toReplace = toReplace.Replace("PlaceHolder", entityName);
        toReplace = toReplace.Replace("placeHolder", entityNameLowerCase);
        return toReplace;
    }
}