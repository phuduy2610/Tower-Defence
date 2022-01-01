using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Gate : Entity
{
    private const string GATEDOWNSTRING = "GateDown";
    private int GATEDOWNHASH = Animator.StringToHash(GATEDOWNSTRING);
    private Animator animator;
    //Set cứng, sửa lại khi có upgrade
    private int energyRate = 10;
    public int EnergyRate
    {
        get
        {
            return energyRate;
        }
        set
        {
            energyRate = value;
        }
    }

    private void Awake()
    {
        hpShow.SetMax(this.hp);
        animator = GetComponent<Animator>();
    }

    private void Start() {
        StartCoroutine("GenerateEnergy");
    }
    protected override void Attack()
    {
        throw new System.NotImplementedException();
    }

    protected override void Move()
    {
        throw new System.NotImplementedException();
    }

    protected override void OnKilled()
    {
        animator.SetTrigger(GATEDOWNHASH);
        hpShow.gameObject.SetActive(false);
        StopCoroutine("GenerateEnergy");
    }

    IEnumerator GenerateEnergy()
    {

        while (LevelManager.Instance != null)
        {
            LevelManager.Instance.EnergyCount += energyRate;
            yield return new WaitForSeconds(1.0f);
        }

    }
}
