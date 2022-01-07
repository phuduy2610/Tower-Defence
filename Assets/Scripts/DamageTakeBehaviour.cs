using System;
using System.Collections.Generic;
using UnityEngine;

public class DamageTakeBehaviour : MonoBehaviour
{
    public enum DamageType { FIRE, ICE, STUCK }
    private Dictionary<DamageType, DamageEffectTake> damageMap = new Dictionary<DamageType, DamageEffectTake>();
    private List<DamageType> currentEffect = new List<DamageType>();

    private void Awake()
    {
        IceEffectTake iceEffect = GetComponent<IceEffectTake>();
        StuckEffect stuckEffect = GetComponent<StuckEffect>();
        damageMap.Add(DamageType.ICE, iceEffect);
        damageMap.Add(DamageType.STUCK, stuckEffect);
    }

    public void ApplyEffect(DamageType damageType,float damageAmount)
    {
        if (damageType == DamageType.FIRE)
        {
            return;
        }
        DamageEffectTake curr;
        if (currentEffect.Contains(damageType))
        {
            damageMap.TryGetValue(damageType,out curr);
            curr.ResetTime();
        }
        else
        {
            currentEffect.Add(damageType);
            damageMap.TryGetValue(damageType, out curr);
            curr.DamageTake = damageAmount;
        }
        curr.ApplyEffect();
    }
}
