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

    public Material NormalMaterial;
    public Material HungryMaterial;

    private void Start() {
        CurrentHunger = MaxHunger;
        UpdateHungerGraphics();
    }

    private void Update() {
        CurrentHunger -= HungerLossRate * Time.deltaTime;

        if(CurrentHunger <= 0)
        {
            Die();
        }
        else if((CurrentHunger / MaxHunger < SearchForFoodThreshold) && (SearchingForFood == false))
        {
            SearchingForFood = true;
            UpdateHungerGraphics();
            SendMessage("StartSearchForFood");
        }else {
            UpdateHungerGraphics();
        }
    }

    private void UpdateHungerGraphics()
    {
        foreach (Renderer r in transform.GetComponentsInChildren<Renderer>())
        {
            if(SearchingForFood)
                r.material = HungryMaterial;
            else
                r.material = NormalMaterial;
        }
    }

    public void EatFood(float foodAmt) {
        CurrentHunger += foodAmt;
        // NOTE: Disabling this for now, because having it off makes food better. Upgraded food can overfill a person, making it so that there's a meaningfull reason to upgrade in the first place.
        //CurrentHunger = Mathf.Clamp(CurrentHunger, 0, MaxHunger);

        if(CurrentHunger / MaxHunger > SearchForFoodThreshold)
        {
            SearchingForFood = false;
            SendMessage("StopSearchingForFood");
        }
    }

    private void Die()
    {
        GameManager._Instance.RemoveHuman(this.gameObject);
        AudioManager._Instance.CreateSoundAtPoint(AudioManager._Instance.DeathSting, transform.position, 0.03f);
    }
}
