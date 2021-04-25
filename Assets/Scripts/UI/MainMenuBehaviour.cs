using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuBehaviour : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject optionMenu;
    public GameObject controlMenu;

    private void Start()
    {
        OpenMenu();
    }

    public void OpenOption()
    {
        optionMenu.SetActive(true);

        if (mainMenu)
            mainMenu.SetActive(false);
        if (controlMenu)
            controlMenu.SetActive(false);
    }

    public void OpenMenu()
    {
        mainMenu.SetActive(true);

        if (optionMenu)
            optionMenu.SetActive(false);
        if (controlMenu)
            controlMenu.SetActive(false);
    }
    public void OpenControl()
    {
        controlMenu.SetActive(true);

        if (optionMenu)
            optionMenu.SetActive(false);
        if (mainMenu)
            mainMenu.SetActive(false);
    }

    public void OpenLevel(int build)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(build);
    }

    public void OpenQuit()
    {
        Application.Quit();
    }
}
