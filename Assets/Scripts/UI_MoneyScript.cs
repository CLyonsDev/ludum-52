using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_MoneyScript : MonoBehaviour
{
    public IntVariable MoneyVariable;
    public TMP_Text Text;

    public void UpdateMoneyText()
    {
        Text.SetText("b"+MoneyVariable.Value.ToString("#,##0"));
    }
}
