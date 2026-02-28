using UnityEngine;

/// <summary>
/// Attach to any 2D object with a Collider2D to drag it with the mouse.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class Dragable : MonoBehaviour
{
    private Camera _cam;

    private Vector3 _offsetWorld;
    private float _zDistanceToCamera;
    private bool _dragging;

    private void Awake()
    {
        if (_cam == null) _cam = Camera.main;
        if (_cam == null)
            Debug.LogError("Draggable: No camera assigned and no Camera.main found.");
    }

    public void OnMouseDown()
    {
        Debug.Log("Mouse Down");
        if (_cam == null) return;
        Debug.Log("Cam is not null");

        _dragging = true;

        // How far this object is from the camera (needed for ScreenToWorldPoint in perspective).
        _zDistanceToCamera = Mathf.Abs(_cam.transform.position.z - transform.position.z);

        Vector3 mouseWorld = GetMouseWorld();
        _offsetWorld = transform.position - mouseWorld;
    }

    public void OnMouseUp()
    {
        Debug.Log("Mouse Up");
        _dragging = false;
    }

    public void OnMouseDrag()
    {
        Debug.Log("Mouse Drag");
        if (!_dragging || _cam == null) return;

        Vector3 mouseWorld = GetMouseWorld();
        transform.position = mouseWorld + _offsetWorld;
    }

    private Vector3 GetMouseWorld()
    {
        Vector3 mouse = Input.mousePosition;
        mouse.z = _zDistanceToCamera;
        return _cam.ScreenToWorldPoint(mouse);
    }
}