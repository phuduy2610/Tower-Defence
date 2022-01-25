using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Droppable : MonoBehaviour
{
    [SerializeField]
    private Enemy owner;

    [SerializeField]
    private Transform point;

    [SerializeField]
    private GameObject[] items;

    [SerializeField]
    [Range(0,1)]
    private float dropRate = 0f;

    private void OnEnable()
    {
        owner.onDeath += DropItem;
    }

    private void OnDisable()
    {
        owner.onDeath -= DropItem;
    }

    private void DropItem()
    {
        float num = Random.Range(0, 1f);

        if (num <= dropRate)
        {
            int index = Random.Range(0, items.Length);
            Instantiate(items[index], point.position, Quaternion.identity);
        }
    }
}
