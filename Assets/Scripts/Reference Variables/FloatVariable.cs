using UnityEngine;

[CreateAssetMenu(fileName ="Float Variable", menuName ="Scriptable Object Variables/Float Variable")]
public class FloatVariable : ScriptableObject {
    public float Value;

    [TextArea(3, 10)]
    public string Description;
}
