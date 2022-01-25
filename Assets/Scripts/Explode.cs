using UnityEngine;

public class Explode : MonoBehaviour
{
    [SerializeField]
    [Min(0f)]
    private float damage = 0f;

    [SerializeField]
    private DamageTakeBehaviour.DamageType damageType;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            animator.SetTrigger("Explode");
            collision.gameObject.GetComponentInParent<Enemy>().OnGetAttacked(damage); 
            var damageScript = collision.gameObject.GetComponentInParent<DamageTakeBehaviour>();
            if (damageScript != null)
            {
                damageScript.ApplyEffect(damageType, damage);
            }
        }
    }
}
