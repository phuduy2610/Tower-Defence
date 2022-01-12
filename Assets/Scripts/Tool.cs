using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tool : Entity
{
    public delegate void OnUpgrade();

    public OnUpgrade onUpgrade;

    public delegate void OnDestroyed();

    public OnDestroyed onDestroyed;

    [SerializeField]
    protected int maxLevel = 1;

    protected int currLevel = 1;

    [SerializeField]
    private float costMultiplier;

    [SerializeField]
    protected float lvlUpDamageMultiplier;

    [SerializeField]
    private float baseCost;

    [SerializeField]
    protected SpriteRenderer sr;

    public virtual void DestroyTool()
    {
        onDestroyed?.Invoke();
    }

    public int CurrLevel
    {
        get => currLevel;
    }

    public int MaxLevel
    {
        get => maxLevel;
    }

    public float LvlUpDamageMultiplier
    {
        get => lvlUpDamageMultiplier;
    }
    public float CostMultiplier 
    { get => costMultiplier; }
    public float BaseCost 
    { get => baseCost; }

    public virtual void OnSelected()
    {
        sr.color = Color.red;
    }

    public virtual void OnDeselected()
    {
        sr.color = Color.white;
    }

    public float CurrentUpCost => baseCost * costMultiplier * currLevel;

    public virtual void Upgrade()
    {
        if (currLevel < maxLevel)
        {
            onUpgrade?.Invoke();
            currLevel += 1;
            damage = damage * lvlUpDamageMultiplier;
        }
    }
}
