using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    public IntVariable MoneyVariable;
    [SerializeField] private int StarterMoney = 100;

    public static MoneyManager _Instance;
    public GameEvent UpdateMoneyUiText;

    private void Awake()
    {
        _Instance = this;
    }

    private void Start()
    {
        MoneyVariable.Value = StarterMoney;
    }

    public void AddMoney(int amount)
    {
        StarterMoney += amount;
    }

    public bool RemoveMoney(int amount)
    {
        // Returns 'true' if money is successfully removed.
        // If 'false' is returned, no money was removed from the player.

        if (MoneyVariable.Value - amount < 0)
        {
            return false;
        }
        else
        {
            MoneyVariable.Value -= amount;
            UpdateMoneyUiText.Raise();
            return true;
        }
    }
}