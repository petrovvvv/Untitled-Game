using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    [SerializeField] private LayerMask ground;
    private bool grounded;
    private BoxCollider2D boxCollider;
    // Awake is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        boxCollider = gameObject.GetComponent<BoxCollider2D>();
    }

    private void OnTriggerStay2D(Collider2D c)
    {
        // c's layer mask is (1 << layer)
        grounded = c != null && ((1 << c.gameObject.layer) & ground) != 0;
    }

    private void OnTriggerExit2D(Collider2D c)
    {
        grounded = false;
    }

    public bool IsGrounded()
    {
        return grounded;
    }

}
