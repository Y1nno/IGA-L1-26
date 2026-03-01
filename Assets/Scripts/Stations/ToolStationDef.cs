using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "ToolStationDef", menuName = "Scriptable Objects/ToolStationDef")]
public class ToolStationDef : Station
{
    [Header("ToolStation Settings")]
    public List<ToolStepDef> recipes;

    public Ingredient GetOutputForIngredients(List<Ingredient> ingredients, ToolDef tool)
    {
        foreach (var recipe in recipes)
        {
            Debug.Log("Count: " + recipe.ingredients.Count + ", Ingredients provided: " + ingredients.Count);
            if (recipe.ingredients.Count == ingredients.Count)
            {
                Debug.Log("Found matching recipe");
                if (recipe.ingredients.Count > 1) recipe.ingredients.Sort();
                bool match = true;
                Debug.Log("Tool needed: " + recipe.toolNeeded + ", Tool provided: " + tool);
                if (recipe.toolNeeded != tool)
                {
                    return null;
                }
                Debug.Log("Checking ingredients...");
                for (int i = 0; i < ingredients.Count; i++)
                {
                    if (recipe.ingredients[i] != ingredients[i].definition)
                    {
                        match = false;
                        break;
                    }
                }
                if (match)
                {
                    return recipe.GetOutput();
                }
            }
        }
        return null;
    }

    public override void Interact()
    {
        // Interaction logic for the holding station
    }
}
