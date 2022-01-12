using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class Tower : Tool, IPointerEnterHandler, IPointerExitHandler
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

    private bool destroy = false;

    private bool selected = false;

    private GameObject target;
    protected override void Attack()
    {
        if (target != null)
        {
            if (target.GetComponentInParent<Entity>().Hp > 0)
            {
                animator.SetTrigger("Attack");
                var gameObj = Instantiate(attackBall, transform.position, Quaternion.identity);
                gameObj.GetComponent<DamageBallScript>().Attack(target, damage);
            } else
            {
                enemies.Remove(target);
                if (enemies.Count > 0)
                    target = enemies[0];
                else
                    target = null;
            }
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
        if (destroy)
        {
            this.enabled = false;
        }
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
    }

    protected override void OnKilled()
    {
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
        if (!selected)
            HideRange();
    }

    public override void DestroyTool()
    {
        base.DestroyTool();
        animator.SetTrigger("Destroy");
        OnDeselected();
        destroy = true;
    }

    public override void OnSelected()
    {
        base.OnSelected();
        ShowRange();
        selected = true;
    }

    public override void OnDeselected()
    {
        base.OnDeselected();
        HideRange();
        selected = false;
    }
}
