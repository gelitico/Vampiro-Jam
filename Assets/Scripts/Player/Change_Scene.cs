using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Collections;

public class Change_Scene : MonoBehaviour
{
    [SerializeField] private string nombreEscenaAInvocarse;

    [Header("Configuracion de Interaccion")]
    public LayerMask capaInteractuable; 
    public float distanciaMaximaRayo = 3f; 

    [Header("Configuracion de la Pausa")]
    [Tooltip("Segundos que el juego y el jugador permanecerán congelados al entrar a la nueva escena")]
    [SerializeField] private float segundosCongelado = 5f;

    // Variables de control internas
    private bool jugadorCerca = false; 
    private GameObject jugadorRef; 
    private Camera camaraPrincipal; 

    //---------------------------------------------MANO
    private ControlManoUI manoUI;
    private bool estabaMirando = false;

    void Start()
    {
        camaraPrincipal = Camera.main;

        manoUI = FindObjectOfType<ControlManoUI>();
    }

    void Update()
    {
        bool mirandoAhora = false;
        if (jugadorCerca)
        {
            mirandoAhora = EstaMirandoAlObjeto();

            if (mirandoAhora && !estabaMirando)
            {
                if (manoUI != null)
                {
                    
                    manoUI.ActivarAccion("lanzar"); 
                }
            }
        }

        estabaMirando = mirandoAhora;
        // Si el jugador está cerca y presiona el clic izquierdo
        if (jugadorCerca && Mouse.current.leftButton.wasPressedThisFrame)
        {
            // ... y además está mirando directamente al objeto interactuable
            if (EstaMirandoAlObjeto())
            {   if (manoUI != null)
                {
                    manoUI.ActivarAccion("clicar");
                }
                IniciarCambioEscena();
            }
        }
    }

    private bool EstaMirandoAlObjeto()
    {
        if (camaraPrincipal == null) return false;

        Ray rayo = camaraPrincipal.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(rayo, out hit, distanciaMaximaRayo, capaInteractuable))
        {
            if (hit.transform == this.transform)
            {
                return true; 
            }
        }
        return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            jugadorCerca = true;
            jugadorRef = other.gameObject; 
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = false;
            jugadorRef = null;
        }
    }

    private void IniciarCambioEscena()
    {
        if (jugadorRef != null)
        {
            // Evitamos que el jugador se destruya al cambiar de escena
            DontDestroyOnLoad(jugadorRef); 

            // Conseguimos sus componentes de movimiento
            CharacterController controller = jugadorRef.GetComponent<CharacterController>();
            PlayerInput inputSystem = jugadorRef.GetComponent<PlayerInput>();
            Rigidbody rb = jugadorRef.GetComponent<Rigidbody>();

            // Desactivamos sus componentes para que no pueda interactuar ni moverse
            if (controller != null) controller.enabled = false;
            if (inputSystem != null) inputSystem.enabled = false;
            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;
                rb.isKinematic = true;
            }

            // Hacemos que este script sobreviva temporalmente para encargarse de descongelar todo en la nueva escena
            DontDestroyOnLoad(this.gameObject);

            // Iniciamos la cuenta atrás pasando el jugador como parámetro
            StartCoroutine(TemporizadorCongelarJuego(segundosCongelado, jugadorRef));
        }

        // Cargamos la nueva escena de forma inmediata
        SceneManager.LoadScene(nombreEscenaAInvocarse);
    }

    private IEnumerator TemporizadorCongelarJuego(float tiempoEspera, GameObject jugador)
    {
        // 1. ESPERA UN FOTOGRAMA: Nos aseguramos de que Unity ya esté cargando/cargado la nueva escena
        yield return null;

        // 2. CONGELAMOS EL TIEMPO GLOBAL DEL JUEGO
        Time.timeScale = 0f;
        Debug.Log($"Juego congelado en la nueva escena. Esperando {tiempoEspera} segundos...");

        // 3. ESPERA LOS SEGUNDOS ESPECIFICADOS
        // Usamos WaitForSecondsRealtime porque WaitForSeconds normal no avanza si Time.timeScale es 0
        yield return new WaitForSecondsRealtime(tiempoEspera);

        // 4. DESCONGELAMOS EL TIEMPO GLOBAL DEL JUEGO
        Time.timeScale = 1f;

        // 5. DEVOLVEMOS EL MOVIMIENTO AL JUGADOR
        if (jugador != null)
        {
            CharacterController controller = jugador.GetComponent<CharacterController>();
            PlayerInput inputSystem = jugador.GetComponent<PlayerInput>();
            Rigidbody rb = jugador.GetComponent<Rigidbody>();

            if (controller != null) controller.enabled = true;
            if (inputSystem != null) inputSystem.enabled = true;
            if (rb != null) rb.isKinematic = false;

            Debug.Log("¡Tiempo de espera finalizado! Todo vuelve a la normalidad.");
        }

        // Buscamos la nueva cámara principal de la escena recién cargada para futuros comportamientos
        camaraPrincipal = Camera.main;

        // Destruimos este objeto del botón interactuable que ya cumplió su función
        Destroy(this.gameObject);
    }
}