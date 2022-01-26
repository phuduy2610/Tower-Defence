using UnityEngine;
using UnityEngine.Sprites;
public class ArrowBehaviour : MonoBehaviour
{
    [SerializeField]
    private float movementSpeed;
    private float damage = 0f;
    private float power = 0f;
    private float time = 0f;
    private Vector2 customGravity;
    private Vector2 originalPos;
    private Vector2 direction;
    private Player.FACEDIRECTION facing;
    private float alpha = -30 * Mathf.Deg2Rad;
    private const string ENEMYTAG = "Enemy";
    private const float gravity = 5f;
    [SerializeField]
    AudioClip hitSound;
    [SerializeField]
    AudioClip fireSound;
    Player player;
    [SerializeField]
    [Min(1f)]
    private float multi = 1f;
    private void Start() {
        player = FindObjectOfType<Player>();
        player.PlaySoundEffect(fireSound);
        GetComponent<SpriteRenderer>().sprite = MenuController.Instance.ArrowSprite;
    }

    public void Setup(Vector3 targetPos, float power, float damage)
    {
        targetPos.z = 0;
        multi *= (MenuController.Instance.WeaponSelected + 1f) * 0.5f;
        this.damage = damage * multi;
        this.power = power;
        direction = (targetPos - transform.position).normalized;
        customGravity = Vector2.Perpendicular(direction) * gravity;

        if (direction.x < 0)
            alpha *= -1;

        direction = new Vector2(direction.x * Mathf.Cos(alpha) + direction.y * Mathf.Sin(alpha), -1 * direction.x * Mathf.Sin(alpha) + direction.y * Mathf.Cos(alpha));

        if (customGravity.y > 0f)
        {
            customGravity *= -1;
            facing = Player.FACEDIRECTION.RIGHT;
        }
        else
        {
            facing = Player.FACEDIRECTION.LEFT;
        }
        originalPos = transform.position;

    }

    private void Update()
    {
        time += Time.deltaTime;
        var prePos = transform.position;
        transform.position = originalPos + (movementSpeed * power * time * direction) + (time * time) * 0.5f * customGravity;
        var posChanges = transform.position - prePos;
        if (facing == Player.FACEDIRECTION.RIGHT)
            transform.right = posChanges;
        else
            transform.right = -posChanges;

        if (CheckIfTouchGround())
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(ENEMYTAG))
        {
            collision.gameObject.GetComponentInParent<Enemy>().OnGetAttacked(damage);
            player.PlaySoundEffect(hitSound);
            Destroy(gameObject);
        }
    }

    private bool CheckIfTouchGround()
    {
        return customGravity.x * (transform.position.x - originalPos.x) + customGravity.y * (transform.position.y - originalPos.y) > 0;
    }

}
