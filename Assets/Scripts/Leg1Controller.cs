using UnityEngine;

public class Leg1Controller : MonoBehaviour
{
    [SerializeField] private GameObject player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void OnTriggerEnter2D(Collider2D c)
    {
        // Set player to be able to jump
        player.GetComponent<PlayerController>().EnableJump();
        // Disable leg object
        gameObject.SetActive(false);
    }
}
