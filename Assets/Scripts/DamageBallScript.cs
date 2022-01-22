using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageBallScript : MonoBehaviour
{
    GameObject target;
    SpriteRenderer sr;
    private float damage = 0f;

    [SerializeField]
    private DamageTakeBehaviour.DamageType damageType;

    [SerializeField]
    private float moveSpeed;

    [SerializeField]
    private GameObject hitEffect;

    private GameObject hitCenter;

    public void Attack(GameObject target, float damage)
    {
        this.target = target;
        hitCenter = target.GetComponentInParent<Enemy>().HitCenter;
        this.damage = damage;
    }

    private void Update()
    {
        if (target != null && target.activeInHierarchy)
        {
            transform.right = hitCenter.transform.position - transform.position;
            transform.position = Vector2.MoveTowards(transform.position, hitCenter.transform.position, moveSpeed * Time.deltaTime);
        } else
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == target.gameObject)
        {
            var tar = target.gameObject;
            if (tar != null)
            {
                tar.GetComponentInParent<Enemy>().OnGetAttacked(damage);
                var damageScript = tar.GetComponentInParent<DamageTakeBehaviour>();
                if (damageScript != null)
                {
                    damageScript.ApplyEffect(damageType, damage);
                }
                Instantiate(hitEffect, transform.position, Quaternion.identity);
                //Play sound
            }
            Destroy(gameObject);
        }
    }
}
