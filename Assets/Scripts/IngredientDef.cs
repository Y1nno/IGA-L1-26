using UnityEngine;

[CreateAssetMenu(fileName = "IngredientDef", menuName = "Scriptable Objects/IngredientDef")]
public class IngredientDef : ScriptableObject
{
    public string displayName;
    public IngredientType ingredientType;
    public Sprite icon;
}
