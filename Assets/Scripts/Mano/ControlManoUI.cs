using UnityEngine;
using UnityEngine.UI;

public class ControlManoUI : MonoBehaviour
{
    private Image imagenUI;//comp img del obj

   
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
        CambiarSprite(spriteIdle);//img default mano idle al empezar
    }

    void Update()
    {
        
        if (enAccion)//si estamos haciendo algo (con la mano cambiada) inicia el contador
        {
            temporizador -= Time.deltaTime;
            if (temporizador <= 0f)
            {
                CambiarSprite(spriteIdle);// cuando acaba volvemos a idle y ya no estamos en accion
                enAccion = false;
            }
        }
    }

    
    public void ActivarAccion(string tipoAccion)// funcion publica que llamamos desde otros codigos, con el string ponemos lo que queremos que se llame desde alli
    {
        Sprite spriteSeleccionado = spriteIdle;

        switch (tipoAccion.ToLower())//comparar el texto recibido con: tolower es para que de igual como escribas el texto con mayusculas o lo q sea se lea 
        {
            case "clicar"://case=posibles opciones
                spriteSeleccionado = spriteClicando;
                break;//se acabo sal fuera
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
        if (imagenUI != null && sprite != null)//comprobacion de que el sprite y la img no son nulos
        {
            imagenUI.sprite = sprite;
        }
    }
}