using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    LevelCreator levelCreator;
    private void Start() {
        levelCreator = FindObjectOfType<LevelCreator>();
        levelCreator.CreateLevel(1);
        // foreach(KeyValuePair<Point,Tile> item in LevelCreator.TilesDictionary){
        //     Debug.Log(item.Key.X +";" + item.Key.Y + "\ntype:" +  item.Value.type );
        //     Debug.Log(item.Value.WorldPos);
        // }

    }
}
