using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DishDef", menuName = "Scriptable Objects/DishDef")]
public class DishDef : ScriptableObject
{
    public RecipeType dishType;
    public List<StepsDef> steps;
    public List<IngredientDef> ingredients;
}
