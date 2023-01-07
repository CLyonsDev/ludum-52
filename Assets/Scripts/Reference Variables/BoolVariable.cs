using UnityEngine;

[CreateAssetMenu(fileName = "Bool Variable", menuName = "Scriptable Object Variables/Bool Variable")]
public class BoolVariable : ScriptableObject
{
    public bool Value;

    [TextArea(3, 10)]
    public string Description;
}
