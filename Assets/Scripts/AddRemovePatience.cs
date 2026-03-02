using UnityEngine;

public class AddRemovePatience : MonoBehaviour
{
    public void Execute(float amount)
    {
        if (amount > 0)
        {
            PatienceManager.Instance.IncreasePatience(amount);
        }
        else if (amount < 0)
        {
            PatienceManager.Instance.DecreasePatience(-amount);
        }
    }
}
