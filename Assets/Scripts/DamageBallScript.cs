using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageBallScript : MonoBehaviour
{
    GameObject target;
    private float damage = 0f;

    [SerializeField]
    private DamageTakeBehaviour.DamageType damageType;

    [SerializeField]
    private float moveSpeed;

    [SerializeField]
    private GameObject hitEffect;

    private GameObject hitCenter;
    [HideInInspector]
    public Tower towerOG;


    [SerializeField]
    private CircleCollider2D circleCollider;

    private Vector3 currentPos;

    private Vector3 offset;

    private Vector3 prevPos;

    public void Attack(GameObject target, float damage)
    {
        this.target = target;
        hitCenter = target.GetComponentInParent<Entity>().HitCenter;
        this.damage = damage;
        offset = circleCollider.offset;
        currentPos = transform.position + offset;
        prevPos = hitCenter.transform.position;
        //CheckInTarget();
    }

    private void Update()
    {
        if (target != null && target.activeInHierarchy)
        {
            transform.right = hitCenter.transform.position - currentPos;
            currentPos = Vector2.MoveTowards(currentPos, hitCenter.transform.position, moveSpeed * Time.deltaTime);
            if (currentPos == prevPos || currentPos == hitCenter.transform.position)
            {
                DoDamge();
            } else
            {
                prevPos = currentPos;
                transform.position = currentPos - offset;
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == target)
        {
            DoDamge();
        }
    }

    //private void CheckInTarget()
    //{
    //    float radius = circleCollider.radius;
    //    Collider2D[] colliders = Physics2D.OverlapCircleAll(currentPos, radius);
    //    foreach (var collider in colliders)
    //    {
    //        if (collider.gameObject == target)
    //        {
    //            DoDamge();
    //            break;
    //        }
    //    }
    //}

    public void DoDamge()
    {
        var tar = target.gameObject;
        if (tar != null)
        {
            tar.GetComponentInParent<Entity>().OnGetAttacked(damage);
            var damageScript = tar.GetComponentInParent<DamageTakeBehaviour>();
            if (damageScript != null)
            {
                damageScript.ApplyEffect(damageType, damage);
            }
            Instantiate(hitEffect, currentPos, Quaternion.identity);
            //Play sound
            if (towerOG != null)
            {
                towerOG.PlaySoundEffect();
            }
        }
        Destroy(gameObject);
    }
}
