using UnityEngine;

public class ToolHolderPutterBack : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D collision)
    {
        transform.parent.GetComponent<ToolHolder>().SeeIfCollisionIsTool(collision);
    }
}
