using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class PlatingStation : Clickable
{
    private List<IngredientSpot> ingredientSpots = new List<IngredientSpot>();
    public PlatingStationDef definition;
    public bool readyToServe = false;
    public Sprite defaultSprite;
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

        defaultSprite = GetComponent<SpriteRenderer>().sprite;
    }

    public override void OnClick()
    {
        if (readyToServe)
        {
            var served = ingredientSpots[0].ingredient;
            if (served != null)
            {
                GetComponent<SpriteRenderer>().sprite = defaultSprite;
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
            if (result.GetComponent<Ingredient>().definition.plated)
            {
                Color tempColor = result.GetComponent<SpriteRenderer>().color;
                result.GetComponent<SpriteRenderer>().color = new Color(tempColor.r, tempColor.g, tempColor.b, 255);
            }
        }
        return result;
    }

    public void ClearIngredients()
    {
        foreach (var spot in ingredientSpots)
        {
            //Debug.Log("Clearing spot: " + spot.transform.name);
            if (spot.ingredient != null)
            {
                //Debug.Log("Clearing ingredient: " + spot.ingredient.name);
                var ing = spot.ingredient;
                spot.SetIngredient(null);
                Destroy(ing.gameObject);
            }
        }
        readyToServe = false;
        GetComponent<SpriteRenderer>().sprite = defaultSprite;
    }

    public List<Ingredient> GetIngredients()
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

    public IngredientSpot GetFirstEmptySpot()
    {
        foreach (var spot in ingredientSpots)
        {
            if (spot.ingredient == null)
            {
                return spot;
            }
        }
        return null;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("Trigger entered by: " + other.name);
        //Debug.Log("Is trigger an ingredient: " + (other.GetComponent<Ingredient>() != null));
        if (other.GetComponent<Ingredient>() != null)
        {
            Ingredient ingredient = other.GetComponent<Ingredient>();
            if (ingredient.definition.plated)
            {
                AddIngredient(ingredient, ingredientSpots[ingredientSpots.Count - 1]);
            }
            for (int i = 0; i < ingredientSpots.Count; i++)
            {
                //Debug.Log("Checking spot " + i + "Bool: " + (ingredientSpots[i].ingredient == null));
                //Debug.Log("ReadyToServe: " + readyToServe);
                //Debug.Log("Can add ingredient: " + definition.CanAddIngredient(GetIngredients(), ingredient));
                if (ingredientSpots[i].ingredient == null && !readyToServe && definition.CanAddIngredient(GetIngredients(), ingredient))
                {
                    AddIngredient(ingredient, ingredientSpots[i]);
                    if (GetComponent<TutorialSignaler>())
                    {
                        DetermineSignal();
                    }
                    Ingredient output = definition.GetOutputForIngredients(GetIngredients());
                    //Debug.Log("Output ingredient: " + output);
                    if (output != null)
                    {
                        //Debug.Log("Output found: " + output.name);
                        ClearIngredients();
                        AddIngredient(output, ingredientSpots[i]);
                        readyToServe = true;
                        SpriteRenderer sr = GetComponent<SpriteRenderer>();
                        if (sr != null)
                        {
                            Color tempColor = output.GetComponent<SpriteRenderer>().color;
                            output.GetComponent<SpriteRenderer>().color = new Color(tempColor.r, tempColor.g, tempColor.b, 0);
                            sr.sprite = output.definition.heldSprite;
                        }
                    }
                    return;
                }
            }
        }
    }

    public void DetermineSignal()
    {
        if (GetIngredients().Count == 2)
        {
            GetComponent<TutorialSignaler>().Signal(LevelSignal.AddedBreadToPlate);
        }
    }
}
