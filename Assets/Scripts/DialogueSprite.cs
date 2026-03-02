using UnityEngine;

public class DialogueSprite : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public static DialogueSprite Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void SetSprite(Sprite newSprite)
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = newSprite;
        }
    }

    public void ClearSprite()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = null;
        }
    }

    public void SetScale(Vector2 newScale)
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.transform.localScale = newScale;
        }
    }

    public void SetPosition(Vector2 newPosition)
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.transform.position = newPosition;
        }
    }
}
