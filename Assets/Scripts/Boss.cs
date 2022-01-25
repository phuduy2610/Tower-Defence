using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
    [SerializeField]
    protected float defaultSkillCooldown = 10f;

    protected float skillCooldown = 10f;

    protected bool isUsingSkill = false;

    //skill trigger parameter
    protected const string SKILLTRIGGER = "Cast";
    //skill hash
    protected int SKILLHASH = Animator.StringToHash(SKILLTRIGGER);

    protected void Awake()
    {
        animator = GetComponent<Animator>();
        skillCooldown = defaultSkillCooldown;
    }

    protected void Update()
    {
        if (deadFlag)
        {
            return;
        }

        if (isUsingSkill)
        {
            return;
        }

        if (skillCooldown > 0)
        {
            skillCooldown -= Time.deltaTime;
        } else
        {
            skillCooldown = defaultSkillCooldown;
            DoSkill();
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

    protected void DoSkill()
    {
        animator.SetTrigger(SKILLHASH);
        isUsingSkill = true;
    }

    protected void EndSkill()
    {
        isUsingSkill = false;
    }

    protected void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(PLAYERTAG))
        {
            attackedEntity = null;
        }
    }

    protected void OnTriggerEnter2D(Collider2D other)
    {
        string otherTag = other.gameObject.tag;
        switch (otherTag)
        {
            case ARROWTAG:
            case TOWERTAG:
            case ENEMYTAG:
                break;
            case PORTALTAG:
                //đã phát hiện đối phương nên tấn công
                stopFlag = true;
                //lấy thông tin đối phương
                attackedEntity = other.gameObject.GetComponentInParent<Gate>();
                break;
            case PLAYERTAG:
                //đã phát hiện đối phương nên tấn công
                stopFlag = true;
                //lấy thông tin đối phương
                attackedEntity = other.gameObject.GetComponentInParent<Player>();
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
    private void FixedUpdate()
    {
        spriteRenderer.sortingOrder = (int)Mathf.Abs((int)transform.position.y - LevelCreator.Instance.topLeftTile.y);
    }
}
