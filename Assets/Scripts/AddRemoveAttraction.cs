using UnityEngine;

public class AddRemoveAttraction : MonoBehaviour
{
    public void Execute(float amount)
    {
        if (amount > 0)
        {
            AffectionManager.Instance.IncreaseApproval(amount);
        }
        else if (amount < 0)
        {
            AffectionManager.Instance.DecreaseApproval(-amount);
        }
    }
}
