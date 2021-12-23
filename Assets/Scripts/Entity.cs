using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    [SerializeField]
    protected float hp;

    [SerializeField]
    protected float damage;

    [SerializeField]
    protected float moveSpeed;

    [SerializeField]
    protected ResourceBar hpShow;

    public float Hp { get => hp; }

    public float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }

    protected abstract void Move();

    protected abstract void Attack();

    protected abstract void OnKilled();

    public virtual void OnGetAttacked(float damage)
    {
        hp -= damage; 
        hpShow?.SetVal(this.hp);
        if (hp <= 0) 
        {
            OnKilled();
        }
        Debug.Log(hp);
    }
}
