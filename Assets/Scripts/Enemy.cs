using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    public bool Grounded() => grounded;
    [SerializeField]
    protected bool grounded = false;
    //die trigger parameter
    protected const string DIETRIGGER = "Die";
    //attack trigger parameter
    protected const string ATTACKTRIGGER = "Attack";
    //attack string
    protected const string ATTACKSTRING = "enemyAttack";
    //Gate tag
    protected const string PORTALTAG = "Portal";
    //Gate tag
    protected const string PLAYERTAG = "Player";
    //Arrow tag
    protected const string ARROWTAG = "Arrow";
    //tower tag
    protected const string TOWERTAG = "Tower";
    //enemy tag
    protected const string ENEMYTAG = "Enemy";
    //item tag
    protected const string ITEMTAG = "Item";
    //Ally hurtbox layer
    protected const string ALLYHURTBOX = "AllyHurtbox";
    //attack hash
    protected int ATTACKHASH = Animator.StringToHash(ATTACKTRIGGER);
    //die hash
    protected int DIEHASH = Animator.StringToHash(DIETRIGGER);
    //Flag để stop di chuyển khi đã đi hết map (Sửa sau khi có thành hoặc người chơi để tấn công)
    protected bool stopFlag = false;
    //Flag để thông báo chết :))) 
    protected bool deadFlag = false;
    //Tile đang đi trên 
    protected Tile tileWalkingOn;
    //Vị trí tiếp theo cần đi đến
    protected Vector3 nextWaypoints = Vector3.zero;
    //Điểm đi qua
    //List các vị trí đã đi qua
    protected List<Point> alreadyThrough = new List<Point>();
    //Enemy đang attack
    protected Entity attackedEntity = null;
    //Animator
    protected Animator animator;
    //
    [SerializeField]
    protected OnAttackedEffect AttackedEffect;

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
        if (attackedEntity != null)
        {
            attackedEntity.OnGetAttacked(this.damage);
        }
        //Play sound
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
            case ITEMTAG:
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

    protected private Vector3 FindNextWaypoint()
    {

        Point currentPoint = new Point(tileWalkingOn.GridPosition.X, tileWalkingOn.GridPosition.Y);
        //Xét các vị trí trên phải và dưới 
        Point upPoint = new Point(currentPoint.X - 1, currentPoint.Y);
        Point rightPoint = new Point(currentPoint.X, currentPoint.Y + 1);
        Point downPoint = new Point(currentPoint.X + 1, currentPoint.Y);

        //Tạo mảng để đi lần lượt qua
        Point[] pointsToCheck = { upPoint, rightPoint, downPoint };
        //Tạo List gồm các tile P để xét
        List<Point> pointsToCheckList = new List<Point>();

        for (int i = 0; i < pointsToCheck.Length; i++)
        {
            Tile tempTile;
            if (LevelCreator.Instance.TilesDictionary.TryGetValue(pointsToCheck[i], out tempTile))
            {
                if (tempTile.type == TilesType.P)
                {
                    if (!alreadyThrough.Contains(pointsToCheck[i]))
                    {
                        pointsToCheckList.Add(pointsToCheck[i]);
                    }

                }
            }
        }
        Point nextPoint = findCloserPoint(pointsToCheckList);

        //Tìm tile đang xét trong Dictionary
        Tile nextTile;
        if (LevelCreator.Instance.TilesDictionary.TryGetValue(nextPoint, out nextTile))
        {
            //Nếu đi qua rồi thì skip còn không thì đó sẽ là ô tiếp theo cần đi đến
            //Debug.Log(nextTile.GridPosition.X + " ; " + nextTile.GridPosition.Y);
            return nextTile.WorldPos;
        }
        else
        {
            //Nếu tìm trong dictionary không ra thì có nghĩa đã đi hết map --> Dừng
            stopFlag = true;
            return Vector3.zero;
        }
        //Debug.Log(nextTile.GridPosition.X + " ; " + nextTile.GridPosition.Y);

    }

    protected override void OnKilled()
    {
        deadFlag = true;
        animator.SetTrigger(DIEHASH);
    }

    public override void KillOff()
    {
        AttackedEffect.ClearEffect();
        //Reset lại các cờ
        deadFlag = false;
        stopFlag = false;
        //Fix dis shet shibe :33
        hp = maxHp;

        //Reset lại thuật toán tìm đường đi
        nextWaypoints = Vector3.zero;
        alreadyThrough.Clear();
        //Nhả lại object này về Level
        WaveSpawner.Instance.myPool.ReleaseObject(gameObject);

    }

    public override void OnGetAttacked(float damage)
    {
        if (hp > 0)
        {
            AttackedEffect.StartEffect();
        }
        base.OnGetAttacked(damage);
    }

    protected Point findCloserPoint(List<Point> pointsToCheck)
    {
        Point GateTarget = LevelCreator.Instance.gatePoint;
        List<float> distance = new List<float>();
        for (int i = 0; i < pointsToCheck.Count; i++)
        {
            distance.Add(Mathf.Abs(GateTarget.X - pointsToCheck[i].X) + Mathf.Abs(GateTarget.Y - pointsToCheck[i].Y));
        }
        int index = 0;
        float min = distance[0];
        if (distance.Count > 1)
        {
            for (int i = 1; i < distance.Count; i++)
            {
                if (distance[i] < min)
                {
                    min = distance[i];
                    index = i;
                }
            }
        }
        alreadyThrough.Add(pointsToCheck[index]);
        return pointsToCheck[index];

    }
}
