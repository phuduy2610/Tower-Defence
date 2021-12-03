using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] pathTiles;
    private int width = 5;
    private int height = 5;
    // Start is called before the first frame update
    void Start()
    {
        CreateLevel(1);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void CreateLevel(int levelIndex)
    {
        //Lấy size của một ô tile ( chỉ lấy cái đầu do bằng nhau hết)
        float tileWidth = pathTiles[0].GetComponent<SpriteRenderer>().sprite.bounds.size.x;
        float tileHeight = pathTiles[0].GetComponent<SpriteRenderer>().sprite.bounds.size.y;
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                GameObject currentTile = Instantiate(pathTiles[Random.Range(0, pathTiles.Length)]);
                //Ô tiếp theo thì kế bên ô hiện tại nên += tích của hệ số i và j
                currentTile.transform.position = new Vector3(currentTile.transform.position.x + j * tileWidth, currentTile.transform.position.y - i * tileHeight, 0);
            }
        }
    }
}
