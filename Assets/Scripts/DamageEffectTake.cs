using System;
using UnityEngine;

public abstract class DamageEffectTake: MonoBehaviour
{
    [SerializeField]
    protected float timer = 0f;
    [SerializeField]
    protected float duration = 0f;
    [SerializeField]
    protected float damageTake = 0f;
    [SerializeField]
    protected Entity entity;

    public Entity Target
    {
        set => entity = value;
        get => entity;
    }

    public abstract void ApplyEffect();

    public abstract void ClearEffect();

    public void ResetTime()
    {
        timer = duration;
    }

    public float Timer
    {
        get => timer;
        set => timer = value;
    }

    public float DamageTake
    {
        get => damageTake;
        set => damageTake = value;
    }

    public float DefaultTime
    {
        get => duration;
        set => duration = value;
    }
}
