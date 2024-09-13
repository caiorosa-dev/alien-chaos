using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageFlash : MonoBehaviour
{
    [ColorUsage(true, true)]
    [SerializeField] private Color flashColor = Color.white;
    [SerializeField] private float flashDuration = 0.2f;

    private SpriteRenderer spriteRenderer;
    private Material material;

    private Coroutine _damageFlashCoroutine;
    private float currentFlashAmount = 0f;

    public void TriggerDamageFlash()
    {
        if (_damageFlashCoroutine != null)
        {
            StopCoroutine(_damageFlashCoroutine);
        }

        _damageFlashCoroutine = StartCoroutine(DamageFlasher());
    }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        Init();
    }

    private void Init()
    {
        material = spriteRenderer.material;
    }

    private IEnumerator DamageFlasher()
    {
        SetFlashColor();

        float elapsedTime = 0f;

        while (elapsedTime < flashDuration)
        {
            elapsedTime += Time.deltaTime;

            currentFlashAmount = Mathf.Lerp(1f, 0f, elapsedTime / flashDuration);
            SetFlashAmount(currentFlashAmount);

            yield return null;
        }

        currentFlashAmount = 0f;
        SetFlashAmount(currentFlashAmount);
    }

    private void SetFlashAmount(float amount)
    {
        material.SetFloat("_FlashAmount", amount);
    }

    private void SetFlashColor()
    {
        material.SetColor("_FlashColor", flashColor);
    }
}
