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
        player = GameObject.FindGameObjectWithTag("Player")?.GetComponent<Player>();
    }

    public override void Use()
    {
        player?.ChangeHealth(changeAmount);
    }
}
