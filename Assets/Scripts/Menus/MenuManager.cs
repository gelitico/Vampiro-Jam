using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour

{
    
    public void RetryGame()
    {
       
        SceneManager.LoadScene("Calle_1");
    }

    public void LoadMainMenu()
    {
        
        SceneManager.LoadScene("Main_Menu");
    }

    // Cierra el juego
    public void QuitGame()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }

    
}
