using UnityEngine;

[CreateAssetMenu(fileName = "New Customer", menuName = "Scriptable Objects/Customer")]
public class CustomerSO : ScriptableObject
{
    public string customerName;
    public Sprite image;
}
