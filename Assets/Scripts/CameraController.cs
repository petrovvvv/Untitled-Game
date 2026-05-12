using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float speed;
    private Vector3 pos;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        pos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, pos) < 0.1)
        {
            transform.position = pos;
        }
        transform.position = Vector3.Lerp(transform.position, pos, speed * Time.deltaTime);
    }

    public void SetPosition(Vector3 newPos)
    {
        pos = newPos;
    }

    public Vector3 GetPosition()
    {
        return pos;
    }

    public void ResetCamera(Vector3 newPos)
    {
        pos = newPos;
        transform.position = newPos;
    } 
}
