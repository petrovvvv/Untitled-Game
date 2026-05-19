 /*
  *  Custom physics engine. Replacement for Unity's Rigitbody2D.
  */

// TODO: make single diagonal cast
// TODO: maybe make specific clibmable wall layer

using System;
using System.Collections.Generic;
using UnityEngine;

public class Physics : MonoBehaviour
{   
    private float skinWidth = 0.02f;
    private LayerMask selfMask;
    private LayerMask groundMask;
    private BoxCollider2D col;

    void Start()
    {
        selfMask = 1 << gameObject.layer;
        groundMask = 1 << LayerMask.NameToLayer("Ground");
    }

    void FixedUpdate()
    {
        // Check for accidental collision, then move out
        RaycastHit2D hit = Cast(new Vector2(1, 1), ~selfMask, 0f);
        if (hit)
        {
            // Move a little outside the object
            ColliderDistance2D dist = col.Distance(hit.collider);
            if (dist.isOverlapped) {
                transform.Translate(dist.normal * (dist.distance - skinWidth*2));
                Move(0, dist.distance*5f);  // Move down a bit to counteract floating
            }
        }
    }

    public void SetCollider(BoxCollider2D c)
    {
        col = c;
    }

    // Returns if object is touching ground. If true, sets object as child of ground
    // Otherwise, removes it from previous heirarchy
    public bool IsGrounded()
    {
        RaycastHit2D groundHit = Cast(Vector2.down, groundMask, 0f);
        if (groundHit)
        {
            transform.SetParent(groundHit.transform);
        } else
        {
            transform.SetParent(null);
        }
        return groundHit;
    }

    public bool HitHead()
    {
        RaycastHit2D topHit = Cast(Vector2.up, ~selfMask, 0f);
        return topHit;
    }

    public bool OnWall()
    {
        // Cast ray in direction object is facing
        Vector2 dir = new Vector2(transform.localScale.x, 0f);
        RaycastHit2D sideHit = Cast(dir, ~selfMask, 0f);
        return sideHit;
    }

    public void Move(float x, float y)
    {
        // BoxCast in direction object is moving to check for obstacles
        x = CastLen(Vector2.right, x);
        y = CastLen(Vector2.up, y);
        
        transform.Translate(new Vector2(x, y));
    }

    // Casts a ray and returns the max length that the object can move along that ray
    public float CastLen(Vector2 dir, float len) {
        float dirSign = Math.Sign(len);
        RaycastHit2D hit = Cast(dir * dirSign, ~selfMask, Math.Abs(len));
        if (!hit)
        {
            return len;
        }
        return (hit.distance - skinWidth) * dirSign;
    }

    // Casts a ray and returns its RayCastHit2D, or NULL if none
    public RaycastHit2D Cast(Vector2 dir, LayerMask mask, float len)
    {
        Bounds bounds = col.bounds;
        List<RaycastHit2D> hits = new List<RaycastHit2D>();
        ContactFilter2D filter = new ContactFilter2D();
        bounds.Expand(skinWidth * -2f);
        filter.SetLayerMask(mask);
        Physics2D.BoxCast(bounds.center, bounds.size,  0f, dir, filter, hits, len + skinWidth*2f);
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.isTrigger)
            {
                continue;
            }
            // Return how far we can move without hitting something
            return hit;
        }
        return default(RaycastHit2D);
    }

    public RaycastHit2D RayCast(Vector2 dir, LayerMask mask, float len)
    {
        return Physics2D.Raycast(col.bounds.center, dir, len + skinWidth, mask);
    }
}

