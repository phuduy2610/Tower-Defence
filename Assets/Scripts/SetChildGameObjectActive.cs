using UnityEngine;
using System.Collections.Generic;

public class SetChildGameObjectActive : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> gameObjects;

    public void SetActive()
    {
        foreach (GameObject obj in gameObjects)
        {
            obj.SetActive(true);
        }
    }
}
