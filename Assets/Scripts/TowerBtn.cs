using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class TowerBtn : MonoBehaviour
{

    //Temp before button happend
    [SerializeField]
    private GameObject towerPrefab;
    public GameObject TowerPrefab
    {
        get
        {
            return towerPrefab;
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }



}
