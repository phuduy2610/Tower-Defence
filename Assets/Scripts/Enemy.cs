using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{

    //Flag để stop di chuyển khi đã đi hết map (Sửa sau khi có thành hoặc người chơi để tấn công)
    bool stopFlag = false;
    //Tile đang đi trên 
    private Tile tileWalkingOn;
    //Vị trí tiếp theo cần đi đến
    private Vector3 nextWaypoints = Vector3.zero;
    //List các vị trí đã đi qua
    private List<Point> alreadyThrough = new List<Point>();
    private void Start()
    {
    }

    private void Update()
    {
        //Nếu cờ bật lên thì dừng không đi nữa
        if (!stopFlag)
        {
            Move();
        }
    }
    protected override void Attack()
    {
    }

    protected override void Move()
    {
        // if (waypointIndex < waypoints.Length)
        // {
        //     transform.position = Vector2.MoveTowards(transform.position, waypoints[waypointIndex], moveSpeed * Time.deltaTime);
        //     if ((Vector2)transform.position == waypoints[waypointIndex])
        //     {
        //         waypointIndex++;
        //     }
        // }

        if (nextWaypoints != Vector3.zero)
        {
            //Di chuyển tới waypoint tiếp theo
            transform.position = Vector3.MoveTowards(transform.position, nextWaypoints, moveSpeed * Time.deltaTime);
            if (nextWaypoints == transform.position)
            {
                //Tìm waypoint tiếp theo
                nextWaypoints = FindNextWaypoint();
            }
        }
    }



    private void OnTriggerEnter2D(Collider2D other)
    {
        //Xem đang đứng trên tile nào
        tileWalkingOn = other.gameObject.GetComponent<Tile>();

        //Nếu nextWaypoints chưa được khởi tạo thì bắt đầu tìm
        if (nextWaypoints == Vector3.zero)
        {
            //Debug.Log("First");
            nextWaypoints = FindNextWaypoint();
        }
        //Debug.Log(tileWalkingOn.GridPosition.X + ";" + tileWalkingOn.GridPosition.Y);
    }

    private Vector3 FindNextWaypoint()
    {
        //Xét các vị trí trên phải và dưới 
        Point currentPoint = new Point(tileWalkingOn.GridPosition.X, tileWalkingOn.GridPosition.Y);
        Point upPoint = new Point(currentPoint.X - 1, currentPoint.Y);
        Point rightPoint = new Point(currentPoint.X, currentPoint.Y + 1);
        Point downPoint = new Point(currentPoint.X + 1, currentPoint.Y);
        //Tạo mảng để đi lần lượt qua
        Point[] pointsToCheck = { upPoint, rightPoint, downPoint };

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



}
