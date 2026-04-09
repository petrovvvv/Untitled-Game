 /*
  *  Custom physics engine. Replacement for Unity's Rigitbody2D.
  */

using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]

public class Physics : MonoBehaviour
{   
    private float skinWidth = 0.02f;
    private BoxCollider2D boxCollider;
    [SerializeField] private LayerMask selfLayer;
    [SerializeField] private LayerMask groundLayer;

    void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    public bool IsGrounded()
    {
        RaycastHit2D groundHit = Physics2D.BoxCast(boxCollider.bounds.center,
                                                     boxCollider.bounds.size, 0f,
                                                     Vector2.down, skinWidth, groundLayer);
        return groundHit.collider != null;
    }

    public bool HitHead()
    {
        Bounds bounds = boxCollider.bounds;
        bounds.Expand(skinWidth * -2f);
        RaycastHit2D topHit = Physics2D.BoxCast(bounds.center, bounds.size, 0f, Vector2.up, skinWidth, ~selfLayer);
        return topHit.collider != null;
    }

    public void  Move(float x, float y)
    {
        // BoxCast in direction object is moving to check for obstacles
        x = CastLen(Vector2.right, x);
        y = CastLen(Vector2.up, y);
        
        transform.Translate(new Vector2(x, y));
    }

    // TODO: make single diagonal cast
    private float CastLen(Vector2 dir, float len) {
        float dirSign = Math.Sign(len);
        Bounds bounds = boxCollider.bounds;
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

