using System.Collections;
using UnityEngine;


public class flash : MonoBehaviour
{
    public CanvasGroup canvasGroupFlash;      
    public float duracionFlash = 0.5f;      
    
void Start()
    {
        StartCoroutine(EfectoFlashBlanco());
    }

    

   IEnumerator EfectoFlashBlanco()
{
    if (canvasGroupFlash != null)
    {
        canvasGroupFlash.gameObject.SetActive(true);
        canvasGroupFlash.alpha = 1f;
    }


    // 4. Desvanecemos el blanco
    float tiempoTranscurrido = 0f;
    while (tiempoTranscurrido < duracionFlash)// bucle q se repite const mientras el tiempo transcurrideo sea menor a la duracion
    {
        tiempoTranscurrido += Time.deltaTime;

        if (canvasGroupFlash != null) //canvas group = poder modificar varios atributos del canvas a la vez
        {
            canvasGroupFlash.alpha = Mathf.Clamp01(1f - (tiempoTranscurrido / duracionFlash));//1-el porcentaje del tiempo transcurrido, Clamp01 = vamor resultante siempre entre 0 y 1
        }
        yield return null; //espera 1 fotograma
    }

    if (canvasGroupFlash != null)
    {
        canvasGroupFlash.alpha = 0f; // cuando se acabe el bucle siempre 0
    }
}
}