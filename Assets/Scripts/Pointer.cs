using UnityEngine;
using UnityEngine.InputSystem;

public class Pointer : MonoBehaviour
{
    private bool _holding = false;
    private Vector2 _offsetWorld;
    private Camera _cam;
    private float _zDistanceToCamera;

    private void Awake()
    {
        _cam = Camera.main;
        _zDistanceToCamera = Mathf.Abs(_cam.transform.position.z - transform.position.z);
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

    public void OnClick(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _holding = true;
        }
        if (context.canceled)
        {
            Debug.Log("Click released");
        }
    }

}
