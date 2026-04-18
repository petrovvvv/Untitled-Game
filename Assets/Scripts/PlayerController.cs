using System;
using System.Linq.Expressions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

/*
 * TODO: 
 *  - Fix wall climbing speed
 *  - Add health system
 *  - Add attacks
 */

[RequireComponent(typeof(Physics))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float gravity;
    [SerializeField] private float maxFall;
    [SerializeField] private float startSpeed;
    [SerializeField] private float normalSpeed;
    [SerializeField] private float jumpSpeed;
    [SerializeField] private UIController ui;   // TODO: change this later

    private Physics physics;
    private InputAction moveAction;
    private InputAction jumpAction;
    private GameObject curChild;           // Current active player object
    private BoxCollider2D curCollider;     // curChild's collider

    private static float coyoteTime = 0.1f;
    private static float wallJumpAirTime = 0.2f;
    private float airTime;      // Time since leaving ground
    private float wallJumpTime; // Time since leaving wall
    private float speed;        // Current walk speed
    private float dX, dY;       // Amt to move this loop
    private bool doubleJumped;  // True iff has made their 2nd jump mid-air
    private bool oneLeg;
    private bool twoLegs;
    private bool twoArms;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        physics = GetComponent<Physics>();
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
        curChild = transform.GetChild(0).gameObject;
        curCollider = curChild.GetComponent<BoxCollider2D>();

        // Make sure the correct player sprite is set up
        curChild.SetActive(true);
        transform.GetChild(1).gameObject.SetActive(false);

        airTime = 0f;
        wallJumpTime = wallJumpAirTime + 1f;
        speed = startSpeed;
        dY = 0;
        doubleJumped = false;
        oneLeg = false;
        twoLegs = false;
        twoArms = false;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 movement = moveAction.ReadValue<Vector2>();
        bool grounded = physics.IsGrounded(curCollider);

        // Horizontal movement
        if (wallJumpTime >= wallJumpAirTime) {
            dX = movement.x * speed;
        }
        wallJumpTime += Time.deltaTime;

        // Vertical movement  
        bool climb = twoArms && !grounded && physics.OnWall(curCollider);
        bool jump = false;  // Whether a jump started this iteration

        if (grounded)
        {
            airTime = 0f;
            dY = 0f;
            doubleJumped = false;
        } else
        {
            airTime += Time.deltaTime;
            if (dY > 0f && (physics.HitHead(curCollider) || jumpAction.WasReleasedThisFrame()))
            {
                // Stop jump early
                dY = 0f;
            } else if (climb)
            {
                if (dY > 0f)
                {
                    // Normal gravity when moving upwards to prevent floating
                    dY -= gravity * Time.deltaTime;
                }
                else
                {
                    // Fall down slower when on wall
                    dY -= gravity * 0.2f * Time.deltaTime;
                }
            } else {
                // Normal fall
                dY -= gravity * Time.deltaTime;
                dY = Math.Max(dY, -maxFall);
            }
        }

        if (oneLeg && jumpAction.WasPressedThisFrame())
        {
            jump = Jump(grounded, climb);
        }

        physics.Move(dX * Time.deltaTime, dY * Time.deltaTime, curCollider);
        SetDir(dX);
        curChild.GetComponent<Player>().SetAnimation(dX, grounded, jump);
    }

    private void SetDir(float dx)
    {
        if (dx < 0f)
        {
            // Object faces left
            transform.localScale = new Vector3(-1f, 1f, 1f);
        } else if (dx > 0f)
        {
            // Object faces right
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }

    private bool Jump(bool grounded, bool climb)
    {
        if (!grounded || airTime > coyoteTime)
        {
            if (!twoLegs || doubleJumped)
            {
                // Can't jump
                return false;
            } else if (!climb) {
                // This is a double jump
                doubleJumped = true;
            }
        }
        if (climb)
        {
            // Wall jump
            dX = -transform.localScale.x * speed;   // TODO; adjust speed
            wallJumpTime = 0f;
        }
        dY = jumpSpeed;
        return true;
    }

    public void AddLeg1()
    {
        // Change to full-sized sprite and collider, since player is now upright
        transform.GetChild(0).gameObject.SetActive(false);
        curChild = transform.GetChild(1).gameObject;
        curChild.SetActive(true);
        curCollider = curChild.GetComponent<BoxCollider2D>();

        oneLeg = true;

       // ui.DisableText();
        //ui.EnableJumpText();
    }

    public void AddLeg2()
    {
        twoLegs = true;
        speed = normalSpeed;

       // ui.DisableText();
       // ui.EnableDoubleJumpText();
    }

    public void AddArm1() {}
    public void AddArm2()
    {
        twoArms = true;
    }
}