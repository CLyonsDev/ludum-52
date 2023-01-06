using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FoodManager : MonoBehaviour {
    public GameObject[] FoodPrefabs;
    public List<Transform> PlacedFood = new List<Transform>();

    public static FoodManager _Instance;

    private void Awake() {
        if(_Instance == null)
            _Instance = this;
        else
            Destroy(this);
    }

    private void Start() {
        
    }

    private void Update() {


        
    }

    public void CreateFood(int prefabIndex, Vector3 position)
    {
        GameObject newFood = Instantiate(FoodPrefabs[prefabIndex], position, Quaternion.identity);
        PlacedFood.Add(newFood.transform);
    }

    public void RemoveFood(GameObject foodToRemove)
    {
        PlacedFood.Remove(foodToRemove.transform);
        Destroy(foodToRemove);
    }
}