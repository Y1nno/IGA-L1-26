using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Pointer : MonoBehaviour
{
    private bool _holding = false;

    private static Pointer _instance;
    public static Pointer Instance => _instance;
    public float throwForce = 0.2f;
    private readonly List<Collider2D> _overlapping = new List<Collider2D>();

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

    public void Update()
    {
        Vector2 screenPosition = Mouse.current.position.ReadValue();
        Vector3 mousePosition = new Vector3(screenPosition.x, screenPosition.y, 0f);

        float distanceFromCamera = 10f;
        mousePosition.z = distanceFromCamera;

        Vector3 worldPositionFixedDepth = Camera.main.ScreenToWorldPoint(mousePosition);
        transform.position = worldPositionFixedDepth;
    }

    #region Input System
    public void OnClick(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _holding = true;
            GameObject closest = GetClosestOverlappedObject();
            //Debug.Log($"Closest under pointer: {closest.name}");
            //Debug.Log($"All overlapping objects: {_overlapping.Count}");
            if (closest != null)
            {
                closest.GetComponent<Clickable>().OnClick();
            }
        }

        if (context.canceled)
        {
            _holding = false;
            if (transform.childCount > 0)
            {
                DropItem();
            }
        }
    }

    public bool IsHolding() => _holding;

    public void PutInHand(GameObject item)
    {
        if (transform.childCount > 0)
        {
            Debug.Log("Already holding an item, cannot pick up another, Child count: " + transform.childCount);
            return;
        }
        if (item.TryGetComponent<Rigidbody2D>(out var rb))
        {
            rb.simulated = false;
            rb.linearVelocity = Vector2.zero; // Stop any existing movement
        }
        item.transform.SetParent(transform);
        item.transform.localPosition = Vector3.zero;
    }

    public void DropItem()
    {
        if (transform.childCount > 0)
        {
            Transform item = transform.GetChild(0);
            item.SetParent(null);
            if (item.TryGetComponent<Rigidbody2D>(out var rb))
            {
                rb.simulated = true;
                rb.linearVelocity = Mouse.current.delta.ReadValue() * throwForce;
            }
        }
    }
    #endregion

    #region Trigger tracking
    private void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log($"Pointer entered trigger: {other.name}");
        if (!_overlapping.Contains(other))
            _overlapping.Add(other);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        //Debug.Log($"Pointer exited trigger: {other.name}");
        _overlapping.Remove(other);
    }

    public GameObject GetClosestOverlappedObject()
    {
        CleanupOverlapList();

        if (_overlapping.Count == 0)
            return null;

        Vector2 pointerPos = transform.position;

        Collider2D best = null;
        float bestSqrDist = float.PositiveInfinity;

        for (int i = 0; i < _overlapping.Count; i++)
        {
            Collider2D col = _overlapping[i];
            if (col == null || !col.enabled || !col.gameObject.activeInHierarchy) continue;

            Vector2 closestPoint = col.ClosestPoint(pointerPos);
            float sqrDist = (closestPoint - pointerPos).sqrMagnitude;

            if (sqrDist < bestSqrDist)
            {
                bestSqrDist = sqrDist;
                best = col;
            }
        }

        return best != null ? best.gameObject : null;
    }

    private void CleanupOverlapList()
    {
        for (int i = _overlapping.Count - 1; i >= 0; i--)
        {
            Collider2D col = _overlapping[i];
            if (col == null || !col.enabled || !col.gameObject.activeInHierarchy)
                _overlapping.RemoveAt(i);
        }
    }
    #endregion
}