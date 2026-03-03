using UnityEngine;
using UnityEngine.UIElements;

public class PutBreadOnCuttingBoard : MonoBehaviour
{

    public void Execute()
    {
        GameObject cuttingBoard = GameObject.Find("CuttingBoard");
        if (cuttingBoard != null)
        {
            Ingredient ingredient = Resources.Load<IngredientDef>("ScriptableObjects/Ingredients/Demon Bread").InitIntoGameObject().GetComponent<Ingredient>();
            cuttingBoard.GetComponent<ToolStation>().AddIngredient(ingredient);
            GameObject platingStation = GameObject.Find("Plating Station");
            if (platingStation != null)
            {
                platingStation.GetComponent<SpriteRenderer>().enabled = true;
                platingStation.GetComponent<BoxCollider2D>().enabled = true;
            }
        }
        else
        {
            Debug.LogWarning("CuttingBoard not found in the scene.");
        }
    }
}
