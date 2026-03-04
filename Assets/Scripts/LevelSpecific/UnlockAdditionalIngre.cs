using UnityEngine;

public class UnlockAdditionalIngre : MonoBehaviour
{
    public void Execute()
    {
        GameObject.Find("StationController").GetComponent<StationController>().Activate("BloodBerryTomatoStation");
        GameObject.Find("StationController").GetComponent<StationController>().Activate("DevilsLettuceStation");
        OrderManager.Instance.pendingOrders = 10000;
        OrderManager.Instance.possibleOrders.Add(OrderManager.Instance.BLT);
        OrderManager.Instance.AddPrompt(OrderManager.Instance.explanationBLT);
    }
}
