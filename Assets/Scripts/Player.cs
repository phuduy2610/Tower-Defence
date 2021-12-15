using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Entity
{
    private Vector2 direction = Vector2.zero;
    private Animator animator;
    private int MOVE = Animator.StringToHash("Move");
    protected override void Attack()
    {
        throw new System.NotImplementedException();
    }

    // Move player on screen. 
    // Precondition: none
    // Postcondition: none
    protected override void Move()
    {
        if(direction == Vector2.zero)
        {
            animator.SetBool(MOVE, false);
        } else
        {
            animator.SetBool(MOVE, true);
            if (direction.x < 0)
            {
                transform.localScale = new Vector2(-1f * Mathf.Abs(transform.localScale.x), transform.localScale.y);
            }
            else if (direction.x > 0)
            {
                transform.localScale = new Vector2(Mathf.Abs(transform.localScale.x), transform.localScale.y);
            }
        }
        gameObject.transform.position += new Vector3(direction.x, direction.y) * Time.deltaTime * MoveSpeed;
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        Move();
    }

    // Get player movement input. Must only be called by player input. 
    // Precondition: none
    // Postcondition: none
    public void GetMovementInput(InputAction.CallbackContext callbackContext)
    {
        direction = callbackContext.ReadValue<Vector2>();
    }

    // example
    // Sorts an array into ascending order.
    // Precondition: anArray is an array of num integers and 1 <= num <= MAX_ARRAY,
    // where MAX_ARRAY is a global constant that specifi es the maximum size of anArray .
    // Postcondition: anArray[0]<= anArray[1]<= … <= anArray[num - 1];
    // num is unchanged.
}
