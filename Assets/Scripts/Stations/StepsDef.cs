using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StepsDef", menuName = "Scriptable Objects/StepsDef")]
public class StepsDef : ScriptableObject
{
    public List<IngredientDef> ingredients;
    public IngredientDef output;

    public Ingredient GetOutput()
    {
        return output.InitIntoGameObject().GetComponent<Ingredient>();
    }
}
