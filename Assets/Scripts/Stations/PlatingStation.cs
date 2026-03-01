using UnityEngine;
using System.Collections.Generic;

public class PlatingStation : Clickable
{
    private List<IngredientSpot> ingredientSpots = new List<IngredientSpot>();
    public PlatingStationDef definition;
    public bool readyToServe = false;
    public void Awake()
    {
        foreach (Transform childTransform in transform)
        {
            IngredientSpot spot = new IngredientSpot
            {
                transform = childTransform,
                ingredient = null
            };
            ingredientSpots.Add(spot);
        }
    }

    public override void OnClick()
    {
        if (readyToServe)
        {
            var served = ingredientSpots[0].ingredient;
            if (served != null)
            {
                ingredientSpots[0].SetIngredient(null);
                served.gameObject.GetComponent<Rigidbody2D>().simulated = true;
                served.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                Pointer.Instance.PutInHand(served.gameObject);
            }

            readyToServe = false;
            return;
        }

        var ingredient = GetIngredientOnClick();
        if (ingredient != null)
        {
            Pointer.Instance.PutInHand(ingredient.gameObject);
        }
    }
    private void AddIngredient(Ingredient ingredient, IngredientSpot spot)
    {
        spot.SetIngredient(ingredient);
        //ingredientSpots.Remove(spot);
        //ingredientSpots.Add(spot);
        ingredient.transform.position = spot.transform.position;
        ingredient.gameObject.GetComponent<Rigidbody2D>().simulated = false;
        ingredient.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;

        Debug.Log("Ingredient added: " + ingredient.name);
    }

    private Ingredient GetIngredientOnClick()
    {
        Ingredient result = null;
        foreach (var spot in ingredientSpots)
        {
            if (spot.ingredient != null)
            {
                result = spot.RetrieveIngredient();
                break;
            }
        }
        if (result != null)
        {
            result.gameObject.GetComponent<Rigidbody2D>().simulated = true;
            result.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        }
        return result;
    }

    public void ClearIngredients()
    {
        foreach (var spot in ingredientSpots)
        {
            Debug.Log("Clearing spot: " + spot.transform.name);
            if (spot.ingredient != null)
            {
                Debug.Log("Clearing ingredient: " + spot.ingredient.name);
                var ing = spot.ingredient;
                spot.SetIngredient(null);
                Destroy(ing.gameObject);
            }
        }
    }

    private List<Ingredient> GetIngredients()
    {
        List<Ingredient> ingredients = new List<Ingredient>();
        foreach (var spot in ingredientSpots)
        {
            if (spot.ingredient != null)
            {
                ingredients.Add(spot.ingredient);
            }
        }
        return ingredients;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Trigger entered by: " + other.name);
        //Debug.Log("Is trigger an ingredient: " + (other.GetComponent<Ingredient>() != null));
        if (other.GetComponent<Ingredient>() != null)
        {
            Ingredient ingredient = other.GetComponent<Ingredient>();
            for (int i = 0; i < ingredientSpots.Count; i++)
            {
                Debug.Log("Checking spot " + i + "Bool: " + (ingredientSpots[i].ingredient == null));
                Debug.Log("ReadyToServe: " + readyToServe);
                Debug.Log("Can add ingredient: " + definition.CanAddIngredient(GetIngredients(), ingredient));
                if (ingredientSpots[i].ingredient == null && !readyToServe && definition.CanAddIngredient(GetIngredients(), ingredient))
                {
                    AddIngredient(ingredient, ingredientSpots[i]);
                    Ingredient output = definition.GetOutputForIngredients(GetIngredients());
                    Debug.Log("Output ingredient: " + output);
                    if (output != null)
                    {
                        Debug.Log("Output found: " + output.name);
                        ClearIngredients();
                        AddIngredient(output, ingredientSpots[i]);
                        readyToServe = true;
                    }
                    return;
                }
            }
        }
    }
}
