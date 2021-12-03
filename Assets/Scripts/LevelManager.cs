using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] pathTiles;
    private int width = 5;
    private int height = 5;

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

    // Update is called once per frame
    void Update()
    {

    }

    //Hàm tạo Level
    private void CreateLevel(int levelIndex)
    {
        PlaceTile(startPoint);
    }

    //Hàm đặt các Tile lên màn hình
    private void PlaceTile(Vector3 startPoint)
    {
        //Lấy size của một ô tile ( chỉ lấy cái đầu do bằng nhau hết)
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                GameObject currentTile = Instantiate(pathTiles[Random.Range(0, pathTiles.Length)]);
                //Ô tiếp theo thì kế bên ô hiện tại nên += tích của hệ số i và j
                currentTile.transform.position = new Vector3(startPoint.x + j * TileSize, startPoint.y - i * TileSize, 0);
            }
        }
    }

    private Vector3 GetTopLeftPointOfCamera()
    {
        return cameraView.ViewportToWorldPoint(new Vector3(0, 1, cameraView.nearClipPlane));
    }
}
