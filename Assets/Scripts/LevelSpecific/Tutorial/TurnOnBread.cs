using UnityEngine;

public class TurnOnBread : MonoBehaviour
{
    public void Execute()
    {
        GameObject.Find("StationController").GetComponent<StationController>().Activate("DemonBreadStation");
    }
}
