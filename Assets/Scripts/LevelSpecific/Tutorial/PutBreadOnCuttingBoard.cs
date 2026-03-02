using UnityEngine;

public class PutBreadOnCuttingBoard : MonoBehaviour
{
    public void Execute()
    {
        GameObject cuttingBoard = GameObject.Find("CuttingBoard");
        if (cuttingBoard != null)
        {
            Ingredient ingredient = Resources.Load<IngredientDef>("ScriptableObjects/Ingredients/Demon Bread").InitIntoGameObject().GetComponent<Ingredient>();
            cuttingBoard.GetComponent<ToolStation>().AddIngredient(ingredient);
        }
        else
        {
            Debug.LogWarning("CuttingBoard not found in the scene.");
        }
    }
}
