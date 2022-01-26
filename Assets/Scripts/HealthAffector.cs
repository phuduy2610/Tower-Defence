using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthAffector : ItemScript
{
    private Player player;

    [SerializeField]
    private float changeAmount = 0f;

    public override void Setup()
    {
        player = FindObjectOfType<Player>();
    }

    public override void Use()
    {
        if (player != null)
        {
            player.ChangeHealth(changeAmount);
        }
    }
}
