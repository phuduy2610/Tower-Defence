using UnityEngine;

public class Flying : Enemy
{
    private const string FALLSTRING = "Fall";
    private int FALLHASH = Animator.StringToHash(FALLSTRING);
    [SerializeField]
    private float defaultFallTime = 0f;
    [SerializeField]
    private float defaultFallSpeed = 0f;
    private float fallSpeed = 0f;
    private float fallTime = 0f;

    GameObject target;
    private void Update()
    {
        if (deadFlag)
        {
            if (fallTime > -1f)
            {
                transform.position += Vector3.down * fallSpeed * Time.deltaTime;
                fallTime += Time.deltaTime;
            }
            if (fallTime > defaultFallTime)
            {
                animator.SetTrigger(DIEHASH);
                animator.SetBool(FALLHASH, false);
                fallTime = -2f;
            }
            return;
        } else
        {
            if (!stopFlag)
                Move();
            else
                Attack();
        }
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        fallSpeed = defaultFallSpeed;
    }

    private void Start()
    {
        target = LevelCreator.Instance.gate.gameObject;
    }

    protected override void Attack()
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName(ATTACKSTRING))
        {
            animator.SetTrigger(ATTACKHASH);
            target.GetComponentInParent<Gate>().OnGetAttacked(damage);
        }
    }

    protected override void Move()
    {
        transform.position = Vector2.MoveTowards(transform.position, target.transform.position, moveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(PORTALTAG))
        {
            stopFlag = true;
        }
    }

    protected override void OnKilled()
    {
        deadFlag = true;
        animator.SetBool(FALLHASH, true);
    }

    public override void KillOff()
    {
        AttackedEffect.ClearEffect();
        deadFlag = false;
        stopFlag = false;
        hp = maxHp;
        fallTime = 0f;

        //Nhả lại object này về Level
        WaveSpawner.Instance.myPool.ReleaseObject(gameObject);

    }

}
