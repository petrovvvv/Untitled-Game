using UnityEngine;
[RequireComponent(typeof(BoxCollider2D))]

public class Enemy : MovingObject
{
    [SerializeField] private int health;
    [SerializeField] private int damage;

    private BoxCollider2D box;
    private LayerMask playerMask;
    private float skinWidth = 0.01f;
    private int curHealth;

    void Start()
    {
        InitMovingObject();
        box = GetComponent<BoxCollider2D>();
        playerMask = 1 << LayerMask.NameToLayer("Player");
        curHealth = health;
    }

    void Update()
    {
        Vector2 mvmt = CalculateMvmt();
        transform.Translate(mvmt);

        CheckSides();
    }

    public void TakeDamage(int n)
    {
        curHealth -= n;
        if (curHealth <= 0)
        {
            // TODO: animation
            gameObject.SetActive(false);
        }
    }

    private void CheckSides()
    {
        Bounds bounds = box.bounds;
        bounds.Expand(-skinWidth * 2f);

        RaycastHit2D hit = Physics2D.BoxCast(bounds.center, bounds.size, 0f,
                                                Vector2.left, skinWidth * 2f, playerMask);
        if (!hit)
        {
            hit = Physics2D.BoxCast(bounds.center, bounds.size, 0f,
                                                Vector2.right, skinWidth * 2f, playerMask);
            if (!hit)
            {
                return;
            }
        }

        hit.collider.GetComponent<Player>().TakeDamage(damage, false);
    }
}
