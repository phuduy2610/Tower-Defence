using UnityEngine;
using System.Collections.Generic;

public class SetAllChildrenGameObjectActive : MonoBehaviour
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
