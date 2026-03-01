using UnityEngine;
using UnityEngine.UI;

public class ApprovalMananger : MonoBehaviour
{
    private static ApprovalMananger _instance;
    public static ApprovalMananger Instance => _instance;
    public Slider approvalBar;
    [Tooltip("Speed at which approval decays over time in seconds")]
    public float approvalDecaySpeed = 0.05f;
    [Tooltip("Amount by which approval decreases each time")]
    public float approvalDecayAmount = .5f;
    public float lastDecayTime = 0f;
    public bool isDecaying = false;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    public void StartDecay()
    {
        isDecaying = true;
    }

    public void Update()
    {
        if (isDecaying && Time.time - lastDecayTime >= approvalDecaySpeed)
        {
            DecreaseApproval(approvalDecayAmount);
            lastDecayTime = Time.time;
        }
    }

    public void ToggleDecay()
    {
        isDecaying = !isDecaying;
    }

    public void IncreaseApproval(float amount)
    {
        approvalBar.value += amount;
        if (approvalBar.value > approvalBar.maxValue)
        {
            approvalBar.value = approvalBar.maxValue;
        }
    }

    public void DecreaseApproval(float amount)
    {
        approvalBar.value -= amount;
        if (approvalBar.value < approvalBar.minValue)
        {
            approvalBar.value = approvalBar.minValue;
        }
    }
}
