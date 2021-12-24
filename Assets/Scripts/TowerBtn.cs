using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ToolType
{
    tower,
    trap
}
public class TowerBtn : MonoBehaviour
{

    //Temp before button happend
    [SerializeField]
    private GameObject towerPrefab;

    [SerializeField]
    private Sprite icon;
    public Sprite Icon
    {
        get
        {
            return icon;
        }
    }
    public GameObject TowerPrefab
    {
        get
        {
            return towerPrefab;
        }
    }
    [SerializeField]
    private int price;
    public int Price
    {
        get
        {
            return price;
        }
    }

    [SerializeField]
    private Text priceTxt;

    [SerializeField]
    private ToolType type;
    public ToolType Type
    {
        get
        {
            return type;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        priceTxt.text = price.ToString();

    }

    // Update is called once per frame




}
