using UnityEngine;

[System.Serializable]
public class BoolReference{

    public bool UseConstant = false;
    public bool ConstantValue;
    public BoolVariable Variable;

    public bool Value
    {
        get { return UseConstant ? ConstantValue : Variable.Value; }
        set
        {
            if (UseConstant)
                ConstantValue = value;
            else
                Variable.Value = value;
        }
    }
}
