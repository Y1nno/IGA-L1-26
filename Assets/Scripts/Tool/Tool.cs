using UnityEngine;

public class Tool : Clickable
{
    public ToolDef definition;
    [HideInInspector]
    public ToolHolder holder;

    public override void OnClick()
    {
        Pointer.Instance.PutInHand(this.gameObject);
    }
}
