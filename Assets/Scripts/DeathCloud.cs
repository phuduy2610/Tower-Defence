using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathCloud : Skill
{
    [SerializeField]
    private GameObject effect;

    [SerializeField]
    private float damage = 1f;

    [SerializeField]
    [Min(1)]
    private int amountToDestroy = 3;
    protected override void ActivateSkill()
    {
        Tower[] towers = LevelManager.Instance.towersHolder.GetComponentsInChildren<Tower>();
        List<Tower> towerList = new List<Tower>();
        towerList.AddRange(towers);
        int amount = 0;

        while (amount < amountToDestroy && towerList.Count > 0)
        {
            int index = Random.Range(0, towerList.Count);
            var obj = Instantiate(effect, towerList[index].transform.position, Quaternion.identity);
            obj.GetComponent<DestroyTool>().SetTarget(towerList[index]);
            towerList.RemoveAt(index);
            amount += 1;
        }
    }
}
