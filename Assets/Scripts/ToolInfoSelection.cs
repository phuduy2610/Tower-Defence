using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ToolInfoSelection : MonoBehaviour
{
    private Tool toolSelected;

    [SerializeField]
    private TextMeshProUGUI damageAmount;

    [SerializeField]
    private TextMeshProUGUI currentLevel;
    
    [SerializeField]
    private TextMeshProUGUI upCost;

    [SerializeField]
    private TextMeshProUGUI upString;

    [SerializeField]
    private Button upgradeBtn;

    public void SetTool(Tool tool)
    {
        toolSelected = tool;
    }

    public void RemoveTool()
    {
        toolSelected = null;
    }

    // nếu đúng là tool hiện tại thì xóa khỏi tool đang được chọn
    public bool TryRemoveTool(Tool tool)
    {
        if (toolSelected == null)
        {
            return false;
        }
        if (toolSelected.Equals(tool))
        {
            RemoveTool();
            return true;
        }
        return false;
    }

    public void DeselectTool()
    {
        toolSelected?.OnDeselected();
    }

    public void ShowSelection()
    {
        if (toolSelected == null)
        {
            return;
        }
        
        gameObject.SetActive(true);
        var cost = 0;

        // kiểm tra còn nâng cấp được ko, nếu ko thì nút up ko bấm dc
        if (toolSelected.CurrLevel >= toolSelected.MaxLevel)
        {
            upgradeBtn.interactable = false;
            upString.text = "Max!";
        }
        else
        {
            upString.text = "Upgrade";
            cost = ((int)toolSelected.CurrentUpCost);
            upgradeBtn.interactable = true;
        }

        // set thông tin tool
        damageAmount.text = toolSelected.Damage.ToString();
        currentLevel.text = toolSelected.CurrLevel.ToString();
        upCost.text = cost.ToString();
    }

    public void HideSelection()
    {
        gameObject.SetActive(false);
    }

    public void UpgradeTool()
    {
        if (LevelManager.Instance.ReduceEnergy((int)toolSelected.CurrentUpCost))
        {
            toolSelected.Upgrade();
            ShowSelection();
        }
    }
}
