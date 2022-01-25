using UnityEngine;
using UnityEngine.UI;

public class InventoryDisplayer : MonoBehaviour
{
    [SerializeField]
    private Image[] itemIcon;

    public void ChangeItem(Sprite icon, int index)
    {
        itemIcon[index].sprite = icon;
        itemIcon[index].color = Color.white;
    }

    public void RemoveItem(int index)
    {
        itemIcon[index].sprite = null;
        itemIcon[index].color = new Color(1f, 1f, 1f, 0f);
    }

    public void ClearAll()
    {
        foreach (var item in itemIcon)
        {
            item.sprite = null;
        }
    }
}
