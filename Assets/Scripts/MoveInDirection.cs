using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveInDirection : MonoBehaviour
{
    [SerializeField]
    [Min(0f)]
    private float speed = 0f;

    private Vector3 direction = Vector3.zero;


    public void Setup(Vector3 direction)
    {
        this.direction = direction;
        transform.localScale = new Vector3(direction.x, 1f, 1f);
        enabled = true;
    }

    private void Update()
    {
        transform.position += speed * Time.deltaTime * direction;
    }
}
