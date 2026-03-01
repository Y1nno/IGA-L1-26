using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ProcessingStation : Clickable
{
    public ProcessingStationState state { get; private set; } = ProcessingStationState.Empty;
    public RawImage progressBar;
    public ProcessingStationDef definition;

    private float progress;
    private List<Ingredient> ingredients = new List<Ingredient>();
    private Ingredient ingredientOnClick;


    public void Update()
    {
        if (state == ProcessingStationState.Processing)
        {
            progress += Time.deltaTime;
            if (progress >= definition.secondsForCooking)
            {
                ChangeState(ProcessingStationState.Ready);
                if (ingredients.Count > 1) ingredients.Sort();
                Ingredient output = definition.GetOutputForIngredients(ingredients);
                ClearIngredients();
                AddIngredient(output);
            }
        }
        if (state == ProcessingStationState.Ready && definition.canBeRuined)
        {
            progress += Time.deltaTime;
            if (progress >= definition.secondsForCooking + definition.additionalSecondsForBurning)
            {
                ChangeState(ProcessingStationState.Ruined);
                ClearIngredients();
                AddIngredient(definition.GetRuinedOutput());
            }
        }
        UpdateProgressBar();
    }

    private void StartCooking()
    {
        ChangeState(ProcessingStationState.Processing);
        progress = 0f;
    }

    public void ToggleOnButton()
    {
        if (state == ProcessingStationState.Full)
        {
            Debug.Log("Starting cooking process");
            StartCooking();
            return;
        }
        if (state == ProcessingStationState.Processing)
        {
            progress = 0;
            Debug.Log("Stopping cooking process");
            ChangeState(ProcessingStationState.Full);
            return;
        }
    }

    public override void OnClick()
    {
        Interact();
    }

    public void UpdateProgressBar()
    {
        Color startingColor = progressBar.color;
        Color readyColor = Color.green;
        Color ruiningColor = Color.red;
        if (state == ProcessingStationState.Processing)
        {
            progressBar.color = Color.Lerp(startingColor, readyColor, progress / definition.secondsForCooking);
        }
        else if (state == ProcessingStationState.Ready && definition.canBeRuined)
        {
            progressBar.color = Color.Lerp(readyColor, ruiningColor, (progress - definition.secondsForCooking) / definition.additionalSecondsForBurning);
        }
        else if (progress == 0)
        {
            progressBar.color = startingColor;
        }
    }

    public void Interact()
    {
        Debug.Log("Interacting with station in state: " + state);
        switch (state)
        {
            case ProcessingStationState.Processing:
            case ProcessingStationState.Empty:
                {
                    break;
                }
            case ProcessingStationState.Full:
            case ProcessingStationState.Ready:
            case ProcessingStationState.Ruined:
                {
                    Debug.Log("Interacting with ingredient: " + GetIngredientOnClick().name);
                    Pointer.Instance.PutInHand(GetIngredientOnClick().gameObject);
                    if (ingredientOnClick == null)
                    {
                        ChangeState(ProcessingStationState.Empty);
                    }
                    progress = 0;
                    UpdateProgressBar();
                    break;
                }
        }
    }

    private void AddIngredient(Ingredient ingredient)
    {
        if (ingredients.Contains(ingredient)){return;}
        ingredients.Add(ingredient);
        ingredient.gameObject.SetActive(false);

        Debug.Log("Ingredient added: " + ingredient.name);
        if (ingredients.Count > 0 && state == ProcessingStationState.Empty)
        {
            ChangeState(ProcessingStationState.Full);
        }
    }

    private Ingredient GetIngredientOnClick()
    {
        Ingredient result = ingredients.Count > 0 ? ingredients[ingredients.Count - 1] : null;
        result.gameObject.SetActive(true);
        ingredients.Remove(ingredientOnClick);
        ingredientOnClick = ingredients.Count > 0 ? ingredients[ingredients.Count - 1] : null;
        return result;
    }

    public void ClearIngredients()
    {
        for (int i = 0; i < ingredients.Count; i++)
        {
            Destroy(ingredients[i].gameObject);
        }
        ingredients.Clear();
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("Trigger entered by: " + other.name);
        //Debug.Log("Is trigger an ingredient: " + (other.GetComponent<Ingredient>() != null));
        if (state != ProcessingStationState.Empty && state != ProcessingStationState.Full) {return;}
        if (other.gameObject.transform.parent == Pointer.Instance.transform){return;}
        if (other.GetComponent<Ingredient>() != null)
        {
            Ingredient ingredient = other.GetComponent<Ingredient>();
            if (ingredient != null)
            {
                AddIngredient(ingredient);
                other.gameObject.SetActive(false);
            }
        }
    }

    public void ChangeState(ProcessingStationState newState)
    {
        state = newState;
        Debug.Log("State changed to: " + newState);
        //gameObject.GetComponent<SpriteRenderer>().sprite = definition.SetSpriteForState(newState);
        if (newState == ProcessingStationState.Empty)
        {
            progress = 0;
            ClearIngredients();
        }
    }
}
