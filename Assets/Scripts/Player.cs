using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class Player : MonoBehaviour
{
    private Animator anim;
    private SpriteRenderer sprite;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }

    public void SetAnimation(float dx)
    {
        if (dx < 0f)
        {
            sprite.flipX = true;
        } else if (dx > 0f)
        {
            sprite.flipX = false;
        }
        anim.SetBool("Walk", dx != 0);
    }

    public void AddEyes()
    {
        anim.SetTrigger("Eyes");
        transform.Find("BlindCurtain").gameObject.GetComponent<Animator>().SetTrigger("Eyes");
        transform.GetChild(0).GetComponent<Animator>().SetTrigger("Eyes");
    }
}
