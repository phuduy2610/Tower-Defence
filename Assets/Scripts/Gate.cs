using UnityEngine;

public class Gate : Entity
{
    private const string GATEDOWNSTRING = "GateDown";
    private int GATEDOWNHASH = Animator.StringToHash(GATEDOWNSTRING);
    private Animator animator;

    private void Awake()
    {
        hpShow.SetMax(this.hp);
        animator = GetComponent<Animator>();
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
    }
}
