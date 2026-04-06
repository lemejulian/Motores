using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject opcionesMenu;
    public GameObject mainMenu;

    public void OpenOptionsPanel()
    {
        mainMenu.SetActive(false);
        opcionesMenu.SetActive(true);
    }

    public void OpenMainMenuPanel()
    {
        mainMenu.SetActive(true);
        opcionesMenu.SetActive(false);
    }

   
public void quitGame()
{
    Debug.Log("El jugador salió del juego en: ");

    Application.Quit();
}

    public void PlayGame()
    {
        SceneManager.LoadScene("Level");
    }
}