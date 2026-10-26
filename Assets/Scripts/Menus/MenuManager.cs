using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour

{
    public GameObject panelUI;
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

    public void DeletePanel()
    {
        Destroy(panelUI);
    }
}
