using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class BoilerPlateGeneratorWindow : EditorWindow
{
    [SerializeField] private VisualTreeAsset visualTreeAsset;

    [MenuItem("Window/UI Toolkit/BoilerPlateGeneratorWindow")]
    public static void ShowExample()
    {
        BoilerPlateGeneratorWindow wnd = GetWindow<BoilerPlateGeneratorWindow>();
        wnd.titleContent = new GUIContent("BoilerPlateGeneratorWindow");
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        var guids = AssetDatabase.FindAssets("t:BoilerPlateGeneratorSettings");

        if (guids.Length == 0)
            throw new FileNotFoundException("Scriptable object of type BoilerPlateGeneratorSettings is not found");
        
        var settingsGuid = new GUID(guids[0]);
        var settings = AssetDatabase.LoadAssetByGUID<BoilerPlateGeneratorSettings>(settingsGuid);
        
        root.Add(visualTreeAsset.Instantiate());

        var nameField = root.Q<TextField>("NameField");
        var sourcePathField = root.Q<TextField>("SourceField");
        var outputPathField = root.Q<TextField>("OutputField");
        
        nameField.SetValueWithoutNotify("");

        sourcePathField.SetValueWithoutNotify(settings.sourcePath);
        outputPathField.SetValueWithoutNotify(settings.outputPath);

        sourcePathField.RegisterValueChangedCallback(callback =>
        {
            settings.sourcePath = callback.newValue;
            EditorUtility.SetDirty(settings);
        });
        outputPathField.RegisterValueChangedCallback(callback =>
        {
            settings.outputPath = callback.newValue;
            EditorUtility.SetDirty(settings);
        });

        root.Q<Button>("Generate").clicked += () =>
        {
            BoilerPlateGenerator.CreateView(nameField.value, sourcePathField.value, outputPathField.value);
            BoilerPlateGenerator.CreatePresenter(nameField.value, sourcePathField.value, outputPathField.value);
            BoilerPlateGenerator.CreateUseCase(nameField.value, sourcePathField.value, outputPathField.value);
            BoilerPlateGenerator.CreateFactory(nameField.value, sourcePathField.value, outputPathField.value);
        };
    }
}
