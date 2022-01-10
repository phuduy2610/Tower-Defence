using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneTimeTrap : Tool
{
    [SerializeField]
    private DamageTakeBehaviour.DamageType damageType;

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private bool groundedTarget;

    private bool damagable = true;

    private Enemy attackedEntity;

    public override void DestroyTool()
    {
        animator.SetTrigger("Attack");
        damagable = false;
    }

    protected override void Attack()
    {
        // kiểm tra bằng bit xem nó có cùng loại target hay không ( nxor )
        if (!(groundedTarget ^ attackedEntity.Grounded()))
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
