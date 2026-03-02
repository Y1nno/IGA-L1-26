using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class Ticket : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public IngredientDef order;
    public Image ticketImage;

    public void Start()
    {
    }

    public void SubmitDish(Ingredient dish)
    {
        if (dish == null || !dish.definition.plated)
        {
            Debug.LogWarning("No dish provided for submission.");
            return;
        }

        OrderManager.Instance.ResolveOrder(this.gameObject);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Pointer.Instance.SetCurrentTicket(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (Pointer.Instance.GetCurrentTicket() == this)
        {
            Pointer.Instance.SetCurrentTicket(null);
        }
    }

    public void SetCustomer(CustomerSO customer)
    {
        if (order.sprite != null)
        {
            ticketImage.sprite = order.sprite;
            foreach (Image child in GetComponentsInChildren<Image>())
            {
                if (child.name == "CustomerImage")
                {
                    child.sprite = ticketImage.sprite;
                }
            }
        }
    }

    public void SetOrder(IngredientDef order)
    {
        this.order = order;
        GetComponentInChildren<TMP_Text>().text = order.displayName;
    }
}