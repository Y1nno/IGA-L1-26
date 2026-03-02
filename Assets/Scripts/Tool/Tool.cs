using UnityEngine;

public class Tool : Clickable
{
    public ToolDef definition;
    [HideInInspector]
    public ToolHolder holder;

    public override void OnClick()
    {
        Pointer.Instance.PutInHand(this.gameObject);
        SetRotation(definition.AngleWhenPickedUp);
    }

    public void SetRotation(float angle)
    {
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }
}
