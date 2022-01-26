using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBlocker : MonoBehaviour
{
    [SerializeField]
    private GameObject[] lvlBtns;

    private void OnEnable()
    {
        SetAvailableUpTo(MenuController.Instance.CurrentLevel);
    }

    public void SetAvailableUpTo(int level)
    {
        level = Mathf.Clamp(level - 1, 0, Constant.MAXLEVEL - 1);
        for (int i = Constant.MAXLEVEL - 1; i > level; i--)
        {
            lvlBtns[i].SetActive(false);
        }
    }
}
