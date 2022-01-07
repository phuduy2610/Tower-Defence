using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileHolder : MonoBehaviour
{
    public Tile TileIsOn
    {
        get => tileIsOn;
        set => tileIsOn = value;
    }
    private Tile tileIsOn;
}
