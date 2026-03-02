using UnityEngine;
using UnityEngine.UI;

public class PatienceManager : MonoBehaviour
{
    private static PatienceManager _instance;
    public static PatienceManager Instance => _instance;
    public Slider patienceBar;
    [Tooltip("Speed at which patience decays over time in seconds")]
    public float patienceDecaySpeed = 0.05f;
    [Tooltip("Amount by which patience decreases each time")]
    public float patienceDecayAmount = .5f;
    public float lastDecayTime = 0f;
    public bool isDecaying = false;

    [Header("Color")]
    public Color fullPatienceColor = Color.green;
    public Color noPatienceColor = Color.red;

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
        if (isDecaying && Time.time - lastDecayTime >= patienceDecaySpeed)
        {
            DecreasePatience(patienceDecayAmount);
            lastDecayTime = Time.time;
            UpdatePatienceBarColor();
        }
        if (patienceBar.value <= patienceBar.minValue)
        {
            OnZeroPatience();
        }
    }

    public void ToggleDecay()
    {
        isDecaying = !isDecaying;
    }

    public void IncreasePatience(float amount)
    {
        patienceBar.value += amount;
        if (patienceBar.value > patienceBar.maxValue)
        {
            patienceBar.value = patienceBar.maxValue;
        }
    }

    public void DecreasePatience(float amount)
    {
        patienceBar.value -= amount;
        if (patienceBar.value < patienceBar.minValue)
        {
            patienceBar.value = patienceBar.minValue;
        }
    }
    private void UpdatePatienceBarColor()
    {
        float t = (patienceBar.value - patienceBar.minValue) / (patienceBar.maxValue - patienceBar.minValue);
        patienceBar.fillRect.GetComponent<Image>().color = Color.Lerp(noPatienceColor, fullPatienceColor, t);
    }

    public void ResetPatience()
    {
        patienceBar.value = patienceBar.maxValue;
        UpdatePatienceBarColor();
    }

    public void OnZeroPatience()
    {
        GlobalGameManager.Instance.LoadLevel("Game Over");
    }
}
