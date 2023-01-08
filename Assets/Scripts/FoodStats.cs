using UnityEngine;

public class FoodStats : MonoBehaviour {
    public float ReliefAmt;
    public float FoodLifetime = 6f;

    private void Start() {
        Invoke("DestroyAfterDelay", FoodLifetime);
    }

    void DestroyAfterDelay()
    {
        FoodManager._Instance.RemoveFood(this.gameObject);
    }
}