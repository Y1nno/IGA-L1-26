using UnityEngine;
using UnityEngine.UI;

public class AffectionManager : MonoBehaviour
{
    private static AffectionManager _instance;
    public static AffectionManager Instance => _instance;

    public Slider AffectionBar;
    [Tooltip("Speed at which Affection decays over time in seconds")]
    public float AffectionDecaySpeed = 0.05f;
    [Tooltip("Amount by which Affection decreases each time")]
    public float AffectionDecayAmount = .5f;
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
        if (isDecaying && Time.time - lastDecayTime >= AffectionDecaySpeed)
        {
            DecreaseAffection(AffectionDecayAmount);
            lastDecayTime = Time.time;
        }
        if (AffectionBar.value <= AffectionBar.minValue)
        {
            OnZeroAffection();
        }
    }

    public void ToggleDecay()
    {
        isDecaying = !isDecaying;
    }

    public void IncreaseAffection(float amount)
    {
        AffectionBar.value += amount;
        if (AffectionBar.value > AffectionBar.maxValue)
        {
            AffectionBar.value = AffectionBar.maxValue;
        }
        if (GetComponent<TutorialSignaler>())
        {
            GetComponent<TutorialSignaler>().Signal(LevelSignal.AffectionChange);
        }
    }

    public void DecreaseAffection(float amount)
    {
        AffectionBar.value -= amount;
        if (AffectionBar.value < AffectionBar.minValue)
        {
            AffectionBar.value = AffectionBar.minValue;
        }
        if (GetComponent<TutorialSignaler>())
        {
            GetComponent<TutorialSignaler>().Signal(LevelSignal.AffectionChange);
        }
    }

    public void ResetAffection()
    {
        AffectionBar.value = AffectionBar.maxValue;
    }

    public void OnZeroAffection()
    {
        GlobalGameManager.Instance.LoadLevel("Game Over");
    }
}
