using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTimer : MonoBehaviour
{
    [SerializeField]
    [Min(0f)]
    private float timer = 0f;

    private void Update()
    {
        timer -= Time.deltaTime;    
        if (timer < 0f)
        {
            Destroy(gameObject);
        }
    }
}
