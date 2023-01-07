using UnityEngine;

[CreateAssetMenu(fileName = "FoodObject", menuName = "ludum-52/FoodObject", order = 0)]
public class FoodObject : ScriptableObject {
    public GameObject FoodPrefab;
    public int FoodPrice = 10;
    public float HungerReliefAmt = 35f;
}