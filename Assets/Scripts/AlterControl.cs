using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlterControl : MonoBehaviour
{
    private SpriteRenderer sr;

    private Tower tower;

    private TileHolder tileControl;

    [SerializeField]
    [Min(0f)]
    private float duration = 5f;

    private void Awake()
    {
        sr = GetComponentInParent<SpriteRenderer>();
        tower = GetComponentInParent<Tower>();
        tileControl = GetComponentInParent<TileHolder>();

        if (tower != null)
        {
            sr.color = Color.black;
            tower.ChangeTarget(Tower.AttackTarget.ally);
            tower.CheckRange();
            tileControl.AlterAccess(false);
        } else
        {
            Destroy(this);
        }
    }

    private void Update()
    {
        duration -= Time.deltaTime;
        if (duration <= 0f)
        {
            sr.color = Color.white; 
            tower.ChangeTarget(Tower.AttackTarget.enemy);
            tileControl.AlterAccess(true);
            Destroy(this);
        }
    }
}
