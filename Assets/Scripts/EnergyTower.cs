using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyTower : Tool
{
    [SerializeField]
    private int bonusEnergy = 10;

    [SerializeField]
    private Animator animator;

    private Gate gate;
    // Start is called before the first frame update
    void Start()
    {
        gate = FindObjectOfType<Gate>();
        Attack();
    }

    // Update is called once per frame
    void Update()
    {

    }

    protected override void Attack()
    {
        gate.EnergyRate += bonusEnergy;
    }

    protected override void Move()
    {
        throw new System.NotImplementedException();
    }

    protected override void OnKilled()
    {
        throw new System.NotImplementedException();
    }

    //Fix this shet shibe :33
    public override void DestroyTool()
    {
        gate.EnergyRate -= bonusEnergy;
        animator.SetTrigger("Destroy");
    }

    public override void KillOff()
    {
    }
}
