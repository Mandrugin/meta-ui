using UnityEngine;

[CreateAssetMenu(fileName = "BoilerPlateGeneratorSettings", menuName = "Scriptable Objects/BoilerPlateGeneratorSettings")]
public class BoilerPlateGeneratorSettings : ScriptableObject
{
    public string sourcePath;
    public string outputPath;
}
