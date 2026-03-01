using UnityEngine;
using UnityEngine.EventSystems;

public class Ticket : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // Assign or resolve your ticket/order data however you do it
    public string ticketId;

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
}