using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ProcessingStationDef", menuName = "Scriptable Objects/ProcessingStationDef")]
public class ProcessingStationDef : Station
{
    [Header("Processing Settings")]
    public float secondsForCooking;
    public float additionalSecondsForBurning;
    public bool canBeRuined;
    public ProcessingType processingType;
    public Sprite emptySprite;
    public Sprite fullSprite;
    public Sprite readySprite;
    public Sprite ruinedSprite;
    public List<StepsDef> recipes;
    public IngredientDef ruinedOutput;

    public Ingredient GetOutputForIngredients(List<Ingredient> ingredients)
    {
        foreach (var recipe in recipes)
        {

            if (recipe.ingredients.Count == ingredients.Count)
            {
                if (recipe.ingredients.Count > 1) recipe.ingredients.Sort();
                bool match = true;
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
        return GetRuinedOutput();
    }

    public Ingredient GetRuinedOutput()
    {
        return ruinedOutput.InitIntoGameObject().GetComponent<Ingredient>();
    }

    public Sprite SetSpriteForState(ProcessingStationState state)
    {
        switch (state)
        {
            case ProcessingStationState.Empty:
                return emptySprite;
            case ProcessingStationState.Full:
                return fullSprite;
            case ProcessingStationState.Ready:
                return readySprite;
            case ProcessingStationState.Ruined:
                return ruinedSprite;
        }
        Debug.LogWarning($"Unhandled Sprite for ProcessingStationState: {state}");
        return null;
    }

    public override void Interact()
    {
        // no implementation needed for ScriptableObject
    }
}
