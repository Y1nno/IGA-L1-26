using UnityEngine;
using System.Collections.Generic;

public class ToolStation : Clickable
{
    public ToolStationDef definition;
    private List<IngredientSpot> ingredientSpots = new List<IngredientSpot>();

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
        GameObject ingredientObject = GetIngredientOnClick()?.gameObject;
        if (ingredientObject != null)
        {
            Pointer.Instance.PutInHand(ingredientObject);
        }
    }


    private void AddIngredient(Ingredient ingredient, IngredientSpot spot)
    {
        spot.SetIngredient(ingredient);
        //ingredientSpots.Remove(spot);
        //ingredientSpots.Add(spot);
        ingredient.transform.position = spot.transform.position;
        ingredient.GetComponent<Rigidbody2D>().simulated = false;
        ingredient.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;

        Debug.Log("Ingredient added: " + ingredient.name);
    }

    private Ingredient GetIngredientOnClick()
    {
        for (int i = 0; i < ingredientSpots.Count; i++)
        {
            var spot = ingredientSpots[i];
            if (spot.ingredient == null) continue;

            var result = spot.ingredient;
            spot.SetIngredient(null);

            var rb = result.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.simulated = true;
                rb.bodyType = RigidbodyType2D.Dynamic;
            }

            return result;
        }

        return null;
    }

    public void ClearIngredients()
    {
        foreach (var spot in ingredientSpots)
        {
            if (spot.ingredient == null) continue;

            var ing = spot.ingredient;
            spot.SetIngredient(null);

            ing.transform.SetParent(null);

            var rb = ing.GetComponent<Rigidbody2D>();
            if (rb != null) rb.simulated = false;

            var col = ing.GetComponent<Collider2D>();
            if (col != null) col.enabled = false;

            ing.gameObject.SetActive(false);
            Destroy(ing.gameObject);
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
        if (other.GetComponent<Ingredient>() != null && !other.GetComponent<Ingredient>().definition.plated)
        {
            Ingredient ingredient = other.GetComponent<Ingredient>();
            for (int i = 0; i < ingredientSpots.Count; i++)
            {
                if (ingredientSpots[i].ingredient == null)
                {
                    AddIngredient(ingredient, ingredientSpots[i]);
                    return;
                }
            }
        }
        else if (other.GetComponent<Pointer>() != null && other.GetComponent<Pointer>().GetComponentInChildren<Tool>() != null)
        {
            Debug.Log("Tool entered: " + other.name);
            Tool tool = other.GetComponent<Pointer>().GetComponentInChildren<Tool>();
            Debug.Log("Ingredients: " + GetIngredients());
            Ingredient output = definition.GetOutputForIngredients(GetIngredients(), tool.definition);
            Debug.Log("Output: " + output);
            if (output != null)
            {
                ClearIngredients();
                AddIngredient(output, ingredientSpots[0]);
            }
        }
    }
}

public class IngredientSpot
{
    public Transform transform;
    public Ingredient ingredient;

    public Ingredient RetrieveIngredient()
    {
        Ingredient temp = ingredient;
        ingredient = null;
        return temp;
    }

    public void SetIngredient(Ingredient newIngredient)
    {
        ingredient = newIngredient;
    }
}
