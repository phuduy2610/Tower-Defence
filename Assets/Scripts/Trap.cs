using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : Tool
{
    [SerializeField]
    private DamageTakeBehaviour.DamageType damageType;

    [SerializeField]
    private Animator animator;

    private bool damagable = true;

    private Enemy attackedEntity;

    public override void DestroyTool()
    {
        animator.SetTrigger("Attack");
        damagable = false;
    }

    protected override void Attack()
    {
        if (attackedEntity.Grounded())
        {
            animator.SetTrigger("Attack");
            attackedEntity.OnGetAttacked(damage);
            var tar = attackedEntity.GetComponentInParent<DamageTakeBehaviour>();
            if (tar != null)
            {
                tar.ApplyEffect(damageType, damage);
            }
            damagable = false;
        }
    }

    protected override void Move()
    {
    }

    protected override void OnKilled()
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && damagable)
        {
            attackedEntity = collision.gameObject.GetComponentInParent<Enemy>();
            Attack();
        }
    }
}
