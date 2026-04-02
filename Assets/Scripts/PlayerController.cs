using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    
    // Private objects
    private Rigidbody2D body;
	private BoxCollider2D boxCollider;
	private InputAction moveAction;
	private InputAction jumpAction;

    // Serialized fields
    [SerializeField] private float initSpeed;

    // Private fields
    private float speed;

    // Awake is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        speed = initSpeed;
        body = GetComponent<Rigidbody2D>();
		boxCollider = GetComponent<BoxCollider2D>();
        moveAction = InputSystem.actions.FindAction("Move");
		jumpAction = InputSystem.actions.FindAction("Jump");
    }

    // Update is called once per frame
    void Update()
    {
        body.linearVelocity = new Vector2(moveAction.ReadValue<Vector2>().x * speed,
                                        body.linearVelocity.y);
    }
}
