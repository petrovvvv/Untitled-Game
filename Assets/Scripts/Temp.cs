/*
using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerSpriteController))]
[RequireComponent(typeof(Physics))]
public class PlayerController : MonoBehaviour
{
    // Private objects
    private Physics physics;
    private PlayerSpriteController spriteController;
	private InputAction moveAction;
	private InputAction jumpAction;

    // Serialized fields
    [SerializeField] private float initSpeed;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float jumpSpeed;
    [SerializeField] private float gravity;
    [SerializeField] private float maxFall;

    // Private fields
    private float dX, dY, speed, airTime;
    private float coyoteTime = 0.01f;
    private bool canJump, canWalk;

    // Awake is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        speed = initSpeed;
        physics = GetComponent<Physics>();
        spriteController = GetComponent<PlayerSpriteController>();
        moveAction = InputSystem.actions.FindAction("Move");
		jumpAction = InputSystem.actions.FindAction("Jump");
        dY = 0;
        canJump = false;
        canWalk = true;
    }

    // Update is called once per frame
    void Update()
    {
        // Horizontal movement
        dX = moveAction.ReadValue<Vector2>().x * speed;

        // Vertical movement  
        bool grounded = physics.IsGrounded();
        if (grounded)
        {
            Debug.Log(airTime);
            airTime = 0f;
            dY = 0f;
        } else
        {
            airTime += Time.deltaTime;
            if (physics.HitHead() || (jumpAction.WasReleasedThisFrame() && dY > 0f))
            {
                dY = 0f;
            }
             dY -= gravity * Time.deltaTime;
            // Clamp fall speed
            dY = Math.Max(dY, -maxFall);
        }

        if (jumpAction.WasPressedThisFrame() && (grounded || airTime <= coyoteTime))
        {
            dY = jumpSpeed;
        }

        physics.Move(dX * Time.deltaTime, dY * Time.deltaTime);
    }

    // Called when player gets their 1st leg
    public void EnableJump()
    {
        spriteController.addLeg1();
        speed = walkSpeed;
        canWalk = false;    // Initially, can only move by jumping
        canJump = true;
    }

    // Called when player gets their 2nd leg
    public void EnableRun()
    {
        spriteController.addLeg2();
        canWalk = true;
    }

    public bool HasLeg()
    {
        return canJump;
    }
}
*/