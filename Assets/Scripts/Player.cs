using System;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.InputSystem;

/*
 * TODO: 
 *  - Finish health system:
 *      - Do we need a lock on health??
 *      - Add animation indicating i-frames
 *  - Add attacks
 *  - Decide what to do with first arm
 */

[RequireComponent(typeof(Physics))]
[RequireComponent(typeof(Animator))]

public class Player : MonoBehaviour
{
    [SerializeField] private float gravity;
    [SerializeField] private float maxFall;
    [SerializeField] private float jumpSpeed;
    [SerializeField] private float speed;
    [SerializeField] private GameObject UICanvas;

    private Physics physics;
    private Animator anim;
    private InputAction moveAction;
    private InputAction jumpAction;
    private GameObject curChild;           // Current active player object
    private BoxCollider2D curCollider;     // curChild's collider

    // Constants
    private float startSpeed = 2f;
    private float normalSpeed = 5f;
    private static float coyoteTime = 0.1f;
    private static float wallJumpAirTime = 0.225f;
    private float iTime = 1f;   // i-frame time after being hit

    // Health
    private int health;
    private int maxHealth;
    private float  hitTime;
    private Vector2 checkpoint; // Where to reset after taking damage
    private Vector2 savepoint;  // Where to reset after dying/reloading from save

    // Movement
    private float airTime;      // Time since leaving ground
    private float wallJumpTime; // Time since leaving wall
    private float dX, dY;       // Amt to move this loop
    private bool canMove;       // If false, disables player movement
    private bool doubleJumped;  // True iff has made their 2nd jump mid-air
    private bool wasInAir;      // Whether the previous frame was spent in mid-air

    // Abilities
    private bool oneLeg;
    private bool twoLegs;
    private bool oneArm;
    private bool twoArms;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        physics = GetComponent<Physics>();
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
        curChild = transform.GetChild(0).gameObject;
        curCollider = curChild.GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();

        // Make sure the correct player sprite is set up
        curChild.SetActive(true);
        transform.GetChild(1).gameObject.SetActive(false);

        health = 0;
        maxHealth = 0;
        SetSavepoint(transform.position);

        hitTime = iTime + 1f;
        airTime = 0f;
        wallJumpTime = wallJumpAirTime + 1f;
        speed = startSpeed;
        dY = 0;
        canMove = true;
        doubleJumped = false;
        wasInAir = true;

        oneLeg = false;
        twoLegs = false;
        oneArm = false;
        twoArms = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!canMove)
        {
            return; // TODO: maybe still want gravity??
        }
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
                } else if (wasInAir)
                {
                    // Just hit wall, stop fall
                    dY = 0f;
                } else
                {
                    // Fall down slower when on wall
                    dY -= gravity * 0.4f * Time.deltaTime;
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
        SetAnimation(movement.x, grounded, jump, climb);
        wasInAir = !grounded && !climb;
        hitTime += Time.deltaTime;
        UpdateHearts();
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
        if (climb)
        {
            // Wall jump
            dX = -transform.localScale.x * speed;   // TODO; adjust speed
            wallJumpTime = 0f;
        } else if (!grounded || airTime > coyoteTime)
        {
            if (!twoLegs || doubleJumped)
            {
                // Can't jump
                return false;
            } else {
                // This is a double jump
                doubleJumped = true;
            }
        }
        dY = jumpSpeed;
        return true;
    }

    private void SetAnimation(float dX, bool grounded, bool jump, bool climb)
    {
        anim.SetBool("Walk", dX != 0);
        anim.SetBool("Grounded", grounded);
        anim.SetBool("Left", transform.localScale.x < 0f);
        anim.SetBool("Wall", climb);
        if (jump)
        {
            anim.SetTrigger("Jump");
        }
    }

    private void UpdateHearts()
    {
        for (int i = 0; i < maxHealth; i++)
        {
            UICanvas.transform.Find("Hearts").transform.GetChild(i).GetComponent<Animator>().SetBool("Active", i < health);
        }
    }

    public void SetCheckpoint(Vector2 v)
    {
        checkpoint = v;
    }

    public void SetSavepoint(Vector2 v)
    {
        checkpoint = v;
        savepoint = v;
    }

    public void addHeart(int n)
    {
        Transform heart;
        for (int i = 0; i < n; i++)
        {
            heart = UICanvas.transform.Find("Hearts").transform.GetChild(maxHealth+i);
            if (heart == null)
            {
                maxHealth += i;
                return;
            }
            heart.gameObject.SetActive(true);
        }
        maxHealth += n;
        health = maxHealth;
    }

    public void TakeDamage(int n, bool returnToCheckpoint)
    {
        if (hitTime > iTime)
        {
            health = Math.Max(health-n, 0);
            if (health == 0)
            {
                // TODO: possibly reset other things as well, good enough for now
                transform.position = savepoint;
                health = maxHealth;
            }
            else if (returnToCheckpoint)
            {
                transform.position = checkpoint;
            }
        }
    }

    public void Heal(int n)
    {
        health = Math.Min(health+n, maxHealth);
    }

    public void DisableMvmt()
    {
        canMove = false;
        Debug.Log("mvmt disabled");
    }

    public void EnableMvmt()
    {
        canMove = true;
         Debug.Log("mvmt enabled");
    }

    public void AddEyes()
    {
        anim.SetTrigger("Eyes");
        addHeart(3);

    }

    public void AddLeg()
    {
        if (!oneLeg) {
            oneLeg = true;
            EnableMvmt();   // Need to call this in case mvmt was disabled mid-crawl
            anim.SetTrigger("Leg1");

            // Change to full-sized sprite and collider, since player is now upright
            transform.GetChild(0).gameObject.SetActive(false);
            curChild = transform.GetChild(1).gameObject;
            curChild.SetActive(true);
            curCollider = curChild.GetComponent<BoxCollider2D>();

            speed = normalSpeed;
        } else
        {
            twoLegs = true;
            anim.SetTrigger("Leg2");
        }
        addHeart(1);
    }

    public void AddArm()
    {
        if (!oneArm) {
            oneArm = true;
            anim.SetTrigger("Arm1");
        } else
        {
            anim.SetTrigger("Arm2");
            twoArms = true;
        }
        addHeart(1);
    }
}