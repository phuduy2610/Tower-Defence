using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StraightDamageBall : ItemScript
{
    [SerializeField]
    private MoveInDirection mover;
    private Player owner;

    public MoveInDirection Mover { get => mover;}

    public override void Setup()
    {
        owner = GameObject.FindGameObjectWithTag("Player")?.GetComponent<Player>();
    }

    public override void Use()
    {
        if (owner != null)
        {
            var obj = Instantiate(gameObject, owner.transform.position, Quaternion.identity);
            obj.GetComponent<MoveInDirection>().GetComponent<StraightDamageBall>().Mover.Setup(owner.transform.localScale.x * Vector3.right);
        }
    }
}
