using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    
    // Private objects
    private Rigidbody2D body;
    // boxColliders[0] = initial, [1] = to be used after 2st leg gained
	private BoxCollider2D[] boxColliders;
    private BoxCollider2D curCollider;
    private PlayerSpriteController spriteController;
	private InputAction moveAction;
    private InputAction sprintAction;
	private InputAction jumpAction;

    // Serialized fields
    [SerializeField] private float initSpeed;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float jumpSpeed;
    [SerializeField] private LayerMask groundLayer;

    // Private fields
    private float speed;
    private bool canJump;
    private bool canWalk;
    private bool canRun;

    // Awake is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        speed = initSpeed;
        body = gameObject.GetComponent<Rigidbody2D>();
		boxColliders = gameObject.GetComponents<BoxCollider2D>();
        boxColliders[0].enabled = true;
        boxColliders[1].enabled = false;
        curCollider = boxColliders[0];
        spriteController = gameObject.GetComponent<PlayerSpriteController>();
        moveAction = InputSystem.actions.FindAction("Move");
        sprintAction = InputSystem.actions.FindAction("Sprint");
		jumpAction = InputSystem.actions.FindAction("Jump");
        canJump = false;
        canWalk = true;
        canRun = false;
    }

    // Update is called once per frame
    void Update()
    {
        float x = body.linearVelocity.x;
        float y = body.linearVelocity.y;
        float moveDir = moveAction.ReadValue<Vector2>().x;

        // Horizontal movement
        if (canRun && sprintAction.IsPressed())
        {
            x = moveDir * runSpeed;
        } else if (canWalk)
        {
            x = moveDir * speed;
        } else if (!IsGrounded())
        {
            // With only one leg, only move x while jumping
            x = moveDir * speed;
        }
        
        // Jumping
        if (canJump && jumpAction.WasPressedThisFrame() && IsGrounded())
        {
            y = jumpSpeed;
        }

        // Set player movement once
        body.linearVelocity = new Vector2(x, y);
    }

    // Returns true iff player is touching the ground
    private bool IsGrounded()
    {
		// BoxCast takes center, size, angle tilt, direction, length, and layer to check
		RaycastHit2D groundRay = Physics2D.BoxCast(curCollider.bounds.center, curCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
		return groundRay.collider != null;	// Hit something on the ground
	}

    // Called when player gets their 1st leg
    public void EnableJump()
    {
        spriteController.addLeg1();
        // New sprite is different dimensions, so needs new collider
        boxColliders[0].enabled = false;
        boxColliders[1].enabled = true;
        curCollider = boxColliders[1];
        speed = walkSpeed;
        canWalk = false;    // Initially, can only move by jumping
        canJump = true;
    }

    // Called when player gets their 2nd leg
    public void EnableRun()
    {
        spriteController.addLeg2();
        canWalk = true;
        canRun = true;
    }
}
