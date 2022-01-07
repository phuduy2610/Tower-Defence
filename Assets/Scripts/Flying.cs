using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flying : Enemy
{
    GameObject target;
    private void Update()
    {
        if (deadFlag)
        {
            return;
        }
        if (!stopFlag)
            Move();
        else
            Attack();
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        target = LevelCreator.Instance.gate.gameObject;
    }

    protected override void Attack()
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName(ATTACKSTRING))
        {
            animator.SetTrigger(ATTACKHASH);
            target.GetComponentInParent<Gate>().OnGetAttacked(damage);
        }
    }

    protected override void Move()
    {
        transform.position = Vector2.MoveTowards(transform.position, target.transform.position, moveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(PORTALTAG))
        {
            stopFlag = true;
        }
    }

    protected override void OnKilled()
    {
        deadFlag = true;
        animator.SetTrigger(DIEHASH);
    }

    public override void KillOff()
    {
        AttackedEffect.ClearEffect();
        deadFlag = false;
        stopFlag = false;
        hp = maxHp;

        //Nhả lại object này về Level
        WaveSpawner.Instance.myPool.ReleaseObject(gameObject);

    }

    public override void OnGetAttacked(float damage)
    {
        if (hp > 0)
        {
            AttackedEffect.StartEffect();
        }
        base.OnGetAttacked(damage);
    }
}
