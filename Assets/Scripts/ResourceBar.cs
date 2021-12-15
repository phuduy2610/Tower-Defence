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

    // Set resource bar value  
    // Precondition: none
    // Postcondition: none
    public void SetVal(float value)
    {
        currVal = Mathf.Clamp(value, 0, maxVal);
        fill.value = currVal / maxVal;
    }

    // Decrease resource bar value 
    // Precondition: none
    // Postcondition: none
    public void DecVal(float value)
    {
        currVal -= value;
        SetVal(currVal);
    }

    // Increase resource bar value 
    // Precondition: none
    // Postcondition: none
    public void IncVal(float value)
    {
        currVal += value;
        SetVal(currVal);
    }
}
