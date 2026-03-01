using Unity.VisualScripting;
using UnityEngine;

public class IngredientBasketStation: Clickable
{
    public IngredientBasketStationDef definition;

    public void Start()
    {
    }

    public override void OnClick()
    {
        //Debug.Log("IngredientBasketStation clicked");
        //Debug.Log($"Definition: {definition}");
        if (definition != null)
        {
            definition.Interact();
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        GameObject item = other.gameObject;
        if (item.gameObject.GetComponent<Pointer>() != null) {return;}
        //Debug.Log($"Bool 1: {item.GetComponent<Ingredient>().definition == definition.ingredient}");
        //Debug.Log($"Bool 2: {item.transform.parent != Pointer.Instance.transform}");
        if (item.GetComponent<Ingredient>().definition == definition.ingredient && item.transform.parent != Pointer.Instance.transform)
        {
            Destroy(item);
        }
    }
}
