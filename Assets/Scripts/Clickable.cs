using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public abstract class Clickable : MonoBehaviour
{
    public abstract void OnClick();

}
