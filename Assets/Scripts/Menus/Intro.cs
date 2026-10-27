using System.Collections;
using UnityEngine;

using UnityEngine.SceneManagement;

public class Intro : MonoBehaviour
{
    public GameObject boton;
    void Start() //para que se inicia justo al arrancar
    {
        int esPrimeraVez = PlayerPrefs.GetInt("YaJugado", 0);//player prefs (base de datos unity)get int busca si hemos guardado ya jugado, valor 0 al ppio
        
        if (esPrimeraVez == 0)
        {
            PlayerPrefs.SetInt("YaJugado", 1);//ya no es 0, ya han jugado
            PlayerPrefs.Save();//se guarda la info en la base de datos
        
        if (boton != null) boton.SetActive(false);//que no se pueda dar al boton en la primera vez
        StartCoroutine(EsperarYCargarMenu()); // inicia la cuenta atras

        }
        else
        {
             //si no es la primera vez
            if (boton != null) boton.SetActive(true);
            StartCoroutine(EsperarYCargarMenu());
            
             
        }
    }

    private IEnumerator EsperarYCargarMenu()
    {
        yield return new WaitForSeconds(13f);//espera exactamente lo q dura
        CargarMenuDirecto();
        // Carga la escena 
       
    }

    private void CargarMenuDirecto()
    {
        SceneManager.LoadScene("Main_Menu");
    }
}
