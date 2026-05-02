using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private GameObject left;
    [SerializeField] private GameObject right;
    private CameraController cam;

    void Start()
    {
        cam = GameObject.Find("Main Camera").gameObject.GetComponent<CameraController>();
    }
     
    void OnTriggerEnter2D(Collider2D c)
    {
        Debug.Log("door hit");
        if (c.transform.position.x < transform.position.x)
        {
            // Left -> right
            cam.SetPosition(right.transform.position);

        } else
        {
            // Right -> left
            cam.SetPosition(left.transform.position);
        }
    }
}
