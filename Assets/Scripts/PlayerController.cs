using System;
using UnityEngine;
using UnityEngine.InputSystem;

/*
 * TODO: 
 *  - replace sprite controller with animations, remove sprite controller object
 *  - figure out how player will move with only one leg
 */

[RequireComponent(typeof(Physics))]

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float gravity;
    [SerializeField] private float maxFall;
    [SerializeField] private float speed;
    [SerializeField] private float jumpSpeed;

    private Physics physics;
    private SpriteRenderer sprite;
    private Animator anim;
    private InputAction moveAction;
    private InputAction jumpAction;
    private GameObject curChild;    // Current active player object
    private BoxCollider2D curCollider;     // curChild's collider
    private static float coyoteTime = 0.1f;
    private float dY;
    private float dX;
    private float airTime;

    private bool canJump;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        physics = GetComponent<Physics>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
        curChild = transform.GetChild(0).gameObject;
        curCollider = curChild.GetComponent<BoxCollider2D>();

        // Make sure the correct player sprite is set up
        curChild.SetActive(true);
        transform.GetChild(1).gameObject.SetActive(false);

        dY = 0f;
        airTime = 0f;
        canJump = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Horizontal movement
        dX = moveAction.ReadValue<Vector2>().x * speed;

        // Vertical movement  
        bool grounded = physics.IsGrounded(curCollider);
        if (grounded)
        {
            airTime = 0f;
            dY = 0f;
        } else
        {
            airTime += Time.deltaTime;
            if (physics.HitHead(curCollider) || (jumpAction.WasReleasedThisFrame() && dY > 0f))
            {
                dY = 0f;
            }
             dY -= gravity * Time.deltaTime;
            // Clamp fall speed
            dY = Math.Max(dY, -maxFall);
        }

        if (canJump && jumpAction.WasPressedThisFrame() && (grounded || airTime <= coyoteTime))
        {
            dY = jumpSpeed;
        }

        physics.Move(dX * Time.deltaTime, dY * Time.deltaTime, curCollider);
        curChild.GetComponent<Player>().SetAnimation(dX);
    }

    public void AddLeg1()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        curChild = transform.GetChild(1).gameObject;
        curChild.SetActive(true);
        curCollider = curChild.GetComponent<BoxCollider2D>();
    }
}