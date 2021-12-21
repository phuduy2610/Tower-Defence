using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
public class LevelManager : Singelton<LevelManager>
{
    public ObjectPool myPool { get; set; }
    Animator portalAnimator;
    bool isSpawn = true;
    public TowerBtn ClickedBtn { get; private set; }
    public Transform towersHolder;
    RaycastHit2D hit;
    Tile tileMouseOn;
    //Temp before button happend



    //Giữ các Tháp

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


        if (!EventSystem.current.IsPointerOverGameObject() && ClickedBtn != null)
        {
            hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()), Vector2.zero);
            if (hit.collider != null)
            {
                if (Mouse.current.leftButton.wasPressedThisFrame)
                {
                    PlaceTower();
                }
            }
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
        enemy.transform.position = LevelCreator.Instance.portal.transform.position;

        yield return null;
    }

    public void pickTower(TowerBtn towerBtn)
    {
        this.ClickedBtn = towerBtn;
        Hover.Instance.Activate(towerBtn.Icon); 
        //Debug.Log(ClickedBtn);
    }

    private void PlaceTower()
    {
        tileMouseOn = hit.collider.gameObject.GetComponent<Tile>();
        if (tileMouseOn != null)
        {
            if (tileMouseOn.type == TilesType.G)
            {
                GameObject tower = (GameObject)Instantiate(ClickedBtn.TowerPrefab, tileMouseOn.WorldPos, Quaternion.identity,towersHolder);
                tower.GetComponent<SpriteRenderer>().sortingOrder = tileMouseOn.GridPosition.X;
                //Debug.Log(tileMouseOn.GridPosition.X + ";" + tileMouseOn.GridPosition.Y);
                BuyTower();
                Hover.Instance.DeActivate();
            }
        }
    }

    private void BuyTower()
    {
        ClickedBtn = null;
    }


}
