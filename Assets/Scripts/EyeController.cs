using UnityEngine;

public class EyeObject : MonoBehaviour
{
    // All objects to be made visible when eyes are acquired
    [SerializeField] private GameObject blindness;
    [SerializeField] private GameObject player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

  void OnTriggerEnter2D(Collider2D collision)
  {
    // Disable blindness
    blindness.GetComponent<BlindnesCurtain>().TurnOff();
    // Turn on eyes on player sprite
    player.GetComponent<PlayerSpriteController>().addEyes();
    // Disable eye object
    gameObject.SetActive(false);
  }
}
