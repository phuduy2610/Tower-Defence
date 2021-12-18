using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    [SerializeField]
    protected float hp;

    [SerializeField]
    protected float moveSpeed;

    public float Hp { get => hp; set => hp = value; }

    public float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }

    protected abstract void Move();

    protected abstract void Attack();

    public virtual void OnGetAttack(float damage)
    { 
        this.hp -= damage;
    }
}
