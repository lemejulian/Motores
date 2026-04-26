using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuDerrota : MonoBehaviour
{
    public GameObject defeatMenu; 

    
    public void ShowDefeatMenu()
    {
        defeatMenu.SetActive(true);
        Time.timeScale = 0f; 
    }

    
    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene("Menuinicio"); 
    }

    
    public void QuitGame()
    {
        Debug.Log("El jugador salió del juego desde el menú de derrota.");
        Application.Quit();
    }
}
