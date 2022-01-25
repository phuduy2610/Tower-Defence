using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileHolder : MonoBehaviour
{
    private bool accessible = true;

    public Tile TileIsOn
    {
        get
        {
            if (accessible)
                return tileIsOn;
            else
                return null;
        }
        set => tileIsOn = value;
    }
    private Tile tileIsOn;

    public void AlterAccess(bool value)
    {
        accessible = value;
    }
}
