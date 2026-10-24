using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
  public void RetryGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Calle_1");
    }

    // Cierra el juego
    public void QuitGame()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }
}
