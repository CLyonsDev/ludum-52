using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateProgressBar : MonoBehaviour
{
    public IntVariable CurrentValue;
    public IntVariable MaxValue;

    private Image img;

    private void Start() {
        img = GetComponent<Image>();
    }

    private void Update() {
        img.fillAmount = Mathf.Lerp(img.fillAmount, (CurrentValue.Value / MaxValue.Value), 20 * Time.deltaTime);
    }
}
