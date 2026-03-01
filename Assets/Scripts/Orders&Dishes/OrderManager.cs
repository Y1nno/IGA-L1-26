using UnityEngine;
using System.Collections.Generic;

public class OrderManager : MonoBehaviour
{
    private static OrderManager _instance;
    public static OrderManager Instance => _instance;
    public GameObject orderPrefab;
    public GameObject orderPanel;

    public float orderApprovalReward = 5.0f;

    public List<GameObject> orders = new List<GameObject>();

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

    public void CreateOrder()
    {
        GameObject newOrder = Instantiate(orderPrefab, orderPanel.transform);
        orders.Add(newOrder);
    }
    public void ResolveOrderAtIndex(int index)
    {
        if (index >= 0 && index < orders.Count)
        {
            GameObject orderToResolve = orders[index];
            RemoveOrder(orderToResolve);
            ApprovalMananger.Instance.IncreaseApproval(orderApprovalReward);
        }
    }

    public void ResolveOrder(GameObject order)
    {
        if (order != null && orders.Contains(order))
        {
            RemoveOrder(order);
            ApprovalMananger.Instance.IncreaseApproval(orderApprovalReward);
        }
    }

    public void RemoveOrder(GameObject order)
    {
        if (orders.Contains(order))
        {
            orders.Remove(order);
            Destroy(order);
        }
    }

    public GameObject GetOrder(int index)
    {
        if (index >= 0 && index < orders.Count)
        {
            return orders[index];
        }
        return null;
    }

    public void RemoveFirstOrder()
    {
        if (orders.Count > 0)
        {
            GameObject firstOrder = orders[0];
            orders.RemoveAt(0);
            Destroy(firstOrder);
        }
    }
}