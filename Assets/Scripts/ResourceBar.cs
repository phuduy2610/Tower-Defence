using UnityEngine;
using UnityEngine.UI;

public class ResourceBar : MonoBehaviour
{
    [SerializeField]
    private Slider fill;

    [SerializeField]
    private float maxVal;

    [SerializeField]
    private float currVal;

 
    public void SetMax(float value)
    {
        maxVal = value;
    }

    public void SetVal(float value)
    {
        currVal = Mathf.Clamp(value, 0, maxVal);
        fill.value = currVal / maxVal;
    }
}
