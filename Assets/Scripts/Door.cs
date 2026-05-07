using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private GameObject leftBot;
    [SerializeField] private GameObject rightTop;
    [SerializeField] private bool vert;
    private CameraController cam;

    void Start()
    {
        cam = GameObject.Find("Main Camera").gameObject.GetComponent<CameraController>();
    }
     
    void OnTriggerEnter2D(Collider2D c)
    {
        // If vertical door, use y values, otherwise use x
        float player = vert ? c.transform.position.y : c.transform.position.x;
        float door  = vert ? transform.position.y : transform.position.x;
        if (player < door)
        {
            // Left -> right or bottom -> top
            cam.SetPosition(rightTop.transform.position);

        } else
        {
            // Right -> left or top -> bottom
            cam.SetPosition(leftBot.transform.position);
        }
    }
}
