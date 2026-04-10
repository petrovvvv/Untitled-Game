 /*
  *  Custom physics engine. Replacement for Unity's Rigitbody2D.
  */

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
        RaycastHit2D groundHit = Physics2D.BoxCast(c.bounds.center,
                                                     c.bounds.size, 0f,
                                                     Vector2.down, skinWidth, groundLayer);
        return groundHit.collider != null;
    }

    public bool HitHead(BoxCollider2D c)
    {
        Bounds bounds = c.bounds;
        bounds.Expand(skinWidth * -2f);
        RaycastHit2D topHit = Physics2D.BoxCast(bounds.center, bounds.size, 0f, Vector2.up, skinWidth, ~selfLayer);
        return topHit.collider != null;
    }

    public void  Move(float x, float y, BoxCollider2D c)
    {
        // BoxCast in direction object is moving to check for obstacles
        x = CastLen(Vector2.right, x, c);
        y = CastLen(Vector2.up, y, c);
        
        transform.Translate(new Vector2(x, y));
    }

    // TODO: make single diagonal cast
    private float CastLen(Vector2 dir, float len, BoxCollider2D c) {
        float dirSign = Math.Sign(len);
        Bounds bounds = c.bounds;
        List<RaycastHit2D> hits = new List<RaycastHit2D>();
        ContactFilter2D filter = new ContactFilter2D();

        bounds.Expand(skinWidth * -2f);
        filter.SetLayerMask(~selfLayer);
        Physics2D.BoxCast(bounds.center, bounds.size,  0f,
                                        dir * dirSign, filter, hits, Math.Abs(len) + skinWidth);
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.isTrigger)
            {
                continue;
            }
            // Return how far we can move without hitting something
            return (hit.distance - skinWidth) * dirSign;
        }
        return len;
    }
}

