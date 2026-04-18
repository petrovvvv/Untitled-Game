/*
 *  For simplicity, currently both player types have the same class, TODO: probably make a class heirarchy
 */

using UnityEngine;

[RequireComponent(typeof(Animator))]
public abstract class Player : MonoBehaviour
{
    protected Animator anim;
    
    protected void Startup()
    {
        anim = GetComponent<Animator>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Startup();
    }

    public virtual void SetAnimation(float dx, bool grounded, bool jump)
    {
        anim.SetBool("Walk", dx != 0);
    }

    public virtual void AddEyes(){}
    public virtual void AddLeg(){}
    public virtual void AddArm(){}
}