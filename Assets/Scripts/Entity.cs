using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    [SerializeField]
    float hp;

    [SerializeField]
    float moveSpeed;

    protected abstract void Move();

    protected abstract void Attack();

    protected virtual void OnGetAttack(float damage)
    { 
        this.hp -= damage;
    }
}
