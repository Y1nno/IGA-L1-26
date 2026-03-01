using System.Collections.Generic;
using UnityEngine;

public class ProcessingStation : Station
{
    public ProcessingStationState state { get; private set; } = ProcessingStationState.Empty;
    private ProcessingStationDef definition;

    private float secondsForCooking;
    private float additionalSecondsForBurning;
    private float progress;
    private List<Ingredient> ingredients = new List<Ingredient>();
    private Ingredient ingredientOnClick;


    public void Update()
    {
        if (state == ProcessingStationState.Processing)
        {
            progress += Time.deltaTime;
            if (progress >= secondsForCooking)
            {
                state = ProcessingStationState.Ready;
                ingredients.Sort();
                ingredientOnClick = definition.GetOutputForIngredients(ingredients);
            }
        }
        if (state == ProcessingStationState.Ready)
        {
            progress += Time.deltaTime;
            if (progress >= secondsForCooking + additionalSecondsForBurning)
            {
                state = ProcessingStationState.Ruined;
                ingredientOnClick = definition.GetRuinedOutput();
            }
        }
    }

    private void StartCooking()
    {
        state = ProcessingStationState.Processing;
        progress = 0f;
    }

    public override void Interact()
    {
        switch (state)
        {
            case ProcessingStationState.Empty:
                {
                    break;
                }
            case ProcessingStationState.Full:
            case ProcessingStationState.Ready:
            case ProcessingStationState.Ruined:
                {
                    Pointer.Instance.PutInHand(GetIngredientOnClick().gameObject);
                    break;
                }
        }
    }

    private void AddIngredient(Ingredient ingredient)
    {
        ingredients.Add(ingredient);
        if (ingredients.Count > 0)
        {
            state = ProcessingStationState.Full;
        }
    }

    private Ingredient GetIngredientOnClick()
    {
        Ingredient result = ingredientOnClick;
        ingredients.Remove(ingredientOnClick);
        ingredientOnClick = ingredients.Count > 0 ? ingredients[ingredients.Count - 1] : null;
        return result;
    }

    public void ClearIngredients()
    {
        ingredients.Clear();
        state = ProcessingStationState.Empty;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Ingredient>() != null)
        {
            Ingredient ingredient = other.GetComponent<Ingredient>();
            if (ingredient != null)
            {
                AddIngredient(ingredient);
            }
        }
    }

    public void ChangeState(ProcessingStationState newState)
    {
        state = newState;
        definition.SetSpriteForState(newState);
    }
}
