using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class LevelManager : Singelton<LevelManager>
{
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
        //Đợi animation chạy xong thì thả quái


        //Check xem có ấn vào nút hay không
        if (!EventSystem.current.IsPointerOverGameObject() && ClickedBtn != null)
        {
            hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()), Vector2.zero);
            if (hit.collider != null && hit.collider.gameObject.tag == "Tile")
            {
                //Nếu ấn vào sẽ gọi hàm để bắt đầu xử lí
                TowerHandle();
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
        else
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


    }
}
