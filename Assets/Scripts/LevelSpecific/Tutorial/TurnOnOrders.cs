using UnityEngine;

public class TurnOnOrders : MonoBehaviour
{

    public void Execute()
    {
        OrderManager.Instance.StartTakingOrders();
        GameObject.Find("StationController").GetComponent<StationController>().Activate("HumanMeatStation");
    }
}
