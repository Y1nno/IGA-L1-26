using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "PlatingStationDef", menuName = "Scriptable Objects/PlatingStationDef")]
public class PlatingStationDef : Station
{
    [Header("Plating Station Settings")]
    public List<StepsDef> recipes;

    public Ingredient GetOutputForIngredients(List<Ingredient> ingredients)
    {
        if (ingredients.Count > 1) ingredients.Sort();
        foreach (var recipe in recipes)
        {
            //Debug.Log("Count: " + recipe.ingredients.Count + ", Ingredients provided: " + ingredients.Count);
            if (recipe.ingredients.Count == ingredients.Count)
            {
                //Debug.Log("Found matching recipe");
                if (recipe.ingredients.Count > 1) recipe.ingredients.Sort();
                bool match = true;
                //Debug.Log("Checking ingredients...");
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
                    return recipe.GetOutputs()[0];
                }
            }
        }
        return null;
    }

    public bool CanAddIngredient(List<Ingredient> current, Ingredient toAdd)
    {
        var candidateCounts = new Dictionary<IngredientDef, int>();

        void AddToCounts(Ingredient ing)
        {
            if (ing == null || ing.definition == null)
            {
                Debug.LogWarning("Candidate ingredient or its definition is null.");
                return;
            }

            var def = ing.definition;
            candidateCounts[def] = candidateCounts.TryGetValue(def, out var c) ? c + 1 : 1;
        }

        foreach (var ing in current) AddToCounts(ing);
        AddToCounts(toAdd);

        foreach (var recipe in recipes)
        {
            /*
            // Print candidate defs
            foreach (var kvp in candidateCounts)
            {
                Debug.Log($"Candidate wants: {kvp.Key.name} (id {kvp.Key.GetInstanceID()}) x{kvp.Value}");
            }

            // Print recipe defs
            foreach (var def in recipe.ingredients)
            {
                if (def == null)
                {
                    Debug.LogWarning("Recipe contains a null ingredient definition!");
                    continue;
                }
                Debug.Log($"Recipe has: {def.name} (id {def.GetInstanceID()})");
            }
            */

            // subset check
            var recipeCounts = new Dictionary<IngredientDef, int>();
            foreach (var def in recipe.ingredients)
            {
                if (def == null) continue;
                recipeCounts[def] = recipeCounts.TryGetValue(def, out var c) ? c + 1 : 1;
            }

            bool subset = true;
            foreach (var kvp in candidateCounts)
            {
                if (!recipeCounts.TryGetValue(kvp.Key, out int recipeCount) || kvp.Value > recipeCount)
                {
                    //Debug.Log($"FAIL: recipe doesn't match {kvp.Key.name} (candidate id {kvp.Key.GetInstanceID()})");
                    subset = false;
                    break;
                }
            }

            if (subset) return true;
        }

        return false;
    }

    public override void Interact()
    {
        // Interaction logic for the plating station not needed
    }
}
