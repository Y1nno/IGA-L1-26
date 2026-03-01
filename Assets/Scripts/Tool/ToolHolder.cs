using UnityEngine;

public class ToolHolder: Clickable
{
    public ToolHolderDef def;
    public ToolHolderState state = ToolHolderState.Occupied;

    public void ResetTool()
    {
        state = ToolHolderState.Occupied;
    }

    public void ChangeState(ToolHolderState newState)
    {
        state = newState;
        switch (state)
        {
            case ToolHolderState.Empty:
                if (def.emptySprite != null)
                {
                    gameObject.GetComponent<SpriteRenderer>().sprite = def.emptySprite;
                }
                break;
            case ToolHolderState.Occupied:
                if (def.occupiedSprite != null)
                {
                    gameObject.GetComponent<SpriteRenderer>().sprite = def.occupiedSprite;
                }
                break;
        }
    }

    public override void OnClick()
    {
        if (state == ToolHolderState.Empty)
        {

        }
        else if (state == ToolHolderState.Occupied)
        {
            //Debug.Log("ToolHolder clicked while occupied");
            Tool tool = def.tool.InstantiateTool();
            tool.holder = this;
            Pointer.Instance.PutInHand(tool.gameObject);
            ChangeState(ToolHolderState.Empty);
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (state == ToolHolderState.Occupied) {return;}
        if (other.gameObject.GetComponent<Tool>() == null) {return;}
        if (other.gameObject.transform.parent == Pointer.Instance.transform){return;}
        Tool tool = other.gameObject.GetComponent<Tool>();
        if (tool != null)
        {
            ChangeState(ToolHolderState.Occupied);
            other.gameObject.SetActive(false);
        }
    }
}
