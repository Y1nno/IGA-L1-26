using JetBrains.Annotations;
using UnityEngine;


[CreateAssetMenu(fileName = "IngredientBasketStationDef", menuName = "Scriptable Objects/IngredientBasketStation")]
public class IngredientBasketStationDef : Station
{
    [Header("Ingredient Basket Settings")]
    public IngredientDef ingredient;
    public BasketType basketType;
    public float itemScale = 1.0f;

    public override void Interact()
    {
        Debug.Log($"Interacting with ingredient basket: {ingredient.name}, creating object");
        GameObject item = ingredient.InitIntoGameObject();
        item.transform.localScale *= itemScale;
        Pointer.Instance.PutInHand(item);
    }
}
