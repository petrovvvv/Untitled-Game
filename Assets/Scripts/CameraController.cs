using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float speed;
    private Vector3 pos;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, pos, speed * Time.deltaTime);
        Debug.Log(pos);
    }

    public void SetPosition(Vector3 newPos)
    {
        pos = newPos;
    }
}
