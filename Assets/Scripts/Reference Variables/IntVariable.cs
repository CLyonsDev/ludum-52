using UnityEngine;

[CreateAssetMenu(fileName = "Int Variable", menuName = "Scriptable Object Variables/Int Variable")]
public class IntVariable : ScriptableObject
{
    public int Value;

    [TextArea(3, 10)]
    public string Description;
}
