using System;
using System.IO;

public static class BoilerPlateGenerator
{
    public static void CreateView(string entityName, string sourcePath, string outputPath)
    {
        const string viewsFolder = "Views";
        
        Directory.CreateDirectory(Path.Combine(outputPath, viewsFolder, entityName));
        
        var viewFileName = entityName + "View.cs";
        var viewText = File.ReadAllText(Path.Combine( sourcePath, viewsFolder, "PlaceHolderView.cs"));
        viewText = ReplacePlaceHolder(viewText, entityName);
        
        File.WriteAllText(Path.Combine( outputPath, viewsFolder, entityName, viewFileName), viewText);
    }

    public static void CreatePresenter(string entityName, string sourcePath, string outputPath)
    {
        Directory.CreateDirectory(Path.Combine(outputPath, "Presenters", entityName));
        
        var presenterFileName = entityName + "Presenter.cs";
        var iViewFileName = "I" + entityName + "View.cs";
        
        var iViewText = File.ReadAllText(Path.Combine(sourcePath, "Presenters", "IPlaceHolderView.cs"));
        iViewText = ReplacePlaceHolder(iViewText, entityName);
        File.WriteAllText(Path.Combine(outputPath, "Presenters", entityName, iViewFileName), iViewText);
        
        var presenterText = File.ReadAllText(Path.Combine(sourcePath, "Presenters", "PlaceHolderPresenter.cs"));
        presenterText = ReplacePlaceHolder(presenterText, entityName);
        File.WriteAllText(Path.Combine(outputPath,"Presenters", entityName, presenterFileName), presenterText);
    }

    public static void CreateUseCase(string entityName, string sourcePath, string outputPath)
    {
        Directory.CreateDirectory(Path.Combine(outputPath, "UseCases", entityName));
        
        var useCaseFileName = entityName + "UseCase.cs";
        var iPresenterFileName = "I" + entityName + "Presenter.cs";
        var iFactoryFileName = "I" + entityName + "Factory.cs";
        
        var useCaseText = File.ReadAllText(Path.Combine(sourcePath, "UseCases", "PlaceHolderUseCase.cs"));
        useCaseText = ReplacePlaceHolder(useCaseText, entityName);
        File.WriteAllText(Path.Combine(outputPath, "UseCases", entityName, useCaseFileName), useCaseText);
        
        var iPresenterText = File.ReadAllText(Path.Combine(sourcePath, "UseCases", "IPlaceHolderPresenter.cs"));
        iPresenterText = ReplacePlaceHolder(iPresenterText, entityName);
        File.WriteAllText(Path.Combine(outputPath, "UseCases", entityName, iPresenterFileName), iPresenterText);
        
        var iFactoryText = File.ReadAllText(Path.Combine(sourcePath, "UseCases", "IPlaceHolderFactory.cs"));
        iFactoryText = ReplacePlaceHolder(iFactoryText, entityName);
        File.WriteAllText(Path.Combine(outputPath, "UseCases", entityName, iFactoryFileName), iFactoryText);
    }

    public static void CreateFactory(string entityName, string sourcePath, string outputPath)
    {
        var factoryFileName = entityName + "Factory.cs";
        
        var factoryText = File.ReadAllText(Path.Combine(sourcePath, "Factories", "PlaceHolderFactory.cs"));
        factoryText = ReplacePlaceHolder(factoryText, entityName);
        File.WriteAllText(Path.Combine(outputPath, "Factories", factoryFileName), factoryText);
    }

    private static string ReplacePlaceHolder(string toReplace, string entityName)
    {
        var upperCaseName = entityName;
        var lowerCaseCase = GetLowerCaseName(entityName);
        
        toReplace = toReplace.Replace("PlaceHolder", upperCaseName, StringComparison.Ordinal);
        toReplace = toReplace.Replace("placeHolder", lowerCaseCase, StringComparison.Ordinal);
        return toReplace;
    }
    
    private static string GetLowerCaseName(string upperCaseName)
    {
        string firstChar = upperCaseName.Substring(0, 1);
        firstChar = firstChar.ToLower();
        return firstChar + upperCaseName.Substring(1, upperCaseName.Length - 1);
    }
}
