using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class Tower : Entity, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private SpriteRenderer rangeSR;

    [SerializeField]
    private GameObject attackBall;

    [SerializeField]
    private float defaultFireRate;

    [SerializeField]
    private Animator animator;

    private float fireRate;
    private List<GameObject> enemies = new List<GameObject>(); 


    private GameObject target;
    protected override void Attack()
    {
        if (target != null)
        {
            animator.SetTrigger("Attack");
            var gameObj = Instantiate(attackBall, transform.position, Quaternion.identity);
            gameObj.GetComponent<DamageBallScript>().Attack(target, damage);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (enemies.Count == 0)
                target = collision.gameObject;

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
                if (target == collision.gameObject)
                {
                    if (enemies.Count == 0)
                        target = null;
                    else
                        target = enemies[0];
                } 
            }
        }
    }

    private void Update()
    {
        if (fireRate < 0)
        {
            Attack();
            fireRate = defaultFireRate;
        } else
        {
            fireRate -= Time.deltaTime;
        }
    }

    protected override void Move()
    {
        throw new System.NotImplementedException();
    }

    protected override void OnKilled()
    {
        throw new System.NotImplementedException();
    }

    private void Awake()
    {
        fireRate = defaultFireRate;
    }

    public void ShowRange()
    {
        Color temp = rangeSR.color;
        temp.a = 0.2f;
        rangeSR.color = temp;
    }

    public void HideRange()
    {
        Color temp = rangeSR.color;
        temp.a = 0f;
        rangeSR.color = temp;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ShowRange();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        HideRange();
    }
}
