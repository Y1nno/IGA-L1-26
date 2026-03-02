using UnityEngine;
using UnityEngine.UI;

public class AffectionManager : MonoBehaviour
{
    private static AffectionManager _instance;
    public static AffectionManager Instance => _instance;

    public Slider ApprovalBar;
    [Tooltip("Speed at which Approval decays over time in seconds")]
    public float ApprovalDecaySpeed = 0.05f;
    [Tooltip("Amount by which Approval decreases each time")]
    public float ApprovalDecayAmount = .5f;
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
        if (isDecaying && Time.time - lastDecayTime >= ApprovalDecaySpeed)
        {
            DecreaseApproval(ApprovalDecayAmount);
            lastDecayTime = Time.time;
        }
        if (ApprovalBar.value <= ApprovalBar.minValue)
        {
            OnZeroApproval();
        }
    }

    public void ToggleDecay()
    {
        isDecaying = !isDecaying;
    }

    public void IncreaseApproval(float amount)
    {
        ApprovalBar.value += amount;
        if (ApprovalBar.value > ApprovalBar.maxValue)
        {
            ApprovalBar.value = ApprovalBar.maxValue;
        }
        if (GetComponent<TutorialSignaler>())
        {
            GetComponent<TutorialSignaler>().Signal(LevelSignal.ApprovalChange);
        }
    }

    public void DecreaseApproval(float amount)
    {
        ApprovalBar.value -= amount;
        if (ApprovalBar.value < ApprovalBar.minValue)
        {
            ApprovalBar.value = ApprovalBar.minValue;
        }
        if (GetComponent<TutorialSignaler>())
        {
            GetComponent<TutorialSignaler>().Signal(LevelSignal.ApprovalChange);
        }
    }

    public void ResetApproval()
    {
        ApprovalBar.value = ApprovalBar.maxValue;
    }

    public void OnZeroApproval()
    {
        GlobalGameManager.Instance.LoadLevel("Game Over");
    }
}
