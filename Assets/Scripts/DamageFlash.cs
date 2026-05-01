using System;
using UnityEngine;

public class DamageFlash : MonoBehaviour
{
    [SerializeField] private Color flashColor;
    [SerializeField] private float flashSpeed;

    private SpriteRenderer render;
    private MaterialPropertyBlock properties;
    private float flashAmt;
    private float target;
    private bool flashOn;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        render = GetComponent<SpriteRenderer>();
        properties = new MaterialPropertyBlock();
        flashAmt = 0f;
        target = 0f;
        flashOn = false;
    }

    void Update()
    {
        if (flashOn) {
            if (flashAmt <= 0.1f && target == 0f)
            {
                flashAmt = 0f;
                target = 1f;
            }
            else if (flashAmt >= 0.9f && target == 1f)
            {
                flashAmt = 1f;
                target = 0f;
            }
            else
            {
                flashAmt = Mathf.Lerp(flashAmt, target, flashSpeed * Time.deltaTime);
            }
            SetFlash();
        }
    }

    public void FlashOn()
    {
        flashOn = true;
        flashAmt = 1f;
        target = 0f;
        SetFlash();
    }

    public void FlashOff()
    {
        flashOn = false;
        flashAmt = 0f;
        SetFlash();
    }

    private void SetFlash()
    {
        render.GetPropertyBlock(properties);
        properties.SetFloat("_FlashAmt", flashAmt);
        render.SetPropertyBlock(properties);
    }
}
