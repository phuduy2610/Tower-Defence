using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
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
        cameraView = Camera.main;
        startPoint = GetTopLeftPointOfCamera();
        CreateLevel(1);
    }


    //Hàm tạo Level
    private void CreateLevel(int levelIndex)
    {
        //
        string[] mapData = new string[]{
            "G,P,G,P,P","P,G,P,P,P"
        };

        //Tạo ra mảng đã cắt các kí tự ra thành từng string nhỏ
        string[,] tilesMap = splitString(mapData);

        //Tính số hàng và số cột khi đã trừ đi các dấu ,
        int rows = tilesMap.GetUpperBound(0) - tilesMap.GetLowerBound(0) + 1;
        int cols = tilesMap.GetUpperBound(1) - tilesMap.GetLowerBound(1) + 1;

        //Chạy vòng trên ma trận tạo ở trên để đặt các tile
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j <cols; j++)
            {
                PlaceTile(i, j, startPoint, sortingTile(tilesMap[i,j]));
            }
        }
    }

    //Tách chuỗi đọc được thành các kí tự để phân thành Tile
    private string[,] splitString(string[] mapData)
    {
        char separator = ',';
        int count;
        string token;
        int column = mapData[0].Replace(",", "").Length;
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



    //Hàm dùng để phân loại các loại Tile
    private GameObject sortingTile(string tileType)
    {
        GameObject selectedTile;
        if (tileType == "P")
        {
            selectedTile = pathTiles[Random.Range(0, pathTiles.Length)];
        }
        else if (tileType == "G")
        {
            selectedTile = groundTiles[Random.Range(0, groundTiles.Length)];
        }
        else
        {
            selectedTile = edgeTiles[Random.Range(0, edgeTiles.Length)];
        }
        return selectedTile;
    }

    //Hàm đặt các Tile lên màn hình
    private void PlaceTile(int line, int column, Vector3 startPoint, GameObject tile)
    {
        GameObject currentTile;
        //Lấy size của một ô tile ( chỉ lấy cái đầu do bằng nhau hết)
        currentTile = Instantiate(tile);
        //Ô tiếp theo thì kế bên ô hiện tại nên += tích của hệ số i và j
        currentTile.transform.position = new Vector3(startPoint.x + column * TileSize, startPoint.y - line * TileSize, 0);

    }

    private Vector3 GetTopLeftPointOfCamera()
    {
        return cameraView.ViewportToWorldPoint(new Vector3(0, 1, cameraView.nearClipPlane));
    }
}
