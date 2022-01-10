using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeEnemy : Enemy
{

    private void Awake() {
        animator = GetComponent<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D other)
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
                // stopFlag = true;
                // //lấy thông tin đối phương
                // attackedEntity = other.gameObject.GetComponentInParent<Gate>();
                break;
            case PLAYERTAG:
                // //đã phát hiện đối phương nên tấn công
                // stopFlag = true;
                // //lấy thông tin đối phương
                // attackedEntity = other.gameObject.GetComponentInParent<Player>();
                break;
            default:
                //Xem đang đứng trên tile nào
                tileWalkingOn = other.gameObject.GetComponent<Tile>();

                //Nếu nextWaypoints chưa được khởi tạo thì bắt đầu tìm
                if (nextWaypoints == Vector3.zero)
                {
                    //Debug.Log("First");
                    nextWaypoints = FindNextWaypoint();
                    firstWaypoints = nextWaypoints;

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
            //Attack();
        }
    }
}
