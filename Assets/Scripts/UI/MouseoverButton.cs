using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MouseoverButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Sprite normalSprite;
    public Sprite highlightedSprite;

    public void Awake()
    {
        normalSprite = GetComponent<Image>().sprite;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        GetComponent<Image>().sprite = highlightedSprite;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GetComponent<Image>().sprite = normalSprite;
    }
}
