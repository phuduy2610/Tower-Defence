using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Collections;
using System;
public class LevelManager : Singleton<LevelManager>
{
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

    // biến chứa bảng selection
    [SerializeField]
    private ToolInfoSelection toolInfoSelection;

    private Dictionary<Tile, GameObject> TowerDictionary = new Dictionary<Tile, GameObject>();

    // xóa tool ( tháp, trap ) sau khoảng thời gian time
    public void DestroyToolOnTileAfter(Tile tile, float time)
    {
        StartCoroutine(DestroyToolAfter(tile, time));
    }

    // coroutine của thằng trên =)))
    private IEnumerator DestroyToolAfter(Tile tile, float time)
    {
        yield return new WaitForSeconds(time);
        GameObject tool;
        TowerDictionary.TryGetValue(tile, out tool);
        tile.TurnColorWhite();
        tile.IsEmpty = true;
        TowerDictionary.Remove(tile); 
        Destroy(tool);
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

    public bool ReduceEnergy(int amount)
    {
        if (energyCount - amount >= 0)
        {
            energyCount -= amount;
            return true;
        } else
        {
            return false;
        }
    }

    //Giữ các Tháp

    private void Awake()
    {
        LevelCreator.Instance.CreateLevel(MenuController.Instance.LevelIndex);
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
        if (Time.timeScale == 0)
        {
            return;
        }

        //Ấn chuột phải để bỏ tower đang chọn
        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            if (ClickedBtn != null)
                DroppingTower();
            else
                SelectionOff();
        }

        hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()), Vector2.zero);

        if (hit.collider != null && hit.collider.gameObject.layer != LayerMask.NameToLayer("UI"))
        {
            //Check xem có ấn vào nút hay không
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
            else 
            {
                // nếu nhấn chuột trái lên tool
                if (Mouse.current.leftButton.wasPressedThisFrame)
                {
                    RaycastHit2D[] raycastHit2Ds = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()), Vector2.zero);
                    foreach (RaycastHit2D hit in raycastHit2Ds)
                    {
                        if (hit.collider != null && hit.collider.gameObject.CompareTag("Tile"))
                        {
                            GameObject tower;
                            TowerDictionary.TryGetValue(hit.collider.gameObject.GetComponent<Tile>(), out tower);

                            if (tower != null)
                            {
                                Tool tool = tower.GetComponent<Tool>();
                                if (tool != null)
                                {
                                    // hiện info mới cũng như deselect tool cũ
                                    SelectionOff();
                                    tool.OnSelected();
                                    toolInfoSelection.SetTool(tool);
                                    toolInfoSelection.ShowSelection();
                                }
                            }
                            break;
                        }
                    }
                }
            }
        } 
    }

    // combo deslect tool, hide info selection
    private void SelectionOff()
    {
        toolInfoSelection.DeselectTool();
        toolInfoSelection.RemoveTool();
        toolInfoSelection.HideSelection();
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
        if (tileMouseOn != null)
        {
            tileMouseOn.TurnColorWhite();
        }
    }

    private void DestroyTool()
    {
        tileMouseOn.TurnColorGreen();
        GameObject towerDelete;
        if (TowerDictionary.TryGetValue(tileMouseOn, out towerDelete))
        {
            if (towerDelete.GetComponent<TileHolder>().TileIsOn != null)
            {
                if (Mouse.current.leftButton.wasPressedThisFrame)
                {
                    Hover.Instance.DeActivate();
                    tileMouseOn.TurnColorWhite();
                    Tool tool = towerDelete.GetComponent<Tool>();
                    // xóa info tool cũ cũng như tắt hiện nếu đang select tool đó
                    if (toolInfoSelection.TryRemoveTool(tool))
                        toolInfoSelection.HideSelection();

                    tool.DestroyTool();
                }
            } else
            {
                tileMouseOn.TurnColorRed();
            }
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
                DestroyTool();
            }
            else
            {
                tileMouseOn.TurnColorRed();
            }
        }
    }
}
