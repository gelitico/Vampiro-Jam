using System.Collections;
using UnityEngine;
using UnityEngine.Video;

public class IntroManager : MonoBehaviour
{
    public GameObject panelCinematica;    
    public GameObject panelMenuPrincipal; 
    public VideoPlayer videoPlayer;      

    public CanvasGroup canvasGroupFlash;      
    public float duracionFlash = 0.5f;        

    void Start()
    {
        if (panelMenuPrincipal != null) panelMenuPrincipal.SetActive(false);
        if (panelCinematica != null) panelCinematica.SetActive(true);

        
        if (canvasGroupFlash != null)
        {
            canvasGroupFlash.alpha = 0f;
            canvasGroupFlash.gameObject.SetActive(true);
        }

        if (videoPlayer != null)
        {
            videoPlayer.isLooping = false;
            videoPlayer.loopPointReached += TerminoCinematica;
            videoPlayer.Play();
        }
    }

    void SaltoCinematica()
    {
        if (panelCinematica == null)
        {
            StartCoroutine(EfectoFlashBlanco());
        }
    }

    void TerminoCinematica(VideoPlayer vp)
    {
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached -= TerminoCinematica;
        }

        StartCoroutine(EfectoFlashBlanco());
    }

   IEnumerator EfectoFlashBlanco()
{
    Debug.Log("Iniciando transición de flash...");

    // 1. Encendemos el menú principal
    if (panelMenuPrincipal != null) 
    {
        panelMenuPrincipal.SetActive(true);

        Animator animMenu = panelMenuPrincipal.GetComponent<Animator>();
        if (animMenu != null)
        {
            animMenu.SetTrigger("Aparecer"); 
        }
    }

    // 2. Activamos el flash blanco
    if (canvasGroupFlash != null)
    {
        canvasGroupFlash.gameObject.SetActive(true);
        canvasGroupFlash.alpha = 1f;
        Debug.Log("Flash activado al 100% de opacidad.");
    }
    else
    {
        Debug.LogWarning("¡El Canvas Group Flash está vacio en el Inspector!");
    }

    // 3. Apagamos la cinemática
    if (panelCinematica != null)
    {
        panelCinematica.SetActive(false);
    }

    // 4. Desvanecemos el blanco
    float tiempoTranscurrido = 0f;
    while (tiempoTranscurrido < duracionFlash)
    {
        tiempoTranscurrido += Time.deltaTime;
        if (canvasGroupFlash != null)
        {
            canvasGroupFlash.alpha = Mathf.Clamp01(1f - (tiempoTranscurrido / duracionFlash));
        }
        yield return null;
    }

    if (canvasGroupFlash != null)
    {
        canvasGroupFlash.alpha = 0f;
    }
}
}