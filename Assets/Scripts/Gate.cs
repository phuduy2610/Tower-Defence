using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Gate : Entity
{

    public event System.Action OnGateDestroy;
    private Player player;
    private const string GATEDOWNSTRING = "GateDown";
    private int GATEDOWNHASH = Animator.StringToHash(GATEDOWNSTRING);
    private Animator animator;
    private bool isGenerate = false;
    //Set cứng, sửa lại khi có upgrade
    private int energyRate = 10;
    public int EnergyRate
    {
        get
        {
            return energyRate;
        }
        set
        {
            energyRate = value;
        }
    }

    private void Awake()
    {
        hpShow.SetMax(this.hp);
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (WaveSpawner.Instance.state == WaveSpawner.SpawnState.SPAWNING ||WaveSpawner.Instance.state == WaveSpawner.SpawnState.WAITING)
        {
            if (!isGenerate)
            {
                isGenerate = true;
                StartCoroutine("GenerateEnergy");
            }
        }
        else
        {
            isGenerate = false;
        }
    }

    private void Start()
    {
        player = FindObjectOfType<Player>();
    }

    private void OnFullyOpen()
    {
        if (OnGateDestroy != null)
        {
            OnGateDestroy();
        }
    }
    protected override void Attack()
    {
        throw new System.NotImplementedException();
    }

    protected override void Move()
    {
        throw new System.NotImplementedException();
    }

    protected override void OnKilled()
    {
        animator.SetTrigger(GATEDOWNHASH);
        hpShow.gameObject.SetActive(false);
        StopCoroutine("GenerateEnergy");
        Invoke("OnFullyOpen", 2f);
    }

    IEnumerator GenerateEnergy()
    {
        while (isGenerate)
        {
            LevelManager.Instance.EnergyCount += energyRate;
            yield return new WaitForSeconds(1.0f);
        }

    }

    public override void KillOff()
    {
    }
}
