using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChefPrompt : MonoBehaviour
{
    public ChefPrompts prompt;
    public Button option1;
    public Button option2;
    public TMP_Text promptText;

    public void Start()
    {
        if (option1 == null || option2 == null || promptText == null)
        {
            List<Button> buttons = new List<Button>(GetComponentsInChildren<Button>());
            foreach (Button btn in buttons)
            {
                if (btn.name == "option 1")
                {
                    option1 = btn;
                }
                else if (btn.name == "option 2")
                {
                    option2 = btn;
                }
            }
            promptText = GetComponentInChildren<TMP_Text>();
        }
    }
    public void SetPrompt(ChefPrompts prompt)
    {
        this.prompt = prompt;
        promptText.text = prompt.promptText;
        option1.GetComponentInChildren<TMP_Text>().text = prompt.button1Text;
        if (prompt.button2Text != null)
        {
            option2.gameObject.SetActive(true);
            option2.GetComponentInChildren<TMP_Text>().text = prompt.button2Text;
            RandomizeOrder();
        }
        else
        {
            option2.gameObject.SetActive(false);
        }
    }

    public void ResolvePrompt(Button button)
    {
        if (button.GetComponentInChildren<TMP_Text>().text == prompt.button1Text)
        {
            prompt.button1Results.Invoke();
            OrderManager.Instance.ResolvePrompt(this.gameObject);
        }
        else if (button.GetComponentInChildren<TMP_Text>().text == prompt.button2Text)
        {
            prompt.button2Results.Invoke();
            OrderManager.Instance.ResolvePrompt(this.gameObject);
        }
    }

    private void RandomizeOrder()
    {
        if (Random.value > 0.5f)
        {
            // Swap the buttons
            Button temp = option1;
            option1 = option2;
            option2 = temp;
        }
    }
}
