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
    private GroundChecker groundCheck;

    // Serialized fields
    [SerializeField] private float initSpeed;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float jumpSpeed;

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
        // Ground checker MUST be the first object underneath player in the heirarchy
        canJump = false;
        canWalk = true;
        canRun = false;
    }

    // Start called after all objects are instantiated
    void Start()
    {
        // TODO: disable until jumping added?
        groundCheck = gameObject.transform.GetChild(0).gameObject.GetComponent<GroundChecker>();
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
        } else if (!groundCheck.IsGrounded())
        {
            // With only one leg, only move x while jumping
            x = moveDir * speed;
        }
        
        // Jumping
        if (canJump && jumpAction.WasPressedThisFrame() && groundCheck.IsGrounded())
        {
            y = jumpSpeed;
        }

        // Set player movement once
        body.linearVelocity = new Vector2(x, y);

        // Always keep sprite upright
        transform.eulerAngles = new Vector3(0,0,0);
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

    public bool HasLeg()
    {
        return canJump;
    }
}
