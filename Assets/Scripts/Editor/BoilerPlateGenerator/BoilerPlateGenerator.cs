using System;
using System.IO;

public static class BoilerPlateGenerator
{
    public static void CreateView(string entityName, string sourcePath, string outputPath)
    {
        const string viewsFolder = "Views";
        sourcePath = Path.Combine(sourcePath, viewsFolder);
        outputPath = Path.Combine(outputPath, viewsFolder);
        
        Directory.CreateDirectory(Path.Combine(outputPath, entityName));
        
        var viewFileName = entityName + "View.cs";
        var viewText = File.ReadAllText(Path.Combine( sourcePath, "PlaceHolderView.cs"));
        viewText = ReplacePlaceHolder(viewText, entityName);
        
        File.WriteAllText(Path.Combine( outputPath, entityName, viewFileName), viewText);
    }

    public static void CreatePresenter(string entityName, string sourcePath, string outputPath)
    {
        const string presentersFolder = "Presenters";
        sourcePath = Path.Combine(sourcePath, presentersFolder);
        outputPath = Path.Combine(outputPath, presentersFolder);
        
        Directory.CreateDirectory(Path.Combine(outputPath, entityName));
        
        var presenterFileName = entityName + "Presenter.cs";
        var iViewFileName = "I" + entityName + "View.cs";
        
        var iViewText = File.ReadAllText(Path.Combine(sourcePath, "IPlaceHolderView.cs"));
        iViewText = ReplacePlaceHolder(iViewText, entityName);
        File.WriteAllText(Path.Combine(outputPath, entityName, iViewFileName), iViewText);
        
        var presenterText = File.ReadAllText(Path.Combine(sourcePath, "PlaceHolderPresenter.cs"));
        presenterText = ReplacePlaceHolder(presenterText, entityName);
        File.WriteAllText(Path.Combine(outputPath, entityName, presenterFileName), presenterText);
    }

    public static void CreateUseCase(string entityName, string sourcePath, string outputPath, bool useCaseItself)
    {
        const string useCasesFolder = "UseCases";
        sourcePath = Path.Combine(sourcePath, useCasesFolder);
        outputPath = Path.Combine(outputPath, useCasesFolder);
        
        Directory.CreateDirectory(Path.Combine(outputPath, entityName));
        
        var useCaseFileName = entityName + "UseCase.cs";
        var iPresenterFileName = "I" + entityName + "Presenter.cs";
        var iFactoryFileName = "I" + entityName + "Factory.cs";
        
        if(useCaseItself)
        {
            var useCaseText = File.ReadAllText(Path.Combine(sourcePath, "PlaceHolderUseCase.cs"));
            useCaseText = ReplacePlaceHolder(useCaseText, entityName);
            File.WriteAllText(Path.Combine(outputPath, entityName, useCaseFileName), useCaseText);            
        }

        var iPresenterText = File.ReadAllText(Path.Combine(sourcePath, "IPlaceHolderPresenter.cs"));
        iPresenterText = ReplacePlaceHolder(iPresenterText, entityName);
        File.WriteAllText(Path.Combine(outputPath, entityName, iPresenterFileName), iPresenterText);
        
        var iFactoryText = File.ReadAllText(Path.Combine(sourcePath, "IPlaceHolderFactory.cs"));
        iFactoryText = ReplacePlaceHolder(iFactoryText, entityName);
        File.WriteAllText(Path.Combine(outputPath, entityName, iFactoryFileName), iFactoryText);
    }

    public static void CreateFactory(string entityName, string sourcePath, string outputPath)
    {
        const string factoriesFolder = "Factories";
        sourcePath = Path.Combine(sourcePath, factoriesFolder);
        outputPath = Path.Combine(outputPath, factoriesFolder);
        
        var factoryFileName = entityName + "Factory.cs";
        
        var factoryText = File.ReadAllText(Path.Combine(sourcePath, "PlaceHolderFactory.cs"));
        factoryText = ReplacePlaceHolder(factoryText, entityName);
        File.WriteAllText(Path.Combine(outputPath, factoryFileName), factoryText);
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
