using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundTrap : MonoBehaviour
{
    [SerializeField]
    private DamageTakeBehaviour.DamageType damageType;

    [SerializeField]
    private float damage =0f;

    [SerializeField]
    private Animator animator;

    private bool damagable = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && damagable)
        {
            var obj = collision.gameObject;
            var enemy = obj.GetComponentInParent<Enemy>();
            if (enemy.Grounded())
            {
                animator.SetTrigger("Attack");
                enemy.OnGetAttacked(damage);
                var tar = obj.GetComponentInParent<DamageTakeBehaviour>();
                if (tar != null)
                {
                    tar.ApplyEffect(damageType, damage);
                }
                damagable = false;
            }
        }
    }
}
