using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hunger : MonoBehaviour
{
    public float CurrentHunger;
    public float MaxHunger;
    public float SearchForFoodThreshold = 0.85f; // Percentage
    public bool SearchingForFood = false;

    public float HungerLossRate;

    private void Start() {
        CurrentHunger = MaxHunger;
    }

    private void Update() {
        CurrentHunger -= HungerLossRate * Time.deltaTime;

        if((CurrentHunger / MaxHunger < SearchForFoodThreshold) && (SearchingForFood == false))
        {
            SearchingForFood = true;
            SendMessage("StartSearchForFood");
        }
    }

    public void EatFood(float foodAmt) {
        CurrentHunger += foodAmt;
        CurrentHunger = Mathf.Clamp(CurrentHunger, 0, MaxHunger);

        if(CurrentHunger / MaxHunger > SearchForFoodThreshold)
        {
            SearchingForFood = false;
            SendMessage("StopSearchingForFood");
        }
    }
}
