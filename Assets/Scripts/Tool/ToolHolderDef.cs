using UnityEngine;

[CreateAssetMenu(fileName = "ToolHolderDef", menuName = "Scriptable Objects/ToolHolderDef")]
public class ToolHolderDef : ScriptableObject
{
    public ToolDef tool;
    public Sprite occupiedSprite;
    public Sprite emptySprite;
}
