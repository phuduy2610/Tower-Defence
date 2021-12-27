using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    //Dùng để làm pool chứa khi cần tạo thêm object ( enemy, tia bắn ,...)
    [SerializeField]
    private GameObject[] enemyPrefabs;
    private List<GameObject> objectsInPool = new List<GameObject>();
    //Hàm dùng để tạo và trả về object game cần
    public GameObject GetObject(string type)
    {

        foreach (GameObject gameObject in objectsInPool)
        {
            if(gameObject.name == type && !gameObject.activeInHierarchy){
                gameObject.SetActive(true);
                return gameObject;
            }
        }


        for (int i = 0; i < enemyPrefabs.Length; i++)
        {
            //Tìm object cần tạo
            if (enemyPrefabs[i].name == type)
            {
                //Tạo object cần get
                GameObject newObject = Instantiate(enemyPrefabs[i]);
                //Đặt lại tên ( vì tên mặc định sẽ có chữ clone)
                newObject.name = type;
                objectsInPool.Add(newObject);
                return newObject;
            }
        }
        return null;
    }

    public void ReleaseObject(GameObject gameObject)
    {
        gameObject.SetActive(false);
    }
}
