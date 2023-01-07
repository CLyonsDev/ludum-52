using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FoodManager : MonoBehaviour
{
    private AudioManager audioManager;
    public FoodObject[] FoodPrefabs;
    public FoodObject CurrentFood;
    [SerializeField] private int FoodIndex = 0;
    public List<Transform> PlacedFood = new List<Transform>();

    public static FoodManager _Instance;

    public GameEvent UpdateMoneyUiGameEvent;

    private void Awake()
    {
        if (_Instance == null)
            _Instance = this;
        else
            Destroy(this);
    }

    private void Start() {
        CurrentFood = FoodPrefabs[FoodIndex];
        audioManager = AudioManager._Instance;
    }

    public void CreateFood(Vector3 position)
    {
        if (MoneyManager._Instance.RemoveMoney(CurrentFood.FoodPrice))
        {
            GameObject newFood = Instantiate(CurrentFood.FoodPrefab, position, Quaternion.identity);
            newFood.GetComponent<FoodStats>().ReliefAmt = CurrentFood.HungerReliefAmt;
            PlacedFood.Add(newFood.transform);
            UpdateMoneyUiGameEvent.Raise();
            
            AudioClip createClip = audioManager.RandomSoundFromArray(audioManager.SpawnFoodSounds);
            audioManager.CreateSoundAtPoint(createClip, position, 0.05f);
        }
        else
        {
            Debug.LogWarning("Not enough money to purchase food");
        }
    }

    public void RemoveFood(GameObject foodToRemove)
    {
        GameObject go = foodToRemove.transform.root.gameObject;
        PlacedFood.Remove(go.transform);
        Destroy(go);
    }

    public void UpgradeFood(int upgradePrice)
    {
        if (FoodIndex < FoodPrefabs.Length - 1)
        {
            if (MoneyManager._Instance.RemoveMoney(upgradePrice))
            {
                FoodIndex++;
                CurrentFood = FoodPrefabs[FoodIndex];
            }
            else
            {
                Debug.LogWarning("Not enough money to upgrade food");
            }
        }else{
            Debug.LogWarning("Can't upgrade food any further");
        }
    }
}