using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine.UI;

public class Player : Entity
{

    private bool death = false;
    //bien arrow
    private GameObject arrow;
    // bien the hien viec bat dau ban de thay doi animation
    private bool startFire = false;
    // tinh trang nut ban
    private float fire = 0;
    // huong di chuyen cua nhan vat
    private Vector2 direction = Vector2.zero;
    // bien luc ban
    private float currPower = 0;
    // animator cua nhan vat
    private Animator animator;
    // lay hash cua para move trong animator
    private int MOVEHASH = Animator.StringToHash("Move");
    // lay hash cua para charge trong animator
    private int CHARGEHASH = Animator.StringToHash("Charge");
    // lay hash cua para fire trong animator
    private int FIREHASH = Animator.StringToHash("Fire");
    // lay hash cua para die trong animator
    private int DIEHASH = Animator.StringToHash("Die");
    // UI luc ban
    [SerializeField]
    private ResourceBar powerShow;
    // vi tri hitbox ( noi spawn arrow )
    [SerializeField]
    private Transform hitbox;
    // vi tri hitbox ( noi spawn arrow )
    [SerializeField]
    private GameObject hurtbox;
    // bien arrow prefab
    [SerializeField]
    private GameObject arrowPrefab;
    // luc ban toi da
    [Min(0)]
    [SerializeField]
    private float maxPower;
    // toc do tang cua thanh luc
    [Min(1)]
    [SerializeField]
    private float powerIncRate;
    [SerializeField]
    private Image flashImage;

    private SpriteRenderer spriteRenderer;

    private float halfPlayerSide;

    private float halfPlayerHeight;

    private Vector2 topLeftTilePos;

    private Vector2 bottomRightTilePos;

    public enum FACEDIRECTION { LEFT, RIGHT}

    protected override void Attack()
    {
        if (fire > 0)
        {
            if (!startFire)
            {
                startFire = true;
                Pointer.Instance.switchPointer(Pointer.PointerType.attack);
                animator.SetBool(MOVEHASH, false);
                animator.SetBool(CHARGEHASH, true);
                powerShow.gameObject.SetActive(true);
            }
            currPower = Mathf.Clamp(currPower + powerIncRate * Time.deltaTime, 0, maxPower);
            powerShow.SetVal(currPower);
        }
        else if (currPower > 0)
        {
            Pointer.Instance.switchPointer(Pointer.PointerType.@default);
            animator.SetBool(CHARGEHASH, false);
            animator.SetTrigger(FIREHASH);
            powerShow.gameObject.SetActive(false);
            startFire = false;

            // spawn arrow
            var mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            arrow = Instantiate(arrowPrefab, hitbox.position, Quaternion.identity);
            arrow.SetActive(false);
            arrow.GetComponent<ArrowBehaviour>().Setup(mousePos, currPower, damage);

            if (mousePos.x > transform.position.x)
                ChangeFacingDirection(arrow.transform, FACEDIRECTION.RIGHT);
            else
                ChangeFacingDirection(arrow.transform, FACEDIRECTION.LEFT);

            //
            currPower = 0;
            powerShow.SetVal(0);
        }
    }

    protected override void Move()
    {
        if (direction == Vector2.zero)
        {
            animator.SetBool(MOVEHASH, false);
        } else
        {
            animator.SetBool(MOVEHASH, true);
            if (direction.x < 0)
            {
                ChangeFacingDirection(transform,FACEDIRECTION.LEFT);
            }
            else if (direction.x > 0)
            {
                ChangeFacingDirection(transform,FACEDIRECTION.RIGHT);
            }
        }
        var temp = gameObject.transform.position;
        temp += new Vector3(direction.x, direction.y) * Time.deltaTime * MoveSpeed;
        gameObject.transform.position = new Vector2(Mathf.Clamp(temp.x, topLeftTilePos.x, bottomRightTilePos.x), Mathf.Clamp(temp.y, bottomRightTilePos.y, topLeftTilePos.y));
    }

    // doi huong nhin cua nhan vat
    private void ChangeFacingDirection(Transform trans, FACEDIRECTION direction)
    {
        switch (direction)
        {
            case FACEDIRECTION.LEFT:
                trans.localScale = new Vector2(-1f * Mathf.Abs(transform.localScale.x), transform.localScale.y);
                break;
            case FACEDIRECTION.RIGHT:
                trans.localScale = new Vector2(Mathf.Abs(transform.localScale.x), transform.localScale.y);
                break;
            default:
                break;
        };
    }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        hpShow.SetMax(hp);
        powerShow.SetMax(maxPower);
        powerShow.SetVal(0);

        halfPlayerSide = spriteRenderer.bounds.extents.x;
        halfPlayerHeight = spriteRenderer.bounds.extents.y;
        var tileSize = LevelCreator.Instance.TileSize;

        topLeftTilePos.x = LevelCreator.Instance.topLeftTile.x + halfPlayerSide;
        topLeftTilePos.y = LevelCreator.Instance.topLeftTile.y - halfPlayerHeight;
        bottomRightTilePos.x = LevelCreator.Instance.bottomRightTile.x + tileSize - halfPlayerSide;
        bottomRightTilePos.y = LevelCreator.Instance.bottomRightTile.y + tileSize - halfPlayerHeight;
    }

    private void Update()
    {
        if (death)
        {
            return;
        }
        // fire == 1 la dang nhan nut ban
        if (fire != 1) 
        {
            Move();
        } else
        {
            // dang nhan nut ban thi huong cua nhan vat se thay doi theo con tro chuot
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

            if(mousePos.x > transform.position.x)
                ChangeFacingDirection(transform,FACEDIRECTION.RIGHT);
            else
                ChangeFacingDirection(transform,FACEDIRECTION.LEFT);
        }
        Attack();
    }

    // lay input di chuyen
    public void GetMovementInput(InputAction.CallbackContext callbackContext)
    {
        direction = callbackContext.ReadValue<Vector2>();
    }

    // lay input ban
    public void GetFireInput(InputAction.CallbackContext callbackContext)
    {
        fire = callbackContext.ReadValue<float>();
    }

    protected override void OnKilled()
    {
        animator.SetTrigger(DIEHASH);
        death = true;
        hurtbox.SetActive(false);
    }

    public void FireArrow()
    {
        arrow.SetActive(true);
    }

    public override void OnGetAttacked(float damage)
    {
        StartCoroutine(FlashColorOnAttacked());
        base.OnGetAttacked(damage);
    }


    private IEnumerator FlashColorOnAttacked()
    {
        flashImage.gameObject.SetActive(true);

        yield return new WaitForSeconds(0.05f);

        flashImage.gameObject.SetActive(false);
    }

    public override void KillOff()
    {
        
    }
}
