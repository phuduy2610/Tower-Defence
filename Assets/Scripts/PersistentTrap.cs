using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentTrap : Tool
{
    [SerializeField]
    private DamageTakeBehaviour.DamageType damageType;

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private float defaultCooldown = 0f;

    [SerializeField]
    private bool groundedTarget;

    private float cooldown = 0f;

    private bool damagable = true;

    private List<GameObject> enemies = new List<GameObject>();

    public override void DestroyTool()
    {
        animator.SetTrigger("Destroy");
        damagable = false;
        cooldown = defaultCooldown;
        this.enabled = false;
    }

    private void Update()
    {
        if (cooldown < 0)
        {
            Attack();
            cooldown = defaultCooldown;
        } else
        {
            cooldown -= Time.deltaTime;
        }
    }

    protected override void Attack()
    {
        if (damagable)
        {
            animator.SetTrigger("Attack");
        }
    }

    protected void DoDamage()
    {
        if (enemies.Count > 0)
        {
            foreach (var enemy in enemies)
            {
                var curr = enemy.GetComponentInParent<Enemy>();
                // kiểm tra bằng bit xem nó có cùng loại target hay không ( nxor )
                if (!(groundedTarget ^ curr.Grounded()))
                {
                    curr.OnGetAttacked(damage);
                    var tar = enemy.GetComponentInParent<DamageTakeBehaviour>();
                    if (tar != null)
                    {
                        tar.ApplyEffect(damageType, damage);
                    }
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            enemies.Add(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (enemies.Count > 0)
            {
                enemies.Remove(collision.gameObject);
            }
        }
    }

    protected override void Move()
    {
    }

    protected override void OnKilled()
    {
    }

    public override void KillOff()
    {

    }
}
