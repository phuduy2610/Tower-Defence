using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceEffectTake : DamageEffectTake
{
    [SerializeField]
    float iceMoveSpeedModifier = 0.5f;

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
        float modifiedSpeed = entity.DefaultSpeed * iceMoveSpeedModifier;
        if (entity.MoveSpeed > modifiedSpeed)
        {
            entity.MoveSpeed = modifiedSpeed;
        }
        timer = duration;
    }

    public override void ClearEffect()
    {
        entity.MoveSpeed = entity.DefaultSpeed;
    }
}
