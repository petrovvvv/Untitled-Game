using System;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private Vector3[] waypoints;
    [SerializeField] private float speed;
    private int i;
    private BoxCollider2D col;
    private LayerMask selfLayer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        i = 0;
        col = GetComponent<BoxCollider2D>();
        selfLayer = 1 << gameObject.layer;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mvmt = CalculateMvmt();
        transform.Translate(mvmt);
        GameObject obj = adjacentObject(mvmt);
        if (obj)
        {
            Physics phys = obj.GetComponentInParent<Physics>();
            if (phys)
            {
                phys.Move(mvmt.x, mvmt.y);
            }
        }
    }

    private Vector2 CalculateMvmt()
    {
        if (transform.position == waypoints[i])
        {
            i = (i + 1) % waypoints.Length;
        }

        float dX = waypoints[i].x - transform.position.x;
        float dY = waypoints[i].y - transform.position.y;
        float angle = Mathf.Atan(dY / dX);
        float totalDist = Vector3.Distance(waypoints[i], transform.position);
        float frameDist = speed * Time.deltaTime;
        if (totalDist < frameDist)
        {
            frameDist = totalDist;
        }

        return new Vector2(frameDist * Mathf.Cos(angle) * Mathf.Sign(dX), frameDist * Mathf.Sin(angle) * Math.Sign(dY));
    }

    private GameObject adjacentObject(Vector2 dir)
    {
        Bounds bounds = col.bounds;

        // Check vertical first
        RaycastHit2D hit = Physics2D.BoxCast(bounds.center, bounds.size,  0f, Vector2.up, 0.1f, ~selfLayer);
        if (hit)
        {
            return hit.collider.gameObject;
        }

        // Check horizontal in direction platform is moving
        hit = Physics2D.BoxCast(bounds.center, bounds.size, 0f, new Vector2(Mathf.Sign(dir.x), 0f), 0.1f, ~selfLayer);
        if (hit)
        {
            return hit.collider.gameObject;
        }
        return null;
    }
}
