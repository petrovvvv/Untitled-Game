/*
 *  For simplicity, currently both player types have the same class, TODO: probably make a class heirarchy
 */

using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public abstract class Player : MonoBehaviour
{
    protected Animator anim;
    protected SpriteRenderer sprite;
    
    protected void Startup()
    {
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Startup();
    }

    public virtual void SetAnimation(float dx, bool grounded, bool jump)
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
        anim.SetBool("Walk", dx != 0);
    }

    public virtual void AddEyes(){}
    public virtual void AddLeg(){}
    public virtual void AddArm(){}
}
