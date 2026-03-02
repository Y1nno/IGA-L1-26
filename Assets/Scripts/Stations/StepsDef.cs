using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StepsDef", menuName = "Scriptable Objects/StepsDef")]
public class StepsDef : ScriptableObject
{
    public List<IngredientDef> ingredients;
    public List<IngredientDef> outputs;

    public List<Ingredient> GetOutputs()
    {
        List<Ingredient> outputIngredients = new List<Ingredient>();
        foreach (var output in outputs)
        {
            outputIngredients.Add(output.InitIntoGameObject().GetComponent<Ingredient>());
        }
        return outputIngredients;
    }
}
