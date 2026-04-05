using UnityEngine;

public class LegController : MonoBehaviour
{
    [SerializeField] private GameObject player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void OnTriggerEnter2D(Collider2D c)
    {
        PlayerController p = player.GetComponent<PlayerController>();
        if (!p.HasLeg()) {
            // Set player to be able to jump
            p.EnableJump();
        } else
        {
            p.EnableRun();
        }
        // Disable leg object
        gameObject.SetActive(false);
    }
}
