using UnityEngine;

public class EyeObject : MonoBehaviour
{
    // All objects to be made visible when eyes are acquired
    [SerializeField] private GameObject blindness;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

  void OnTriggerEnter2D(Collider2D collision)
  {
    // Disable blindness
    blindness.GetComponent<BlindnesCurtain>().TurnOff();
    // Disable eye object
    gameObject.SetActive(false);
  }
}
