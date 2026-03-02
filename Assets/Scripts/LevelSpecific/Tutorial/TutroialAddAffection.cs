using UnityEngine;

public class TutroialAddAffection : MonoBehaviour
{
    public void Execute(float amount)
    {
        if (amount > 0)
        {
            AffectionManager.Instance.IncreaseAffection(amount);
        }
        else
        {
            AffectionManager.Instance.DecreaseAffection(-amount);
        }
    }
}
