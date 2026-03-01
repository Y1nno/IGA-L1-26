using UnityEngine;

public class ItemDeleter : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D other)
    {
        GameObject item = other.gameObject;
        if (item.GetComponent<Pointer>() != null) {return;}
        if (item.GetComponent<Ingredient>() != null) {Destroy(item);}
        //TODO: Add more conditions for other item types that should not be destroyed
        if (item.GetComponent<Tool>() != null)
        {
            item.GetComponent<Tool>().holder.ResetTool();
            Destroy(item);
        }
    }
}
