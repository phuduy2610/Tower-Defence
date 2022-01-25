using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventorySystem : Singleton<InventorySystem>
{
    [SerializeField]
    private InventoryDisplayer inventDisplayer;

    private StaticList<GameObject> effectList = new StaticList<GameObject>(10);

    public bool AddItem(Item item)
    {
        if (!effectList.IsFull())
        {
            int index = effectList.Add(item.EffectObj);
            if (index != -1)
            {
                inventDisplayer.ChangeItem(item.Icon, index);
                return true;
            }
        }
        return false;
    }

    public void UseItem(int index)
    {
        if (index < effectList.Count && effectList[index] != null)
        {
            if (Time.timeScale > 0)
            {
                var itemScript = effectList[index].GetComponent<ItemScript>();
                itemScript.Setup();
                itemScript.Use();
                effectList.RemoveAt(index);
                inventDisplayer.RemoveItem(index);
            }
        }
    }

    public void Input(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {
            int index = (int)callbackContext.ReadValue<float>();
            UseItem(index - 1);
        }
    }
}
