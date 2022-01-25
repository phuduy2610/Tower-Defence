using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField]
    private Sprite icon;

    [SerializeField]
    private GameObject effectObj;

    public Sprite Icon => icon;
    public GameObject EffectObj => effectObj;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (InventorySystem.Instance.AddItem(this))
            {
                Destroy(gameObject);
            }
        }
    }
}
