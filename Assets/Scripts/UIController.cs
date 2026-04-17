// Hacky script for getting some basic tutorial text in. TODO: change this later

using UnityEngine;

public class UIController : MonoBehaviour
{
    private GameObject curText;

    void Start()
    {
        curText = transform.Find("Start Text").gameObject;
    }

    public void DisableText()
    {
        curText.SetActive(false);
    }
    public void EnableJumpText()
    {
        curText = transform.Find("Jump Text").gameObject;
        curText.SetActive(true);
    }

    public void EnableDoubleJumpText()
    {
        curText = transform.Find("Double-Jump Text").gameObject;
        curText.SetActive(true);
    }
}
