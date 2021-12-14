using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public ObjectPool myPool { get; set; }
    LevelCreator levelCreator;
    Animator portalAnimator;
    bool isSpawn = true;
    private void Awake()
    {
        levelCreator = FindObjectOfType<LevelCreator>();
        levelCreator.CreateLevel(1);
    }
    private void Start()
    {
        myPool = GetComponent<ObjectPool>();
        portalAnimator = levelCreator.portal.GetComponent<Animator>();
        
        
        // foreach(KeyValuePair<Point,Tile> item in LevelCreator.TilesDictionary){
        //     Debug.Log(item.Key.X +";" + item.Key.Y + "\ntype:" +  item.Value.type );
        //     Debug.Log(item.Value.WorldPos);
        // }

    }

    private void Update()
    {
        if (portalAnimator.GetCurrentAnimatorStateInfo(0).IsName("portalIdle") && isSpawn)
        {
            StartWave();
            isSpawn = false;
        }

    }
    private void StartWave()
    {
        StartCoroutine("SpawnWave");
    }

    private IEnumerator SpawnWave()
    {
        //Nhớ update khi thêm enemy
        int monsterIndex = 0;
        string type = string.Empty;
        switch (monsterIndex)
        {
            case 0:
                type = "Normal";
                break;
        }
        GameObject enemy = myPool.GetObject(type);
        enemy.transform.position = levelCreator.portal.transform.position;

        yield return null;
    }

    // private void IsStartSpawning()
    // {

    // }
}
