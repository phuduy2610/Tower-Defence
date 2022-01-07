using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StuckEffect : DamageEffectTake
{
    private void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            ClearEffect();
            this.enabled = false;
        }
    }

    public override void ApplyEffect()
    {
        this.enabled = true;
        if (entity.MoveSpeed > 0)
        {
            entity.MoveSpeed = 0;
        }
        timer = duration;
    }

    public override void ClearEffect()
    {
        entity.MoveSpeed = entity.DefaultSpeed;
    }
}
