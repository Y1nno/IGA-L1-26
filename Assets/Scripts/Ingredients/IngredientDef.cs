using UnityEngine;

[CreateAssetMenu(fileName = "IngredientDef", menuName = "Scriptable Objects/IngredientDef")]
public class IngredientDef : ScriptableObject
{
    public string displayName;
    public bool platable;
    public IngredientType ingredientType;
    public GameObject prefab;
    public Sprite sprite;

    public GameObject InitIntoGameObject()
    {
        GameObject obj = null;
        if (prefab != null)
        {
            obj = Instantiate(prefab);
        }
        obj.GetComponent<Ingredient>().Setup(this);
        Debug.Log($"Initialized ingredient into GameObject: {obj.name}");
        return obj;
    }
}
