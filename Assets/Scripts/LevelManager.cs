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
        string[] mapData = new string[]{
            "GPGPP","PGPPP"
        };

        //Lấy size của một ô tile ( chỉ lấy cái đầu do bằng nhau hết)
        for (int i = 0; i < mapData.Length; i++)
        {
            for (int j = 0; j < mapData[i].Length; j++)
            {
                PlaceTile(i, j, startPoint, sortingTileType(mapData[i].Substring(j, 1)));
            }
        }
    }

    private GameObject[] sortingTileType(string tileType){
        if(tileType == "G"){
            return groundTiles;
        }
        else{
            return pathTiles;
        }
    }

    //Hàm đặt các Tile lên màn hình
    private void PlaceTile(int line, int column, Vector3 startPoint, GameObject[] tilesHolder)
    {
        GameObject currentTile;
        //Lấy size của một ô tile ( chỉ lấy cái đầu do bằng nhau hết)
        currentTile = Instantiate(tilesHolder[Random.Range(0, tilesHolder.Length)]);
        //Ô tiếp theo thì kế bên ô hiện tại nên += tích của hệ số i và j
        currentTile.transform.position = new Vector3(startPoint.x + column * TileSize, startPoint.y - line * TileSize, 0);


    }

    private Vector3 GetTopLeftPointOfCamera()
    {
        return cameraView.ViewportToWorldPoint(new Vector3(0, 1, cameraView.nearClipPlane));
    }
}
