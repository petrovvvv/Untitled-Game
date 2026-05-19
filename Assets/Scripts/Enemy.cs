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
        Debug.Log("Moving " + mvmt);
        transform.Translate(mvmt);

        CheckSides();
    }

    public void TakeDamage(int n)
    {
        curHealth -= n;
        Debug.Log("Taking " + n + " damage, health = " + curHealth);
        if (curHealth <= 0)
        {
            // TODO
            gameObject.SetActive(false);
        }
    }

    /*void OnCollisionEnter2D(Collision2D collision)
    {
        ColliderDistance2D dist = box.Distance(collision.collider);
        Debug.Log(dist.normal);
    }*/

    // Deals damage to player if they are touching the sides
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

    /*private void CheckTop()
    {
        Bounds b = box.bounds;
        RaycastHit2D hit = Physics2D.Raycast(b.center, Vector2.up, b.extents.y + skinWidth,
                                                playerMask);
        if (hit)
        {
            TakeDamage(transform.Find("Player").GetComponent<Player>().GetDamage());
        }
    }*/
}
