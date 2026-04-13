using System;
using UnityEngine;
using UnityEngine.InputSystem;

/*
 * TODO: 
 *  - figure out how player will move with only one leg
 */

[RequireComponent(typeof(Physics))]

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float gravity;
    [SerializeField] private float maxFall;
    [SerializeField] private float startSpeed;
    [SerializeField] private float normalSpeed;
    [SerializeField] private float jumpSpeed;

    private Physics physics;
    private SpriteRenderer sprite;
    private Animator anim;
    private InputAction moveAction;
    private InputAction jumpAction;
    private GameObject curChild;    // Current active player object
    private BoxCollider2D curCollider;     // curChild's collider
    private static float coyoteTime = 0.1f;
    private float airTime;
    private float speed;
    private float dX, dY;
    private bool canJump;
    private bool oneLeg;    // True if ONLY one leg

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

        airTime = 0f;
        speed = startSpeed;
        dY = 0;
        canJump = false;
        oneLeg = false;
    }

    // Update is called once per frame
    void Update()
    {
        bool grounded = physics.IsGrounded(curCollider);
        bool jump = false;  // Whether a jump started this iteration

        // Horizontal movement
        if (oneLeg && !grounded)
        {
            // For one leg, only move normalSpeed when jumping
            dX = moveAction.ReadValue<Vector2>().x * normalSpeed;
        } else
        {
            dX = moveAction.ReadValue<Vector2>().x * speed;
        }

        // Vertical movement  
        if (grounded)
        {
            airTime = 0f;
            dY = 0f;
        } else
        {
            airTime += Time.deltaTime;
            if (dY > 0 && (physics.HitHead(curCollider) || jumpAction.WasReleasedThisFrame()))
            {
                dY = 0f;
            }
            dY -= gravity * Time.deltaTime;
            // Clamp fall speed
            dY = Math.Max(dY, -maxFall);
        }

        if ((grounded || airTime <= coyoteTime) &&canJump && jumpAction.WasPressedThisFrame())
        {
            dY = jumpSpeed;
            jump = true;
        }

        physics.Move(dX * Time.deltaTime, dY * Time.deltaTime, curCollider);
        curChild.GetComponent<Player>().SetAnimation(dX, grounded, jump);
    }

    public void AddLeg1()
    {
        // Change to full-sized sprite, since player is now upright
        transform.GetChild(0).gameObject.SetActive(false);
        curChild = transform.GetChild(1).gameObject;
        curChild.SetActive(true);
        curCollider = curChild.GetComponent<BoxCollider2D>();

        canJump = true;
        oneLeg = true;
    }

    public void AddLeg2()
    {
        oneLeg = false;
        speed = normalSpeed;
    }

    public void AddArm1() {}
    public void AddArm2() {}
}