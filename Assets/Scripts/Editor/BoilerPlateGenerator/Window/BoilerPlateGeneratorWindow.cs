using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class BoilerPlateGeneratorWindow : EditorWindow
{
    [SerializeField] private VisualTreeAsset _visualTreeAsset;

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
        
        root.Add(_visualTreeAsset.Instantiate());

        foreach (var visualElement in root.Query().ToList())
        {
            Debug.Log(visualElement.name);
        }

        root.Q<Button>("Generate").clicked += Generate;
    }

    private void Generate()
    {
        var first = AssetDatabase.FindAssets("t:BoilerPlateGeneratorSettings").First();
        Debug.Log(AssetDatabase.GUIDToAssetPath(first));
    }
}
