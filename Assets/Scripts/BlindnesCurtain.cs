using UnityEngine;

public class BlindnesCurtain : MonoBehaviour
{
    private Animator anim;
    // Awake is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    // Play animation to lift l
    public void TurnOff()
    {
        anim.SetTrigger("AcquiredEyes");
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
