using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    [SerializeField]
    private Transform pathHolder;
    private int waypointIndex = 0;
    private Vector2[] waypoints;

    private void Start()
    {
        waypoints = new Vector2[pathHolder.childCount];
        for (int i = 0; i < pathHolder.childCount; i++)
        {
            waypoints[i] = pathHolder.GetChild(i).position;
        }
    }

    private void Update()
    {
        Move();
    }
    protected override void Attack()
    {
    }

    protected override void Move()
    {
        if (waypointIndex < waypoints.Length )
        {
            transform.position = Vector2.MoveTowards(transform.position, waypoints[waypointIndex], moveSpeed * Time.deltaTime);
            if((Vector2)transform.position == waypoints[waypointIndex]){
                waypointIndex ++;
            }
        }

    }



}
