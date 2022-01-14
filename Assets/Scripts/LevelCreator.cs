using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCreator : Singelton<LevelCreator>
{
    public Vector2 topLeftTile = Vector2.zero;

    public Vector2 bottomRightTile = Vector2.zero;

    //Các mảng game Object sẽ chứa các prefab của tile để vẽ lên màn hình (chia theo 3 loại)
    [SerializeField]
    private GameObject[] pathTiles;

    [SerializeField]
    private GameObject[] groundTiles;

    [SerializeField]
    private GameObject[] edgeTiles;

    [SerializeField]
    private GameObject gatePrefab;

    [SerializeField]
    private GameObject portalPrefab;

    [SerializeField]
    private GameObject characterObject;
    //Object camera dùng để lấy kích thước màn hình
    Camera cameraView;

    //Điểm bắt đầu đặt tile lên
    Vector3 startPoint;

    //Biến holder giữ các tile
    private Transform tilesHolder;

    //Lấy script của camera movement để lát setLimit cho camera
    [SerializeField]
    private CameraMovement cameraMovement;

    //Chứa toạ độ của tile tương ứng với tile đó ( ví dụ lấy Point(0,1) thì ra tile nào)
    public Dictionary<Point, Tile> TilesDictionary { get; private set; } = new Dictionary<Point, Tile>();

    //Trả về size của map cho camera

    // Tạo một property trả về size của một tile ( hình vuông nên chỉ cần lấy x)
    public float TileSize
    {
        get
        {
            return pathTiles[0].GetComponent<SpriteRenderer>().sprite.bounds.size.x;
        }
    }

    //Các Object spawn trên màn hình
    public List<GameObject> portal { get; private set; } = new List<GameObject>();
    public GameObject character { get; private set; }
    public GameObject gate { get; private set; }


    //Điểm spawn cổng enemy
    List<Point> portalPoint = new List<Point>();
    public Point gatePoint;

    //Điểm spawn character
    Point characterPoint;

    // Start is called before the first frame update

    //Hàm tạo Level
    public void CreateLevel(int levelIndex)
    {
        //Lấy camera và lấy góc bên trái trên
        cameraView = Camera.main;
        startPoint = GetTopLeftPointOfCamera();

        //Tạo Object để chứa các tile (chủ yếu để dễ nhìn trong hierarchy thôi)
        tilesHolder = new GameObject("Tiles Holder").transform;

        //Đọc từ text lên dữ liệu tạo level
        string[] mapData = ReadLevelText(levelIndex);

        //Tạo ra mảng đã cắt các kí tự ra thành từng string nhỏ ( mảng này chứa các kí tự TileType như G,P,E1,...)
        string[,] tilesMap = SplitString(mapData);

        //Tính số hàng và số cột khi đã trừ đi các dấu ,
        int rows = tilesMap.GetUpperBound(0) - tilesMap.GetLowerBound(0);
        int columns = tilesMap.GetUpperBound(1) - tilesMap.GetLowerBound(1) + 1;

        //maxTile để lấy toạ độ của tile cuối cùng, mục đích để set limit cho camera
        Vector3 maxTile = Vector3.zero;

        //Biến tạm để lưu loại của tile
        TilesType typeTemp;
        //Chạy vòng trên ma trận tạo ở trên để đặt các tile
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                //sortingTile sẽ trả về gameObject dựa trên kí tự đọc được ở file Level.txt
                GameObject tileNeedPlace = sortingTile(tilesMap[i, j], out typeTemp);
                maxTile = PlaceTile(i, j, startPoint, tileNeedPlace, typeTemp);
                if (i == 0 && j == 0)
                {
                    topLeftTile = maxTile;
                }
            }
        }
        bottomRightTile = maxTile;

        //Đặt giới hạn cho camera có thể di chuyển
        cameraMovement.SetLimits(new Vector3(maxTile.x + TileSize, maxTile.y - TileSize));
        //Tạo portal
        SpawnPortals();
        SpawnCharacter();
    }

    //Tách chuỗi đọc được thành các kí tự để phân thành Tile 
    private string[,] SplitString(string[] mapData)
    {
        char separator = ',';
        int count;
        string token;
        //Đọc số string
        int column = mapData[0].Length - mapData[0].Replace(",", "").Length + 1;
        string[,] allToken = new string[mapData.Length, column];
        for (int i = 0; i < mapData.Length; i++)
        {
            int j = 0;
            int startPos = 0;
            int foundPos = mapData[i].IndexOf(separator);
            while (foundPos >= 0)
            {
                count = foundPos - startPos;
                token = mapData[i].Substring(startPos, count);
                allToken[i, j] = token;
                j++;
                startPos = foundPos + 1;
                foundPos = mapData[i].IndexOf(separator, startPos);
            }
            count = mapData[i].Length - startPos;
            token = mapData[i].Substring(startPos, count);
            allToken[i, j] = token;
        }

        return allToken;
    }

    //Hàm đọc file text lên và trả về ma trận string
    private string[] ReadLevelText(int levelNum)
    {
        string levelFileName = "Level" + levelNum.ToString();
        TextAsset bindData = Resources.Load(levelFileName) as TextAsset;
        string data = bindData.text.Replace(System.Environment.NewLine, string.Empty);
        return data.Split('-');
    }

    //Hàm dùng để phân loại các loại Tile
    private GameObject sortingTile(string tileCode, out TilesType typeofTile)
    {

        GameObject selectedTile;
        TilesType tileType;
        //Parse string đọc được ra kiểu enum TilesType và so sánh
        if (System.Enum.TryParse(tileCode, out tileType))
        {
            typeofTile = tileType;
            switch (tileType)
            {
                case TilesType.G:
                    selectedTile = groundTiles[Random.Range(0, groundTiles.Length)];
                    break;
                case TilesType.G1:
                    selectedTile = groundTiles[Random.Range(0, groundTiles.Length)];
                    break;
                case TilesType.P:
                    selectedTile = pathTiles[Random.Range(0, pathTiles.Length)];
                    break;
                case TilesType.P1:
                    selectedTile = pathTiles[Random.Range(0, pathTiles.Length)];
                    break;
                case TilesType.P2:
                    selectedTile = pathTiles[Random.Range(0, pathTiles.Length)];
                    break;
                case TilesType.E0:
                    selectedTile = edgeTiles[0];
                    break;
                case TilesType.E1:
                    selectedTile = edgeTiles[1];
                    break;
                case TilesType.E2:
                    selectedTile = edgeTiles[2];
                    break;
                case TilesType.E3:
                    selectedTile = edgeTiles[3];
                    break;
                case TilesType.E4:
                    selectedTile = edgeTiles[4];
                    break;
                case TilesType.E5:
                    selectedTile = edgeTiles[5];
                    break;
                case TilesType.E6:
                    selectedTile = edgeTiles[6];
                    break;
                case TilesType.E7:
                    selectedTile = edgeTiles[7];
                    break;
                case TilesType.E8:
                    selectedTile = edgeTiles[8];
                    break;
                case TilesType.E9:
                    selectedTile = edgeTiles[9];
                    break;
                case TilesType.E10:
                    selectedTile = edgeTiles[10];
                    break;
                case TilesType.E11:
                    selectedTile = edgeTiles[11];
                    break;
                default:
                    selectedTile = null;
                    break;
            }
        }
        else
        {
            typeofTile = 0;
            selectedTile = null;
        }
        return selectedTile;
    }

    //Hàm đặt các Tile lên màn hình
    private Vector3 PlaceTile(int line, int column, Vector3 startPoint, GameObject tile, TilesType type)
    {
        //Tạo kiểu tile
        Tile currentTile;
        //Tạo ra tile bằng instantiate
        currentTile = Instantiate(tile).GetComponent<Tile>();


        //Cho các ô được tạo vào tilesHolder
        currentTile.transform.SetParent(tilesHolder);
        //Ô tiếp theo thì kế bên ô hiện tại nên += tích của hệ số i và j
        //Sử dụng line và column để làm toạ độ x,y của tile; Vị trí thật trên world thì dùng cho vào worldPos của tile để set
        Point point = new Point(line, column);
        currentTile.Setup(point, new Vector3(startPoint.x + column * TileSize, startPoint.y - line * TileSize, 0), type);

        //Xét xem phải tile object không
        CheckObject(currentTile);

        TilesDictionary.Add(point, currentTile);


        //Debug.Log("Locate: " + TilesDictionary[point].GridPosition.X +","+ TilesDictionary[point].GridPosition.Y + " " + TilesDictionary[point].type );
        return currentTile.transform.position;

    }

    //Hàm lấy góc bên trái của camera 
    private Vector3 GetTopLeftPointOfCamera()
    {
        return cameraView.ViewportToWorldPoint(new Vector3(0, 1, cameraView.nearClipPlane));
    }

    private void SpawnPortals()
    {
        Tile portalTile;
        foreach (Point portalP in portalPoint)
        {
            if (TilesDictionary.TryGetValue(portalP, out portalTile))
            {
                portal.Add(Instantiate(portalPrefab, portalTile.WorldPos, Quaternion.identity));
            }
        }

        Tile gateTile;
        if (TilesDictionary.TryGetValue(gatePoint, out gateTile))
        {
            gate = Instantiate(gatePrefab, gateTile.WorldPos, Quaternion.identity);
        }
    }

    private void CheckObject(Tile objectTile)
    {
        if (objectTile.type == TilesType.P1)
        {
            portalPoint.Add(new Point(objectTile.GridPosition));
        }
        if (objectTile.type == TilesType.P2)
        {
            gatePoint = new Point(objectTile.GridPosition);
        }
        //Xét vị trí thành tương tự dưới đây
    }

    private void SpawnCharacter()
    {
        characterPoint = new Point(5, 8);
        Tile characterTile;
        if (TilesDictionary.TryGetValue(characterPoint, out characterTile))
        {
            character = Instantiate(characterObject, characterTile.WorldPos, Quaternion.identity);
        }
    }
}
