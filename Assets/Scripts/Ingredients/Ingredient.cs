using System;
using Unity.VisualScripting;
using UnityEngine;

public class Ingredient : Clickable, IComparable<Ingredient>
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
    }

    public override void OnClick()
    {
        if (gameObject.GetComponent<Rigidbody2D>() != null)
        {
            gameObject.GetComponent<Rigidbody2D>().simulated = true;
            gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        }
        Pointer.Instance.PutInHand(gameObject);

    }
    public int CompareTo(Ingredient other)
    {
        if (other == null) return 1;
        return string.Compare(definition.displayName, other.definition.displayName, StringComparison.Ordinal);
    }
}