using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeEnemy : Enemy
{
    public float attackDistance;
    private Rigidbody2D rbd2D;
    [SerializeField]
    private GameObject fireBallPrefab;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        rbd2D = GetComponent<Rigidbody2D>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        string otherTag = other.gameObject.tag;
        switch (otherTag)
        {
            case ITEMTAG:
            case ARROWTAG:
            case TOWERTAG:
            case ENEMYTAG:
                break;
            case PORTALTAG:
                break;
            case PLAYERTAG:
                break;
            default:
                //Xem đang đứng trên tile nào
                tileWalkingOn = other.gameObject.GetComponent<Tile>();

                //Nếu nextWaypoints chưa được khởi tạo thì bắt đầu tìm
                if (nextWaypoints == Vector3.zero)
                {
                    //Debug.Log("First");
                    nextWaypoints = FindNextWaypoint();
                }
                //Debug.Log(tileWalkingOn.GridPosition.X + ";" + tileWalkingOn.GridPosition.Y);
                break;
        }
    }


    private void Start()
    {

    }
    private void Update()
    {

        RaycastHit2D hit = Physics2D.Raycast(transform.position + Vector3.down*0.1f + Vector3.right*0.1f, transform.right, attackDistance, LayerMask.GetMask(ALLYHURTBOX));
        if (hit.collider != null)
        {
            if (hit.collider.gameObject.tag == PLAYERTAG)
            {
                stopFlag = true;
                attackedEntity = hit.collider.gameObject.GetComponentInParent<Player>();
            }
            else if (hit.collider.gameObject.tag == PORTALTAG)
            {
                stopFlag = true;
                attackedEntity = hit.collider.gameObject.GetComponentInParent<Gate>();
            }
        }
        else
        {
            stopFlag = false;
        }

        if (deadFlag)
        {
            return;
        }
        //Nếu cờ bật lên thì dừng không đi nữa
        if (!stopFlag)
        {
            Move();
        }
        else
        {
            Attack();
        }
    }

    private void LauchFireBall(){
        GameObject fireBall = Instantiate(fireBallPrefab,transform.position - transform.up*0.1f + transform.right,Quaternion.identity);
    }
}
