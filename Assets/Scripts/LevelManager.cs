using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    //Các mảng game Object sẽ chứa các prefab của tile để vẽ lên màn hình (chia theo 3 loại)
    [SerializeField]
    private GameObject[] pathTiles;

    [SerializeField]
    private GameObject[] groundTiles;

    [SerializeField]
    private GameObject[] edgeTiles;

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
    public Dictionary<Point, Tile> TilesDictionary { get; private set; }= new Dictionary<Point, Tile>();

    //Trả về size của map cho camera

    // Tạo một property trả về size của một tile ( hình vuông nên chỉ cần lấy x)
    public float TileSize
    {
        get
        {
            return pathTiles[0].GetComponent<SpriteRenderer>().sprite.bounds.size.x;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        //Lấy camera và lấy góc bên trái trên
        cameraView = Camera.main;
        startPoint = GetTopLeftPointOfCamera();
        //Gọi hàm tạo level
        CreateLevel(1);
    }


    //Hàm tạo Level
    private void CreateLevel(int levelIndex)
    {
        //Tạo Object để chứa các tile (chủ yếu để dễ nhìn trong hierarchy thôi)
        tilesHolder = new GameObject("Tiles Holder").transform;

        //Đọc từ text lên dữ liệu tạo level
        string[] mapData = ReadLevelText();

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
                GameObject tileNeedPlace = sortingTile(tilesMap[i,j],out typeTemp);
                maxTile = PlaceTile(i, j, startPoint,tileNeedPlace,typeTemp);
            }
        }
        //Đặt giới hạn cho camera có thể di chuyển
        cameraMovement.SetLimits(new Vector3(maxTile.x + TileSize, maxTile.y - TileSize));
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
    private string[] ReadLevelText()
    {
        TextAsset bindData = Resources.Load("Level1") as TextAsset;
        string data = bindData.text.Replace(System.Environment.NewLine, string.Empty);
        return data.Split('-');
    }

    //Hàm dùng để phân loại các loại Tile
    private GameObject sortingTile(string tileCode,out TilesType typeofTile)
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
                case TilesType.P:
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
            typeofTile = 0 ;
            selectedTile = null;
        }
        return selectedTile;
    }

    //Hàm đặt các Tile lên màn hình
    private Vector3 PlaceTile(int line, int column, Vector3 startPoint, GameObject tile,TilesType type)
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
        currentTile.Setup( point, new Vector3(startPoint.x + column * TileSize, startPoint.y - line * TileSize, 0),type);
        
        TilesDictionary.Add(point,currentTile);
        //Debug.Log("Locate: " + TilesDictionary[point].GridPosition.X +","+ TilesDictionary[point].GridPosition.Y + " " + TilesDictionary[point].type );
        return currentTile.transform.position;

    }

    //Hàm lấy góc bên trái của camera 
    private Vector3 GetTopLeftPointOfCamera()
    {
        return cameraView.ViewportToWorldPoint(new Vector3(0, 1, cameraView.nearClipPlane));
    }
}
