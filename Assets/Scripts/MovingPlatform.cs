using System;
using UnityEngine;
using UnityEngine.Rendering;

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
    }

    private Vector2 CalculateMvmt()
    {
        if (transform.position == waypoints[i])
        {
            i = (i + 1) % waypoints.Length;
        }

        float dX = waypoints[i].x - transform.position.x;
        float dY = waypoints[i].y - transform.position.y;
        float frameDist = Math.Min(speed * Time.deltaTime, Vector3.Distance(waypoints[i], transform.position));
        if (dX == 0) {
            // Vertical movement only
            return new Vector2(0f, frameDist * Math.Sign(dY));
        }

        float angle = Mathf.Atan(dY / dX);

        return new Vector2(frameDist * Mathf.Cos(angle) * Mathf.Sign(dX), frameDist * Mathf.Sin(angle) * Math.Sign(dY));
    }
}
