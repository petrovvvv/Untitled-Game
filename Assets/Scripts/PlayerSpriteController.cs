using UnityEngine;

public class PlayerSpriteController : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    [SerializeField] private Sprite eyeSprite;
    [SerializeField] private Sprite leg1Sprite;
    [SerializeField] private Sprite leg2Sprite;
    // Awake is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    public void addEyes()
    {
        spriteRenderer.sprite = eyeSprite;
    }

    public void addLeg1()
    {
        spriteRenderer.sprite = leg1Sprite;
    }

    public void addLeg2()
    {
        spriteRenderer.sprite = leg2Sprite;
    }
}
