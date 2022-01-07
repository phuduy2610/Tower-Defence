using UnityEngine;
using System.Collections;

public class OnAttackedEffect : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void StartEffect()
    {
        StartCoroutine(StartEffectCoroutine());
    }

    public void ClearEffect()
    {
        StopCoroutine(StartEffectCoroutine());
    }

    private IEnumerator StartEffectCoroutine()
    {
        spriteRenderer.color = Color.red;

        yield return new WaitForSeconds(0.2f);

        if (spriteRenderer != null)
            spriteRenderer.color = Color.white;
    }
}
