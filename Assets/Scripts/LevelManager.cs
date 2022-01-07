using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
public class LevelManager : Singelton<LevelManager>
{
    //list để giữ các collider khi dùng để raycast vào scene
    public ContactFilter2D filter2D;
    //Biến giữ các bool Object

    //Biến đợi animation của cổng xong trước khi spawn quái
    //Cờ bắt đầu spawn
    //Nút tower nào được click
    public TowerBtn ClickedBtn { get; set; }
    //Object giữ các tower cho đỡ rối giao diện
    [HideInInspector]
    public Transform towersHolder;
    //Dùng để xác định vị trí của chuột trên tile nào
    RaycastHit2D hit;
    Tile tileMouseOn;
    //Temp before button happend
    private int energyCount;
    [SerializeField]
    Text energyText;
    private static Dictionary<Tile, GameObject> TowerDictionary = new Dictionary<Tile, GameObject>();

    public static void DestroyTrap(GameObject trap, float time)
    {
        var currentTile = trap.GetComponent<TileHolder>().TileIsOn;
        TowerDictionary.Remove(currentTile);
        currentTile.TurnColorWhite();
        currentTile.IsEmpty = true;
        Destroy(trap, time);
    }
    public int EnergyCount
    {
        get
        {
            return energyCount;
        }
        set
        {
            this.energyCount = value;
            this.energyText.text = value.ToString();
        }
    }


    //Giữ các Tháp

    private void Awake()
    {
        LevelCreator.Instance.CreateLevel(1);
    }
    private void Start()
    {
        towersHolder = new GameObject("Towers Holder").transform;

        // foreach(KeyValuePair<Point,Tile> item in LevelCreator.TilesDictionary){
        //     Debug.Log(item.Key.X +";" + item.Key.Y + "\ntype:" +  item.Value.type );
        //     Debug.Log(item.Value.WorldPos);
        // }
        EnergyCount = 500;
    }

    private void Update()
    {
        hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()), Vector2.zero);

        //Check xem có ấn vào nút hay không
        if (hit.collider != null && hit.collider.gameObject.layer != LayerMask.NameToLayer("UI"))
        {
            if (ClickedBtn != null)
            {
                RaycastHit2D[] raycastHit2Ds = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()), Vector2.zero);
                foreach (RaycastHit2D hit in raycastHit2Ds)
                {
                    if (hit.collider != null && hit.collider.gameObject.CompareTag("Tile"))
                    {
                        //Nếu ấn vào sẽ gọi hàm để bắt đầu xử lí
                        this.hit = hit;
                        TowerHandle();
                        break;
                    }
                }
            }
        } 

        //Ấn chuột phải để bỏ tower đang chọn
        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            DroppingTower();
        }
    }




    public void PickTower(TowerBtn towerBtn)
    {
        if (EnergyCount >= towerBtn.Price)
        {
            //Gán nút đang chọn 
            this.ClickedBtn = towerBtn;
            //Bật icon
            Hover.Instance.Activate(towerBtn.Icon);
            //Debug.Log(ClickedBtn);
        }


    }

    private void PlaceTower()
    {
        tileMouseOn.TurnColorGreen();
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            //Tạo tower 
            GameObject tool = (GameObject)Instantiate(ClickedBtn.TowerPrefab, tileMouseOn.WorldPos, Quaternion.identity, towersHolder);
            tool.GetComponent<SpriteRenderer>().sortingOrder = tileMouseOn.GridPosition.X;
            tool.GetComponent<TileHolder>().TileIsOn = tileMouseOn;
            TowerDictionary.Add(tileMouseOn, tool);
            tileMouseOn.IsEmpty = false;
            //Debug.Log(tileMouseOn.GridPosition.X + ";" + tileMouseOn.GridPosition.Y);
            BuyTower();
        }
    }


    private void BuyTower()
    {
        if (EnergyCount >= ClickedBtn.Price)
        {
            EnergyCount -= ClickedBtn.Price;
            Hover.Instance.DeActivate();
            tileMouseOn.TurnColorWhite();
        }
        //Khi đặt r thì tắt icon đi và cho tile về màu trắng

    }

    private void DroppingTower()
    {
        //Bỏ tower đang chọn
        Hover.Instance.DeActivate();
        tileMouseOn.TurnColorWhite();
    }

    private void DestroyTower()
    {
        tileMouseOn.TurnColorGreen();
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Hover.Instance.DeActivate();
            tileMouseOn.TurnColorWhite();
            tileMouseOn.IsEmpty = true;
            GameObject towerDelete;
            if (TowerDictionary.TryGetValue(tileMouseOn, out towerDelete))
            {
                Destroy(towerDelete);
            }
            TowerDictionary.Remove(tileMouseOn);
        }
    }


    private void TowerHandle()
    {
        //Xét nếu không được chuột trỏ vào thì thành màu trắng
        if (tileMouseOn != null)
        {
            tileMouseOn.TurnColorWhite();
        }
        //Lấy ra tile được trỏ vào
        tileMouseOn = hit.collider.gameObject.GetComponent<Tile>();
        //Đúng kiểu và trống mới được đặt (hiện xanh)
        if (ClickedBtn.Type == ToolType.tower)
        {
            if (tileMouseOn.IsEmpty && tileMouseOn.type == TilesType.G)
            {
                PlaceTower();
            }
            else
            {
                //Không được đặt hiện đỏ
                tileMouseOn.TurnColorRed();
            }
        }
        else if (ClickedBtn.Type == ToolType.trap)
        {
            if (tileMouseOn.IsEmpty && tileMouseOn.type == TilesType.P)
            {
                PlaceTower();
            }
            else
            {
                //Không được đặt hiện đỏ
                tileMouseOn.TurnColorRed();
            }
        }
        else
        {
            if (!tileMouseOn.IsEmpty)
            {
                DestroyTower();
            }
            else
            {
                tileMouseOn.TurnColorRed();
            }
        }


    }
}
