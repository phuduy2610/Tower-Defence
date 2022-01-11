using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeFireBall : MonoBehaviour
{
 // Start is called before the first frame update
    private Rigidbody2D rigid_body;
    private const string PORTALTAG = "Portal";
    //Gate tag
    private const string PLAYERTAG = "Player";
    private float flyDistance;
    [SerializeField]
    private float speed;
    private Entity attackedEntity = null;
    private RangeEnemy rangeEnemy;
    [SerializeField]
    private int damage;
    private Animator animator; 
    private Vector3 startPos;
    void Awake()
    {
        rigid_body = GetComponent<Rigidbody2D>();

    }

    private void Start() {
        animator = GetComponent<Animator>();
        rangeEnemy = FindObjectOfType<RangeEnemy>();
        flyDistance = rangeEnemy.attackDistance;    
        startPos = transform.position;
        rigid_body.AddForce(Vector2.right * speed);
    }

    // Update is called once per frame
    void Update()
    {
        if ((transform.position - startPos).magnitude > flyDistance)
        {
            Destroy(gameObject);
        }
        if(attackedEntity != null){
            StartCoroutine("DealDamage");
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Hit");
        string otherTag = other.gameObject.tag;
        switch (otherTag)
        {
            case PORTALTAG:
                attackedEntity = other.gameObject.GetComponentInParent<Gate>();
                break;
            case PLAYERTAG:
                attackedEntity = other.gameObject.GetComponentInParent<Player>();
                break;
            default:
                break;
        }
    }

    private IEnumerator DealDamage()
    {
        rigid_body.velocity = Vector2.zero;
        animator.SetTrigger("isHit");
        attackedEntity?.OnGetAttacked(damage);
        attackedEntity = null;
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length+animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
        Destroy(gameObject);
        yield break;
    }

}
