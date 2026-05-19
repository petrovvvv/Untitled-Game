using System;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
    [SerializeField] private Vector3[] waypoints;
    [SerializeField] private float speed;
    private int i;

    protected void InitMovingObject()
    {
        i = 0;
    }

    void Start()
    {
        InitMovingObject();
    }

    void Update()
    {
        Vector2 mvmt = CalculateMvmt();
        transform.Translate(mvmt);
    }
    protected Vector2 CalculateMvmt()
    {
        if (Vector3.Distance(transform.position,waypoints[i]) <= 0.05)
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
