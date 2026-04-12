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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }

    public virtual void SetAnimation(float dx, bool grounded, bool jump)
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

    public virtual void AddEyes(){}
    public virtual void AddLeg(){}
    public virtual void AddArm(){}
}
