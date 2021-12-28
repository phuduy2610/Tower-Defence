using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    //die trigger parameter
    private const string DIETRIGGER = "Die";
    //attack trigger parameter
    private const string ATTACKTRIGGER = "Attack";
    //attack string
    private const string ATTACKSTRING = "enemyAttack";
    //Gate tag
    private const string PORTALTAG = "Portal";
    //Gate tag
    private const string PLAYERTAG = "Player";
    //Arrow tag
    private const string ARROWTAG = "Arrow";

    private const string ENEMYTAG = "Enemy";
    //attack hash
    private int ATTACKHASH = Animator.StringToHash(ATTACKTRIGGER);
    //die hash
    private int DIEHASH = Animator.StringToHash(DIETRIGGER);
    //Flag để stop di chuyển khi đã đi hết map (Sửa sau khi có thành hoặc người chơi để tấn công)
    bool stopFlag = false;
    //Flag để thông báo chết :))) 
    bool deadFlag = false;
    //Tile đang đi trên 
    private Tile tileWalkingOn;
    //Vị trí tiếp theo cần đi đến
    private Vector3 nextWaypoints = Vector3.zero;
    private Vector3 firstWaypoints = Vector3.zero;
    //List các vị trí đã đi qua
    private List<Point> alreadyThrough = new List<Point>();
    //Enemy đang attack
    private Entity attackedEntity = null;
    //Animator
    private Animator animator;
    //
    [SerializeField]
    private OnAttackedEffect AttackedEffect;

    //[SerializeField]
    //private GameObject hitBox;
    private void Awake()
    {
        animator = GetComponent<Animator>();
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
            Attack();
        }
    }

    protected override void Attack()
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName(ATTACKSTRING))
        {
            if (attackedEntity != null)
                animator.SetTrigger(ATTACKHASH);
            else
                stopFlag = false;
        }
    }

    private void DealDamage()
    {
        attackedEntity?.OnGetAttacked(this.damage);
    }

    protected override void Move()
    {
        if (nextWaypoints != Vector3.zero)
        {
            //Di chuyển tới waypoint tiếp theo
            transform.position = Vector3.MoveTowards(transform.position, nextWaypoints, moveSpeed * Time.deltaTime);
            if (nextWaypoints == transform.position)
            {
                //Tìm waypoint tiếp theo
                nextWaypoints = FindNextWaypoint();

                //var tempPos = Vector3.ClampMagnitude(nextWaypoints - tileWalkingOn.transform.position,1f);
                //hitBox.transform.localPosition = tempPos;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(PLAYERTAG))
        {
            attackedEntity = null;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        string otherTag = other.gameObject.tag;
        switch (otherTag)
        {
            case ARROWTAG:
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
            case ENEMYTAG:
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

    private Vector3 FindNextWaypoint()
    {
        //Xét các vị trí trên phải và dưới 
        Point currentPoint = new Point(tileWalkingOn.GridPosition.X, tileWalkingOn.GridPosition.Y);
        Point upPoint = new Point(currentPoint.X - 1, currentPoint.Y);
        Point rightPoint = new Point(currentPoint.X, currentPoint.Y + 1);
        Point downPoint = new Point(currentPoint.X + 1, currentPoint.Y);
        //Tạo mảng để đi lần lượt qua
        Point[] pointsToCheck = { rightPoint, upPoint, downPoint };

        for (int i = 0; i < pointsToCheck.Length; i++)
        {
            //Tìm tile đang xét trong Dictionary
            Tile nextTile;
            if (LevelCreator.TilesDictionary.TryGetValue(pointsToCheck[i], out nextTile))
            {
                //nếu tìm được loại là P thì xét tiếp xem ô đó đã đi qua chưa
                if (nextTile.type == TilesType.P)
                {
                    //Nếu đi qua rồi thì skip còn không thì đó sẽ là ô tiếp theo cần đi đến
                    if (!alreadyThrough.Contains(nextTile.GridPosition))
                    {
                        //Debug.Log(nextTile.GridPosition.X + " ; " + nextTile.GridPosition.Y);
                        alreadyThrough.Add(nextTile.GridPosition);
                        return nextTile.WorldPos;
                    }
                }
            }
            else
            {
                //Nếu tìm trong dictionary không ra thì có nghĩa đã đi hết map --> Dừng
                stopFlag = true;
                return Vector3.zero;
            }
            //Debug.Log(nextTile.GridPosition.X + " ; " + nextTile.GridPosition.Y);
        }
        return Vector3.zero;


    }

    protected override void OnKilled()
    {
        deadFlag = true;
        animator.SetTrigger(DIEHASH);
    }

    public void KillOff()
    {
        //Reset lại các cờ
        deadFlag = false;
        stopFlag = false;
        //Fix dis shet shibe :33
        this.hp = 100.0f;

        //Reset lại thuật toán tìm đường đi
        nextWaypoints = firstWaypoints;
        alreadyThrough.Clear();

        //Nhả lại object này về Level
        WaveSpawner.Instance.myPool.ReleaseObject(gameObject);

    }

    public override void OnGetAttacked(float damage)
    {
        AttackedEffect.StartEffect();
        base.OnGetAttacked(damage);
    }
}
