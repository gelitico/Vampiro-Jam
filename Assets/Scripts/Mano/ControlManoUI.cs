using UnityEngine;
using UnityEngine.UI;

public class ControlManoUI : MonoBehaviour
{
    private Image imagenUI;

   
    public Sprite spriteIdle;
    public Sprite spriteClicando;
    public Sprite spriteCogiendo;
    public Sprite spriteLanzando;

   
    public float duracionAccion = 0.5f;
    private float temporizador = 0f;
    private bool enAccion = false;

    void Start()
    {
        imagenUI = GetComponent<Image>();
        CambiarSprite(spriteIdle);
    }

    void Update()
    {
        
        if (enAccion)
        {
            temporizador -= Time.deltaTime;
            if (temporizador <= 0f)
            {
                CambiarSprite(spriteIdle);
                enAccion = false;
            }
        }
    }

    
    public void ActivarAccion(string tipoAccion)
    {
        Sprite spriteSeleccionado = spriteIdle;

        switch (tipoAccion.ToLower())
        {
            case "clicar":
                spriteSeleccionado = spriteClicando;
                break;
            case "coger":
                spriteSeleccionado = spriteCogiendo;
                break;
            case "lanzar":
                spriteSeleccionado = spriteLanzando;
                break;
        }

        CambiarSprite(spriteSeleccionado);
        temporizador = duracionAccion;
        enAccion = true;
    }

    private void CambiarSprite(Sprite sprite)
    {
        if (imagenUI != null && sprite != null)
        {
            imagenUI.sprite = sprite;
        }
    }
}