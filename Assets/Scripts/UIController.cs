// Hacky script for getting some basic tutorial text in. TODO: change this later

using UnityEngine;
using UnityEngine.InputSystem;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;

    private InputAction pauseAction;

    void Start()
    {
        pauseAction = InputSystem.actions.FindAction("Pause");
        pauseMenu.SetActive(false);
    }

    public void Update()
    {
        if (pauseAction.WasPressedThisFrame())
        {
            Pause();
        }
    }

    public void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
    }

    public void Unpause()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}