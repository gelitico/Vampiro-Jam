using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Change_Scene : MonoBehaviour
{
    [SerializeField] private string nombreEscenaALevedar;

    [Header("Configuracion de Interaccion")]
    [SerializeField] private LayerMask capaInteractuable; // seleccionar la capa del timbre en el inspector
    [SerializeField] private float distanciaMaximaRayo = 3f; // que tan cerca debe mirar para interactuar


    private bool jugadorCerca = false;
    private GameObject jugadorRef;
    private Camera camaraPrincipal;

    void Start()
    {
        // referencia de la camara principal al inicio
        camaraPrincipal = Camera.main;
    }

    void Update()
    {
        if (jugadorCerca && Keyboard.current.eKey.wasPressedThisFrame)
        {
            if (EstaMirandoAlObjeto())
            {
                CambiarDeEscena();
            }
        }
    }

    private bool EstaMirandoAlObjeto()
    {
        if (camaraPrincipal == null) return false;

        // Creamos un rayo desde el centro exacto de la pantalla (la mirada del jugador)
        Ray rayo = camaraPrincipal.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        // Lanzamos el rayo al frente
        if (Physics.Raycast(rayo, out hit, distanciaMaximaRayo, capaInteractuable))
        {
            // Comprobamos si el rayo golpeó a este mismo objeto (el timbre)
            if (hit.transform == this.transform)
            {
                return true;
            }
        }

        return false;
    }

    // Detecta cuando el jugador entra en el área del timbre
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Asegúrate de que tu jugador tenga la etiqueta "Player"
        {
            jugadorCerca = true;
            jugadorRef = other.gameObject;
            Debug.Log("Presiona 'E' para entrar");
        }
    }

    // Detecta cuando el jugador se aleja del timbre
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = false;
            jugadorRef = null;
        }
    }

    private void CambiarDeEscena()
    {
        if (jugadorRef != null)
        {
            Vector3 posicionUltima = jugadorRef.transform.position; // guardar posicion actual del jugador antes de cambiar de scena

            if (jugadorRef.TryGetComponent<CharacterController>(out CharacterController controller))
            {
                controller.enabled = false;
            }
            if (jugadorRef.TryGetComponent<Rigidbody>(out Rigidbody rb))
            {
                rb.linearVelocity = Vector3.zero;
                rb.isKinematic = true;
            }

            DontDestroyOnLoad(jugadorRef); // mantiene al jugador "vivo"

            jugadorRef.transform.position = posicionUltima; // forzamos posicion guardada

            // volvemos a encender las fisicas un fotograma despues de que cargue la escena
            SceneManager.sceneLoaded += (scene, mode) => {
                if (controller != null) controller.enabled = true;
                if (rb != null) rb.isKinematic = false;
            };
        }

        SceneManager.LoadScene(nombreEscenaALevedar);
    }
}
