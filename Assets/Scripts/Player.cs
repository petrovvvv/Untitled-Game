using System;
using UnityEngine;
using UnityEngine.InputSystem;

struct respawnPoint
{
    public Vector2 player;
    public Vector3 camera;
};

[RequireComponent(typeof(Physics))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(DamageFlash))]

public class Player : MonoBehaviour
{
    public bool debug;

    [SerializeField] private float gravity;
    [SerializeField] private float maxFall;
    [SerializeField] private float jumpSpeed;
    [SerializeField] private float speed;
    [SerializeField] private GameObject UICanvas;

    private Physics physics;
    private Animator anim;
    private DamageFlash flash;
    private CameraController cam;
    private InputAction moveAction;
    private InputAction jumpAction;
    private BoxCollider2D curBox;

    // Constants
    private float startSpeed = 2f;
    private float normalSpeed = 5f;
    private static float coyoteTime = 0.1f;
    private static float wallJumpAirTime = 0.225f;
    private float iTime = 0.5f;   // i-frame time after being hit

    // Health and damage
    private int health;
    private int maxHealth;
    private int damage;
    private float  hitTime;
    private respawnPoint checkpoint; // Where to reset after taking damage
    private respawnPoint savepoint;  // Where to reset after dying/reloading from save
    private LayerMask enemyMask;

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
        BoxCollider2D[] boxes = GetComponents<BoxCollider2D>();
        curBox = boxes[0];
        curBox.enabled = true;
        boxes[1].enabled = false;
        physics.SetCollider(curBox);
        anim = GetComponent<Animator>();
        flash = GetComponent<DamageFlash>();
        cam = GameObject.Find("Main Camera").gameObject.GetComponent<CameraController>();

        health = 0;
        maxHealth = 0;
        SetSavepoint(transform.position);
        enemyMask = 1 << LayerMask.NameToLayer("Enemy");
        damage = 1;

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

        if (debug)
        {
            AddEyes();
            AddLeg();
            //AddLeg();
            //AddArm();
            //AddArm();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!canMove)
        {
            return;
        }
        Vector2 movement = moveAction.ReadValue<Vector2>();
        bool grounded = physics.IsGrounded();

        // Horizontal movement
        if (wallJumpTime >= wallJumpAirTime) {
            dX = movement.x * speed;
        }

        // Vertical movement  
        bool climb = twoArms && !grounded && physics.OnWall();
        bool jump = false;  // Whether a jump started this iteration

        if (grounded)
        {
            airTime = 0f;
            dY = 0f;
            doubleJumped = false;
        } else
        {
            if (dY > 0f && (physics.HitHead() || jumpAction.WasReleasedThisFrame()))
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

        // Deal damage to enemies
        //HitBelow();

        // Timers and other checks
        wasInAir = !grounded && !climb;
        wallJumpTime += Time.deltaTime;
        airTime += Time.deltaTime;
        float oldHitTime = hitTime;
        hitTime += Time.deltaTime;

        // Update sprites
        physics.Move(dX * Time.deltaTime, dY * Time.deltaTime);
        SetDir(dX);
        SetAnimation(movement.x, grounded, jump, climb);
        if (oldHitTime < iTime && hitTime >= iTime)
        {
            flash.FlashOff();
        }
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

    // Deals damage to enemy if player hits them from above
    private void HitBelow()
    {
        RaycastHit2D hit = physics.Cast(Vector2.down, enemyMask, 0f);
        if (hit)
        {
            hit.collider.gameObject.GetComponent<Enemy>().TakeDamage(damage);
        }
    }
    public void SetCheckpoint(Vector2 v)
    {
        checkpoint.player = v;
        checkpoint.camera = cam.GetPosition();
    }

    public void SetSavepoint(Vector2 v)
    {
        SetCheckpoint(v);
        savepoint = checkpoint;
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
            hitTime = 0f;
            flash.FlashOn();
            if (health == 0)
            {
                // TODO: possibly reset other things as well, good enough for now
                transform.position = savepoint.player;
                cam.ResetCamera(savepoint.camera);
                health = maxHealth;
            }
            else if (returnToCheckpoint)
            {
                transform.position = checkpoint.player;
                cam.SetPosition(checkpoint.camera);
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
    }

    public void EnableMvmt()
    {
        canMove = true;
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
            curBox.enabled = false;
            curBox = GetComponents<BoxCollider2D>()[1];
            curBox.enabled = true;
            physics.SetCollider(curBox);

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