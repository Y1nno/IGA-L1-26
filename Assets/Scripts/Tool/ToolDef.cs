using UnityEngine;

[CreateAssetMenu(fileName = "ToolDef", menuName = "Scriptable Objects/ToolDef")]
public class ToolDef : ScriptableObject
{
    public Tool toolPrefab;

    public Tool InstantiateTool()
    {
        return Instantiate(toolPrefab);
    }
}
