using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTool : MonoBehaviour
{
    private TileHolder tileHolder;
    public void SetTarget(Tool tool)
    {
        tileHolder = tool.GetComponent<TileHolder>();
    }

    public void DestroyTarget()
    {
        LevelManager.Instance.DestroyToolOnTileAfter(tileHolder.TileIsOn, 0f);
    }
}
