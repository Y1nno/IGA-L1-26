using Unity.VisualScripting;
using UnityEngine;

public class Ingredient : Clickable
{
    public IngredientDef definition;

    public void Setup(IngredientDef def)
    {
        definition = def;
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null && def.prefab != null)
        {
            spriteRenderer.sprite = def.prefab.GetComponent<SpriteRenderer>().sprite;
        }
        spriteRenderer.sprite = def.sprite;
    }

    public override void OnClick()
    {
        Pointer.Instance.PutInHand(gameObject);
    }
}
