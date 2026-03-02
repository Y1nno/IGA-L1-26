using UnityEditor.UI;
using UnityEngine;

public class ToolHolder: Clickable
{
    public ToolHolderDef def;
    public ToolHolderState state = ToolHolderState.Occupied;

    public void ResetTool()
    {
        ChangeState(ToolHolderState.Occupied);
    }

    public void ChangeState(ToolHolderState newState)
    {
        state = newState;
        switch (state)
        {
            case ToolHolderState.Empty:
                    gameObject.GetComponent<SpriteRenderer>().sprite = def.emptySprite;
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
            GetComponent<Rigidbody2D>().simulated = false;
        }
        else if (state == ToolHolderState.Occupied)
        {

            GetComponent<Rigidbody2D>().simulated = true;
            //Debug.Log("ToolHolder clicked while occupied");
            Tool tool = def.tool.InstantiateTool();
            tool.holder = this;
            Pointer.Instance.PutInHand(tool.gameObject);
            ChangeState(ToolHolderState.Empty);

            //Tutorial Only
            //Debug.Log("Boolean check for tutorial knife");
            //Debug.Log("Current level: " + GlobalGameManager.Instance.currentLevelName);
            //Debug.Log("Tool name: " + def.tool.name);
            if (GlobalGameManager.Instance.currentLevelName == "Tutorial" && def.tool.name == "KnifeDef")
            {
                GetComponent<TutorialSignaler>().Signal(LevelSignal.GrabbedKnife);
            }
        }
    }

    public void SeeIfCollisionIsTool(Collider2D other)
    {
        //Debug.Log("Checking collision for tool holder: " + other.name);
        //Debug.Log("Is the tool holder occupied? " + (state == ToolHolderState.Occupied));
        if (state == ToolHolderState.Occupied) {return;}
        //Debug.Log("Is the other a tool? " + (other.gameObject.GetComponent<Tool>() != null));
        if (other.gameObject.GetComponent<Tool>() == null) {return;}
        //Debug.Log("Is the other already held? " + (other.gameObject.transform.parent == Pointer.Instance.transform));
        if (other.gameObject.transform.parent == Pointer.Instance.transform){return;}
        Tool tool = other.gameObject.GetComponent<Tool>();
        if (tool != null)
        {
            ChangeState(ToolHolderState.Occupied);
            Destroy(other.gameObject);
        }
    }
}
