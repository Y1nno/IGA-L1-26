using System.Collections.Generic;
using UnityEngine;

public class StationController : MonoBehaviour
{
    public Dictionary<string, GameObject> stations = new Dictionary<string, GameObject>();
    public bool StationsActiveOnStart { get; private set; }
    public void Start()
    {
        foreach (var station in transform.GetComponentsInChildren<IngredientBasketStation>())
        {
            Debug.Log($"Found station: {station.name}");
            stations.Add(station.name, station.gameObject);
        }
        if (!StationsActiveOnStart)
        {
            foreach (var station in stations.Values)
            {
                station.SetActive(false);
            }
        }
    }

    public void Activate(string stationName)
    {
        if (stations.TryGetValue(stationName, out GameObject station))
        {
            station.SetActive(true);
        }
        else
        {
            Debug.LogWarning($"Station '{stationName}' not found in StationController.");
        }
    }
}
