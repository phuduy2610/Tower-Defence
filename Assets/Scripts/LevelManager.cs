using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class LevelManager : Singelton<LevelManager>
{
    public ObjectPool myPool { get; set; }
    Animator portalAnimator;
    bool isSpawn = true;

    RaycastHit2D hit;
    Tile tileMouseOn;

    //Giữ các Tháp
     private Transform towersHolder;

    //Temp before button happend
    [SerializeField]
    private GameObject towerPrefab;
    public GameObject TowerPrefab{
        get{
            return towerPrefab;
        }
    }

    private void Awake()
    {
        LevelCreator.Instance.CreateLevel(1);
    }
    private void Start()
    {
        myPool = GetComponent<ObjectPool>();
        portalAnimator = LevelCreator.Instance.portal.GetComponent<Animator>();
        towersHolder = new GameObject("Towers Holder").transform;
        
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
        SpawnTower();

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
        enemy.transform.position = LevelCreator.Instance.portal.transform.position;

        yield return null;
    }

    private void SpawnTower(){
        hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()), Vector2.zero);
        if (hit.collider != null)
        {
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                tileMouseOn = hit.collider.gameObject.GetComponent<Tile>();
                if (tileMouseOn != null)
                {
                    if (tileMouseOn.type == TilesType.G)
                    {
                        GameObject tower = (GameObject)Instantiate(towerPrefab,tileMouseOn.WorldPos,Quaternion.identity);
                        tower.GetComponent<SpriteRenderer>().sortingOrder = tileMouseOn.GridPosition.X;
                        //Debug.Log(tileMouseOn.GridPosition.X + ";" + tileMouseOn.GridPosition.Y);
                        tower.transform.SetParent(towersHolder);
                    }
                }
            }
        }
    }
}
