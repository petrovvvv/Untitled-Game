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
    [SerializeField] private LayerMask selfLayer;
    [SerializeField] private LayerMask groundLayer;

    public bool IsGrounded(BoxCollider2D c)
    {
        RaycastHit2D groundHit = Cast(c, Vector2.down, groundLayer, skinWidth);
        return groundHit;
    }

    public bool HitHead(BoxCollider2D c)
    {
        RaycastHit2D topHit = Cast(c, Vector2.up, ~selfLayer, skinWidth);
        return topHit;
    }

    public bool OnWall(BoxCollider2D c)
    {
        // Cast ray in direction object is facing
        Vector2 dir = new Vector2(transform.localScale.x, 0f);
        RaycastHit2D sideHit = Cast(c, dir, ~selfLayer, skinWidth);
        return sideHit;
    }

    public void  Move(float x, float y, BoxCollider2D c)
    {
        // BoxCast in direction object is moving to check for obstacles
        x = CastLen(Vector2.right, x, c);
        y = CastLen(Vector2.up, y, c);
        
        transform.Translate(new Vector2(x, y));
    }

    private float CastLen(Vector2 dir, float len, BoxCollider2D c) {
        float dirSign = Math.Sign(len);
        RaycastHit2D hit = Cast(c, dir * dirSign, ~selfLayer, Math.Abs(len));
        if (!hit)
        {
            return len;
        }
        return (hit.distance - skinWidth) * dirSign;
    }

    private RaycastHit2D Cast(BoxCollider2D c, Vector2 dir, LayerMask mask, float len)
    {
        Bounds bounds = c.bounds;
        List<RaycastHit2D> hits = new List<RaycastHit2D>();
        ContactFilter2D filter = new ContactFilter2D();
        bounds.Expand(skinWidth * -2f);
        filter.SetLayerMask(mask);
        Physics2D.BoxCast(bounds.center, bounds.size,  0f, dir, filter, hits, len + skinWidth);
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
}

