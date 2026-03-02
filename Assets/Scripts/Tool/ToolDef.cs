using UnityEngine;

[CreateAssetMenu(fileName = "ToolDef", menuName = "Scriptable Objects/ToolDef")]
public class ToolDef : ScriptableObject
{
    public Tool toolPrefab;
    public float AngleWhenPickedUp = 0f;

    public Tool InstantiateTool()
    {
        return Instantiate(toolPrefab);
    }
}
