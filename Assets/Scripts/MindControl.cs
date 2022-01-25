using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MindControl : Skill
{
    [SerializeField]
    private GameObject effect;

    [SerializeField]
    [Min(1)]
    private int amountToControl = 6;
    protected override void ActivateSkill()
    {
        Tower[] towers = LevelManager.Instance.towersHolder.GetComponentsInChildren<Tower>();
        List<Tower> towerList = new List<Tower>();
        towerList.AddRange(towers);
        int amount = 0;

        while (amount < amountToControl && towerList.Count > 0)
        {
            int index = Random.Range(0, towerList.Count);
            if (towerList[index].GetComponent<Tower>() != null)
            {
                Instantiate(effect, towerList[index].gameObject.transform);
            }
            towerList.RemoveAt(index);
            amount += 1;
        }
    }
}
